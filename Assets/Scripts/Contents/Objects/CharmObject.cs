using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class CharmObject : MonoBehaviour,IInteractable
{
    PopupPanel _popup;

    public GameObject Player { get; set; }
    [SerializeField]  public bool CanInteract { get; set; }


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
        if (collision.gameObject == Player)
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
        int itemId = GetComponent<ItemObject>().ItemId;

        InputManager.Instance.TogglePopupInfo();
        _popup.InfoPanel.PopupUI(itemId);

        Charm charm = new Charm();
        charm.Init(itemId,count:1);
        InventoryManager.Instance.AddItem(charm);

        ResourceManager.Instance.Destroy(gameObject);
    }
}
