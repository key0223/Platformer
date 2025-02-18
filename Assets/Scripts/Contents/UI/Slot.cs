using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] protected int _slotIndex = 0;
    public int SlotIndex { get { return _slotIndex; } set { _slotIndex = value; } }
}
