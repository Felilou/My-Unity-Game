using System.Collections.Generic;
using UnityEngine;
//Singleton - kümmert sich NUR um playermovement
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : LifecycleActionHandler
{
    private InputSystem_Actions InputActions;
    private Rigidbody rigidbody;
    private Jump jump; //No mono
    private Move move; //No mono
    private Gravity gravity; //No mono
    private GroundState groundState;

    [SerializeField]
    float jump_height;
    
    [SerializeField]
    float move_speed;

    [SerializeField]
    float turn_speed;
    
    [SerializeField]
    float gravity_force;

    [SerializeField]
    float accellaration;

    Camera camera;

    void OnEnable()  { InputActions?.Enable(); }
    void OnDisable() { InputActions?.Disable(); }

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        camera = Camera.main;

        InputActions = new InputSystem_Actions();
        rigidbody = GetComponent<Rigidbody>();
        groundState = gameObject.AddComponent<GroundState>();

        rigidbody.useGravity = false;

        move = new Move(rigidbody, move_speed, InputActions.Player.Move, turn_speed, camera.transform, groundState, accellaration);
        jump = new Jump(rigidbody, jump_height, groundState, InputActions.Player.Jump);
        gravity = new Gravity(rigidbody, gravity_force);
        
    }

    protected override List<ILifecycleAction> AllActions()
    {
        return new List<ILifecycleAction>(){move, jump, gravity};
    }
}
