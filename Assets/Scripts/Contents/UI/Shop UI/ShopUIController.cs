using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUIController : MonoBehaviour
{

    [SerializeField] RectTransform _contentRect;

    [SerializeField] float _slotHeight = 0;
    [SerializeField] float _rollDuration = 0;
    [SerializeField] int _slotCount = 10;

    int _currentSlot = 0;
    bool _isAnimating = false;

    void Start()
    {
        CreateSlots();
        MoveToSlot(_currentSlot, instant: true);
    }

    void Update()
    {
        if (_isAnimating) return;

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (_currentSlot < _slotCount - 1)
            {
                _currentSlot++;
                MoveToSlot(_currentSlot);
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (_currentSlot > 0)
            {
                _currentSlot--;
                MoveToSlot(_currentSlot);
            }
        }
    }
    void CreateSlots()
    {
        for (int i = 0; i < 10; i++)
        {

            GameObject slotObj = ResourceManager.Instance.Instantiate("UI/Shop Item",_contentRect);

            RectTransform rect = slotObj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);

            // Áß¾Ó¿¡¼­ ÇÑ Ä­ À§·Î Á¤·Ä
            rect.anchoredPosition = new Vector2(0, -i * _slotHeight);
        }

    }
    void MoveToSlot(int index, bool instant = false)
    {
        float centerOffset = (_contentRect.parent as RectTransform).rect.height / 2f - _slotHeight / 2f;

        // ½½·ÔÀÌ Áß¾Óº¸´Ù ÇÑ Ä­ À§¿¡ ¿Àµµ·Ï
        float targetY = index * _slotHeight - centerOffset + _slotHeight;

        if (instant)
        {
            _contentRect.anchoredPosition = new Vector2(0, targetY);
        }
        else
        {
            _isAnimating = true;
            _contentRect.DOAnchorPosY(targetY, _rollDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => _isAnimating = false);
        }
    }
}
