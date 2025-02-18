using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class RedSlimeMovement : MonsterMovement
{
    [SerializeField] GameObject _fxPoint;

    void OnEnable()
    {
        _attackInterval = Random.Range(1.5f, 4f);
    }
    protected override void Attack()
    {
        CreateProjectile();
    }

    void CreateProjectile()
    {
        GameObject fireBallGO = ResourceManager.Instance.Instantiate("Projectiles/Red Slime Projectile");
        fireBallGO.transform.SetParent(PoolManager.Instance.HierachyRoot.transform);
        fireBallGO.transform.position = _fxPoint.transform.position;
        fireBallGO.SetActive(true);

        FireballController fireball = fireBallGO.GetComponent<FireballController>();
        Vector2 dir = Random.Range(0, 2) == 0 ? Vector2.left : Vector2.right;
        fireball.Shoot(dir);
    }
}
