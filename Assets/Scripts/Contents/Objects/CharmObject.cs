using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CharmObject : MonoBehaviour,IInteractable
{
    PopupPanel _popup;

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
        if (collision.CompareTag(TAG_PLAYER))
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
        int itemId = GetComponent<ItemObject>().ItemId;

        InputManager.Instance.TogglePopupInfo();
        _popup.InfoPanel.PopupUI(itemId);

        Charm charm = new Charm();
        charm.Init(itemId,count:1);
        InventoryManager.Instance.AddItem(charm);

        ResourceManager.Instance.Destroy(gameObject);
    }
}
