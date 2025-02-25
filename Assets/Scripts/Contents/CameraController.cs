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

    [Header("Zoom Settings")]
    [SerializeField] float _defaultFOV = 4f;
    [SerializeField] float _minFOV = 3.5f;
    [SerializeField] float _zoomInSpeed = 1f;
    [SerializeField] float _zoomOutSpeed = 1.5f;

    float _targetFOV;
    float _targetSpeed;

    Coroutine _coShakeCamera;
    Coroutine _coZoomCamera;
    WaitForSeconds _duration;

    protected override void Awake()
    {
        base.Awake();
        _duration = new WaitForSeconds(_shakeDuration);
        _playerFollowCam = GetComponent<CinemachineVirtualCamera>();
        _noise = _playerFollowCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        //_playerFollowCam.m_Lens.OrthographicSize = Mathf.MoveTowards(_playerFollowCam.m_Lens.FieldOfView, TESTFOV, _zoomInSpeed* Time.deltaTime);
    }

    #region Shake
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
    #endregion

    #region Zoom

    public void ZoomCamera(bool zoomIn = false)
    {
        _targetFOV = zoomIn ? _minFOV : _defaultFOV;
        _targetSpeed = zoomIn ? _zoomInSpeed : _zoomOutSpeed;

        if (_coZoomCamera != null)
        {
            StopCoroutine(_coZoomCamera);
        }

        _coZoomCamera = StartCoroutine(CoZoomCamera(_targetFOV, _targetSpeed));
    }
    IEnumerator CoZoomCamera(float fov, float speed)
    {
        while (!Mathf.Approximately(_playerFollowCam.m_Lens.OrthographicSize, fov))
        {
            _playerFollowCam.m_Lens.OrthographicSize = Mathf.MoveTowards(_playerFollowCam.m_Lens.OrthographicSize, fov, speed * Time.deltaTime);
            yield return null;
        }

        _coZoomCamera = null;
    }

    #endregion
}
