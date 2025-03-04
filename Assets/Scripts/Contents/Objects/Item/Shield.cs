using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    Transform _target;

    void Awake()
    {
        _target = FindObjectOfType<PlayerMovement>().transform;
    }

    void OnEnable()
    {
        StartCoroutine(CoCreateFX());
    }
  
    IEnumerator CoCreateFX()
    {
        yield return new WaitForSeconds(0.025f);

        ParticleMove effect = ResourceManager.Instance.Instantiate("FX/ShieldFX").gameObject.GetComponent<ParticleMove>();
        effect.gameObject.transform.position = transform.position;
        effect.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.3f);

        effect.Init(_target);
    }
}
