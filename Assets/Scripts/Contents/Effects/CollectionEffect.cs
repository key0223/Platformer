using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectionEffect : MonoBehaviour
{
    float _moveSpeed = 5f;
    [SerializeField] float _carryValue;
    public float CarryValue {  get { return _carryValue; } set {  _carryValue = value; } }

    //PlayerMovement _playerMovement;
    PlayerController _playerController;

    RectTransform _target;
    Vector3 _targetPos;

    private void Awake()
    {
        //_playerMovement = FindObjectOfType<PlayerMovement>();
        _playerController = FindObjectOfType<PlayerController>();
    }
    public void EffectStart(Vector2 start, RectTransform target, float range)
    {
        start = start + new Vector2(0, 1) + Random.insideUnitCircle * range;
        transform.position = start;

        _target = target;
    }
    void Update()
    {
        if(_target != null)
        {
            RectTransformUtility.ScreenPointToWorldPointInRectangle(_target, _target.position, Camera.main, out _targetPos);
            transform.position = Vector3.MoveTowards(transform.position, _targetPos, _moveSpeed* Time.deltaTime);

            if((transform.position - _targetPos).sqrMagnitude < 0.05f)
            {
                StartCoroutine(CoDestoryEffect());
            }
        }
    }

    IEnumerator CoDestoryEffect()
    {
        _playerController.PlayerHealth.OnHpHeal(CarryValue);
        GameObject destroyEffect = ResourceManager.Instance.Instantiate("FX/Collection DestroyFX");
        destroyEffect.gameObject.SetActive(true);
        destroyEffect.transform.position = transform.position;

        Destroy(gameObject);
        yield return null;
    }
}
