using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public Color hoverColour;

    private Material material;
    protected Rigidbody rigidBody;
    private Color initialColour;

    protected virtual void Start()
    {
        // Cache the object's material and rigid body
        material = GetComponent<Renderer>().material;
        rigidBody = GetComponent<Rigidbody>();

        // Remember the initial colour of this object
        initialColour = material.color;
    }
    
    public virtual void OnHoverStart()
    {
        // Change the colour of the object to the hover colour
        material.color = hoverColour;
    }

    public virtual void OnHoverEnd()
    {
        // Change the colour of the object back to the normal colour
        material.color = initialColour;
    }

    public virtual void OnGrabbed(Grabber hand)
    {
        // Parent this object to hand that grabbed it
        transform.SetParent(hand.transform);

        // Turn off gravity on this object's rigid body
        rigidBody.useGravity = false;

        // Make this object's rigid body kinematic
        rigidBody.isKinematic = true;
    }

    public virtual void OnDropped()
    {
        // Unparent this object from everything 
        transform.SetParent(null);

        // Turn on gravity on this object's rigid body
        rigidBody.useGravity = true;

        // Make this object's rigid body non-kinematic
        rigidBody.isKinematic = false;
    }
}
