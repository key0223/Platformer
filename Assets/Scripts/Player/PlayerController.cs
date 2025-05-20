using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerMovementData Data { get; private set; }
    public PlayerInput Input { get; private set; }
    public PlayerAnimation Anim { get; private set; }
    public PlayerAction PlayerAction { get; private set; }
    public PlayerStat PlayerStat { get; private set; }
    public PlayerHealth PlayerHealth { get; private set; }  

    void Awake()
    {
        Data = GetComponent<PlayerMovementData>();
        Input = GetComponent<PlayerInput>();
        Anim = GetComponent<PlayerAnimation>();
        PlayerAction = GetComponent<PlayerAction>();
        PlayerStat = GetComponent<PlayerStat>();
        PlayerHealth = GetComponent<PlayerHealth>();
     
        Init();
    }

    void Init()
    {
        Anim.Init(this);
        PlayerAction.Init(this,Data, PlayerStat, Anim);
        PlayerHealth.Init(PlayerStat, Anim);
    }
}
