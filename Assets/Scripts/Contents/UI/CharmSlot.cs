using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Define;

public class CharmSlot : Slot
{
    [SerializeField] int _slotIndex;
    [SerializeField] CharmSlotType _slotType;
    [SerializeField] bool _isEquipped = false;

    [Space(10f)]
    [SerializeField] Image _charmIconImage;

    public int SlotIndex { get { return _slotIndex; } set { _slotIndex = value; } }
    public CharmSlotType SlotType { get { return _slotType; } }
    public bool IsEquipped { get { return _isEquipped; } }

    public void SetSlot(Charm charm)
    {
        if(charm == null)
        {
            ItemId = 0;
            _isEquipped = false;

            _charmIconImage.gameObject.SetActive(_isEquipped);
        }
        else
        {
            ItemId = charm.ItemId;
            _isEquipped = charm.Equipped;

            ItemData itemData  = DataManager.Instance.GetItemData(ItemId);
            CharmData charmData = itemData as CharmData;
            if(charmData != null)
            {
                // TODO: Slot UI Setting
                _charmIconImage.sprite = charmData.charmSprite;
                _charmIconImage.gameObject.SetActive(true);
            }
        }
    }
}
