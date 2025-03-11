using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpellInfoPanel : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _loreText;
    [SerializeField] Image _descriptopnImage;
    [SerializeField] TextMeshProUGUI _descriptopnText;
    [SerializeField] TextMeshProUGUI _keyDescriptionText;
    [SerializeField] TextMeshProUGUI _keyText;

    public void SetUI(int loreStrId,int spellId)
    {
        StringData strData = DataManager.Instance.GetStringData(loreStrId);
        ItemData itemData = DataManager.Instance.GetItemData(spellId);

        if(strData !=null && itemData !=null)
        {
            SpellData spellData = itemData as SpellData;

            _loreText.text = strData.ko;
            _descriptopnImage.sprite = spellData.DecscrtiptionSprite;
            _descriptopnText.text = spellData.itemDescription;
            _keyDescriptionText.text = spellData.keyDescription;
            _keyText.text = spellData.inputKey;

        }
    }
}
