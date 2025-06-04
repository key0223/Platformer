using System;
using System.Collections.Generic;
using UnityEngine;

public class PopupPanelBase : MonoBehaviour
{

    [SerializeField] protected List<Slot> _allSlots = new List<Slot>();

    protected Slot _currentSlot;
    protected Highlighter _highlighter;

    [Header("UI Info")]
    [SerializeField] protected string _panelName;
    [SerializeField] protected RectTransform _frame;

    public string PanelName { get { return _panelName; } }
    public RectTransform Frame { get { return _frame; } }

    void Start()
    {
        _highlighter = UIManager.Instance.PopupPanel.Highlighter;
        Init();

        if(_currentSlot == null && _allSlots.Count > 0)
            _currentSlot = _allSlots[0];

        MoveHighlighter(_currentSlot);
    }

    protected virtual void Init()
    {

    }

    protected void AutoConnectSlots(List<Slot> slots, float maxAngle = 30f)
    {
        foreach (Slot slot in slots)
        {
            slot.Left = FindClosestSlot(slot, slots, Vector2.left, maxAngle);
            slot.Right = FindClosestSlot(slot, slots, Vector2.right, maxAngle);
            slot.Up = FindClosestSlot(slot, slots, Vector2.up, maxAngle);
            slot.Down = FindClosestSlot(slot, slots, Vector2.down, maxAngle);
        }
    }

    protected virtual Slot FindClosestSlot(Slot from, List<Slot> slots, Vector2 dir, float maxAngle)
    {
        Slot closest = null;
        float minDist = float.MaxValue;
        Vector2 fromPos = ((RectTransform)from.transform).anchoredPosition;

        foreach (var slot in slots)
        {
            if (slot == from) continue;
            Vector2 toPos = ((RectTransform)slot.transform).anchoredPosition;
            Vector2 diff = toPos - fromPos;
            float angle = Vector2.Angle(dir, diff);
            if (angle < maxAngle)
            {
                float dist = diff.magnitude;
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = slot;
                }
            }
        }
        return closest;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && _currentSlot.Left != null)
            MoveHighlighter(_currentSlot.Left);
        else if (Input.GetKeyDown(KeyCode.RightArrow) && _currentSlot.Right != null)
            MoveHighlighter(_currentSlot.Right);
        else if (Input.GetKeyDown(KeyCode.UpArrow) && _currentSlot.Up != null)
            MoveHighlighter(_currentSlot.Up);
        else if (Input.GetKeyDown(KeyCode.DownArrow) && _currentSlot.Down != null)
            MoveHighlighter(_currentSlot.Down);
        else if (Input.GetKeyDown(KeyCode.Return))
            SelectItem();
    }
   
    protected void MoveHighlighter(Slot nextSlot)
    {
        if (nextSlot == null) return;
        _currentSlot?.Highlight(false);
        _currentSlot = nextSlot;
        _currentSlot.Highlight(true);
        _highlighter.MoveToSlot(_currentSlot.transform);

        OnSlotChanged();
    }

    protected virtual void OnSlotChanged()
    {

    }
    protected virtual void SelectItem()
    {
        if(_currentSlot == null) return;

        if(_currentSlot.IsArrow)
        {
            PopupPanel popupPanel = UIManager.Instance.PopupPanel;

            if(_currentSlot.ArrowLeft)
            {
                int prevIndex = popupPanel.GetPopupPrevIndex();
                popupPanel.ShowPanel(prevIndex, true);
            }
            else
            {
                int nextIndex = popupPanel.GetPopupNextIndex();
                popupPanel.ShowPanel(nextIndex, false);
            }
            return;

        }
    }

    public virtual void UpdateArrowSlot()
    {

    }
}
