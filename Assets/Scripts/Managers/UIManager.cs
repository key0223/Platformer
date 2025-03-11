using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [SerializeField] HUDPanel _hudUIPanel;
    [SerializeField] PopupPanel _popupUIPanel;

    public HUDPanel HUDPanel { get { return _hudUIPanel; } }
    public PopupPanel PopupPanel { get { return _popupUIPanel; } }
    

    protected override void Awake()
    {
        base.Awake();
    }
}
