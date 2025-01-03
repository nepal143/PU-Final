using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

public class APIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText; // TMP text field for displaying coins
    private string apiUrl = "https://coins-count.onrender.com/coins";

    void Start()
    {
        StartCoroutine(GetCoinsFromAPI());
    }

    IEnumerator GetCoinsFromAPI()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseText = request.downloadHandler.text;
                coinText.text = $"Coins: {responseText}";
            }
            else
            {
                Debug.LogError($"Error fetching coins: {request.error}");
            }
        }
    }
}
