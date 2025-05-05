using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestCode : MonoBehaviour
{
    public event Action<int, int> OnPlayerMove;

    private void Start()
    {
        InvokeRepeating("RepeationgPlayerMove", 0, 3);
    }
    void PlayerMove(int x,int y)
    {
        OnPlayerMove?.Invoke(x,y);
    }

    void RepeationgPlayerMove()
    {
        PlayerMove((int)transform.position.x,(int)transform.position.y);
    }

}
