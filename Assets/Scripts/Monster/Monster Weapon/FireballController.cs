using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
public class FireballController : Projectile
{
    [Header("Movement Settings")]
    [SerializeField] float _bounceForce = 1f;
    [SerializeField] int _maxBounceCount = 1;
    int _currentBounceCount = 0;


    protected override void Awake()
    {
        base.Awake();
        _currentBounceCount = _maxBounceCount;
    }
    
    public void Shoot(Vector2 direction)
    {
        _rigid.velocity = direction * _speed;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_GROUND))
        {
            Vector3 normal = collision.contacts[0].normal;
            _rigid.AddForce(-normal * _bounceForce, ForceMode2D.Impulse);
            _currentBounceCount--;

            if(_currentBounceCount <= 0)
            {
                _currentBounceCount = _maxBounceCount;

                CreateFX();

                ResourceManager.Instance.Destroy(gameObject);
            }
        }
        else if(collision.gameObject.CompareTag(TAG_PLAYER))
        {
            PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.OnDamaged(_damage);

                _currentBounceCount = _maxBounceCount;

                CreateFX();

                ResourceManager.Instance.Destroy(gameObject);
            }
        }
    }

    void CreateFX()
    {
        GameObject fxGO = ResourceManager.Instance.Instantiate("FX/Red Slime ExplosionFX");
        fxGO.transform.SetParent(PoolManager.Instance.HierachyRoot.transform);
        fxGO.transform.position = gameObject.transform.position;
        fxGO.SetActive(true);
    }
}
