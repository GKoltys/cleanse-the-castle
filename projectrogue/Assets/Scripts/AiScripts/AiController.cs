using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

// https://medium.com/@yashasvi.xvi/integrating-ai-in-unity-using-d8d044efb071
public class AiController : MonoBehaviour
{
    public static AiController Instance;

    private static readonly HttpClient httpClient = new HttpClient();
    private float nextAllowedTime;
    private bool responseReturned = false;

    [SerializeField] private string apiKey = "API_KEY";
    private readonly string model = "nvidia/nemotron-3-super-120b-a12b:free";
    // google/gemma-4-26b-a4b-it:free - only good free one I could find. Though limit is reached quickly by all users
    // nvidia/nemotron-3-super-120b-a12b:free - using for testing, responses inadequit but limit not being reached

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public async void AskAi(string prompt)
    {
        if (Time.time < nextAllowedTime) return;

        nextAllowedTime = Time.time + 5f;

        if (!InternetController.Instance.HasInternet())
        {
            Debug.Log("No internet connection");
            return;
        }

        responseReturned = true;
        string response = await CallOpenRouterAPI(prompt);

        Debug.Log("AI response: " + response);

        if (responseReturned)
        {
            Debug.Log("Should be displaying AI response");
            AiNarratorController.Instance.ShowDialogue(response);
        }
    }

    public async Task<string> CallOpenRouterAPI(string prompt)
    {
        string apiUrl = "https://openrouter.ai/api/v1/chat/completions";

        string jsonBody = $@"
        {{
            ""model"": ""{model}"",
            ""messages"": [
                {{
                    ""role"": ""user"",
                    ""content"": ""{EscapeJson(prompt)}""
                }}
            ],
            ""max_tokens"": 40
        }}";

        var content = new StringContent(jsonBody, Encoding.UTF8, "application/json");
        httpClient.DefaultRequestHeaders.Clear();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

        try
        {
            HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
            string result = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                OpenRouterResponse responseObject = JsonConvert.DeserializeObject<OpenRouterResponse>(result);
                return responseObject.choices[0].message.content;
            }
            else
            {
                Debug.LogError($"Error calling OpenRouter: {response.StatusCode}\n{result}");
                responseReturned = false;
                return "API Error";
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Exception: " + ex.Message);
            responseReturned = false;
            return "Exception Error";
        }
    }

    private string EscapeJson(string input)
    {
        return input.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

    public string BuildBossPrompt(string gameEvent, string boss, string performance, string health, string duration)
    {
        return "You are a dark fantasy narrator in a roguelike game based in a castle. " +
            "Respond in under 15 words.\n\n" +

            "Event: " + gameEvent + "\n" +
            "Boss: " + boss + "\n" +
            "Performance: " + performance + "\n" +
            "Health: " + health + "\n" +
            "Fight duration: " + duration + "\n" +

            "Tone: ominous, mocking.";
    }

    // Helper classes for JSON parsing
    [Serializable]
    public class OpenRouterMessage
    {
        public string role;
        public string content;
    }

    [Serializable]
    public class OpenRouterChoice
    {
        public int index;
        public OpenRouterMessage message;
    }

    [Serializable]
    public class OpenRouterResponse
    {
        public List<OpenRouterChoice> choices;
    }
}
