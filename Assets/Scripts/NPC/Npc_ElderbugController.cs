using Data;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Npc_ElderbugController : NpcControllerBase,ISavable
{
    Npc_ElderbugAnimation _anim;

    Dir _playerDir = Dir.Left;

    public Dir PlayerDir
    {
        get { return _playerDir; }
        set
        {
            if (_playerDir == value) return;
            _playerDir = value;
            _anim.UpdateDirection();
        }
    }

    #region State parameters

    bool _isIdle;
    public bool IsIdle { get { return _isIdle; } set { _isIdle = value; } }
    bool _hasPassed = false;
    bool _isPassingBy; // for animation
    public bool IsPassingBy { get { return _isPassingBy; } set { _isPassingBy = value; } }
    public bool IsTalking { get { return _isTalking; } set { _isTalking = value; } }
    public bool IsPlayerOnTheRight => _playerDir == Dir.Right;

    string _currentEvent = "";
    public string CurrentEvent { get { return _currentEvent; } set { _currentEvent = value; } }
    #endregion

    void Awake()
    {
        _anim = GetComponent<Npc_ElderbugAnimation>();
        _interactionType = Define.InteractionType.Listen;
        _isIdle = true;
    }

    void OnEnable()
    {
        RegisterSave();
    }
    void OnDisable()
    {
        DeregisterSave();
    }

    public override void Update()
    {
        if (CanInteract && Input.GetKeyDown(KeyCode.UpArrow) && !_isTalking && !UIManager.Instance.IsAnyUIOpen)
        {
            Interact();
            UIManager.Instance.InteractionStartUI.FadeOut();
        }

        Vector2 dir = Player.transform.position - transform.position;

        if (dir.x > 0)
        {
            PlayerDir = Dir.Right;
        }
        else if (dir.x < 0)
        {
            PlayerDir = Dir.Left;
        }

        if (IsPlayerOnTheRight && _isFirstMeet && dir.magnitude > 5)
        {
            _isPassingBy = true;
            _hasPassed = true;
            _isIdle = false;

        }
        else if (IsPlayerOnTheRight && _isFirstMeet && dir.magnitude < 3)
        {
            _isPassingBy = false;
            _isIdle = true;
        }
    }
    public override void Interact()
    {
        string dialogue = GetDialogue();

        DialogueManager.Instance.MakeDialogueQueue(dialogue, _npcName,
     () =>
     {
         IsTalking = false;
         IsIdle = true;
         UIManager.Instance.ToggleUI(UIType.Dialogue);

     });
        IsTalking = true;
        IsIdle = false;
        UIManager.Instance.ToggleUI(UIType.Dialogue);
    }

    string GetDialogue()
    {
        if(_isFirstMeet)
        {
            _isFirstMeet = false;
            string dialougueId;

            if (_hasPassed)
                dialougueId = "Elderbug_passingBy";
            else
                dialougueId = "Elderbug_first_meet";

            DialogueNode node = DataManager.Instance.DialogueDict[dialougueId];
            if (node.hasFollowingDialogue)
                _currentEvent = node.followingDialogueId;

            return node.dialogueText;
        }
        else
        {
            string dialougueId;

            if (_currentEvent == "")
                dialougueId = "Elderbug_talk_3";
            else
                dialougueId = _currentEvent;

            DialogueNode node = DataManager.Instance.DialogueDict[dialougueId];

            if (node.hasFollowingDialogue)
                _currentEvent = node.followingDialogueId;

            return node.dialogueText;

        }
    }

    #region Save
    public void RegisterSave()
    {
        Debug.Log("Elderbug Registered");
        SaveLoadManager.Instance.Register(this);
    }

    public void DeregisterSave()
    {
        SaveLoadManager.Instance.Deregister(this);
    }

    public object CaptureData()
    {
        NpcProgressData data = new NpcProgressData();
        data.npcId = _npcId;
        data.hasMet = !_isFirstMeet;

        return data;
    }

    public void RestoreData(object loadedata)
    {
        Dictionary<int, NpcProgressData> npcDict = loadedata as Dictionary<int, NpcProgressData>;
        if(npcDict.TryGetValue(_npcId,out NpcProgressData data))
        {
            _isFirstMeet = !data.hasMet;
        }
     
    }
    #endregion
}
