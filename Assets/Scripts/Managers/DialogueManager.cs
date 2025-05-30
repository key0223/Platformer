using System;

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
        UIManager.Instance.DialoguePanel.SetNpcNameText(npcName);
        UIManager.Instance.DialoguePanel.FadeIn();

        _onDialogueComplete = onComplete;

        DialogueQueue newQueue = new DialogueQueue();

        string[] dialogues = dialogueText.Split("#b"); // #b를 기준으로 string을 나눈다.

        foreach(string dialogue in dialogues)
        {
            newQueue.EnqueueDialogue(dialogue);
        }

        SetDialogueQueue(newQueue);
    }

    void SetDialogueQueue(DialogueQueue newQueue)
    {
        _currentDialogueQueue = newQueue;

        StartDialogue();
    }

    public void StartDialogue()
    {
        string newDialogue = _currentDialogueQueue.DequeueDialogue();

        if(newDialogue != null)
        {
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
