using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharmPanel : PopupPanelBase
{
    #region Charm Description
    [SerializeField] TextMeshProUGUI _itemNameText;
    [SerializeField] TextMeshProUGUI _charmCostText;
    [SerializeField] TextMeshProUGUI _itemDescText;
    [SerializeField] Image _itemIconImage;
    #endregion

    [Space(10f)]
    // Highlighter 
    [SerializeField] Transform _initPos;

    protected override void Init()
    {
        _highlighter.MoveToSlot(_initPos);

        InitItemDescUI();
    }

    protected override void MoveSelection(int horizontal, int vertical, bool sectionMove)
    {
        base.MoveSelection(horizontal, vertical, sectionMove);
        UpdateItemDescUI();
    }
    #region Item Description UI
    void UpdateItemDescUI()
    {
        Slot currentSlot = _sections[_currentSection]._rows[_currentRow]._cloumns[_currentColumn];

        CharmSlot equippedSlot = currentSlot as CharmSlot;
        if (equippedSlot != null)
        {
            if(!equippedSlot.IsEquipped)
            {
                _itemNameText.text = "";
                _charmCostText.text = "";

                _itemNameText.gameObject.SetActive(false);
                _charmCostText.gameObject.SetActive(false);
                _itemIconImage.gameObject.SetActive(false);

                _itemDescText.text = "장착된 부적이 없습니다. \n아래에서 부적을 선택하여 장착하고 그 효과를 활성화하십시오.";
            }
            else
            {
                ItemData data = DataManager.Instance.GetItemData(currentSlot.ItemId);

                if (data != null)
                {
                    _itemNameText.text = data.itemName;
                    _itemDescText.text = data.itemDescription;
                }
                else
                {
                    InitItemDescUI();
                }
            }
        }
        else
        {

        }
    }
    void InitItemDescUI()
    {
        _itemNameText.text = "";
        _charmCostText.text = "";
        _itemDescText.text = "";

        _charmCostText.gameObject.SetActive(false);
        _itemIconImage.gameObject.SetActive(false);
    }
    #endregion
}
