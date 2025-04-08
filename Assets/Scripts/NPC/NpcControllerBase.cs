using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcControllerBase : InteractionBase
{

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
