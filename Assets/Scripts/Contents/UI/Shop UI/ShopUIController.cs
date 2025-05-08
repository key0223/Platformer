using Data;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;
public class ShopUIController : MonoBehaviour
{

    [Header("Shop Item Data")]
    [SerializeField] SO_ShopList _shopItemList;

    [Space(10f)]
    [SerializeField] RectTransform _contentRect;

    [Space(10f)]
    [Header("UI")]
    [SerializeField] TextMeshProUGUI _itemNameText;
    [SerializeField] TextMeshProUGUI _itemDescText;

    Dictionary<int, ShopData> _shopItemDict;
    List<ShopItem> _itemList = new List<ShopItem>();

    [SerializeField] float _slotHeight = 0;
    [SerializeField] float _rollDuration = 0;
    [SerializeField] int _slotCount = 10;

    [SerializeField] int _currentSlot = 0;
    bool _isAnimating = false;

    void Start()
    {
        Init();
    }

    void OnEnable()
    {
        InputManager.Instance.UIStateChanged(true);
        InputManager.Instance.IsAnyUIOn = true;
    }

    void OnDisable()
    {
        InputManager.Instance.UIStateChanged(false);
        InputManager.Instance.IsAnyUIOn = false;
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
        else if(Input.GetKeyDown(KeyCode.A))
        {
            this.gameObject.SetActive(false);
        }
        else if(Input.GetKeyDown(KeyCode.X))
        {
            if (_itemList[_currentSlot].IsPurchased) return;
            bool purchased = InventoryManager.Instance.SpendCoin(_itemList[_currentSlot].ItemCost);

            if (purchased)
            {
                _itemList[_currentSlot].IsPurchased = true;
                Item purchasedItem = Item.MakeItem(_itemList[_currentSlot].ItemId);
                InventoryManager.Instance.AddItem(purchasedItem);

                if(purchasedItem.ItemType == ItemType.Map)
                {
                    MapManager.Instance.OnMiniMapPurchased(purchasedItem);
                }
            }
        }

    }
    
    void Init()
    {
        _slotCount = _shopItemList.itemList.Count;
        CreateItemDict();
        CreateSlots();
        MoveToSlot(_currentSlot, animation: false);
    }
    void CreateItemDict()
    {
        _shopItemDict = new Dictionary<int, ShopData>();
        foreach(ShopData shopData in _shopItemList.itemList)
        {
            _shopItemDict.Add(shopData.itemId, shopData);
        }
    }
    void CreateSlots()
    {
        for (int i = 0; i < _slotCount; i++)
        {
            ShopItem shopItem = ResourceManager.Instance.Instantiate("UI/Shop Item", _contentRect).GetComponent<ShopItem>();
            int itemId = _shopItemList.itemList[i].itemId;
            shopItem.Init(itemId, _shopItemDict[itemId].price);

            _itemList.Add(shopItem);

            GameObject itemObj = shopItem.gameObject;

            RectTransform rect = itemObj.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1f);
            rect.anchorMax = new Vector2(0.5f, 1f);
            rect.pivot = new Vector2(0.5f, 1f);

            // Áß¾Ó¿¡¼­ ÇÑ Ä­ À§·Î Á¤·Ä
            rect.anchoredPosition = new Vector2(0, -i * _slotHeight);
        }

    }
    void MoveToSlot(int index, bool animation = true)
    {
        int itemId = _itemList[index].ItemId;

        _itemNameText.text = DataManager.Instance.GetItemData(itemId).itemName;
        _itemDescText.text = _shopItemDict[itemId].npcItemDesc;

        float centerOffset = (_contentRect.parent as RectTransform).rect.height / 2f - _slotHeight / 2f;

        // ½½·ÔÀÌ Áß¾Óº¸´Ù ÇÑ Ä­ À§¿¡ ¿Àµµ·Ï
        float targetY = index * _slotHeight - centerOffset + _slotHeight;

        if (animation)
        {
            _isAnimating = true;
            _contentRect.DOAnchorPosY(targetY, _rollDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => _isAnimating = false);
        }
        else
        {
            _contentRect.anchoredPosition = new Vector2(0, targetY);
        }
    }
}
