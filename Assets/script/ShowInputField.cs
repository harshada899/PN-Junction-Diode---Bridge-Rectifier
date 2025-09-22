using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ShowInputField : MonoBehaviour
{
    public GameObject inputFieldObject; // Assign the GameObject with TMP_InputField
    private TMP_InputField inputField;

    void Start()
    {
        inputField = inputFieldObject.GetComponent<TMP_InputField>();
        inputFieldObject.SetActive(false); // Start hidden
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            inputFieldObject.SetActive(true);                 // Show input field
            inputField.text = "";                             // Clear previous text
            EventSystem.current.SetSelectedGameObject(inputFieldObject); // Focus
            inputField.ActivateInputField();                  // Begin typing
        }

        // Optional: hide input field with ESC
        if (Input.GetKeyDown(KeyCode.Escape) && inputFieldObject.activeSelf)
        {
            inputFieldObject.SetActive(false);
        }
    }
}
