using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected Rigidbody2D _rigid;

    [SerializeField] protected float _speed = 1f;
    [SerializeField] protected float _damage = 1f;

   


    protected virtual void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        SceneChangeManager.Instance.OnSceneChanged += OnSceneChanged;
    }
    void OnDisable()
    {
        SceneChangeManager.Instance.OnSceneChanged -= OnSceneChanged;
    }

    void OnSceneChanged()
    {
        ResourceManager.Instance.Destroy(gameObject);
    }
}
