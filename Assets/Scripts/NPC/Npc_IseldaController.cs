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
            Debug.Log($"PlayerDir changed: {_playerDir} → {value}");
            _playerDir = value;
            _anim.UpdateDirection();
        }
    }

    #region State Parameters
    public bool IsTalking { get; private set; }
    public bool IsPlayerOnTheRight => _playerDir == Dir.Right;
    #endregion

    Coroutine _coIdle;
    void Awake()
    {
        _anim = GetComponent<Npc_IseldaAnimation>();
    }
    public override void Update()
    {
        base.Update();
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
            string dialogue = DataManager.Instance.DialogueDict["first_meet"].dialogueText;
            // Npc 이름 UI 추가 

            UIManager.Instance.DialoguePanel.gameObject.SetActive(true);
            DialogueManager.Instance.MakeDialogueQueue(dialogue);

            IsTalking = true;
            // TODO: 대화가 끝나면 shop UI active
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

    /*
    string FindDialogue()
    {
        string foundDialogue = null;

        foreach(DialogueNode dialogue in DataManager.Instance.DialogueDict.Values)
        {
            string[] parts = dialogue.conditionKey.Split(':');
            if (parts.Length < 2)
                continue;

            string conditionType = parts[0];
            string conditionValue = parts[1];

            switch (conditionType)
            {
                case "conversation":
                    {
                        int requiredCount = int.Parse(conditionValue);
                        if (_conversationCount == requiredCount)
                            foundDialogue = dialogue.dialogueText;
                        break;
                    }
            }
        }
        return foundDialogue;
    }
    */
}
