using Data;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO_DialogueList", menuName = "Scriptable Objects/Dialogue List")]
public class SO_DialogueList : ScriptableObject, ILoader<int, DialogueNode>
{
    [SerializeField]
    public List<DialogueNode> iseldaDialogues;

    public Dictionary<int, DialogueNode> MakeDict()
    {
        Dictionary<int,DialogueNode> dict = new Dictionary<int, DialogueNode>();

        foreach(DialogueNode dialogue in iseldaDialogues)
        {
            dict.Add(dialogue.nodeId, dialogue);
        }

        return dict;
    }
}
