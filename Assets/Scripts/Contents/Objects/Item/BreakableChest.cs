using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableChest : BreakableItem
{
    [Space(10f)]
    [SerializeField] GameObject _closedSprite;
    [SerializeField] GameObject _openedSprite;
    public override void OnDamaged(int damage)
    {
        _currentHp -= damage;

        if(_hitDrop)
        {
            DropItem();
        }

        if(_currentHp <=0)
        {
            _closedSprite.SetActive(false);
            _openedSprite.SetActive(true);
            DropItem();
        }
    }
}
