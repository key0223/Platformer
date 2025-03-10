using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SingletonMonobehaviour<ResourceManager>
{
    protected override void Awake()
    {
        base.Awake();
    }

    public T Load<T> (string path) where T : Object
    {
        if(typeof(T)== typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');
            if (index >= 0)
                name = name.Substring(index + 1);

            // 만들어진 오브젝트가 있으면 풀에서 땡겨온다.
            GameObject go = PoolManager.Instance.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if(original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
            return PoolManager.Instance.Pop(original, parent).gameObject;

        // if it's not poolable
        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null) return;

        Poolable poolable = go.GetComponent<Poolable>();
        if(poolable != null)
        {
            PoolManager.Instance.Push(poolable);
            return;
        }
        Object.Destroy(go);
    }
}
