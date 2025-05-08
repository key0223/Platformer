using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Npc_IseldaController : NpcControllerBase
{
    [SerializeField] GameObject _shopUI;

    Npc_IseldaAnimation _anim;

    Dir _playerDir = Dir.Left;
    public Dir PlayerDir
    { 
        get { return _playerDir; }
        set 
        { 
            if(_playerDir == value) return;
            Debug.Log($"PlayerDir changed: {_playerDir} ¡æ {value}");
            _playerDir = value;
            _anim.UpdateDirection();
        }
    }

    #region State Parameters
    public bool IsTalking { get { return _isTalking; } set { _isTalking = value; } } // for animation
    public bool IsPlayerOnTheRight => _playerDir == Dir.Right;

    public bool IsUIOn => _shopUI.activeSelf;

    #endregion

    Coroutine _coIdle;
    void Awake()
    {
        _anim = GetComponent<Npc_IseldaAnimation>();
        _interactionType = InteractionType.Shop;
    }
    public override void Update()
    {

        if (CanInteract && Input.GetKeyDown(KeyCode.UpArrow) && !_isTalking && !IsUIOn && !UIManager.Instance.IsInvenUIOn)
        {
            Interact();
            UIManager.Instance.InteractionStartUI.FadeOut();
        }
        Vector2 dir = Player.transform.position - transform.position;

        if(dir.x >0)
        {
            PlayerDir = Dir.Right;
        }
        else if(dir.x <0)
        {
            PlayerDir = Dir.Left;
        }

        if(_coIdle==null && (!IsTalking|| IsPlayerOnTheRight))
        {
            _coIdle = StartCoroutine(CoIdle());
        }
    }
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }
    public override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
    public override void Interact()
    {
        if (_isFirstMeet)
        {
            _isFirstMeet = false;

            string dialogue = DataManager.Instance.DialogueDict["Iselda_first_meet"].dialogueText;

            DialogueManager.Instance.MakeDialogueQueue(dialogue,_npcName,
                ()=> 
                { 
                    _shopUI.SetActive(!_shopUI.activeSelf);
                    IsTalking = false;
                }
                );

            IsTalking = true;

            UIManager.Instance.InvokeUIStateChanged(true);
            UIManager.Instance.IsAnyUIOn = true;

        }
        else
        {
            _shopUI.SetActive(!_shopUI.activeSelf);
        }

    }

    IEnumerator CoIdle()
    {
        _anim.StartedIdle = true;

        int randPause = Random.Range(0, 7);

        yield return new WaitForSeconds(randPause);

        _coIdle = null;
    }
}
