using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class PlayerMovement : MonoBehaviour
{
    PlayerMovementData _data;
    PlayerAnimation _anim;
    PlayerStat _stat;
    public Rigidbody2D RB { get; private set; }

    #region State Parameters
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsDashing { get; private set; }
    public bool IsSliding { get; private set; }
    public bool IsAttacking { get; private set; }
    public bool IsDead { get; private set; }


    // Timers
    public float LastOnGroundTime { get; private set; } // 0 이면 땅에 닿은 상태
    public float LastOnWallTime { get; private set; } // 0 = 벽에 닿은 상태
    public float LastOnWallRightTime { get; private set; } // 0 = 오른쪽 벽에 닿아 있는 상태
    public float LastOnWallLeftTime { get; private set; }
    public float LastPressedJumpTime { get; private set; } // 0 = 방금 점프 버튼을 눌렀음
    public float LastPressedDashTime { get; private set; } // 0 = 방금 대시 버튼 눌렀음
    public float LastPressedAttackTime { get; private set; }

    // Jump
    private bool _isJumpCut;
    private bool _isJumpFalling;

    // Wall Jump
    private float _wallJumpStartTime;
    private int _lastWallJumpDir;

    // Dash
    private int _dashesLeft;
    private bool _dashRefilling;
    private Vector2 _lastDashDir;
    private bool _isDashAttacking;


    #endregion

    Vector2 _moveInput;

    #region Check Parameters
    [Header("Checks")]
    [SerializeField] Transform _groundCheckPoint;
    [SerializeField] Vector2 _groundCheckSize = new Vector2(0.5f, 0.5f);
    [Space(5)]
    [SerializeField] Transform _frontWallCheckPoint;
    [SerializeField] Transform _backWallCheckPoint;
    [SerializeField] Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
    [Space(5)]
    [SerializeField] Transform _frontAttackCheckPoint;
    [SerializeField] Vector2 _attackCheckSize = new Vector2(1f, 1f);
    #endregion

    [Header("Layers & Tags")]
    int _groundLayer = (1<<(int)Layer.Ground);
    int _attackableLayer = (1<<(int)Layer.Monster) | (1<<(int)Layer.Breakable);

    void Awake()
    {
        _data = GetComponent<PlayerMovementData>();
        _anim = GetComponent<PlayerAnimation>();
        _stat = GetComponent<PlayerStat>();
        RB = GetComponent<Rigidbody2D>();

    }

    void Start()
    {
        SetGravityScale(_data._gravityScale);
        IsFacingRight = true;
    }
    void Update()
    {
        if(IsDead) return;

        #region Timers
        LastOnGroundTime -= Time.deltaTime;
        LastOnWallTime -= Time.deltaTime;
        LastOnWallRightTime -= Time.deltaTime;
        LastOnWallLeftTime -= Time.deltaTime;

        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;
        LastPressedAttackTime -= Time.deltaTime;
        #endregion

        #region Input
        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        if (_moveInput.x != 0)
            CheckDirectionToFace(_moveInput.x > 0);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnJumpInput();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnJumpUpInput();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            OnDashInput();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            _anim.StartedAttacking = true;
            OnAttackInput();
        }
        #endregion


        #region Collision Check

        if (!IsDashing && !IsJumping)
        {
            // 땅 체크
            if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer))
            {
                if (LastOnGroundTime < -0.1f)
                {
                    _anim.JustLanded = true;
                }

                LastOnGroundTime = _data._coyoteTime;
            }

            // 오른쪽 벽 체크
            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
                    || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)) && !IsWallJumping)
                LastOnWallRightTime = _data._coyoteTime;

            // 왼쪽 벽 체크
            if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
                || (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping)
                LastOnWallLeftTime = _data._coyoteTime;

            LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
        }
        #endregion

        #region Jump Check

        // 점프 시작
        if (IsJumping && RB.velocity.y < 0)
        {
            IsJumping = false;
            _isJumpFalling = true;
        }

        if (IsWallJumping && Time.time - _wallJumpStartTime > _data._wallJumpTime)
        {
            IsWallJumping = false;
        }

        // 착지
        if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
            _isJumpCut = false;
            _isJumpFalling = false;
        }

        if (!IsDashing)
        {
            //Jump
            if (CanJump() && LastPressedJumpTime > 0)
            {
                IsJumping = true;
                IsWallJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;
                Jump();

                _anim.StartedJumping = true;
            }
            // 벽 점프
            else if (CanWallJump() && LastPressedJumpTime > 0)
            {
                IsWallJumping = true;
                IsJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;

                _wallJumpStartTime = Time.time;
                _lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;

                WallJump(_lastWallJumpDir);
            }
        }
        #endregion

        #region Dash Check
        if (CanDash() && LastPressedDashTime > 0)
        {
            //Freeze game for split second. Adds juiciness and a bit of forgiveness over directional input
            Sleep(_data._dashSleepTime);

            //If not direction pressed, dash forward
            if (_moveInput != Vector2.zero)
                _lastDashDir = _moveInput;
            else
                _lastDashDir = IsFacingRight ? Vector2.right : Vector2.left;

            IsDashing = true;
            IsJumping = false;
            IsWallJumping = false;
            _isJumpCut = false;

            StartCoroutine(nameof(CoStartDash), _lastDashDir);
        }
        #endregion

        #region Slide Check
        if (CanSlide() && ((LastOnWallLeftTime > 0 && _moveInput.x < 0) || (LastOnWallRightTime > 0 && _moveInput.x > 0)))
            IsSliding = true;
        else
            IsSliding = false;
        #endregion

        #region Attack Check

        if (CanAttack())
        {
            IsAttacking = true;
            StartCoroutine(CoAttack());
        }

        #endregion

        #region Gravity
        if (!_isDashAttacking)
        {
            if (IsSliding)
            {
                SetGravityScale(0);
            }
            else if (RB.velocity.y < 0 && _moveInput.y < 0)
            {
                // 아래키 눌렀을 때 중력 증가
                SetGravityScale(_data._gravityScale * _data._fastFallGravityMult);
                //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -_data._maxFastFallSpeed));
            }
            else if (_isJumpCut)
            {
                // 점프키 놓았을 때 중력 증가
                SetGravityScale(_data._gravityScale * _data._jumpCutGravityMult);
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -_data._maxFallSpeed));
            }
            else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < _data._jumpHangTimeThreshold)
            {
                // 점프 정점 근처
                SetGravityScale(_data._gravityScale * _data._jumpHangGravityMult);
            }
            else if (RB.velocity.y < 0)
            {
                // 낙하 상태
                SetGravityScale(_data._gravityScale * _data._fallGravityMult);
                // 낙하 속도 제한
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -_data._maxFallSpeed));
            }
            else
            {
                // 기본 중력
                SetGravityScale(_data._gravityScale);
            }
        }
        else
        {
            // 대시 중
            SetGravityScale(0);
        }
        #endregion
    }

    void FixedUpdate()
    {
        if(IsDead) return;

        if (IsAttacking)
        {
            RB.velocity = Vector2.zero; 
            return;
        }

        if (!IsDashing)
        {
            if (IsWallJumping)
            {
                Run(_data._wallJumpRunLerp);
            }
            else
                Run(1);
        }
        else if (_isDashAttacking)
        {
            Run(_data._dashEndRunLerp);
        }

        if (IsSliding)
            Slide();
    }

    #region Input
    public void OnJumpInput()
    {
        LastPressedJumpTime = _data._jumpInputBufferTime;
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut() || CanWallJumpCut())
            _isJumpCut = true;
    }

    public void OnDashInput()
    {
        LastPressedDashTime = _data._dashInputBufferTime;
    }

    public void OnAttackInput()
    {
        LastPressedAttackTime = _data._attackInputBufferTime;
    }
    #endregion

    #region General Methods
    public void SetGravityScale(float scale)
    {
        RB.gravityScale = scale;
    }

    void Sleep(float duration)
    {
        StartCoroutine(nameof(CoSleep), duration);
    }

    IEnumerator CoSleep(float duration)
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = 1;
    }
    #endregion

    // Movement
    #region Run

    void Run(float lerpAmount)
    {
        if (IsAttacking) return;

        float targetSpeed = _moveInput.x * _data._runMaxSpeed;
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        #region 가속 계산
        // 가속 비율
        float accelRate;

        if (LastOnGroundTime > 0)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _data._runAccelAmount : _data._runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _data._runAccelAmount * _data._accelInAir : _data._runDeccelAmount * _data._deccelInAir; // 공중에 있을 때 가속도와 감속도
        #endregion

        #region 점프 연출
        // 점프 자연스럽게 연출
        if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < _data._jumpHangTimeThreshold)
        {
            accelRate *= _data._jumpHangAccelerationMult;
            targetSpeed *= _data._jumpHangMaxSpeedMult;
        }
        #endregion

        #region 관성
        // 관성
        if (_data._doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            // 현재 속도가 목표 속도보다 빠른지 확인
            // 현재 이동 방향과 목표 이동 방향이 같은지 확인
            accelRate = 0;
        }
        #endregion

        // 실제 적용될 속도
        float speedDif = targetSpeed - RB.velocity.x;
        // 적용되는 힘
        float movement = speedDif * accelRate;
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }
    private void Turn()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }
    void Slide()
    {
        if (RB.velocity.y > 0)
        {
            RB.AddForce(-RB.velocity.y * Vector2.up, ForceMode2D.Impulse);
        }
        float speedDif = _data._slideSpeed - RB.velocity.y;
        float movement = speedDif * _data._slideAccel;
        // 속도 제한
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        RB.AddForce(movement * Vector2.up);
    }
    #endregion

    #region Jump
    private void Jump()
    {
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0; // 공중에 있음

        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = _data._jumpForce;
        if (RB.velocity.y < 0)
            force -= RB.velocity.y;

        RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        #endregion
    }

    private void WallJump(int dir)
    {
        //Ensures we can't call Wall Jump multiple times from one press
        LastPressedJumpTime = 0;
        LastOnGroundTime = 0;
        LastOnWallRightTime = 0;
        LastOnWallLeftTime = 0;

        #region Perform Wall Jump
        Vector2 force = new Vector2(_data._wallJumpForce.x, _data._wallJumpForce.y);
        force.x *= dir; //apply force in opposite direction of wall

        if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
            force.x -= RB.velocity.x;

        if (RB.velocity.y < 0) //checks whether player is falling, if so we subtract the velocity.y (counteracting force of gravity). This ensures the player always reaches our desired jump force or greater
            force.y -= RB.velocity.y;

        //Unlike in the run we want to use the Impulse mode.
        //The default mode will apply are force instantly ignoring masss
        RB.AddForce(force, ForceMode2D.Impulse);
        #endregion
    }
    #endregion

    #region Dash
    private IEnumerator CoStartDash(Vector2 dir)
    {
        LastOnGroundTime = 0;
        LastPressedDashTime = 0;

        float startTime = Time.time;

        _dashesLeft--;
        _isDashAttacking = true;

        SetGravityScale(0);

        // 대시 지속 시간
        while (Time.time - startTime <= _data._dashAttackTime)
        {
            RB.velocity = dir.normalized * _data._dashSpeed;
            yield return null;
        }

        startTime = Time.time;

        _isDashAttacking = false;

        // 중력 복원
        SetGravityScale(_data._gravityScale);
        RB.velocity = _data._dashEndSpeed * dir.normalized;

        while (Time.time - startTime <= _data._dashEndTime)
        {
            yield return null;
        }

        //Dash over
        IsDashing = false;
    }

    //대시 쿨다운
    private IEnumerator RefillDash(int amount)
    {
        _dashRefilling = true;
        yield return new WaitForSeconds(_data._dashRefillTime);
        _dashRefilling = false;
        _dashesLeft = Mathf.Min(_data._dashAmount, _dashesLeft + 1);
    }
    #endregion

    #region Attack

    IEnumerator CoAttack()
    {
        Collider2D[] hit;
        hit = Physics2D.OverlapBoxAll(_frontAttackCheckPoint.position, _attackCheckSize, 0, _attackableLayer);


        if(hit.Length>0)
        {
            for (int i = 0; i < hit.Length; i++)
            {
                MonsterMovement monster = hit[i].GetComponent<MonsterMovement>();
                if(monster != null)
                {
                    monster.OnDamaged(_stat.CurrentAttack);
                }

                IBreakable breakable = hit[i].GetComponent<IBreakable>();
                if(breakable != null)
                {
                    breakable.OnDamaged(1); 
                }
            }
            Sleep(0.2f);
            CameraController.Instance.ShakeCamera();
        }

        Debug.Log($"Player Attack: {_stat.CurrentAttack}");

        float remainingTime = Helper.GetRemainingAnimationTime(_anim.Anim);
        yield return new WaitForSeconds(remainingTime);

        IsAttacking = false;
        LastPressedAttackTime = 0;
    }
    #endregion

    #region Damage
    // For animation, Use this method instead of the method in Sat class.
    public void OnDamaged(float damage)
    {
        _anim.Damaged = true;
        _stat.OnDamaged(damage);
        //Debug.Log($"Player current HP : {_stat.CurrentHp}");

        if(_stat.CurrentHp<=0)
        {
            OnDead();
        }

    }
    #endregion

    #region Dead

    void OnDead()
    {
        IsDead = true;
        _anim.OnDead();
    }
    #endregion
    #region Check
    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }

    bool CanJump()
    {
        return LastOnGroundTime > 0 && !IsJumping;
    }
    bool CanWallJump()
    {
        // 1. 일정 시간 내에 점프 입력이 들어왔는지
        // 2. 벽에 붙은 시간이 유효한지
        // 3. 땅에 붙어 있지않은지
        // 4. 현재 벽 점프 중이 아니거나, (오른쪽(왼쪽)벽에 붙어 있고, 이전 점프 방향이 오른쪽(왼쪽)인 경우)
        return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
             (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
    }

    bool CanJumpCut()
    {
        // 점프 도중 버튼을 놓았을 때 속도 줄임
        return IsJumping && RB.velocity.y > 0;
    }

    bool CanWallJumpCut()
    {
        // 벽점프 도중 버튼을 놓았을 때 속도 줄임
        return IsWallJumping && RB.velocity.y > 0;
    }

    private bool CanDash()
    {
        if (!IsDashing && _dashesLeft < _data._dashAmount && LastOnGroundTime > 0 && !_dashRefilling)
        {
            StartCoroutine(nameof(RefillDash), 1);
        }

        return _dashesLeft > 0;
    }

    public bool CanSlide()
    {
        if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && !IsDashing && LastOnGroundTime <= 0)
            return true;
        else
            return false;
    }

    public bool CanAttack()
    {
        if (LastPressedAttackTime > 0 && !IsDashing && !IsAttacking)
            return true;
        else
            return false;
    }
    #endregion

    #region Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
        Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_frontAttackCheckPoint.position, _attackCheckSize);
    }
    #endregion
}
