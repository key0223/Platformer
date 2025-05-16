using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class GruzMotherMovement : MonsterMovement
{
    Rigidbody2D _rigid;
    PlayerMovement _target;

    [Header("Movement Settings")]
    [SerializeField] float _moveSpeed = 2f;
    [SerializeField] float _attackMoveSpeed = 4f;
    [SerializeField] float _distanceThreshold = 2f;
    [SerializeField] Vector2 _skillMoveDistance;

    [Header("Attack pattern waypoint")]
    [SerializeField] List<Transform> _waypoint;

    Coroutine _coSkill;
    delegate IEnumerator AttackPattern();
    AttackPattern Pattern;

    public override CreatureState State
    {
        get { return _state; }
        set
        {
            if (_state == value) return;
            _state = value;

            if(_coPatrol != null)
            {
                StopCoroutine(_coPatrol);
                _coPatrol = null;
            }
            if (_coMoving != null)
            {
                StopCoroutine(_coMoving);
                _coMoving = null;
            }
            if(_coSkill != null)
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

    
    protected override void Update()
    {
        UpdateMovement();
    }

    protected override void UpdateMovement()
    {
        switch (State)
        {
            case CreatureState.Idle:
                UpdateIdle();
                break;
            case CreatureState.Moving:
                UpdateMoving();
                break;
            case CreatureState.Skill:
                UpdateSkill();
                break;
            case CreatureState.Dead:
                break;
        }
    }

    protected override void UpdateIdle()
    {
        if (_coPatrol == null)
            _coPatrol = StartCoroutine(CoPatrol());
    }

    protected override void UpdateMoving()
    {
        if (_coMoving == null)
            _coMoving = StartCoroutine(CoMoving());
    }

    void UpdateSkill()
    {
        if (_coSkill == null)
            _coSkill = StartCoroutine(CoSkill());
    }
    #region General method

    void SwitchSkillAttack()
    {
        int random = Random.Range(0, 2);

        if (random == 1)
        {
            Pattern = CoAttackPattern_1;
        }
        else
            Pattern = CoAttackPattern_2;
    }

    int FindClosestPoint(Transform currentPos)
    {
        float currentClosestDistance = 0;
        int index = 0;

        for (int i = 0; i < _waypoint.Count; i++)
        {
            float distance = (_waypoint[i].position - transform.position).magnitude;

            if (currentClosestDistance == 0)
            {
                currentClosestDistance = distance;
                index = i;
            }
            else
            {
                if (currentClosestDistance > distance)
                {
                    currentClosestDistance = distance;
                    index = i;
                }
            }
        }

        return index;
    }
    #endregion


    #region Coroutine

    protected override IEnumerator CoPatrol()
    {
        _target = GameObject.FindObjectOfType<PlayerMovement>();

        if (_target == null) yield break;

        Vector2 idlePos = (Vector2)transform.position + _skillMoveDistance;

        // TODO : Ground Collision 처리
        //while (Vector2.Distance(idlePos, transform.position) > 0.1f)
        //{
        //    Vector3 dir = idlePos - (Vector2)transform.position;
        //    _rigid.velocity = dir.normalized * _moveSpeed;
        //    yield return null;
        //}
        yield return new WaitForSeconds(0.5f);
        State = CreatureState.Moving;
    }
    protected override IEnumerator CoMoving()
    {
        if (_target == null)
        {
            _target = null;

            yield return new WaitForSeconds(0.3f);
            State = CreatureState.Idle;

        }

        Vector3 dir = (_target.transform.position - transform.position);
        float distance = dir.magnitude;
        if (distance > _distanceThreshold)
            _rigid.velocity = dir.normalized * _moveSpeed;
        else
        {
            _rigid.velocity = Vector2.zero;
            State = CreatureState.Skill;
        }
    }

    // Before Attack 
    IEnumerator CoSkill()
    {
        Vector2 skillPos = (Vector2)transform.position + _skillMoveDistance;

        while (Vector2.Distance(skillPos, transform.position) > 0.1f)
        {
            Vector3 dir = skillPos - (Vector2)transform.position;
            _rigid.velocity = dir.normalized * _moveSpeed;
            yield return null;
        }

        _rigid.velocity = Vector2.zero;
        SwitchSkillAttack();

        yield return StartCoroutine(Pattern());
    }

    IEnumerator CoAttackPattern_1()
    {
        Vector2 targetPos = _target.transform.position;

        while (Vector2.Distance(targetPos, transform.position) > _distanceThreshold)
        {
            Vector2 dir = targetPos - (Vector2)transform.position;
            _rigid.velocity = dir.normalized * _attackMoveSpeed;
            yield return null;
        }

        _rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.3f);
        State = CreatureState.Idle;
    }
    IEnumerator CoAttackPattern_2()
    {
        int startPoint = FindClosestPoint(transform);

        bool onRight = _waypoint.Count / 2 > startPoint;

        Stack<Vector3> paths = new Stack<Vector3>();

        if (onRight) // 왼쪽 -> 오른쪽으로 이동
        {
            for (int i = startPoint - 1; i >= 0; i--)
            {
                paths.Push(_waypoint[i].position);
            }
        }
        else // 오른쪽 -> 왼쪽으로 이동
        {
            for (int i = 0; i < startPoint; i++)
            {
                paths.Push(_waypoint[i].position);
            }
        }

        while (paths.Count > 0)
        {
            Vector2 targetPoint = paths.Pop();
            Vector2 dir = targetPoint - (Vector2)transform.position;

            while (dir.magnitude > _distanceThreshold)
            {
                dir = targetPoint - (Vector2)transform.position;
                _rigid.velocity = dir.normalized * _attackMoveSpeed;
                yield return null;
            }

            _rigid.velocity = Vector2.zero; // 도착 시 정지
            yield return new WaitForSeconds(0.1f);
        }
        State = CreatureState.Idle;
    }
    #endregion
}
