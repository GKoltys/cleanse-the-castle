using UnityEngine;

public class InternetController : MonoBehaviour
{
    public static InternetController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool HasInternet()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
}
