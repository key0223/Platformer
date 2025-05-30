using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapPanel : PopupPanelBase
{
    [SerializeField] TextMeshProUGUI _noMapText;
    [SerializeField] TextMeshProUGUI _rigionNameText;
    [SerializeField] GameObject _miniMapParent;
    [SerializeField] RawImage _renderTexture;

    [Header("Arrow Slot")]
    [SerializeField] List<Slot> _arrowSlot;

    [Header("Marker Bar")]
    [SerializeField] string _markerSlotPrefabPath;
    [SerializeField] RectTransform _markerSlotParent;
    int[] _markerIds = { 10501, 10502, 10503 };
    float _duration = 0.7f;
    bool _isExpanded = false;


    bool _initialized = false;
    [Space(10f)]
    // Highlighter 
    [SerializeField] Transform _initPos;

    List<MarkerBarSlot> _slots = new List<MarkerBarSlot>();
    public GameObject MiniMapParent { get { return _miniMapParent; } }
    public bool IsExpanded { get { return _isExpanded; } set { _isExpanded = value; } }


    protected override void Init()
    {
        if(_initialized) return;

        _initialized = true;

        _sections = new Section[2];
        for (int i = 0; i < _sections.Length; i++)
        {
            _sections[i] = new Section();

            SlotRow row = new SlotRow();
            _sections[i]._rows.Add(row);
        }

        if(_highlighter != null)
        {

            _highlighter.MoveToSlot(_initPos);
        }

        #region Marker Slot Initialize

        for (int i = 0; i < _markerIds.Length; i++)
        {
            MarkerBarSlot markerSlot = ResourceManager.Instance.Instantiate(_markerSlotPrefabPath, _markerSlotParent.gameObject.transform).GetComponent<MarkerBarSlot>();

            markerSlot.gameObject.SetActive(false);
            markerSlot.Init(_markerIds[i]);

            _sections[0]._rows[0]._columns.Add(markerSlot);
            _slots.Add(markerSlot);
        }

        #endregion
        #region Arrow Slot Initialize
        for (int i = 0; i < _arrowSlot.Count; i++)
        {
            Slot slot = _arrowSlot[i];
            _sections[1]._rows[0]._columns.Add(slot);
        }
        #endregion
    }

    #region Refresh UI
    public void RefreshMiniMap()
    {
        if (!_initialized)
            Init();

        if (_miniMapParent.transform.childCount == 0 || MapManager.Instance.CurrentMiniMap == null)
        {
            SetUnavaliable();
            return;
        }
        for (int i = 0; i < _miniMapParent.transform.childCount; i++)
        {
            _miniMapParent.transform.GetChild(i).gameObject.SetActive(false);
        }

        _noMapText.gameObject.SetActive(false);
        _renderTexture.gameObject.SetActive(true);
        MapManager.Instance.CurrentMiniMap.gameObject.SetActive(true);

        Item item = InventoryManager.Instance.GetItem(MapManager.Instance.CurrentMiniMap.ItemID);
        if (item != null)
        {
            MiniMap miniMap = item as MiniMap;

            _rigionNameText.text = miniMap.AreaName;
        }
    }

    public void RefreshMarkerBar()
    {
        foreach (MarkerBarSlot markerSlot in _slots)
        {
            markerSlot.gameObject.SetActive(false);
        }

        for (int i = 0; i < _markerIds.Length; i++)
        {
            if (InventoryManager.Instance.Items.ContainsKey(_markerIds[i]))
            {
                _slots[i].gameObject.SetActive(true);
                _slots[i].Init(_markerIds[i]);
            }
        }
        _isExpanded = false;
        MarkerBarToggle();
    }
    #endregion

    [ContextMenu("TOGGLE")]
    public void MarkerBarToggle()
    {
        float targetX = _isExpanded ? 1f : 0f;

        _markerSlotParent.DOScaleX(targetX, _duration).SetEase(Ease.InOutQuad);
        _isExpanded = !_isExpanded;
    }
    void SetUnavaliable()
    {
        _rigionNameText.text = "";
        _noMapText.gameObject.SetActive(true);
        _renderTexture.gameObject.SetActive(false);
    }
}
