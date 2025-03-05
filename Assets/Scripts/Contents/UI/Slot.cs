using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] int _itemId = 0;
    public int ItemId { get { return _itemId; } set { _itemId = value; } }
}
