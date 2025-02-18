using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PopupPanelBase : MonoBehaviour
{
   
    [SerializeField] 
    protected int _panelIndex = 0;

    [SerializeField]
    protected Transform[] _rowParents;
    protected List<List<Slot>> _slots = new List<List<Slot>>();
    int _currentRow = 0;
    int _currentColumn = 0;

    protected Highlighter _highlighter;

    // Panel index should be populated before runtime
    public int PanelIndex { get { return _panelIndex; } }
    void Start()
    {
        _highlighter = GetComponentInChildren<Highlighter>();

        InitSlotList();
    }

    protected virtual void InitSlotList()
    {
        _slots.Clear();

        foreach(Transform row in _rowParents)
        {
            List<Slot> slotRow = row.GetComponentsInChildren<Slot>().ToList();
            if(slotRow.Count > 0 )
                _slots.Add(slotRow);
        }
    }

   public void MoveHighlighter(int horizontal, int vertical)
    {
        int newRow = _currentRow;
        int newColumn = _currentColumn;

        if (_currentRow == 0) // first row
        {
            if (vertical != 0)
            {
                newColumn = (newColumn + vertical + _slots[_currentRow].Count) % _slots[_currentRow].Count;
            }
            else // ÁÂ¿ì ÀÎÇ²
                newRow = (_currentColumn == 0) ? _currentColumn + 1 : _slots.Count - 1;
        }
        else if (_currentRow == _slots.Count - 1) // last row
        {
            if (vertical != 0)
            {
                if (vertical < 0) // up Arrow
                {
                    newRow = _currentRow - 1; // ÀÌÀü Row
                    newColumn = 0;
                }
                else
                {
                    newRow = 0; // Ã¹ ¹øÂ° Row
                    newColumn = _slots[newRow].Count - 1;
                }
            }
            else
                newColumn = (newColumn + horizontal + _slots[_currentRow].Count) % _slots[_currentRow].Count;
        }
        else // Middle row
        {
            if (vertical != 0)
            {
                newRow = (vertical < 0) ? _currentRow - 1 : _currentRow + 1;
                newColumn = 0;
            }
            else
                newColumn = (newColumn + horizontal + _slots[_currentRow].Count) % _slots[_currentRow].Count;
        }

        // if there's any changes move
        if (newRow !=_currentRow ||  newColumn != _currentColumn)
        {
            _currentRow = newRow;
            _currentColumn = newColumn;
            _highlighter.MoveToSlot(_slots[_currentRow][_currentColumn].transform);
        }
    }

}
