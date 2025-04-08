using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcAnimationBase : MonoBehaviour
{
    protected Animator _anim;
    public Animator Anim { get {  return _anim; } }

    protected virtual void Start()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LateUpdate()
    {
        CheckAnimationState();
    }

    protected virtual void CheckAnimationState()
    {

    }
}
