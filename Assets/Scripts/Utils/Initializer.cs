using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Initializer 
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        Debug.Log("Loaded by the Persist Objects from the Initializer script");
        Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Prefabs/PERSIST OBJECTS")));
    }
}
