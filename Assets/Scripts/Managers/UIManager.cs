using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonobehaviour<UIManager>
{
    [SerializeField] HUDPanel _hudUIPanel;
    [SerializeField] PopupPanel _popupUIPanel;
    [SerializeField] DialoguePanel _dialoguePanel;
    [SerializeField] InteractionStartUI _interactionStartUI;

    public HUDPanel HUDPanel { get { return _hudUIPanel; } }
    public PopupPanel PopupPanel { get { return _popupUIPanel; } }

    public DialoguePanel DialoguePanel {  get { return _dialoguePanel; } }

    // World space UI
    public InteractionStartUI InteractionStartUI { get { return _interactionStartUI; } }
    protected override void Awake()
    {
        base.Awake();
    }
}
