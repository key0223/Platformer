using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    #region Inspector settings
    [SerializeField] HUDPanel _hudUIPanel;
    [SerializeField] PopupPanel _popupUIPanel;
    [SerializeField] DialoguePanel _dialoguePanel;
    [SerializeField] InteractionStartUI _interactionStartUI;
    #endregion

    #region Events
    public event Action<bool> OnUIStateChanged;
    public event Action OnToggleInventory;
    public event Action OnToggleCharmPanel;
    public event Action<bool> OnToggleMiniMap;
    public event Action OnTogglePopupInfo;

    #endregion
    public HUDPanel HUDPanel { get { return _hudUIPanel; } }
    public PopupPanel PopupPanel { get { return _popupUIPanel; } }

    public DialoguePanel DialoguePanel { get { return _dialoguePanel; } }

    // World space UI
    public InteractionStartUI InteractionStartUI { get { return _interactionStartUI; } }

    #region UI state

    bool _isUIOn = false;


    HashSet<string> _onPanels = new HashSet<string>();
    #endregion

    #region KeyCode

    KeyCode _inventory = KeyCode.I;
    KeyCode _miniMap = KeyCode.M;

    #endregion
    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        HandleInput();
    }
    void HandleInput()
    {
        if (Input.GetKeyDown(_inventory) && !IsAnyUIOn("Inventory"))
        {
            InvokeToggleInventory();
        }
        if (Input.GetKeyDown(_miniMap) && !IsAnyUIOn("MiniMap"))
        {
            InvokeToggleMiniMap();
        }
    }

    public bool IsAnyUIOn(string exceptPanelName = null)
    {
        if (_onPanels.Count == 0) return false;

        foreach (string panel in _onPanels)
        {
            if (panel != exceptPanelName)
                return true;
        }
        return false;
    }

    #region Callbacks

    public void UpdateUIStateChanged(string panelName, bool isOn)
    {
        if (isOn)
            _onPanels.Add(panelName);
        else
            _onPanels.Remove(panelName);

        OnUIStateChanged?.Invoke(isOn);
    }

    public void InvokeToggleInventory()
    {
        _isUIOn = !_isUIOn;
        OnToggleInventory?.Invoke();
        UpdateUIStateChanged("Inventory", _isUIOn);
    }
    public void InvokeToggleCharmPanel()
    {
        _isUIOn = !_isUIOn;
        UpdateUIStateChanged("Inventory", _isUIOn);
        OnToggleCharmPanel?.Invoke();
    }
    public void InvokePopupInfo()
    {
        OnTogglePopupInfo?.Invoke();
    }
    public void InvokeToggleMiniMap()
    {
        _isUIOn = !_isUIOn;
        OnToggleMiniMap?.Invoke(_isUIOn);
        UpdateUIStateChanged("MiniMap", _isUIOn);
    }
    #endregion
}
