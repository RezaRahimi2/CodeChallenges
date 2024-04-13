using System.Linq;
using UnityEngine;
using TMPro;

public class ChatInputTaker : MonoBehaviour
{
    public TMP_InputField inputField;

    private int _oldLength;

    #region Constants

    private const int LAST_CHAR_INDEX = 126;
    private const int FIRST_CHAR_INDEX = 32;
    private const string CARET = " ";

    #endregion

    private void Awake()
    {
        int inclusiveOffset = 1;
    }

    private void Start()
    {
        inputField.ActivateInputField();
        inputField.text = CARET;
        PutCaretInPosition();
    }

    private void Update()
    {
        if (Input.anyKey)
        {
            //inputField.ActivateInputField();
            PutCaretInPosition();
        }
    }

    public void ValidateChatInput()
    {
        string text = inputField.text;

        if (text.Length == CARET.Length)
            return;

        string caretlessText = text.Substring(0, text.Length - CARET.Length);

        // If only deletion occurred, return.
        if (caretlessText.Length - _oldLength < 0)
        {
            _oldLength = caretlessText.Length;
            return;
        }

        int lastIndex = caretlessText.Length - 1;
        char newChar = caretlessText[lastIndex];

        _oldLength = caretlessText.Length;
        SetInputFieldText(caretlessText);
    }

    private void PutCaretInPosition()
    {
        inputField.caretPosition = inputField.text.Length - CARET.Length;
    }

    private void SetInputFieldText(string newText)
    {
        inputField.text = newText + CARET;
        PutCaretInPosition();
    }
}