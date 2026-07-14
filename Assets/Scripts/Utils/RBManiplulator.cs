using UnityEngine;

public abstract class RBManiplulatorAction : IFixedUpdateAction
{

    protected readonly Rigidbody rb; 

    public RBManiplulatorAction(Rigidbody rigidbody)
    {
        rb = rigidbody;
    }

    public abstract void FixedTick();

}
