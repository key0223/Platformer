using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;
using static Define;
public class Laser : MonoBehaviour
{
    PlayerController _target = null;
    EyeSlimeMovement _eyeSlime;

    LineRenderer _lineRenderer;
    Color _laserStarColor;
    Color _laserEndColor;

    [Space(5)]
    [Header("Laser Settings")]
    [SerializeField] Transform _laserPoint;
    [SerializeField] GameObject _laserReadyFXPrefab;
    [SerializeField] ParticleSystem _laserHitFX;
    [SerializeField] float _laserDuration = 0.5f;
    [SerializeField] float _laserFadeDuration = 0.2f;
    [SerializeField] float _laserMaxLength = 5f;

    bool _targetInPosition = false;
    bool _usingLaser = false;

    float _damage = 1f;
    float _damageInterval = 0.2f;
    float _elapsedTime = 0;


    void Awake()
    {
        _eyeSlime = GetComponentInParent<EyeSlimeMovement>();
        _lineRenderer = GetComponent<LineRenderer>();
        _laserStarColor = _lineRenderer.startColor;
        _laserEndColor = _lineRenderer.endColor;
    }

    void Update()
    {
        if (_target != null)
        {
            _elapsedTime += Time.deltaTime;
            if (_elapsedTime >= _damageInterval && _usingLaser&& _targetInPosition)
            {
                Attack();
                _elapsedTime = 0;
            }
        }

        if(_usingLaser )
        {
            float laserLength;
            if (_targetInPosition && _target != null)
            {
                laserLength =(_target.transform.position - transform.position).magnitude;

                _laserHitFX.gameObject.transform.position = new Vector3(_target.transform.position.x, _laserPoint.position.y, _target.transform.position.z);
                if (!_laserHitFX.isPlaying)
                    _laserHitFX.Play();
            }
            else
            {
                laserLength = _laserMaxLength;
            }

            Vector3 dir = _eyeSlime.IsFacingRight ? Vector3.right : Vector3.left;
            Vector3 endPoint = _laserPoint.position + dir * laserLength;
            _lineRenderer.SetPosition(0, _laserPoint.position);
            _lineRenderer.SetPosition(1, endPoint);
        }
    }
    void Attack()
    {
        _target.PlayerHealth.OnDamaged(_damage);
    }

    #region Laser
    public IEnumerator CoActivateLaser()
    {
        _lineRenderer.startColor = _laserStarColor;
        _lineRenderer.endColor = _laserEndColor;

        GameObject readyFx = Instantiate(_laserReadyFXPrefab);
        readyFx.transform.SetParent(_laserPoint, false);

        ParticleSystem particle = readyFx.GetComponent<ParticleSystem>();
        float duration = particle.main.duration * 0.6f;
        yield return Helper.GetWait(duration);

        _lineRenderer.enabled = true;

        _usingLaser = true;


        yield return new WaitForSeconds(_laserDuration);

        // FadeOut
        for (float t = 0; t <= 1; t += Time.deltaTime / _laserFadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, t);
            UpdateLineRendererAlpha(alpha);
            yield return null;
        }

        _lineRenderer.enabled = false;
        _usingLaser = false;
    }

    void UpdateLineRendererAlpha(float alpha)
    {
        Color startColor = _lineRenderer.startColor;
        Color endColor = _lineRenderer.endColor;

        startColor.a = alpha;
        endColor.a = alpha;

        _lineRenderer.startColor = startColor;
        _lineRenderer.endColor = endColor;

    }
    #endregion

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_PLAYER))
        {
            _targetInPosition = true;
            _target = collision.gameObject.GetComponent<PlayerController>();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(TAG_PLAYER))
        {
            _laserHitFX.Stop();
            _targetInPosition = false;
            _target = null;
        }
    }

}
