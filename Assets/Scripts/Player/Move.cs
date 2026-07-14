using UnityEngine;
using UnityEngine.InputSystem;

public class Move : RBManiplulatorAction, IUpdateAction
{
    private readonly float moveSpeed;
    private readonly float turnSpeed;
    private readonly InputAction moveAction;
    private readonly Transform cameraTransform;
    private Vector3 dir;

    public Move(Rigidbody rigidbody, float moveSpeed, InputAction moveAction, float turnSpeed, Transform cameraTransform) : base(rigidbody)
    {
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
        Vector3 velocity = new(dir.x * moveSpeed, rb.linearVelocity.y, dir.z * moveSpeed);
        rb.linearVelocity = velocity;
        
        if (dir.sqrMagnitude > 0.001)
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
