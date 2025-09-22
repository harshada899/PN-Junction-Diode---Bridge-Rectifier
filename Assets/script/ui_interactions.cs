using UnityEngine;
using TMPro;

public class ui_interactions : MonoBehaviour
{
    public GameObject gameUi;
    public TMP_InputField inputField;  // Reference to your input field
    private GameObject player;
    private MonoBehaviour playerMovementScript; // Replace with your actual movement script type

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovementScript = player.GetComponent<MonoBehaviour>(); // Replace with PlayerMovement, etc.
    }

    void Update()
    {
        if (inputField != null && playerMovementScript != null)
        {
            if (inputField.isFocused)
            {
                playerMovementScript.enabled = false;
            }
            else
            {
                playerMovementScript.enabled = true;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameUi.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameUi.SetActive(false);
        }
    }

    private void triggeractive(GameObject button)
    {
        button.SetActive(!button.activeInHierarchy);
    }
}
