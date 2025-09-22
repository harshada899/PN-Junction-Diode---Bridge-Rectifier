using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;

public class OllamaInputHandler : MonoBehaviour
{
    public TMP_InputField playerInputField;
    public TMP_Text modelResponseText;
    public Button sendButton;

    private string apiUrl = "http://localhost:11434/api/generate";

    // 🌳 Elisha's personality prompt
    [Header("NPC Persona Prompt")]
    [TextArea(3, 10)]
    public string systemPrompt =
        "You are Elisha, a wise and gentle forest elf who lives under the World Tree. " +
        "You speak in a soft, mystical, and poetic tone. You are kind, insightful, and deeply connected to nature. " +
        "You respond as Elisha, not as an AI. Always stay in character and refer to the player as 'traveler'. but keep the reply under 3 sentences";

    private StringBuilder conversationHistory = new StringBuilder();

    void Start()
    {
        sendButton.onClick.AddListener(OnSendClicked);
        playerInputField.onSubmit.AddListener(delegate { OnSendClicked(); });
    }

    public void OnSendClicked()
    {
        string playerPrompt = playerInputField.text;
        if (!string.IsNullOrEmpty(playerPrompt))
        {
            StartCoroutine(SendPromptToOllama(playerPrompt));
            playerInputField.text = "";
        }
    }

    IEnumerator SendPromptToOllama(string promptText)
    {
        // 🧙 Append player input to conversation
        conversationHistory.AppendLine("Traveler: " + promptText);

        string fullPrompt = systemPrompt + "\n" + conversationHistory.ToString() + "\nElisha:";

        string jsonBody = JsonUtility.ToJson(new OllamaRequest
        {
            model = "mistral", // replace with your Ollama model name
            prompt = fullPrompt,
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
            modelResponseText.text = "Error: " + request.error;
        }
        else
        {
            string result = request.downloadHandler.text;
            string reply = ExtractReplyFromJson(result);
            modelResponseText.text = reply;

            // 🧝 Append Elisha's response to conversation
            conversationHistory.AppendLine("Elisha: " + reply);
        }
    }

    string ExtractReplyFromJson(string json)
    {
        string key = "\"response\":\"";
        int start = json.IndexOf(key) + key.Length;
        int end = json.IndexOf("\"", start);
        if (start >= key.Length && end > start)
            return json.Substring(start, end - start).Replace("\\n", "\n");
        return "Unable to parse response.";
    }

    [System.Serializable]
    public class OllamaRequest
    {
        public string model;
        public string prompt;
        public bool stream;
    }
}
