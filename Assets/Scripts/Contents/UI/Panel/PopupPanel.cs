using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Define;

public class PopupPanel : MonoBehaviour
{
    [Header("Panel Info")]
    [SerializeField] TextMeshProUGUI _titleText;
    [SerializeField] TextMeshProUGUI _leftText;
    [SerializeField] TextMeshProUGUI _rightText;

    [Space(10f)]
    [SerializeField] GameObject _backgroundPanel;
    [SerializeField] InventoryPanel _inventoryPanel;
    [SerializeField] CharmPanel _charmPanel;
    [SerializeField] InformationPanel _informationPanel;
    [SerializeField] DialoguePanel _dialoguePanel;

    [Space(10f)]
    [SerializeField] Highlighter _highlighter;

    [Header("Slide Animation Settings")]
    [Space(10f)]
    [SerializeField] float _slideDuration;
    [SerializeField] float _slideDistance;
    
    int _currentPopupPanel = 0;
    bool _isAnimating = false;

    // Inventory
    List<PopupPanelBase> _panels = new List<PopupPanelBase>();

    HUDPanel _hud;

    public InventoryPanel InvenPanel { get { return _inventoryPanel; } }
    public CharmPanel CharmPanel { get { return _charmPanel; } }
    public InformationPanel InfoPanel { get { return _informationPanel; } }
    public List<PopupPanelBase> Panels{ get { return _panels; } }
    public Highlighter Highlighter { get { return _highlighter; } }
    
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

        UIManager.Instance.OnUIToggled += OnUIToggled;
    }

    void Init()
    {
        InitPanelList();

        _informationPanel.gameObject.SetActive(false);

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
        _titleText.text = _panels[panelIndex].PanelName;
        _leftText.text = _panels[GetPopupPrevIndex()].PanelName;
        _rightText.text = _panels[GetPopupNextIndex()].PanelName;
    }

    public void ShowPanel(int newIndex, bool isLeft)
    {
        if (_isAnimating) return;

        _isAnimating = true;
        RectTransform currentPanel = Panels[CurrentPopupPanel].Frame;
        RectTransform nextPanel = Panels[newIndex].Frame;

        Vector3 startPos = nextPanel.transform.localPosition;
        Vector3 targetPos = currentPanel.transform.localPosition;
        Vector3 offScreenPos = targetPos + new Vector3(isLeft ? -_slideDistance : _slideDistance, 0, 0);

        nextPanel.transform.localPosition = offScreenPos;
        nextPanel.gameObject.SetActive(true);

        // 이동 애니메이션
        nextPanel.transform.DOLocalMove(targetPos, _slideDuration).SetEase(Ease.OutQuart);
        currentPanel.transform.DOLocalMove(isLeft ? targetPos + new Vector3(_slideDistance, 0, 0) : targetPos + new Vector3(-_slideDistance, 0, 0), _slideDuration)
            .SetEase(Ease.OutQuart)
            .OnComplete(() =>
            {
                currentPanel.gameObject.SetActive(false);
                _isAnimating = false;

                Panels[newIndex].UpdateArrowSlot();
            });
        CurrentPopupPanel = newIndex;
    }

    void OnUIToggled(UIType type, bool isOpen)
    {
        switch(type)
        {
            case UIType.Inventory:
                SetInventoryPanel(isOpen);
                break;
            case UIType.Charm:
                SetCharmPanel(isOpen);
                break;
            case UIType.PopupInfo:
                SetInfoPanel(isOpen);
                break;
            case UIType.Dialogue:
                SetDialoguePanel(isOpen);
                break;
        }
    }

    void SetInventoryPanel(bool isOpen)
    {
        _hud.gameObject.SetActive(!isOpen);
        gameObject.SetActive(isOpen);
        _backgroundPanel.gameObject.SetActive(isOpen);
        _inventoryPanel.gameObject.SetActive(isOpen);

        if(isOpen)
        {
            _inventoryPanel.SetCoinText(InventoryManager.Instance.Coin);
        }

        CurrentPopupPanel = (int)UIType.Inventory;
    }

    void SetCharmPanel(bool isOpen)
    {
        _hud.gameObject.SetActive(!isOpen);
        gameObject.SetActive(isOpen);
        _backgroundPanel.gameObject.SetActive(isOpen);
        _inventoryPanel.gameObject.SetActive(!isOpen);

        _charmPanel.OpenByBench = isOpen;
        _charmPanel.gameObject.SetActive(isOpen);

        CurrentPopupPanel = (int)UIType.Charm;
    }
    void SetInfoPanel(bool isOpen)
    {
        gameObject.SetActive(isOpen);
        _backgroundPanel.gameObject.SetActive(isOpen);
        _inventoryPanel.gameObject.SetActive(isOpen);
        _informationPanel.gameObject.SetActive(isOpen);
    }

    void SetDialoguePanel(bool isOpen)
    {
        _dialoguePanel.gameObject.SetActive(isOpen);
    }

    public int GetPopupPrevIndex()
    {
        int prevIndex = (_currentPopupPanel-1 + _panels.Count) % _panels.Count;

        return prevIndex;
    }
    public int GetPopupNextIndex()
    {
        int nextIndex = (_currentPopupPanel+1)% _panels.Count;

        return nextIndex;
    }
}
