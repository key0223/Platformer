using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class LoreObject : MonoBehaviour, IInteractable
{
    PopupPanel _popup;

    [SerializeField] int _loreStrId;
    [SerializeField] int _spellId;

    public GameObject Player { get; set; }
    [SerializeField] public bool CanInteract { get; set; }


    void Start()
    {
        _popup = UIManager.Instance.PopupPanel;
        Player = GameObject.FindGameObjectWithTag(TAG_PLAYER);
    }
    void Update()
    {
        if (CanInteract && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Interact();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == Player)
        {
            CanInteract = true;

        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            CanInteract = false;
        }
    }
    public void Interact()
    {
        //_popup.PopupInfoPanel();
        InputManager.Instance.TogglePopupInfo();
        _popup.InfoPanel.PopupUI(_loreStrId,_spellId);
    }

   
}
