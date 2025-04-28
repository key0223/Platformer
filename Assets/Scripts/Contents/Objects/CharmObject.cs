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
        _interactionType = Define.InteractionType.Examine;
    }

    public override void Interact()
    {
        int itemId = GetComponent<ItemObject>().ItemId;

        InputManager.Instance.TogglePopupInfo();
        _popup.InfoPanel.PopupUI(itemId);

        Item item = Item.MakeItem(itemId);
        InventoryManager.Instance.AddItem(item);

        ResourceManager.Instance.Destroy(gameObject);
    }
}
