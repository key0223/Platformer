using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueQueue
{
    Queue<string> _dialogueQueue = new Queue<string>();

    public void EnqueueDialogue(string dialogue)
    {
        _dialogueQueue.Enqueue(dialogue);
    }

    public string DequeueDialogue()
    {
        if(_dialogueQueue.Count > 0)
            return _dialogueQueue.Dequeue();
        return null;
    }

    public bool HasNextDialogue()
    {
        return _dialogueQueue.Count > 0;
    }
}
