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

    GruzMotherAnimation _anim;

    Dir _currentDir = Dir.None;
    public Dir CurrentDir
    { 
        get { return _currentDir; }
        set 
        {
            if (_currentDir == value) return;
            _currentDir = value;
            _anim.UpdateDir();
        }
    }

    #region State Parameter
    bool _isFirstDamage = false;
    public bool IsMoving { get; private set; }
    #endregion

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
        _stat = GetComponent<MonsterStat>();
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<GruzMotherAnimation>();
    }

    
    protected override void Update()
    {
        if (!_isFirstDamage)
            return;

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

    #region Update State
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
    #endregion


    public override void OnDamaged(float damage, PlayerMovement player = null)
    {
        if(!_isFirstDamage)
        {
            _isFirstDamage = true;
            _anim.OnFirstDamage();
        }
    }

    #region General method

    Dir GetDir(Vector2 dir)
    {
        Dir currentDir = Dir.None;

        if (dir.x > 0)
            currentDir = Dir.Right;
        else if(dir.x<0)
            currentDir = Dir.Left;

        return currentDir;
    }
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
            float distance = (_waypoint[i].position - currentPos.position).magnitude;

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
        IsMoving = true;
        _target = GameObject.FindObjectOfType<PlayerMovement>();

        if (_target == null) yield break;

        Vector2 idlePos = (Vector2)transform.position + _skillMoveDistance;
        Vector3 dir = idlePos - (Vector2)transform.position;
        CurrentDir = GetDir(dir);
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
        CurrentDir = GetDir(dir);
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
        IsMoving = false;

        Vector2 skillPos = (Vector2)transform.position + _skillMoveDistance;

        while (Vector2.Distance(skillPos, transform.position) > 0.1f)
        {
            Vector3 dir = skillPos - (Vector2)transform.position;
            CurrentDir = GetDir(dir);
            _rigid.velocity = dir.normalized * _moveSpeed;
            yield return null;
        }

        _rigid.velocity = Vector2.zero;
        _anim.IsAnticipating = true;

        SwitchSkillAttack();
        yield return StartCoroutine(Pattern());
    }

    IEnumerator CoAttackPattern_1()
    {
        yield return new WaitForSeconds(0.1f);
        Vector2 targetPos = _target.transform.position;

        while (Vector2.Distance(targetPos, transform.position) > _distanceThreshold)
        {
            Vector2 dir = targetPos - (Vector2)transform.position;
            CurrentDir = GetDir(dir);
            _rigid.velocity = dir.normalized * _attackMoveSpeed;
            yield return null;
        }

        _rigid.velocity = Vector2.zero;
        yield return new WaitForSeconds(0.3f);

        State = CreatureState.Idle;
    }
    IEnumerator CoAttackPattern_2()
    {
        int startPoint = FindClosestPoint(_target.transform);

        Queue<Vector3> paths = new Queue<Vector3>();

        if (_currentDir == Dir.Left) // 왼쪽 -> 오른쪽으로 이동
        {
            for (int i = startPoint; i< _waypoint.Count; i++)
            {
                paths.Enqueue(_waypoint[i].position);
            }

            for (int i = _waypoint.Count-1; i >= 0; i--)
            {
                paths.Enqueue(_waypoint[i].position);
            }
        }
        else // 오른쪽 -> 왼쪽으로 이동
        {
            for (int i = startPoint; i >=0; i--)
            {
                paths.Enqueue(_waypoint[i].position);
            }

            for (int i = 0; i< _waypoint.Count; i++)
            {
                paths.Enqueue(_waypoint[i].position);
            }
        }

        while (paths.Count > 0)
        {
            _anim.StartedSlam = true;

            Vector2 targetPoint = paths.Dequeue();
            Vector2 dir = targetPoint - (Vector2)transform.position;
            CurrentDir = GetDir(dir);
            while (dir.magnitude > _distanceThreshold)
            {
                dir = targetPoint - (Vector2)transform.position;
                _rigid.velocity = dir.normalized * _attackMoveSpeed;
                yield return null;
            }

            if(Mathf.Sign(dir.normalized.y)>0)
            {
                _anim.SlamUp = true;
            }
            else
            {
                _anim.SlamDown = true;
            }
            _rigid.velocity = Vector2.zero; // 도착 시 정지
            yield return new WaitForSeconds(0.2f);
        }

        State = CreatureState.Idle;
    }
    #endregion
}
