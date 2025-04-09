using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcControllerBase : InteractionBase
{
    protected int _npcId;

    protected bool _isFirstMeet = true;
    protected bool _isTalking = false;
    public int NpcId { get { return _npcId; } }

    public override void Update()
    {
        if (CanInteract && Input.GetKeyDown(KeyCode.UpArrow) && !_isTalking)
        {
            Interact();
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

    }
}
