using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MarkerBarUI : MonoBehaviour
{
    #region Bar Settings
    [SerializeField] RectTransform _frame;
    [SerializeField] string _prefabPath;
    [SerializeField] float _duration = 1f;
    [SerializeField] List<int> _markerIds = new List<int>();
    bool _isExpanded = false;
    #endregion

    List<MarkerBarSlot> _slots = new List<MarkerBarSlot>();

    void Start()
    {
        FirstInit();
        UIManager.Instance.OnToggleMiniMap += RefreshUI;

    }

    void FirstInit()
    {
        InitList();
        Toggle();
    }
    void InitList()
    {
        foreach (int key in _markerIds)
        {
            if (InventoryManager.Instance.Items.TryGetValue(key, out Item item))
            {
                MarkerBarSlot markerSlot = ResourceManager.Instance.Instantiate(_prefabPath, _frame).GetComponent<MarkerBarSlot>();
                markerSlot.Init(key);

                markerSlot.gameObject.SetActive(true);
                _slots.Add(markerSlot);
            }
        }
    }
    public void RefreshUI()
    {
        ClearList();
        InitList();
        if (_slots.Count == 0) return;

        _frame.gameObject.SetActive(_isExpanded);
    }

    void ClearList()
    {
        for (int i = _slots.Count - 1; i >= 0; i--)
        {
            ResourceManager.Instance.Destroy(_slots[i].gameObject);
            _slots.RemoveAt(i);
        }
    }

    [ContextMenu("TOGGLE")]
    public void Toggle()
    {
        float targetX = _isExpanded ? 1f : 0f;

        _frame.DOScaleX(targetX, _duration).SetEase(Ease.InOutQuad);
        _isExpanded = !_isExpanded;
    }
}
