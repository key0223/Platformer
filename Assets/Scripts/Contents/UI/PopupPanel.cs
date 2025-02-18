using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupPanel : MonoBehaviour
{
    int _currentPanelIndex = 0;
    List<PopupPanelBase> _panels = new List<PopupPanelBase>();

    void Start()
    {
        Init();
    }
    private void Update()
    {
        int horizontal = 0;
        int vertical = 0;

        if (Input.GetKeyDown(KeyCode.LeftArrow)) horizontal = -1;
        if (Input.GetKeyDown(KeyCode.RightArrow)) horizontal = 1;
        if (Input.GetKeyDown(KeyCode.UpArrow)) vertical = -1;
        if (Input.GetKeyDown(KeyCode.DownArrow)) vertical = 1;

        if (horizontal != 0 || vertical != 0)
        {
            _panels[_currentPanelIndex].MoveHighlighter(horizontal, vertical);
        }
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
