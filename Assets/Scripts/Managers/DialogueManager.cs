using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : SingletonMonobehaviour<DialogueManager>
{
    DialogueQueue _currentDialogueQueue;
    public DialogueQueue CurrentDialogueQueue { get { return _currentDialogueQueue; } }
    protected override void Awake()
    {
        base.Awake();
    }

    public void MakeDialogueQueue(string dialogueText, string npcName = null)
    {
        DialogueQueue newQueue = new DialogueQueue();

        string[] dialogues = dialogueText.Split("#b"); // b를 기준으로 string을 나눈다.

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
}
