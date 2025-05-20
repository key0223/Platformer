using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInput Input { get; private set; }

    void Awake()
    {
        Input = GetComponent<PlayerInput>();
        
    }
}
