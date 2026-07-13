using Unity.VisualScripting;
using UnityEngine;

public class Gravity : RBManiplulator
{
    private readonly float strenght;

    public Gravity(Rigidbody rigidbody, float strenght) : base(rigidbody)
    {
        this.strenght = strenght;
    }
    
    
    public override void Tick()
    {
        rb.AddForce(Vector3.down * strenght, ForceMode.Acceleration);
    }

}
