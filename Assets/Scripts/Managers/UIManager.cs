using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [SerializeField] GameObject _hudUIParent;
    [Header("Popup UI")]
    [SerializeField] GameObject _popupUIParent;
    [Space(10f)]
    [SerializeField] GameObject _inventoryPanel;

    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        InputManager.Instance.OnToggleInventory += ToggleInventory;
    }

    void ToggleInventory()
    {
        _inventoryPanel.gameObject.GetComponent<InventoryPanel>().SetCoinText(InventoryManager.Instance.Coin);
        _hudUIParent.SetActive(!_hudUIParent.gameObject.activeSelf);
        _popupUIParent.SetActive(!_popupUIParent.activeSelf);
        _inventoryPanel.SetActive(!_inventoryPanel.activeSelf);
        NotifyUIState();
    }

    void NotifyUIState()
    {
        bool isUIOn = _popupUIParent.activeSelf;
        InputManager.Instance.UIStateChanged(isUIOn);
    }
}
