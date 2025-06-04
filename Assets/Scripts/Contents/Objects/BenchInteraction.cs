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

    public override void Update()
    {
        if (UIManager.Instance.IsAnyUIOpen)
            return;

        base.Update();
    }
    public override void Interact()
    {
        UIManager.Instance.ToggleUI(Define.UIType.Charm);

        //if(MapManager.Instance.CurrentMiniMap !=null)
        //{
        //    MapManager.Instance.CurrentMiniMap.MiniMapUI.UpdateMiniMap();
        //}
    }
}
