using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class DialoguePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _npcNameText;
    [SerializeField] TextMeshProUGUI _dialogueText;

    [SerializeField] float _typingSpeed;

    public bool IsTyping { get; private set; }


    Coroutine _coTyping;


    void Update()
    {
        if(!IsTyping && DialogueManager.Instance.CurrentDialogueQueue.HasNextDialogue() && Input.GetKeyDown(KeyCode.Return))
        {
            DialogueManager.Instance.StartDialogue();
        }
        
    }
    public void StartTyping(string text)
    {
        if(_coTyping != null)
        {
            StopCoroutine(_coTyping);
        }

        _coTyping = StartCoroutine(CoTypeText(text));
    }

    IEnumerator CoTypeText(string text)
    {
        IsTyping = true;
        _dialogueText.text = "";
        StringBuilder sb = new StringBuilder();

        foreach(char letter in text)
        {
            sb.Append(letter);
            _dialogueText.text  =sb.ToString();
            yield return new WaitForSeconds(_typingSpeed);
        }

        IsTyping=false;
    }
}
