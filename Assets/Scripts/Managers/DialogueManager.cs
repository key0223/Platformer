using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : SingletonMonobehaviour<DialogueManager>
{
    Action _onDialogueComplete;

    DialogueQueue _currentDialogueQueue;
    public DialogueQueue CurrentDialogueQueue { get { return _currentDialogueQueue; } }
    protected override void Awake()
    {
        base.Awake();
    }

    public void MakeDialogueQueue(string dialogueText, string npcName = null, Action onComplete = null)
    {
        UIManager.Instance.DialoguePanel.gameObject.SetActive(true);

        _onDialogueComplete = onComplete;

        DialogueQueue newQueue = new DialogueQueue();

        string[] dialogues = dialogueText.Split("#b"); // #b�� �������� string�� ������.

        foreach(string dialogue in dialogues)
        {
            newQueue.EnqueueDialogue(dialogue);
        }

        SetDialogueQueue(newQueue,npcName);
    }

    void SetDialogueQueue(DialogueQueue newQueue, string npcName = null)
    {
        _currentDialogueQueue = newQueue;

        StartDialogue();
    }

    public void StartDialogue()
    {
        string newDialogue = _currentDialogueQueue.DequeueDialogue();

        if(newDialogue != null)
        {
            // TODO : Display UI
            UIManager.Instance.DialoguePanel.StartTyping(newDialogue);
        }
    }

    public void EndDialogue()
    {
        UIManager.Instance.DialoguePanel.gameObject.SetActive(false);
        _onDialogueComplete?.Invoke();
        _onDialogueComplete = null;
    }
}
