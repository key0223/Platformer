using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharmCollectionPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] Image _bottomLineImage;

    [SerializeField] Image _charmImage;
    [SerializeField] TextMeshProUGUI _charmNameText;

    [SerializeField] TextMeshProUGUI _descText;
    [SerializeField] Image _descImage;
    [SerializeField] TextMeshProUGUI _desc2Text;

    InformationPanel _infoPanel;
    Coroutine _coTextSequence;
    private void Awake()
    {
        _infoPanel = GetComponentInParent<InformationPanel>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_coTextSequence != null)
            {
                StopCoroutine(_coTextSequence);
                _coTextSequence = null;

            }

            gameObject.SetActive(false);
            _infoPanel.gameObject.SetActive(false);

            InputManager.Instance.TogglePopupInfo();
        }
    }
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

            SetUIAlpha(0);
            _coTextSequence = StartCoroutine(CoTextSequence());
        }
    }

    IEnumerator CoTextSequence()
    {
       StartCoroutine(UIEffect.CoTextFadeIn(_titleText));
        yield return StartCoroutine(UIEffect.CoImageFadeIn(_bottomLineImage));

        StartCoroutine(UIEffect.CoImageFadeIn(_charmImage));
        yield return StartCoroutine(UIEffect.CoTextFadeIn(_charmNameText));

        yield return StartCoroutine(UIEffect.CoTextFadeIn(_descText));

        yield return StartCoroutine(UIEffect.CoImageFadeIn(_descImage));

        yield return StartCoroutine(UIEffect.CoTextFadeIn(_desc2Text));

    }

    void SetUIAlpha(float value)
    {
        Color titleTextColor = _titleText.color;
        Color bottomLineColor = _bottomLineImage.color;
        Color charmImageColor = _charmImage.color;
        Color charmNameColor = _charmNameText.color;
        Color descColor = _descText.color;
        Color descImageColor = _descImage.color;
        Color desc2Color = _desc2Text.color;

        titleTextColor.a = value;
        bottomLineColor.a = value;
        charmImageColor.a = value;
        charmNameColor.a = value;
        descColor.a = value;
        descImageColor.a = value;
        desc2Color.a = value;

        _titleText.color = titleTextColor;
        _bottomLineImage.color = bottomLineColor;
        _charmImage.color = charmImageColor;
        _charmNameText.color = charmNameColor;
        _descText.color = descColor;
        _descImage.color = descImageColor;
        _desc2Text.color = desc2Color;
    }
}
