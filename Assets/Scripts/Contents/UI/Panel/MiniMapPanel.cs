using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MiniMapPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _rigionNameText;
    [SerializeField] GameObject _miniMapParent;

    public GameObject MiniMapParent {  get { return _miniMapParent; } }

    void OnEnable()
    {
        RefreshMiniMap();
    }

    void RefreshMiniMap()
    {
        for (int i = 0; i < _miniMapParent.transform.childCount; i++)
        {
            _miniMapParent.transform.GetChild(i).gameObject.SetActive(false);
        }

        MapManager.Instance.CurrentMiniMap.gameObject.SetActive(true);

        Item item = InventoryManager.Instance.GetItem(MapManager.Instance.CurrentMiniMap.ItemID);
        if (item != null)
        {
            MiniMap miniMap = item as MiniMap;

            _rigionNameText.text = miniMap.AreaName;
        }
    }
}
