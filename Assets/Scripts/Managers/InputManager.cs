using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonobehaviour<InputManager>
{
    public event Action<bool> OnUIStateChanged;
    public event Action OnToggleInventory;
    public event Action OnToggleCharmPanel;
    public event Action OnTogglePopupInfo;
    public event Action OnToggleMiniMap;

    public bool IsAnyUIOn { get; set; } = false;

    [SerializeField] bool _isInvenUIOn = false;

    public bool IsInvenUIOn { get {  return _isInvenUIOn; } }
    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !IsAnyUIOn)
        {
            _isInvenUIOn = !_isInvenUIOn;
            UIStateChanged(_isInvenUIOn);
            ToggleInventory();
        }
        
        if(Input.GetKeyDown(KeyCode.M) && !IsAnyUIOn)
        {
            ToggleMiniMap();
            IsAnyUIOn = true;
        }
        if(Input.GetKeyUp(KeyCode.M))
        {
            ToggleMiniMap();
            IsAnyUIOn = false;
        }
    }

    public void UIStateChanged(bool state)
    {
        OnUIStateChanged?.Invoke(state);
    }
   
    public void ToggleInventory()
    {
        OnToggleInventory?.Invoke();
    }
    public void ToggleCharmPanel()
    {
        OnToggleCharmPanel?.Invoke();
    }
    public void TogglePopupInfo()
    {
        OnTogglePopupInfo?.Invoke();
    }
    
    public void ToggleMiniMap()
    {
        OnToggleMiniMap?.Invoke();
    }
}

