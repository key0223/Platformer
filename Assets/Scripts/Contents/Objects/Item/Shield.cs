using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    PlayerController _playerController;

    Transform _target;

    ParticleMove _particleMove;

    void Awake()
    {
        _playerController = FindObjectOfType<PlayerController>();
        _target = _playerController.transform;
    }

    void OnEnable()
    {
        StartCoroutine(CoCreateFX());
    }
  
    IEnumerator CoCreateFX()
    {
        yield return new WaitForSeconds(0.025f);

        _particleMove = ResourceManager.Instance.Instantiate("FX/ShieldFX").gameObject.GetComponent<ParticleMove>();
        _particleMove.OnPariticleDeath += AddShieldToPlayer;
        _particleMove.gameObject.transform.position = transform.position;
        _particleMove.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        _particleMove.Init(_target);
    }

    void AddShieldToPlayer()
    {
        _playerController.PlayerHealth.OnAddShield();
        _particleMove.OnPariticleDeath -= AddShieldToPlayer;
        _particleMove = null;

        ResourceManager.Instance.Destroy(gameObject);
    }
}
