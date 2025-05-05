using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The information panel and all of its child objects should be inactive.
public class InformationPanel : MonoBehaviour
{
    [SerializeField] GameObject _backgroundPanel;
    [SerializeField] SpellInfoPanel _spellInfo;
    [SerializeField] CharmCollectionPanel _charmCollection;

    // Spell Info
    public void PopupUI(int loreStrId = 0, int spellId = 0)
    {
        gameObject.SetActive(!gameObject.activeSelf);
        _backgroundPanel.SetActive(!_backgroundPanel.activeSelf);

        _spellInfo.gameObject.SetActive(!_spellInfo.gameObject.activeSelf);
        _spellInfo.SetUI(loreStrId, spellId);
    }

    // Charm collection Info 
    public void PopupUI(int charmId)
    {
        gameObject.SetActive(!gameObject.activeSelf);
        _backgroundPanel.SetActive(!_backgroundPanel.activeSelf);

        _charmCollection.gameObject.SetActive(!_charmCollection.gameObject.activeSelf);
        _charmCollection.SetUI(charmId);
        
    }
}
