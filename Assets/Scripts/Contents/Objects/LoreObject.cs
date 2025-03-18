using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class LoreObject : MonoBehaviour, IInteractable
{
    PopupPanel _popup;

    [SerializeField] InfoPanelType _infoPanelType;
    [SerializeField] int _loreStrId;
    [SerializeField] int _spellId;

    [SerializeField] bool _isPlayerNear = false;


    void Start()
    {
        _popup = UIManager.Instance.PopupPanel;
    }
    void Update()
    {
        if (_isPlayerNear && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Interact();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(TAG_PLAYER))
        {
            _isPlayerNear = true;

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(TAG_PLAYER))
        {
            _isPlayerNear = false;
        }
    }
    public void Interact()
    {
        //_popup.PopupInfoPanel();
        InputManager.Instance.TogglePopupInfo();
        _popup.InfoPanel.PopupUI(_infoPanelType, _loreStrId,_spellId);
    }

   
}
