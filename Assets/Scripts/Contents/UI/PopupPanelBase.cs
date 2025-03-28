using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Section
{
    public  List<SlotRow> _rows= new List<SlotRow>();
}
[Serializable]
public class SlotRow
{
    public List<Slot> _cloumns = new List<Slot>();
}
public class PopupPanelBase : MonoBehaviour
{
    protected PlayerMovement _playerMovement;
    [SerializeField] 
    protected int _panelIndex = 0;

    [Space(10f)]
    [SerializeField]
    protected Section[] _sections;

    protected int _currentSection = 0;
    protected int _currentRow;
    protected int _currentColumn;


    protected Highlighter _highlighter;

    // Panel index should be populated before runtime
    public int PanelIndex { get { return _panelIndex; } }
    void Start()
    {
        _playerMovement = FindObjectOfType<PlayerMovement>();
        _highlighter = GetComponentInChildren<Highlighter>();

        Init();
    }

    protected virtual void Init()
    {

    }

    void Update()
    {
        bool isSectionMove = Input.GetKey(KeyCode.LeftControl);

        if (Input.GetKeyDown(KeyCode.Return))
            SelectItem();

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveSelection(0, -1, isSectionMove);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveSelection(0, 1, isSectionMove);
        else if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveSelection(-1, 0, isSectionMove);
        else if(Input.GetKeyDown(KeyCode.DownArrow))
            MoveSelection(1, 0, isSectionMove);
    }

    protected virtual void MoveSelection(int horizontal, int vertical, bool sectionMove)
    {
        if(sectionMove)
        {
            MoveSection(vertical);
        }
        else
        {
            MoveHighlighter(horizontal, vertical);
        }
    }
    protected void MoveHighlighter(int horizontal, int vertical)
    {
        Section currecntSection = _sections[_currentSection];
        int newRow = Mathf.Clamp(_currentRow + horizontal, 0, currecntSection._rows.Count - 1);
        int newColumn = _currentColumn;

        if(vertical != 0)
        {
            newColumn = Mathf.Clamp(_currentColumn + vertical, 0, currecntSection._rows[newRow]._cloumns.Count - 1);
        }
        else if( horizontal != 0)
        {
            newColumn = FindClosestColumn(currecntSection._rows[newRow], _currentColumn);
        }
        // Move if there are any changes
        if (newRow !=_currentRow ||  newColumn != _currentColumn)
        {
            _currentRow = newRow;
            _currentColumn = newColumn;
            _highlighter.MoveToSlot(_sections[_currentSection]._rows[_currentRow]._cloumns[_currentColumn].transform);
        }
    }

    protected void MoveSection(int vertical)
    {
        int newSection = (_currentSection + vertical + _sections.Length)% _sections.Length;

        if(newSection != _currentSection)
        {
            _currentSection = newSection;
            _currentRow = 0;
            _currentColumn = 0;

            _highlighter.MoveToSlot(_sections[_currentSection]._rows[_currentRow]._cloumns[_currentColumn].transform);
        }
    }

    protected virtual void SelectItem()
    {

    }

    protected int FindClosestColumn(SlotRow row, int currentCol)
    {
        int closestCol = 0;
        int minDistance = 100;
        for (int col = 0; col < row._cloumns.Count; col++)
        {
            int distance = Mathf.Abs(col - currentCol);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestCol = col;
            }
        }

        return closestCol;
    }

}
