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
    }
    void Update()
    {
        if (CanInteract && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Interact();
        }
    }
   
    public override void Interact()
    {
        //_popup.PopupInfoPanel();
        InputManager.Instance.TogglePopupInfo();
        _popup.InfoPanel.PopupUI(_loreStrId,_spellId);
    }

   
}
