using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : SingletonMonobehaviour<InputManager>
{
    public event Action<bool> OnUIStateChanged;
    public event Action OnToggleInventory;

    bool _isUIOn = false;
    protected override void Awake()
    {
        base.Awake();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            _isUIOn = !_isUIOn;
            UIStateChanged(_isUIOn);
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
}
