using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GroundState : MonoBehaviour
{
    
    readonly List<GroundCheck> checks = new();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        checks.AddRange(gameObject.GetComponentsInChildren<GroundCheck>());
        if(checks.Count == 0)
        {
            throw new System.Exception("No ground checks on game object!!!!");
        }
    }

    public bool IsGrounded()
    {
        return checks.Exists(c => c.IsTouchingGround());
    }
}
