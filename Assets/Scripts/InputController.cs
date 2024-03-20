using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputController : MonoBehaviour
{
    public TMP_InputField inputField;
    public DropController dropController;

    private void Start()
    {
        if (inputField != null)
        {
            inputField.characterLimit = 5;
            inputField.ActivateInputField(); // This highlight the input
            inputField.onEndEdit.AddListener(HandleInputSubmit);
        }
    }

    private void HandleInputSubmit(string userInput)
    {
        if (!string.IsNullOrEmpty(userInput))
        {
            if (int.TryParse(userInput, out int parsedInput))
            {
                dropController.CheckForMatches(parsedInput);
            }
            else
            {
                inputField.text = "";
            }
        }

        inputField.text = "";
        if (inputField.gameObject.activeInHierarchy)
        {
            inputField.ActivateInputField();
        }
    }
    public void EnableInput()
    {
        inputField.interactable = true;
        inputField.ActivateInputField();
    }
    public void DisableInput()
    {
        inputField.text = "";
        inputField.interactable = false;
        inputField.DeactivateInputField();
    }



}
