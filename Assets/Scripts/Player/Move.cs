using UnityEngine;
using UnityEngine.InputSystem;

public class Move : RBManiplulatorAction, IUpdateAction
{
    private readonly float moveSpeed;
    private readonly float turnSpeed;
    private readonly InputAction moveAction;
    private readonly Transform cameraTransform;
    private readonly GroundState groundState;
    private Vector3 dir;
    private readonly float accellaration;

    public Move(Rigidbody rigidbody, float moveSpeed, InputAction moveAction, float turnSpeed, Transform cameraTransform, GroundState groundState, float accellaration) : base(rigidbody)
    {
        this.accellaration = accellaration;
        this.groundState = groundState;
        this.moveSpeed = moveSpeed;
        this.moveAction = moveAction;
        this.turnSpeed = turnSpeed;
        this.cameraTransform = cameraTransform;
    }

    public void UpdateTick()
    {
        dir = MoveDir(moveAction.ReadValue<Vector2>());
    }

    public override void FixedTick()
    {
        Vector3 crntVelocity = new(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        Vector3 horizontalVel = Vector3.MoveTowards(crntVelocity, moveSpeed*dir, accellaration * Time.fixedDeltaTime);
        rb.linearVelocity = new(horizontalVel.x, rb.linearVelocity.y, horizontalVel.z);
        
        if (dir.sqrMagnitude > 0.001 && groundState.IsGrounded())
        {
            Quaternion target = Quaternion.LookRotation(dir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, target, turnSpeed * Time.fixedDeltaTime));
        }
    }

    private Vector3 MoveDir(Vector2 input)
    {
        Vector3 cam_forward = cameraTransform.forward;
        Vector3 cam_right = cameraTransform.right;

        cam_forward.y = 0;
        cam_right.y = 0;

        cam_forward.Normalize();
        cam_right.Normalize();

        return cam_forward * input.y + cam_right * input.x;
    }

}
