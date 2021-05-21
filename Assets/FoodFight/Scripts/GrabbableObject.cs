using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObject : MonoBehaviour
{
    public Color hoverColour;

    private Material material;
    private Rigidbody rigidBody;
    private Color initialColour;

    void Start()
    {
        // Cache the object's material and rigid body
        material = GetComponent<Renderer>().material;
        rigidBody = GetComponent<Rigidbody>();

        // Remember the initial colour of this object
        initialColour = material.color;
    }
    
    public void OnHoverStart()
    {
        // Change the colour of the object to the hover colour
        material.color = hoverColour;
    }

    public void OnHoverEnd()
    {
        // Change the colour of the object back to the normal colour
        material.color = initialColour;
    }

    public void OnGrabbed(Grabber hand)
    {
        // Parent this object to hand that grabbed it
        transform.SetParent(hand.transform);

        // Turn off gravity on this object's rigid body
        rigidBody.useGravity = false;

        // Make this object's rigid body kinematic
        rigidBody.isKinematic = true;
    }

    public void OnDropped()
    {
        // Unparent this object from everything 
        transform.SetParent(null);

        // Turn on gravity on this object's rigid body
        rigidBody.useGravity = true;

        // Make this object's rigid body non-kinematic
        rigidBody.isKinematic = false;
    }
}
