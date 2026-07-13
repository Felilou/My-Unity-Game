using UnityEngine;
using UnityEngine.InputSystem;

public class Move : RBManiplulator
{
    private readonly float moveSpeed;
    private readonly float turnSpeed;
    private readonly InputAction moveAction;
    private readonly InputAction mouseDelta;
    private readonly Transform cameraTransform;

    public Move(Rigidbody rigidbody, float moveSpeed, InputAction moveAction, InputAction mouseDelta, float turnSpeed, Transform cameraTransform) : base(rigidbody)
    {
        this.moveSpeed = moveSpeed;
        this.moveAction = moveAction;
        this.mouseDelta = mouseDelta;
        this.turnSpeed = turnSpeed;
        this.cameraTransform = cameraTransform;
    }

    public override void Tick()
    {
        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 dir = moveDir(input);

        Vector3 velocity = new Vector3(dir.x * moveSpeed, rb.linearVelocity.y, dir.z * moveSpeed);
        rb.linearVelocity = velocity;
        
        if (dir.sqrMagnitude > 0.001)
        {
            Quaternion target = Quaternion.LookRotation(dir);
            rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, target, turnSpeed));
        }

    }

    private Vector3 moveDir(Vector2 input)
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
