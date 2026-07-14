using Unity.VisualScripting;
using UnityEngine;

public class Gravity : RBManiplulatorAction
{
    private readonly float strenght;

    public Gravity(Rigidbody rigidbody, float strenght) : base(rigidbody)
    {
        this.strenght = strenght;
    }
    
    
    public override void FixedTick()
    {
        rb.AddForce(Vector3.down * strenght, ForceMode.Acceleration);
    }

}
