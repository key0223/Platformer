using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableItem : MonoBehaviour, IBreakable
{
    [SerializeField] protected int _maxHp;
    protected int _currentHp;

    [Header("FX Settings")]
    [SerializeField] protected bool _hasHitFX = false;
    [SerializeField] protected bool _hitDrop = false; // Whether it drops everytime it gets hit or not
    [SerializeField] protected string _hitFxPrefab;
    [SerializeField] protected string _destroyFxPrefab;

    [Header("DropItem Settings")]
    [SerializeField] protected int _maxItemCount = 15;
    [SerializeField] protected string _dropItemPrefab;

    protected int _remainingDropItemCount;

    void Awake()
    {
        _remainingDropItemCount = _maxItemCount;
        _currentHp = _maxHp;
    }
    public virtual void OnDamaged(int damage)
    {
        _currentHp -= damage;
        if (_hasHitFX)
        {
            CreateFX(_hitFxPrefab);
            //DropItem();
        }
        if (_hitDrop)
        {
            DropItem();
        }
        if (_currentHp <= 0)
        {
            CreateFX(_destroyFxPrefab);
            DropItem();
            Destroy(gameObject);
        }
    }

    protected void DropItem()
    {
        int dropMaxCount = _maxItemCount / _maxHp;
        int dropCount = Random.Range(1, dropMaxCount + 1);

        if (dropCount > _remainingDropItemCount || _currentHp <= 0)
        {
            dropCount = _remainingDropItemCount;
        }

        _remainingDropItemCount -= dropCount;

        for (int i = 0; i < dropCount; i++)
        {
            GameObject dropItem = ResourceManager.Instance.Instantiate(_dropItemPrefab);
            dropItem.transform.position = transform.position;
            dropItem.SetActive(true);
        }
    }

    protected void CreateFX(string prefabPath)
    {
        GameObject fxGO = ResourceManager.Instance.Instantiate(prefabPath);
        fxGO.transform.position = gameObject.transform.position;
        fxGO.SetActive(true);
    }
}
