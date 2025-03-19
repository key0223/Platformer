using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SpellInfoPanel : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _loreText;
    [SerializeField] Image _descriptopnImage;
    [SerializeField] TextMeshProUGUI _descriptopnText;
    [SerializeField] TextMeshProUGUI _keyDescriptionText;
    [SerializeField] Image _keyButtonImage;
    [SerializeField] TextMeshProUGUI _keyText;

    Coroutine _coTextSequence;
    public void SetUI(int loreStrId,int spellId)
    {
        if (_coTextSequence != null)
        {
            StopCoroutine(_coTextSequence);
            _coTextSequence = null;
            return;
        }

        StringData strData = DataManager.Instance.GetStringData(loreStrId);
        ItemData itemData = DataManager.Instance.GetItemData(spellId);

        if(strData !=null && itemData !=null)
        {
            SpellData spellData = itemData as SpellData;

            _descriptopnImage.sprite = spellData.decscrtiptionSprite;
            _descriptopnText.text = spellData.itemDescription;
            _keyDescriptionText.text = spellData.keyDescription;
            _keyText.text = spellData.inputKey;

            SetUIAlpha(0);
          
            _coTextSequence = StartCoroutine(CoTextSequence(strData, spellData));
        }
    }

  
    IEnumerator CoTextSequence(StringData strData, SpellData spellData)
    {
        yield return StartCoroutine(UIEffect.CoTyping(_loreText, strData.ko));

        yield return StartCoroutine(UIEffect.CoImageFadeIn(_descriptopnImage));
        yield return StartCoroutine(UIEffect.CoTextFadeIn(_descriptopnText));
        
        StartCoroutine(UIEffect.CoTextFadeIn(_keyDescriptionText));
        StartCoroutine(UIEffect.CoImageFadeIn(_keyButtonImage));
        StartCoroutine(UIEffect.CoTextFadeIn(_keyText));
    }

    void SetUIAlpha(float value)
    {
        Color descImageColor = _descriptopnImage.color;
        Color descTextColor = _descriptopnText.color;
        Color keyDescTextColor = _keyDescriptionText.color;
        Color keyBtnColor = _keyButtonImage.color;
        Color keyTextColor = _keyText.color;

        descImageColor.a = value;
        descTextColor.a = value;
        keyDescTextColor.a = value;
        keyBtnColor.a = value;
        keyTextColor.a = value;

        _descriptopnImage.color = descImageColor;
        _descriptopnText.color = descTextColor;
        _keyDescriptionText.color = keyDescTextColor;
        _keyButtonImage.color = keyBtnColor;
        _keyText.color = keyTextColor;
    }
}
