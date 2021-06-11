using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TurnableDial : InteractiveObject
{
    public Transform target;
    public UnityEvent<float> onDialChanged;

    private Transform initialTargetParent;
    private float previousYAngle;

    private void Start()
    {
        // Store the initial parent of the dial target
        initialTargetParent = target.parent;

        // Store the initial Y rotation of the dial
        previousYAngle = transform.localEulerAngles.y;
    }

    public override bool OnGrabbed(Grabber hand)
    {
        // Parent the dial target to the hand
        target.SetParent(hand.transform);

        return true;
    }

    private void Update()
    {
        // Calculate the delta between the dial and the target
        Vector3 targetDelta = target.position - transform.position;
        Debug.DrawLine(transform.position, target.position, Color.green);

        // Now project the delta vector onto the plane of the dial
        // NOTE: This one will work perfectly too, but only for completely horizontal dials
        //Vector3 projectedTargetDelta = new Vector3(targetDelta.x, 0, targetDelta.z);
        Vector3 projectedTargetDelta = Vector3.ProjectOnPlane(targetDelta, transform.up);
        Debug.DrawLine(transform.position, transform.position + projectedTargetDelta, Color.red);

        // Calculate the rotation require to face the dial towards the projected target
        Quaternion desiredDialRotation = Quaternion.LookRotation(projectedTargetDelta, transform.up);

        // Apply the rotation to the dial
        transform.rotation = desiredDialRotation;

        // If the angle of the dial has changed
        if(transform.localEulerAngles.y != previousYAngle)
        {
            // Calculate how much the rotation has changed
            float rotationDelta = transform.localEulerAngles.y - previousYAngle;

            // Deal with 0-360 degree wrapping
            if (rotationDelta < -180f)
                rotationDelta += 360f;
            if (rotationDelta > 180f)
                rotationDelta -= 360f;

            // Trigger the on dial changed event
            onDialChanged.Invoke(rotationDelta);

            // Store the new angle of the dial
            previousYAngle = transform.localEulerAngles.y;
        }
    }

    public override void OnDropped()
    {
        // Reparent the dial target to its original parent
        target.SetParent(initialTargetParent);
    }
}
