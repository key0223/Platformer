using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonobehaviour<InventoryManager>
{
    [SerializeField] HUDPanel HUD;
    #region Event
    public event Action<int> OnCoinChanged;
    #endregion
    int _coin;
    public int Coin
    {
        get { return _coin; }
        set
        {
            if (_coin == value) return;

            _coin = value;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    #region Coin
    public void AddCoin(int amount)
    {
        if(amount>0)
        {
            Coin += amount;
            OnCoinChanged?.Invoke(amount);
        }
    }
    public bool SpendCoin(int amount)
    {
        if(amount>0 && Coin>= amount)
        {
            Coin -= amount;
            return true;
        }
        return false;
    }
    #endregion
}
