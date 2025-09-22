using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class OllamaClient : MonoBehaviour
{
    public TMP_Text outputText;
    public string prompt = "Say something interesting about Sakura trees.";

    void Start()
    {
        StartCoroutine(SendPromptToOllama(prompt));
    }

    IEnumerator SendPromptToOllama(string promptText)
    {
        string apiUrl = "http://localhost:11434/api/generate";

        // JSON body for Ollama's API
        string jsonBody = JsonUtility.ToJson(new OllamaRequest
        {
            model = "llama3", // or whatever model you're running locally
            prompt = promptText,
            stream = false
        });

        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Ollama Error: " + request.error);
            outputText.text = "Error: " + request.error;
        }
        else
        {
            string result = request.downloadHandler.text;
            string reply = ExtractReplyFromJson(result);
            outputText.text = reply;
        }
    }

    string ExtractReplyFromJson(string json)
    {
        if (json.Contains("\"response\":\""))
        {
            int start = json.IndexOf("\"response\":\"") + "\"response\":\"".Length;
            int end = json.IndexOf("\"", start);
            return json.Substring(start, end - start).Replace("\\n", "\n");
        }
        return "Unable to parse response.";
    }

    // Helper class for request body
    [System.Serializable]
    public class OllamaRequest
    {
        public string model;
        public string prompt;
        public bool stream;
    }
}
