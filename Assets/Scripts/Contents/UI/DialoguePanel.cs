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
    float _fadeDuration = 0.3f;

    public bool IsTyping { get; private set; }


    Coroutine _coTyping;


    void Update()
    {
        if(!IsTyping && DialogueManager.Instance.CurrentDialogueQueue.HasNextDialogue() && Input.GetKeyDown(KeyCode.Return))
        {
            DialogueManager.Instance.StartDialogue();
        }
        if(!DialogueManager.Instance.CurrentDialogueQueue.HasNextDialogue()&& Input.GetKeyDown(KeyCode.Return))
        {
            DialogueManager.Instance.EndDialogue();
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

    public void FadeIn()
    {
        StartCoroutine(Fade(0, 1f));
    }
    public void FadeOut()
    {
        StartCoroutine(Fade(1f, 0));
    }

    IEnumerator Fade(float start, float end)
    {
        float elapsed = 0f;

        Color color = _npcNameText.color;
        color.a = start;

        _npcNameText.color = color;

        while (elapsed < _fadeDuration)
        {
            elapsed += Time.deltaTime;
            color.a= Mathf.Lerp(start, end, elapsed / _fadeDuration);

            _npcNameText.color = color;

            yield return null;
        }

        color.a = end;
        _npcNameText.color = color;
    }
}
