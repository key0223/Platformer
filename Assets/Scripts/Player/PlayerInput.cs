using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public event Action OnJumpInputDown;
    public event Action OnJumpInputUp;
    public event Action OnDashInput;
    public event Action OnAttackInput;
    public event Action OnHealInputUp;

    public bool IsHealPressed { get; private set; }

    private void Update()
    {
        float x = (Input.GetKey(KeyCode.RightArrow) ? 1 : 0) - (Input.GetKey(KeyCode.LeftArrow) ? 1 : 0);
        float y = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0)  - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);

        MoveInput = new Vector2(x, y);

        if (Input.GetKeyDown(KeyCode.Z)) OnJumpInputDown?.Invoke();
        if(Input.GetKeyUp(KeyCode.Z)) OnJumpInputUp?.Invoke();
        if(Input.GetKeyDown(KeyCode.C)) OnDashInput?.Invoke();
        if(Input.GetKeyDown(KeyCode.X)) OnAttackInput?.Invoke();

        IsHealPressed = Input.GetKey(KeyCode.A);
        if(Input.GetKeyUp(KeyCode.A)) OnHealInputUp?.Invoke();

    }
}
