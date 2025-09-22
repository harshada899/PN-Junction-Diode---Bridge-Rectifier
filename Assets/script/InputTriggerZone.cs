using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class InputTriggerZone : MonoBehaviour
{
    public GameObject inputFieldObject; // Assign the InputField GameObject in Inspector
    private TMP_InputField inputField;
    public GameObject outputfield;

    private bool playerInTrigger = false;

    void Start()
    {
        inputField = inputFieldObject.GetComponent<TMP_InputField>();
        inputFieldObject.SetActive(false);
        outputfield.SetActive(false);
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.F))
        {
            inputFieldObject.SetActive(true);
            outputfield.SetActive(true);
            inputField.text = "";
            EventSystem.current.SetSelectedGameObject(inputFieldObject);
            inputField.ActivateInputField();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && inputFieldObject.activeSelf)
        {
            inputFieldObject.SetActive(false);
            outputfield.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            inputFieldObject.SetActive(false);
        }
    }
}
