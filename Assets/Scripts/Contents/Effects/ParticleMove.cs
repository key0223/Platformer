using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleMove : MonoBehaviour
{
    #region Event
    public event Action OnPariticleDeath;
    #endregion

    ParticleSystem _particleSystem;
    ParticleSystem.Particle[] _particles;

    [SerializeField] Transform _target;
    [SerializeField] float _moveSpeed;

    [SerializeField] string _destroyFXPath;


    void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    public void Init(Transform target)
    {
        _target = target;
    }
    void Update()
    {
        if(_target == null) return;

        int maxParticles = _particleSystem.main.maxParticles;
        if(_particles == null || _particles.Length<maxParticles)
        {
            _particles = new ParticleSystem.Particle[maxParticles];
        }

        int particleCount = _particleSystem.GetParticles(_particles);

        for(int i = 0; i < particleCount; i++)
        {
            Vector3 dir = ((_target.position +new Vector3(0,0.5f)) - _particles[i].position).normalized;
            _particles[i].position += dir * _moveSpeed * Time.deltaTime;

            float dis = ((_target.position + new Vector3(0, 0.5f)) - _particles[i].position).magnitude;
            if(dis <0.3f)
            {
                GameObject destroyFX = ResourceManager.Instance.Instantiate(_destroyFXPath);
                destroyFX.transform.position = _particles[i].position;
                destroyFX.gameObject.SetActive(true);

                _particles[i].remainingLifetime = 0f;
                
            }

        }

        _particleSystem.SetParticles(_particles, particleCount);

        if(!_particleSystem.IsAlive())
        {
            OnPariticleDeath?.Invoke();
            ResourceManager.Instance.Destroy(gameObject);
        }
    }
}
