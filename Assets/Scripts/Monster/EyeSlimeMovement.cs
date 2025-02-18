using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using UnityEngine;
using static Define;
using static UnityEngine.ParticleSystem;

public class EyeSlimeMovement : MonsterMovement
{
    AnimatorClipInfo[] _clipInfo;

    Rigidbody2D _rigid;
    PlayerMovement _target;

    [Header("Movement Settings")]
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] float _distanceThreshold = 5f; // 목표 위치 도달 판정 거리

    [Header("Attack Bullet Settings")]
    [SerializeField] Transform _bulletSpawnPoint;
    [SerializeField] int _bulletCount;
    [SerializeField] float _bulletSpeed;
    [SerializeField] float _startAngle = 90f;
    [SerializeField] float _endAngle = -90f;

    [Space(5)]
    [Header("Attack Laser Settings")]
    [SerializeField] Laser _laser;

    // State Parameters
    bool _isFacingRight = false;
    public bool IsFacingRight { get { return _isFacingRight; } }
    float _attackFrame = 3f;
    WaitForSeconds _waitOneSec = new WaitForSeconds(1);
    Coroutine _coSkill;

    public override CreatureState State
    {
        get { return _state; }
        set
        {
            if (_state == value) return;
            _state = value;
            _monsterAnimation.UpdateAnimation(_state);
            if (_coPatrol != null)
            {
                StopCoroutine(_coPatrol);
                _coPatrol = null;
            }
            if (_coMoving != null)
            {
                StopCoroutine(_coMoving);
                _coMoving = null;
            }
            if (_coSkill != null)
            {
                StopCoroutine(_coSkill);
                _coSkill = null;
            }

        }
    }
    protected override void Awake()
    {
        base.Awake();
        _rigid = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        _clipInfo = _monsterAnimation.Anim.GetCurrentAnimatorClipInfo(0);
    }
    protected override void Update()
    {
        UpdateMovement();
    }
    protected override void Attack()
    {
        if (_coSkill == null)
            _coSkill = StartCoroutine(CoSkill());
    }

    #region Movement Coroutines
    protected override IEnumerator CoPatrol()
    {
        PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
        if (player != null)
        {
            _target = player;
        }

        yield return _waitOneSec;

        State = CreatureState.Moving;
    }
    protected override IEnumerator CoMoving()
    {
        float dir = Mathf.Sign(_target.transform.position.x - transform.position.x);
        if (dir != 0)
            CheckDirectionToFace(dir > 0);

        while (true)
        {
            float distance = Mathf.Abs(_target.transform.position.x - transform.position.x);

            if (distance <= _distanceThreshold)
            {
                _rigid.velocity = Vector3.zero;
                break;
            }
            _rigid.velocity = new Vector2(dir * _moveSpeed, 0);
            yield return null;
        }

        yield return _waitOneSec;
        Attack();
    }


    #endregion

    #region Attack Bullet
    IEnumerator CoSkill()
    {
        State = CreatureState.Skill;

        yield return new WaitForSeconds(0.1f);
        Animator anim = _monsterAnimation.Anim;

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        float targetNormalizedTime = _attackFrame / _clipInfo[0].clip.frameRate;

        while (stateInfo.normalizedTime < targetNormalizedTime)
        {
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        anim.speed = 0;

        #region Random Skill
        int randomSkill = Random.Range(0, 3);

        switch(randomSkill)
        {
            case 0:
                yield return StartCoroutine(CoSkill_Bullet_Pattern0());
                break;
            case 1:
                yield return StartCoroutine(CoSkill_Bullet_Pattern2());
                break;
            case 2:
                yield return StartCoroutine(CoSkill_Laser());
                break;
        }
        #endregion
        anim.speed = 1;

        float clipLength = Helper.GetAnimationClipLenth(_monsterAnimation.Anim, "ATTACK") / 2;
        yield return Helper.GetWait(clipLength);
        State = CreatureState.Idle;
    }

    IEnumerator CoSkill_Bullet_Pattern0()
    {
        float angleStep = (_startAngle - _endAngle) / (_bulletCount - 1); // 각도 간격 계산

        for (int i = 0; i < _bulletCount; i++)
        {
            float angle = _startAngle - (i * angleStep);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                      Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject bullet = CreateProjectile();

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.velocity = dir * _bulletSpeed;

            // 회전 설정
            float bulletAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, bulletAngle);

            // 대기
            yield return new WaitForSeconds(0.2f);
        }
    }

    // 나선형
    IEnumerator CoSkill_Bullet_Pattern1()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 5f)
        {
            float angle = Time.time * 1f;
            Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

            GameObject bullet = CreateProjectile();

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            rigid.velocity = dir * _bulletSpeed;

            float bulletAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, bulletAngle);

            yield return new WaitForSeconds(0.2f);
            elapsedTime += 0.2f;
        }
    }
    // 나선형 180도 제한
    IEnumerator CoSkill_Bullet_Pattern2()
    {
        float elapsedTime = 0f;

        while (elapsedTime < 5f)
        {
            float angle = Mathf.PingPong(Time.time * _startAngle, _startAngle);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                      Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            GameObject bullet = CreateProjectile();

            // 총알 방향 및 속도 설정
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                rigid.velocity = dir * _bulletSpeed; // 속도 적용
            }

            // 총알 회전 설정
            float bulletAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler(0, 0, bulletAngle);

            yield return new WaitForSeconds(0.2f); // 0.2초 간격 대기
            elapsedTime += 0.2f; // 경과 시간 업데이트
        }
    }

    GameObject CreateProjectile()
    {
        GameObject bullet = ResourceManager.Instance.Instantiate("Projectiles/Eye Slime Projectile");
        bullet.transform.SetParent(PoolManager.Instance.HierachyRoot.transform);
        bullet.transform.position = _bulletSpawnPoint.position;
        bullet.SetActive(true);

        return bullet;
    }
    #endregion

    #region Attack Laser

    IEnumerator CoSkill_Laser()
    {
        _laser.gameObject.SetActive(true);
        yield return StartCoroutine(_laser.CoActivateLaser());
        _laser.gameObject.SetActive(false);
    }

    #endregion

    #region General Methods
    void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != _isFacingRight)
            Turn();
    }
    void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        _isFacingRight = !_isFacingRight;
    }
    #endregion

    #region Editor
    protected override void OnDrawGizmosSelected()
    {
        Vector3 startDirection = new Vector3(Mathf.Cos(_startAngle * Mathf.Deg2Rad),
                                              Mathf.Sin(_startAngle * Mathf.Deg2Rad), 0);

        Vector3 endDirection = new Vector3(Mathf.Cos(_endAngle * Mathf.Deg2Rad),
                                           Mathf.Sin(_endAngle * Mathf.Deg2Rad), 0);

        Gizmos.color = Color.green; // 시작 각도 색상
        Gizmos.DrawLine(transform.position, transform.position + startDirection * 2);

        Gizmos.color = Color.blue; // 끝 각도 색상
        Gizmos.DrawLine(transform.position, transform.position + endDirection * 2);
    }
    #endregion
}
