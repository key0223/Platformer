using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPanel : MonoBehaviour
{
    [SerializeField] GameObject _backgroundPanel;
    [SerializeField] InventoryPanel _inventoryPanel;
    [SerializeField] CharmPanel _charmPanel;
    [SerializeField] InformationPanel _informationPanel;

    int _currentPopupPanel = 0;

    // Inventory
    List<PopupPanelBase> _panels = new List<PopupPanelBase>();

    HUDPanel _hud;

    public InventoryPanel InvenPanel { get { return _inventoryPanel; } }
    public CharmPanel CharmPanel { get { return _charmPanel; } }
    public InformationPanel InfoPanel { get { return _informationPanel; } }
    public List<PopupPanelBase> Panels{ get { return _panels; } }

    public int CurrentPopupPanel
    { 
        get { return _currentPopupPanel;} 
        set 
        {
            if (_currentPopupPanel == value) return;

           _currentPopupPanel=value;
            OnPanelChanged(value);
        }
    }


    void Start()
    {
        Init();
        InputManager.Instance.OnToggleInventory += ToggleInventory;
        InputManager.Instance.OnTogglePopupInfo += PopupInfoPanel;
    }

    void Init()
    {
        InitPanelList();

        _hud = UIManager.Instance.HUDPanel;

        _backgroundPanel.gameObject.SetActive(false);
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

    void OnPanelChanged(int panelIndex)
    {
        foreach(PopupPanelBase panel in _panels)
        {
            panel.gameObject.SetActive(false);
        }

        _panels[panelIndex].gameObject.SetActive(true);
    }

    void ToggleInventory()
    {
        _inventoryPanel.gameObject.GetComponent<InventoryPanel>().SetCoinText(InventoryManager.Instance.Coin);
        _hud.gameObject.SetActive(!_hud.gameObject.activeSelf);
        this.gameObject.SetActive(!this.gameObject.activeSelf);
        _backgroundPanel.gameObject.SetActive(!_backgroundPanel.gameObject.activeSelf);
        _inventoryPanel.gameObject.SetActive(!_inventoryPanel.gameObject.activeSelf);

        NotifyUIState();
    }

    public void PopupInfoPanel()
    {
        this.gameObject.SetActive(!this.gameObject.activeSelf);
        _backgroundPanel.gameObject.SetActive(false);
        _inventoryPanel.gameObject.SetActive(false);

        NotifyUIState();
    }
    void NotifyUIState()
    {
        bool isUIOn = this.gameObject.activeSelf;
        InputManager.Instance.UIStateChanged(isUIOn);
    }
}
