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
    void Update()
    {
        _IsTouchingGround = Physics.Raycast(transform.position, Vector3.down, ray_lenght, layer);
    }

    public bool IsTouchingGround()
    {
        return _IsTouchingGround;
    }
}