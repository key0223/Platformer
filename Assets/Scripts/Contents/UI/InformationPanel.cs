using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class InformationPanel : MonoBehaviour
{
    [SerializeField] GameObject _backgroundPanel;
    [SerializeField] SpellInfoPanel _spellInfo;

    public void PopupUI(InfoPanelType panelType,int loreStrId =0,int spellId = 0)
    {
        gameObject.SetActive(!gameObject.activeSelf);
        _backgroundPanel.SetActive(!_backgroundPanel.activeSelf);
       switch (panelType)
        {
            case InfoPanelType.SpellInfo:
                
                _spellInfo.gameObject.SetActive(!_spellInfo.gameObject.activeSelf);
                _spellInfo.SetUI(loreStrId, spellId);
                break;
            case InfoPanelType.SimpleLore:
                break;
        }
    }
}
