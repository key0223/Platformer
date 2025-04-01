using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField]  int _itemId = 0;
    [SerializeField] bool _isArrow;
    [SerializeField] bool _arrowLeft;
    public int ItemId { get { return _itemId; } set { _itemId = value; } }
    public bool IsArrow { get { return _isArrow; }}
    public bool ArrowLeft { get { return _arrowLeft; }}
}
