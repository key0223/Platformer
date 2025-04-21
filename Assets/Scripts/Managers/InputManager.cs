using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonobehaviour<InputManager>
{
    public event Action<bool> OnUIStateChanged;
    public event Action OnToggleInventory;
    public event Action OnTogglePopupInfo;

    public bool IsAnyUIOn { get; set; } = false;

    bool _isInvenUIOn = false;
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
    }

    public void UIStateChanged(bool state)
    {
        OnUIStateChanged?.Invoke(state);
    }
   
    public void ToggleInventory()
    {
        OnToggleInventory?.Invoke();
    }
    public void TogglePopupInfo()
    {
        OnTogglePopupInfo?.Invoke();
    }
}

