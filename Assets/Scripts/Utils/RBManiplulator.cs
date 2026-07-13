using UnityEngine;

public abstract class RBManiplulator : IAction
{

    protected readonly Rigidbody rb; 

    public RBManiplulator(Rigidbody rigidbody)
    {
        rb = rigidbody;
    }

    abstract public void Tick();

}
