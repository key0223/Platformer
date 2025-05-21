using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBuffer 
{
    public float Timer { get; private set; }
    public float BufferTime {  get; private set; }
    public bool IsActive => Timer > 0f; // 입력이 유효한 상태

    public InputBuffer (float bufferTime)
    {
        BufferTime = bufferTime;
        Timer = 0f;
    }

    public void Set()
    {
        Timer = BufferTime;
    }

    public void Update(float deletaTime)
    {
        Timer -= deletaTime;
    }

    public void Reset()
    {
        Timer = 0f;
    }
}
