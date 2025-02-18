using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SingletonMonobehaviour<CameraController>
{
    CinemachineVirtualCamera _playerFollowCam;
    CinemachineBasicMultiChannelPerlin _noise;

    [Header("Shake")]
    [SerializeField] float _shakeStrength = 1f;
    [SerializeField] float _shakeSpeed = 1f;
    [SerializeField] float _shakeDuration = 1f;
    Coroutine _coShakeCamera;
    WaitForSeconds _duration;

    protected override void Awake()
    {
        base.Awake();
        _duration = new WaitForSeconds(_shakeDuration);
        _playerFollowCam = GetComponent<CinemachineVirtualCamera>();
        _noise = _playerFollowCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera()
    {
        if (_coShakeCamera == null)
        {
            _coShakeCamera = StartCoroutine(CoShakeCamera());
        }
    }

    IEnumerator CoShakeCamera()
    {
        StartShakeCamera();
        yield return _duration;
        StopShakeCamera();

        _coShakeCamera = null;
    }
    void StartShakeCamera()
    {
        _noise.m_AmplitudeGain = _shakeStrength;
        _noise.m_FrequencyGain = _shakeSpeed;

    }
    void StopShakeCamera()
    {
        _noise.m_AmplitudeGain = 0;
        _noise.m_FrequencyGain = 0;
    }
}
