using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoreObject : InteractionBase
{
    PopupPanel _popup;

    [SerializeField] int _loreStrId;
    [SerializeField] int _spellId;
  

    public override void Start()
    {
        base.Start();
        _popup = UIManager.Instance.PopupPanel;
        _interactionType = Define.InteractionType.Examine;
    }
   
    public override void Interact()
    {
        //_popup.PopupInfoPanel();
        UIManager.Instance.ToggleUI(Define.UIType.PopupInfo);
        _popup.InfoPanel.PopupUI(_loreStrId,_spellId);
    }
}
