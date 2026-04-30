using NUnit.Framework;
using UnityEngine;

public class AiControllerTests
{
    // assert buildbossprompt works correctly
    [Test]
    public void BuildBossPrompt_ContainsBossFightDetails()
    {
        var obj = new GameObject("AiController");
        var ai = obj.AddComponent<AiController>();

        string prompt = ai.BuildBossPrompt(
            "Boss defeated",
            "Fire Knight",
            "fast",
            "low health",
            "45 seconds"
        );

        Assert.IsTrue(prompt.Contains("Boss defeated"));
        Assert.IsTrue(prompt.Contains("Fire Knight"));
        Assert.IsTrue(prompt.Contains("fast"));
        Assert.IsTrue(prompt.Contains("low health"));
        Assert.IsTrue(prompt.Contains("45 seconds"));
        Assert.IsTrue(prompt.Contains("under 15 words"));
        Assert.IsTrue(prompt.Contains("ominous"));
    }

    // assert json is parsed correctly
    [Test]
    public void ParseOpenRouterResponse_ReturnsMessageContent()
    {
        var obj = new GameObject("AiController");
        var ai = obj.AddComponent<AiController>();

        string json =
            "{\"choices\":[{\"index\":0,\"message\":{\"role\":\"assistant\",\"content\":\"Text\"}}]}";

        string result = ai.ParseOpenRouterResponse(json);

        Assert.AreEqual("Text", result);
    }
}