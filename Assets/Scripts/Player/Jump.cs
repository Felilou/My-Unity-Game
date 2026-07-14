using UnityEngine;
using UnityEngine.InputSystem;

public class Jump : RBManiplulatorAction, IUpdateAction
{
    private readonly float jumpStrenght;
    private readonly GroundState groundState;
    private readonly InputAction jumpAction;
    bool IsInputTriggered;

    public Jump(Rigidbody rigidbody, float jumpStrenght, GroundState groundState, InputAction jumpAction) : base(rigidbody)
    {
        this.jumpStrenght = jumpStrenght;
        this.groundState = groundState;
        this.jumpAction = jumpAction;
    }

    public void UpdateTick()
    {
        if(jumpAction.triggered)
            IsInputTriggered = true;
    }

    public override void FixedTick()
    {
        if (IsInputTriggered && groundState.IsGrounded())
        {
            rb.linearVelocity += new Vector3(0, jumpStrenght, 0);
        }
        IsInputTriggered = false;
    }

    
}
