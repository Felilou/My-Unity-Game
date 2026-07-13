using System.Collections.Generic;
using UnityEngine;
//Singleton - kümmert sich NUR um playermovement
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
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

    Camera camera;

    readonly List<RBManiplulator> rBManiplulators = new();

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
        
        move = new Move(rigidbody, move_speed, InputActions.Player.Move, InputActions.Player.Look, turn_speed, camera.transform);
        jump = new Jump(rigidbody, jump_height, groundState);
        gravity = new Gravity(rigidbody, gravity_force);

        rBManiplulators.Add(jump);
        rBManiplulators.Add(move);
        rBManiplulators.Add(gravity);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rBManiplulators.ForEach((m) => {m.Tick();});
    }
}
