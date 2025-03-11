using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class InformationPanel : MonoBehaviour
{
    [SerializeField] GameObject _backgroundPanel;
    [SerializeField] SpellInfoPanel _spellInfo;


    //void Start()
    //{
    //    gameObject.SetActive(false);
    //    _spellInfo.gameObject.SetActive(false);
    //}
    public void PopupUI(InfoPanelType panelType,int loreStrId =0,int spellId = 0)
    {
        gameObject.SetActive(true);
        _backgroundPanel.SetActive(true);
       switch (panelType)
        {
            case InfoPanelType.SpellInfo:
                
                _spellInfo.gameObject.SetActive(true);
                _spellInfo.SetUI(loreStrId, spellId);
                break;
            case InfoPanelType.SimpleLore:
                break;
        }
    }
}
