using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class generativeai : MonoBehaviour
{
    public string geminiApiKey = "YOUR_FREE_API_KEY"; // Replace with your Gemini API Key
    public TMP_Text outputText;
    public string prompt = "Say something interesting about Sakura trees.";

    void Start()
    {
        StartCoroutine(SendPromptToGemini(prompt));
    }

    IEnumerator SendPromptToGemini(string promptText)
    {
        string apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={geminiApiKey}";

        string jsonBody = "{\"contents\": [{\"parts\": [{\"text\": \"" + promptText + "\"}]}]}";
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);

        UnityWebRequest request = new UnityWebRequest(apiUrl, "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Gemini API Error: " + request.error);
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
        string marker = "\"text\":\"";
        int start = json.IndexOf(marker) + marker.Length;
        int end = json.IndexOf("\"", start);

        if (start < marker.Length || end < 0 || end <= start)
            return "Unable to parse response.";

        string reply = json.Substring(start, end - start);
        return reply.Replace("\\n", "\n");
    }
}
