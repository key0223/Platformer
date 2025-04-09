using Data;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_DialogueList", menuName = "Scriptable Objects/Dialogue List")]
public class SO_DialogueList : ScriptableObject, ILoader<string, DialogueNode>
{
    [SerializeField]
    public List<DialogueNode> iseldaDialogues;

    public Dictionary<string, DialogueNode> MakeDict()
    {
        Dictionary<string,DialogueNode> dict = new Dictionary<string, DialogueNode>();

        foreach(DialogueNode dialogue in iseldaDialogues)
        {
            dict.Add(dialogue.nodeId, dialogue);
        }

        return dict;
    }
}
