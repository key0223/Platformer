using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementData : MonoBehaviour
{
    [Header("Gravity")]
    [HideInInspector] public float _gravityStrength; 
    [HideInInspector] public float _gravityScale; 

    public float _fallGravityMult; // 하강 시 중력 비율
    public float _maxFallSpeed; // 최대 하강 속도
    [Space(5)]
    public float _fastFallGravityMult; // 아래방향 키 눌렀을 때 중력 비율
    public float _maxFastFallSpeed; // 빠르게 떨어질 때 최대 하강 속도 

    [Space(20)]

    [Header("Run")]
    public float _runMaxSpeed;
    public float _runAcceleration; // 가속
    [HideInInspector] public float _runAccelAmount; // 실제 적용되는 힘
    public float _runDecceleration; // 감속
    [HideInInspector] public float _runDeccelAmount;
    [Space(5)]
    [Range(0f, 1)] public float _accelInAir; // 공중에서의 가속
    [Range(0f, 1)] public float _deccelInAir; // 공중에서의 감속
    [Space(5)]
    public bool _doConserveMomentum = true; // 관성 여부

    [Space(20)]

    [Header("Jump")]
    public float _jumpHeight; // 점프 높이
    public float _jumpTimeToApex; // 점프 높이까지 도달하는 시간
    [HideInInspector] public float _jumpForce; // 실제 적용되는 힘

    [Header("Both Jumps")]
    public float _jumpCutGravityMult; // 점프 도중 점프키를 놓았을 때 캐릭터가 빠르게 하강하도록 중력의 강도를 증가
    [Range(0f, 1)] public float _jumpHangGravityMult; //  점프 높이(정점)에 가까울수록 중력 감소
    public float _jumpHangTimeThreshold; // 정점에서 멈추는 시간
    [Space(0.5f)]
    public float _jumpHangAccelerationMult;
    public float _jumpHangMaxSpeedMult;

    [Header("Wall Jump")]
    public Vector2 _wallJumpForce; // 실제 적용되는 힘
    [Space(5)]
    [Range(0f, 1f)] public float _wallJumpRunLerp; // 점프 중 움직이는 속도 보간
    [Range(0f, 1.5f)] public float _wallJumpTime; // 벽 점프 후 움직임 속도가 느려지는 시간
    public bool _doTurnOnWallJump; // 벽 점프 시 플레이어 방향 바꿈 여부

    [Space(20)]

    [Header("Slide")]
    public float _slideSpeed; 
    public float _slideAccel; // 슬라이드 가속

    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float _coyoteTime; // 땅에서 떨어진 후에도 점프할 수 있는 시간
    [Range(0.01f, 0.5f)] public float _jumpInputBufferTime; // 점프 입력을 받는 시간 (타이밍이 조금 어긋나도 점프가 가능하게 만듬)

    [Space(20)]

    [Header("Dash")]
    public int _dashAmount;
    public float _dashSpeed;
    public float _dashSleepTime; // 키를 받았을 때 잠깐 멈추는 시간
    [Space(5)]
    public float _dashAttackTime;
    [Space(5)]
    public float _dashEndTime; // 대시가 끝나고 감속하는 시간
    public Vector2 _dashEndSpeed; // 부드럽게 감속
    [Range(0f, 1f)] public float _dashEndRunLerp; //대시 후 플레이어의 원래 속도 회복 조절
    [Space(5)]
    public float _dashRefillTime;
    [Space(5)]
    [Range(0.01f, 0.5f)] public float _dashInputBufferTime;

    [Space(5)]
    [Range(0.01f, 0.5f)] public float _attackInputBufferTime;


    //Unity Callback, called when the inspector updates
    private void OnValidate()
    {
        // 중력 계산
        _gravityStrength = -(2 * _jumpHeight) / (_jumpTimeToApex * _jumpTimeToApex);

        // 리지드바디 중력 스케일 계산
        _gravityScale = _gravityStrength / Physics2D.gravity.y;

        // 달리기 가속도, 감속도 계산
        _runAccelAmount = (50 * _runAcceleration) / _runMaxSpeed;
        _runDeccelAmount = (50 * _runDecceleration) / _runMaxSpeed;

        // 점프 힘 계싼
        _jumpForce = Mathf.Abs(_gravityStrength) * _jumpTimeToApex;

        #region Variable Ranges
        _runAcceleration = Mathf.Clamp(_runAcceleration, 0.01f, _runMaxSpeed);
        _runDecceleration = Mathf.Clamp(_runDecceleration, 0.01f, _runMaxSpeed);
        #endregion
    }
}
