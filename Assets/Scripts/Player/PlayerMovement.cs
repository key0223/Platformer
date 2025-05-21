using System.Collections;
using UnityEngine;
using static Define;

public class PlayerMovement : MonoBehaviour
{
    PlayerController _controller;
    PlayerMovementData _data;
    PlayerStat _stat;
    PlayerAnimation _anim;
    PlayerAction _action;

    [Header("Layers & Tags")]
    int _groundLayer = (1 << (int)Layer.Ground);


    public PlayerStat Stat { get { return _stat; } }
    public Rigidbody2D RB { get; private set; }

    Vector2 _moveInput;

    #region State Parameters
    public bool IsFacingRight { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsWallJumping { get; private set; }
    public bool IsDashing { get; private set; }
    public bool IsSliding { get; private set; }

    bool _canMove = true;

    // Buffer
    BufferManager _bufferManager = new BufferManager();

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

    #region Check Parameters
    [Header("Checks")]
    [SerializeField] Transform _groundCheckPoint;
    [SerializeField] Vector2 _groundCheckSize = new Vector2(0.5f, 0.5f);
    [Space(5)]
    [SerializeField] Transform _frontWallCheckPoint;
    [SerializeField] Transform _backWallCheckPoint;
    [SerializeField] Vector2 _wallCheckSize = new Vector2(0.5f, 1f);

    #endregion

    void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _data = GetComponent<PlayerMovementData>();
        _stat = GetComponent<PlayerStat>();
        _anim = GetComponent<PlayerAnimation>();
        _action = _controller.PlayerAction;

        RB = GetComponent<Rigidbody2D>();

        SubscribeEvent();
        InitBuffers();
    }

    // Subscribe to events
    void SubscribeEvent()
    {
        _controller.Input.OnJumpInputDown += OnJumpInput;
        _controller.Input.OnJumpInputUp += OnJumpUpInput;
        _controller.Input.OnDashInput += OnDashInput;
        _controller.Input.OnAttackInput += _action.OnAttackInput;
    }

    void InitBuffers()
    {
        _bufferManager.Add(BufferType.Ground, _data._coyoteTime);
        _bufferManager.Add(BufferType.WallRight, _data._coyoteTime);
        _bufferManager.Add(BufferType.WallLeft, _data._coyoteTime);
        _bufferManager.Add(BufferType.Jump, _data._jumpInputBufferTime);
        _bufferManager.Add(BufferType.Dash, _data._dashInputBufferTime);
    }
    void Start()
    {
        UIManager.Instance.OnUIStateChanged += SetMovementEnabled;

        SetGravityScale(_data._gravityScale);
        IsFacingRight = true;

    }
    void Update()
    {
        if (_controller.PlayerHealth.IsDead) return;
        if (!_canMove) return;

        // Buffer
        _bufferManager.UpdateAll(Time.deltaTime);
       
        // Input
        _moveInput = _controller.Input.MoveInput;

        if (_moveInput.x != 0)
            CheckDirectionToFace(_moveInput.x > 0);

        CheckCollision();
        HandleJump();
        HandleDash();
        HandleSlide();
        HandleAttack();
        HandleGravity();
    }

