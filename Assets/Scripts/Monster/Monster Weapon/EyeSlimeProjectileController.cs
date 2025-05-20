using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class EyeSlimeProjectileController : Projectile
{
    Coroutine _coDestroy;
    WaitForSeconds _destroyTime = new WaitForSeconds(5f);

    protected override void Awake()
    {
        base.Awake();
    }

    void OnEnable()
    {
        if (_coDestroy != null)
        {
            StopCoroutine(_coDestroy);
            _coDestroy = null;
        }
        _coDestroy = StartCoroutine(CoAutoDestroy());
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.gameObject.CompareTag(TAG_GROUND))
        {
            CreateFX();
            ResourceManager.Instance.Destroy(gameObject);
        }
        else if(coll.gameObject.CompareTag(TAG_PLAYER))
        {
            PlayerController player = null;

            if(coll.gameObject.TryGetComponent(out player))
            {
                player.PlayerHealth.OnDamaged(_damage);
                CreateFX();
                ResourceManager.Instance.Destroy(gameObject);
            }
        }
    }

    void CreateFX()
    {
        GameObject fx = ResourceManager.Instance.Instantiate("FX/MonsterFX/Poison ExplosionFX");
        fx.transform.SetParent(PoolManager.Instance.HierachyRoot.transform);
        fx.transform.position = gameObject.transform.position;
        fx.SetActive(true);
    }
    IEnumerator CoAutoDestroy()
    {
        yield return _destroyTime;
        _coDestroy = null;
        ResourceManager.Instance.Destroy(gameObject);
    }    
}
