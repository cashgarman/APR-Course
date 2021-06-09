using System.Collections.Generic;
using UnityEngine;

public class ThrowableObject : GrabbableObject
{
    public int maxNumSamples = 10;
    public float throwMultiplier = 150f;

    private FixedJoint joint;
    private Vector3 prevPosition;
    private Queue<Vector3> prevVelocities = new Queue<Vector3>();

    protected override void Start()
    {
        base.Start();

        prevPosition = transform.position;
    }

    public override bool OnGrabbed(Grabber hand)
    {
        // Make sure the object isn't already being held by another hand
        if (joint != null)
            return false;

        // Add a fixed joint between this object's rigid body and the hand's rigid body
        joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = hand.GetComponent<Rigidbody>();

        return true;
    }

    public override void OnDropped()
    {
        // Destroy the fixed joint
        Destroy(joint);

        // Calculate the average velocity over the last number of updates
        var averageVelocity = Vector3.zero;
        foreach(var velocity in prevVelocities)
        {
            averageVelocity += velocity;
        }
        averageVelocity /= prevVelocities.Count;

        // Apply the averaged velocity to the rigid body to throw it (with an optional boost)
        rigidBody.velocity = averageVelocity * throwMultiplier;
    }

    private void FixedUpdate()
    {
        // Calculate the velocity of the object since the last update
        var velocity = transform.position - prevPosition;

        // Store the new previous position of the object
        prevPosition = transform.position;

        // Add the calculated velocity to the list of previous velocities
        prevVelocities.Enqueue(velocity);

        // If we've stored too many samples
        if(prevVelocities.Count > maxNumSamples)
        {
            // Toss out the oldest sample
            prevVelocities.Dequeue();
        }
    }
}
