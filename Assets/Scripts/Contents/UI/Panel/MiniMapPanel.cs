using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _noMapText;
    [SerializeField] TextMeshProUGUI _rigionNameText;
    [SerializeField] GameObject _miniMapParent;
    [SerializeField] RawImage _renderTexture;

    [Header("Arrow Slot")]
    [SerializeField] List<Slot> _arrowSlot;

    [Header("Marker Bar")]
    [SerializeField] MarkerBarUI _markerBar;
    [SerializeField] string _markerSlotPrefabPath;
    [SerializeField] RectTransform _markerSlotParent;



    int[] _markerIds = { 10501, 10502, 10503 };


    [Space(10f)]
    // Highlighter 
    [SerializeField] Transform _initPos;

    List<MarkerBarSlot> _slots = new List<MarkerBarSlot>();
    public GameObject MiniMapParent { get { return _miniMapParent; } }


  
    void OnEnable()
    {
        RefreshMiniMap();
    }

    void RefreshMiniMap()
    {
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

        _markerBar.RefreshUI();
    }

    void SetUnavaliable()
    {
        _rigionNameText.text = "";
        _noMapText.gameObject.SetActive(true);
        _renderTexture.gameObject.SetActive(false);
    }
}