    void CheckCollision()
    {
        if (!IsDashing && !IsJumping)
        {

            Collider2D groundHit = Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer);
            // 땅 체크
            if (groundHit)
            {
                if (_bufferManager.Get(BufferType.Ground).Timer < -0.1f)
                {
                    _anim.JustLanded = true;
                }

                _bufferManager.Get(BufferType.Ground).Set();
            }

            Collider2D rightWallHit = Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer);
            Collider2D leftWallHit = Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer);

            if (!IsWallJumping)
            {
                // 오른쪽 벽 체크
                if ((rightWallHit && IsFacingRight) || (leftWallHit && !IsFacingRight))
                    _bufferManager.Get(BufferType.WallRight).Set();

                // 왼쪽 벽 체크
                if ((rightWallHit && !IsFacingRight) || (leftWallHit && IsFacingRight))
                    _bufferManager.Get(BufferType.WallLeft).Set();
            }
        }
    }

    void HandleJump()
    {
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
        if (_bufferManager.Get(BufferType.Ground).IsActive && !IsJumping && !IsWallJumping)
        {
            _isJumpCut = false;
            _isJumpFalling = false;
        }

        if (!IsDashing)
        {
            //Jump
            if (CanJump() && _bufferManager.Get(BufferType.Jump).IsActive)
            {
                IsJumping = true;
                IsWallJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;
                Jump();

                _anim.StartedJumping = true;
            }
            // 벽 점프
            else if (CanWallJump() && _bufferManager.Get(BufferType.Jump).IsActive)
            {
                IsWallJumping = true;
                IsJumping = false;
                _isJumpCut = false;
                _isJumpFalling = false;

                _wallJumpStartTime = Time.time;
                _lastWallJumpDir = (_bufferManager.Get(BufferType.WallRight).IsActive) ? -1 : 1;

                WallJump(_lastWallJumpDir);
            }
        }
    }
    void HandleDash()
    {
        if (CanDash() && _bufferManager.Get(BufferType.Dash).IsActive)
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
    }
    void HandleSlide()
    {
        // Wall Slide

        if (CanSlide() && ((_bufferManager.Get(BufferType.WallLeft).IsActive && _moveInput.x < 0) || (_bufferManager.Get(BufferType.WallRight).IsActive && _moveInput.x > 0)))
            IsSliding = true;
        else
            IsSliding = false;
    }
    void HandleAttack()
    {
        if (_action.CanAttack(IsDashing))
        {
            StartCoroutine(_action.CoAttack(Sleep));
        }
    }
    void HandleGravity()
    {
        if (!_isDashAttacking)
        {
            if (IsSliding)
                SetGravityScale(0);
            else if (RB.velocity.y < 0 && _moveInput.y < 0)
            {
                // 아래키 눌렀을 때 중력 증가
                SetGravityScale(_data._gravityScale * _data._fastFallGravityMult);
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -_data._maxFastFallSpeed)); // 낙하 속도 제한
            }
            else if (_isJumpCut)
            {
                // 점프키 놓았을 때 중력 증가
                SetGravityScale(_data._gravityScale * _data._jumpCutGravityMult);
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -_data._maxFallSpeed));
            }
            else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < _data._jumpHangTimeThreshold)
            {
                SetGravityScale(_data._gravityScale * _data._jumpHangGravityMult); // 점프 정점 근처
            }
            else if (RB.velocity.y < 0)
            {
                SetGravityScale(_data._gravityScale * _data._fallGravityMult); // 낙하 상태
                RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -_data._maxFallSpeed)); // 낙하 속도 제한
            }
            else
                SetGravityScale(_data._gravityScale); // 기본 중력
        }
        else
            SetGravityScale(0); // 대시 중
    }
    void FixedUpdate()
    {
        if (_controller.PlayerHealth.IsDead) return;

        if (!_canMove || _action.IsAttacking)
        {
            RB.velocity = new Vector2(0, RB.velocity.y);
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
    public void SetMovementEnabled(bool isUIOn)
    {
        _canMove = !isUIOn;

        if (!_canMove)
        {
            RB.velocity = new Vector2(0, RB.velocity.y);
        }
    }
    public void OnJumpInput()
    {
        _bufferManager.Get(BufferType.Jump).Set();
    }

    public void OnJumpUpInput()
    {
        if (CanJumpCut() || CanWallJumpCut())
            _isJumpCut = true;
    }

    public void OnDashInput()
    {
        _bufferManager.Get(BufferType.Dash).Set();
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
    void Run(float lerpAmount)
    {
        if (_action.IsAttacking) return;

        float targetSpeed = _moveInput.x * _data._runMaxSpeed;
        targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);

        // 가속 비율
        float accelRate;

        if (_bufferManager.Get(BufferType.Ground).IsActive)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _data._runAccelAmount : _data._runDeccelAmount;
        else
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? _data._runAccelAmount * _data._accelInAir : _data._runDeccelAmount * _data._deccelInAir; // 공중에 있을 때 가속도와 감속도

        // 점프 자연스럽게 연출
        if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < _data._jumpHangTimeThreshold)
        {
            accelRate *= _data._jumpHangAccelerationMult;
            targetSpeed *= _data._jumpHangMaxSpeedMult;
        }

        // 관성
        if (_data._doConserveMomentum 
            && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) 
            && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) 
            && Mathf.Abs(targetSpeed) > 0.01f 
            && !_bufferManager.Get(BufferType.Ground).IsActive)
        {
            // 현재 속도가 목표 속도보다 빠른지 확인
            // 현재 이동 방향과 목표 이동 방향이 같은지 확인
            accelRate = 0;
        }

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
            RB.AddForce(-RB.velocity.y * Vector2.up, ForceMode2D.Impulse);

        float speedDif = _data._slideSpeed - RB.velocity.y;
        float movement = speedDif * _data._slideAccel;

        // 속도 제한
        movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

        RB.AddForce(movement * Vector2.up);
    }

    private void Jump()
    {
        _bufferManager.Get(BufferType.Jump).Reset();
        _bufferManager.Get(BufferType.Ground).Reset();

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
        _bufferManager.Get(BufferType.Jump).Reset();
        _bufferManager.Get(BufferType.Ground).Reset();
        _bufferManager.Get(BufferType.WallRight).Reset();
        _bufferManager.Get(BufferType.WallLeft).Reset();

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

    #region Dash
    private IEnumerator CoStartDash(Vector2 dir)
    {
        _bufferManager.Get(BufferType.Ground).Reset();
        _bufferManager.Get(BufferType.Dash).Reset();

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

    #region Check
    public void CheckDirectionToFace(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Turn();
    }

    bool CanJump()
    {
        /* LastOnGroundTime  = 플레이어가 마지막으로 땅에 닿은 뒤 경과한 시간 */
        return _bufferManager.Get(BufferType.Ground).IsActive && !IsJumping && !_action.IsAttacking;
    }
    bool CanWallJump()
    {
        // 1. 일정 시간 내에 점프 입력이 들어왔는지
        // 2. 벽에 붙은 시간이 유효한지
        // 3. 땅에 붙어 있지않은지
        // 4. 현재 벽 점프 중이 아니거나, (오른쪽(왼쪽)벽에 붙어 있고, 이전 점프 방향이 오른쪽(왼쪽)인 경우)
        return _bufferManager.Get(BufferType.Jump).IsActive
            && (_bufferManager.Get(BufferType.WallLeft).IsActive || _bufferManager.Get(BufferType.WallRight).IsActive)
            && !_bufferManager.Get(BufferType.Ground).IsActive
            && (!IsWallJumping
            || (_bufferManager.Get(BufferType.WallRight).IsActive && _lastWallJumpDir == 1)
            || (_bufferManager.Get(BufferType.WallLeft).IsActive && _lastWallJumpDir == -1));
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
        if (!IsDashing && _dashesLeft < _data._dashAmount && _bufferManager.Get(BufferType.Ground).IsActive && !_dashRefilling)
        {
            StartCoroutine(nameof(RefillDash), 1);
        }

        return _dashesLeft > 0;
    }

    public bool CanSlide()
    {
        return (_bufferManager.Get(BufferType.WallLeft).IsActive || _bufferManager.Get(BufferType.WallRight).IsActive)
            && !IsJumping && !IsWallJumping && !IsDashing
            && !_bufferManager.Get(BufferType.Ground).IsActive;
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
    }
    #endregion
}
