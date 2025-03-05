using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPanel : MonoBehaviour
{
    List<PopupPanelBase> _panels = new List<PopupPanelBase>();

    //int _currentPanelIndex = 0;

    void Start()
    {
        Init();
    }

    void Init()
    {
        InitPanelList();

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
}
