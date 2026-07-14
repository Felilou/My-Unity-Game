
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Unity.Scripting.LifecycleManagement.CodeGen;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class LifecycleActionHandler : MonoBehaviour
{
    protected abstract List<ILifecycleAction> AllActions(); 
    readonly List<IUpdateAction> updateActions = new();
    readonly List<IFixedUpdateAction> fixedUpdateActions = new();
    readonly List<IAfterUpdateAction> afterUpdateActions = new();
    
    void Start()
    {
        AllActions().ForEach(a =>
        {
            if(a is IUpdateAction)
            {
                updateActions.Add(a as IUpdateAction);
            }

            if(a is IFixedUpdateAction)
            {
                fixedUpdateActions.Add(a as IFixedUpdateAction);
            }

            if(a is IAfterUpdateAction)
            {
                afterUpdateActions.Add(a as IAfterUpdateAction);
            }
        });

    }

    void Update()
    {
        updateActions.ForEach(a => a.UpdateTick());
    }

    void FixedUpdate()
    {
        fixedUpdateActions.ForEach(a => a.FixedTick());
    }

    void LateUpdate()
    {
        afterUpdateActions.ForEach(a => a.AfterUpdateTick());
    }

}