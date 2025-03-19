using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingEffect : MonoBehaviour
{
    Rigidbody2D _rigid;

    int _bounceCount = 0;

    [Header("Bounce Settings")]
    [SerializeField] int _maxBounces = 3;
    [SerializeField] float _bounceForce = 5f;
    [SerializeField] float _bounceDamping = 0.8f;
    [SerializeField] float _startForce = 5f;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        _rigid.drag = 1f;

        Vector2 randomDir = new Vector2(Random.Range(-1f, 1f), 1f).normalized;
        _rigid.AddForce(randomDir * _startForce, ForceMode2D.Impulse);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Define.TAG_GROUND))
        {
            if (_bounceCount < _maxBounces)
            {
                _rigid.velocity = new Vector2(_rigid.velocity.x, _bounceForce);
                _bounceForce *= _bounceDamping;
                _bounceCount++;
            }
            else
            {
                _rigid.velocity = Vector2.zero;
                _rigid.drag = 20f;
            }
        }
    }

}
