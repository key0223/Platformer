using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharmObject : InteractionBase
{
    PopupPanel _popup;

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
        int itemId = GetComponent<ItemObject>().ItemId;

        InputManager.Instance.TogglePopupInfo();
        _popup.InfoPanel.PopupUI(itemId);

        Charm charm = new Charm();
        charm.Init(itemId,count:1);
        InventoryManager.Instance.AddItem(charm);

        ResourceManager.Instance.Destroy(gameObject);
    }
}
