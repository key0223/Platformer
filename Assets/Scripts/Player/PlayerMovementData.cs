using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementData : MonoBehaviour
{
    [Header("Gravity")]
    [HideInInspector] public float _gravityStrength; 
    [HideInInspector] public float _gravityScale; 

    public float _fallGravityMult; // �ϰ� �� �߷� ����
    public float _maxFallSpeed; // �ִ� �ϰ� �ӵ�
    [Space(5)]
    public float _fastFallGravityMult; // �Ʒ����� Ű ������ �� �߷� ����
    public float _maxFastFallSpeed; // ������ ������ �� �ִ� �ϰ� �ӵ� 

    [Space(20)]

    [Header("Run")]
    public float _runMaxSpeed;
    public float _runAcceleration; // ����
    [HideInInspector] public float _runAccelAmount; // ���� ����Ǵ� ��
    public float _runDecceleration; // ����
    [HideInInspector] public float _runDeccelAmount;
    [Space(5)]
    [Range(0f, 1)] public float _accelInAir; // ���߿����� ����
    [Range(0f, 1)] public float _deccelInAir; // ���߿����� ����
    [Space(5)]
    public bool _doConserveMomentum = true; // ���� ����

    [Space(20)]

    [Header("Jump")]
    public float _jumpHeight; // ���� ����
    public float _jumpTimeToApex; // ���� ���̱��� �����ϴ� �ð�
    [HideInInspector] public float _jumpForce; // ���� ����Ǵ� ��

    [Header("Both Jumps")]
    public float _jumpCutGravityMult; // ���� ���� ����Ű�� ������ �� ĳ���Ͱ� ������ �ϰ��ϵ��� �߷��� ������ ����
    [Range(0f, 1)] public float _jumpHangGravityMult; //  ���� ����(����)�� �������� �߷� ����
    public float _jumpHangTimeThreshold; // �������� ���ߴ� �ð�
    [Space(0.5f)]
    public float _jumpHangAccelerationMult;
    public float _jumpHangMaxSpeedMult;

    [Header("Wall Jump")]
    public Vector2 _wallJumpForce; // ���� ����Ǵ� ��
    [Space(5)]
    [Range(0f, 1f)] public float _wallJumpRunLerp; // ���� �� �����̴� �ӵ� ����
    [Range(0f, 1.5f)] public float _wallJumpTime; // �� ���� �� ������ �ӵ��� �������� �ð�
    public bool _doTurnOnWallJump; // �� ���� �� �÷��̾� ���� �ٲ� ����

    [Space(20)]

    [Header("Slide")]
    public float _slideSpeed; 
    public float _slideAccel; // �����̵� ����

    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float _coyoteTime; // ������ ������ �Ŀ��� ������ �� �ִ� �ð�
    [Range(0.01f, 0.5f)] public float _jumpInputBufferTime; // ���� �Է��� �޴� �ð� (Ÿ�̹��� ���� ��߳��� ������ �����ϰ� ����)

    [Space(20)]

    [Header("Dash")]
    public int _dashAmount;
    public float _dashSpeed;
    public float _dashSleepTime; // Ű�� �޾��� �� ��� ���ߴ� �ð�
    [Space(5)]
    public float _dashAttackTime;
    [Space(5)]
    public float _dashEndTime; // ��ð� ������ �����ϴ� �ð�
    public Vector2 _dashEndSpeed; // �ε巴�� ����
    [Range(0f, 1f)] public float _dashEndRunLerp; //��� �� �÷��̾��� ���� �ӵ� ȸ�� ����
    [Space(5)]
    public float _dashRefillTime;
    [Space(5)]
    [Range(0.01f, 0.5f)] public float _dashInputBufferTime;

    [Space(5)]
    [Range(0.01f, 0.5f)] public float _attackInputBufferTime;


    //Unity Callback, called when the inspector updates
    private void OnValidate()
    {
        // �߷� ���
        _gravityStrength = -(2 * _jumpHeight) / (_jumpTimeToApex * _jumpTimeToApex);

        // ������ٵ� �߷� ������ ���
        _gravityScale = _gravityStrength / Physics2D.gravity.y;

        // �޸��� ���ӵ�, ���ӵ� ���
        _runAccelAmount = (50 * _runAcceleration) / _runMaxSpeed;
        _runDeccelAmount = (50 * _runDecceleration) / _runMaxSpeed;

        // ���� �� ���
        _jumpForce = Mathf.Abs(_gravityStrength) * _jumpTimeToApex;

        #region Variable Ranges
        _runAcceleration = Mathf.Clamp(_runAcceleration, 0.01f, _runMaxSpeed);
        _runDecceleration = Mathf.Clamp(_runDecceleration, 0.01f, _runMaxSpeed);
        #endregion
    }
}
