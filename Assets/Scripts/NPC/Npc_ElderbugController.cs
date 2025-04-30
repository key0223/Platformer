using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Npc_ElderbugController : NpcControllerBase
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

    bool _hasPassed = false;
    bool _isPassingBy; // for animation
    public bool IsPassingBy { get { return _isPassingBy; } set { _isPassingBy = value; } }
    public bool IsTalking { get { return _isTalking; } set { _isTalking = value; } }
    public bool IsPlayerOnTheRight => _playerDir == Dir.Right;
    #endregion


    void Awake()
    {
        _anim = GetComponent<Npc_ElderbugAnimation>();
        _interactionType = Define.InteractionType.Listen;
        
    }

    public override void Update()
    {
        Vector2 dir = Player.transform.position - transform.position;

        if (dir.x > 0)
        {
            PlayerDir = Dir.Right;
        }
        else if (dir.x < 0)
        {
            PlayerDir = Dir.Left;
        }

        if(!IsTalking)
        {
            _anim.StartedIdle = true;
        }
        
        if(IsPlayerOnTheRight && _isFirstMeet && dir.magnitude >5)
        {
            _isPassingBy = true;
            _hasPassed = true;

        }
        else if(IsPlayerOnTheRight && _isFirstMeet && dir.magnitude < 3)
        {

            _isPassingBy = false;
        }
    }
    public override void Interact()
    {
        
        if(_isFirstMeet)
        {
            _isFirstMeet = false;

            if(_hasPassed)
            {
                string dialogue = DataManager.Instance.DialogueDict["Elderbug_passingBy"].dialogueText;
            }
            else
            {
                string dialogue = DataManager.Instance.DialogueDict["Elderbug_first_meet"].dialogueText;
            }
        }
    }
}
