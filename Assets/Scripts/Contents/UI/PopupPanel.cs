using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPanel : MonoBehaviour
{
    HUDPanel _hud;
    // Inventory
    List<PopupPanelBase> _panels = new List<PopupPanelBase>();

    //int _currentPanelIndex = 0;

    [SerializeField] InventoryPanel _inventoryPanel;

    public InventoryPanel InvenPanel { get { return _inventoryPanel; } }



    void Start()
    {
        Init();
        InputManager.Instance.OnToggleInventory += ToggleInventory;
    }

    void Init()
    {
        InitPanelList();

        _hud = UIManager.Instance.HUDPanel;

        gameObject.SetActive(false);
    }
    void InitPanelList()
    {
        PopupPanelBase[] panels = GetComponentsInChildren<PopupPanelBase>();

        foreach (PopupPanelBase panel in panels)
        {
            _panels.Add(panel);
            panel.gameObject.SetActive(false);

        }
    }

    void ToggleInventory()
    {
        _inventoryPanel.gameObject.GetComponent<InventoryPanel>().SetCoinText(InventoryManager.Instance.Coin);
        _hud.gameObject.SetActive(!_hud.gameObject.activeSelf);
        this.gameObject.SetActive(!this.gameObject.activeSelf);
        _inventoryPanel.gameObject.SetActive(!_inventoryPanel.gameObject.activeSelf);

        NotifyUIState();
    }

    void NotifyUIState()
    {
        bool isUIOn = this.gameObject.activeSelf;
        InputManager.Instance.UIStateChanged(isUIOn);
    }
}
