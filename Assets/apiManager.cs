using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

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
                // Parse the JSON response
                string responseText = request.downloadHandler.text;
                CoinResponse coinResponse = JsonUtility.FromJson<CoinResponse>(responseText);

                // Update the TextMeshPro text with the coin value
                coinText.text = $": {coinResponse.coins}";
            }
            else
            {
                Debug.LogError($"Error fetching coins: {request.error}");
            }
        }
    }

    // Class to map JSON response
    [System.Serializable]
    public class CoinResponse
    {
        public int coins; 
    }
}
