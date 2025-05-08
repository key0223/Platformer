using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenchInteraction : InteractionBase
{
    PopupPanel _popup;

    public override void Start()
    {
        base.Start();
        _popup = UIManager.Instance.PopupPanel;
        _interactionType = Define.InteractionType.Rest;
    }

    public override void Interact()
    {
        UIManager.Instance.InvokeToggleCharmPanel();
    }
}
