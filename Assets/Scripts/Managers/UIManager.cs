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
    public event Action OnToggleMiniMap;
    public event Action OnTogglePopupInfo;

    #endregion
    public HUDPanel HUDPanel { get { return _hudUIPanel; } }
    public PopupPanel PopupPanel { get { return _popupUIPanel; } }

    public DialoguePanel DialoguePanel {  get { return _dialoguePanel; } }

    // World space UI
    public InteractionStartUI InteractionStartUI { get { return _interactionStartUI; } }

    #region UI state

    bool _isInvenUIOn = false;
    
    public bool IsInvenUIOn { get {  return _isInvenUIOn; } }
    public bool IsAnyUIOn { get; set; } = false;
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
        if(Input.GetKeyDown(_inventory) && !IsAnyUIOn)
        {
            _isInvenUIOn = !_isInvenUIOn;
            InvokeToggleInventory();
        }
        if(Input.GetKeyDown(_miniMap) && !IsAnyUIOn)
        {
            IsAnyUIOn = true;
            InvokeToggleMiniMap();
        }
        if (Input.GetKeyUp(_miniMap))
        {
            IsAnyUIOn = false;
            InvokeToggleMiniMap();
        }
    }

    #region Callbacks

    public void InvokeUIStateChanged(bool state)
    {
        OnUIStateChanged?.Invoke(state);
    }
    public void InvokeToggleInventory()
    {
        OnToggleInventory?.Invoke();
    }
    public void InvokeToggleCharmPanel()
    {
        OnToggleCharmPanel?.Invoke();
    }
    public void InvokePopupInfo()
    {
        OnTogglePopupInfo?.Invoke();
    }
    public void InvokeToggleMiniMap()
    {
        OnToggleMiniMap?.Invoke();
    }
    #endregion
}
