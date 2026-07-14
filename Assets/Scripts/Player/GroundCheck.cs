using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private bool _IsTouchingGround;

    [SerializeField]
    float ray_lenght;

    [SerializeField]
    LayerMask layer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _IsTouchingGround = Physics.Raycast(transform.position, Vector3.down, ray_lenght);
        Debug.DrawRay(transform.position, Vector3.down * ray_lenght,_IsTouchingGround ? Color.green : Color.red);
    }

    public bool IsTouchingGround()
    {
        return _IsTouchingGround;
    }
}