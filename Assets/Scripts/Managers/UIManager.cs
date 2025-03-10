using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [SerializeField] GameObject _hudUIParent;
    [Header("Popup UI")]
    [SerializeField] GameObject _popupUIParent;
    [Space(10f)]
    [SerializeField] InventoryPanel _inventoryPanel;

    public InventoryPanel InvenPanel { get { return _inventoryPanel; } }
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
        _inventoryPanel.gameObject.SetActive(!_inventoryPanel.gameObject.activeSelf);
        NotifyUIState();
    }

    void NotifyUIState()
    {
        bool isUIOn = _popupUIParent.activeSelf;
        InputManager.Instance.UIStateChanged(isUIOn);
    }
}
