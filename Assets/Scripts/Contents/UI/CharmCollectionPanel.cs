using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharmCollectionPanel : MonoBehaviour
{
    [SerializeField] Image _charmImage;
    [SerializeField] TextMeshProUGUI _charmNameText;

    Coroutine _coTextSequence;

    public void SetUI(int charmId)
    {
        if (_coTextSequence != null)
        {
            StopCoroutine(_coTextSequence);
            _coTextSequence = null;
            return;
        }

        ItemData itemData = DataManager.Instance.GetItemData(charmId);

        if (itemData !=null)
        {
            CharmData charmData = itemData as CharmData;

            _charmImage.sprite = charmData.charmSprite;
            _charmNameText.text = charmData.itemName;
        }
    }
}
