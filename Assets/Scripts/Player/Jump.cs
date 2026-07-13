using UnityEngine;

public class Jump : RBManiplulator
{
    private readonly float jumpStrenght;
    private readonly GroundState groundState;

    public Jump(Rigidbody rigidbody, float jumpStrenght, GroundState groundState) : base(rigidbody)
    {
        this.jumpStrenght = jumpStrenght;
        this.groundState = groundState;
    }

    public override void Tick()
    {
        
    }

    
}
