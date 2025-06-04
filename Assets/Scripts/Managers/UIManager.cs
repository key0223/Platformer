using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    #region Inspector settings
    [SerializeField] HUDPanel _hudUIPanel;
    [SerializeField] PopupPanel _popupUIPanel;
    [SerializeField] DialoguePanel _dialoguePanel;
    [SerializeField] InteractionStartUI _interactionStartUI;
    #endregion

    public event Action<UIType, bool> OnUIToggled;

    public HUDPanel HUDPanel { get { return _hudUIPanel; } }
    public PopupPanel PopupPanel { get { return _popupUIPanel; } }

    public DialoguePanel DialoguePanel { get { return _dialoguePanel; } }

    // World space UI
    public InteractionStartUI InteractionStartUI { get { return _interactionStartUI; } }


    // UI states
    HashSet<UIType> _activePanels = new HashSet<UIType>();
    public bool IsAnyUIOpen { get { return _activePanels.Count > 0; } }

    // KeyCode
    KeyCode _inventory = KeyCode.I;
    KeyCode _miniMap = KeyCode.M;

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
        if (Input.GetKeyDown(_inventory))
        {
            ToggleUI(UIType.Inventory);
        }
        if (Input.GetKeyDown(_miniMap))
        {
            ToggleUI(UIType.MiniMap);
        }
    }

    public void ToggleUI(UIType type)
    {
        //  현재 UI를 닫을 것인가 열 것인가?
        bool isOpening = !_activePanels.Contains(type);
        if(isOpening)
            _activePanels.Add(type);
        else
            _activePanels.Remove(type);

        OnUIToggled?.Invoke(type, isOpening);

    }

    public bool IsOtherUIOpen(UIType except)
    {
        return _activePanels.Any(t=> t != except);
    }
}
