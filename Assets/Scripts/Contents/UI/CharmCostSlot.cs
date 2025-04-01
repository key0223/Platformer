using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharmCostSlot : MonoBehaviour
{
    [SerializeField] Image _backgroundImage;
    [SerializeField] Image _usingImage;

    public void SetSlotState(bool isUsing)
    {
        _usingImage.gameObject.SetActive(isUsing);
    }
}
