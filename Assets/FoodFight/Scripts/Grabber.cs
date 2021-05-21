using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    public string gripInputName;

    private Animator animator;
    private GrabbableObject grabbedObject;
    private GrabbableObject highlightedObject;

    void Start()
    {
        // Store the animator for future use
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object is a grabbable object
        GrabbableObject grabbableObject = other.GetComponent<GrabbableObject>();
        if(grabbableObject != null)
        {
            // Highlight the grabbable object
            grabbableObject.OnHoverStart();
            highlightedObject = grabbableObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the other object is a grabbable object
        GrabbableObject grabbableObject = other.GetComponent<GrabbableObject>();
        if (grabbableObject != null)
        {
            // Unhighlight the grabbable object
            grabbableObject.OnHoverEnd();
            highlightedObject = null;
        }
    }

    void Update()
    {
        // When the grip trigger is pressed
        if (Input.GetButtonDown(gripInputName) || Input.GetMouseButtonDown(0))
        {
            // Play the gripped animation
            animator.SetBool("Gripped", true);

            // If we have an object highlighted
            if(highlightedObject != null)
            {
                // Grab the object
                highlightedObject.OnGrabbed(this);
                grabbedObject = highlightedObject;
                highlightedObject = null;
            }
        }

        // When the grip trigger is released
        if (Input.GetButtonUp(gripInputName) || Input.GetMouseButtonUp(0))
        {
            // Stop the gripped animation
            animator.SetBool("Gripped", false);

            // If we have an object grabbed
            if (grabbedObject != null)
            {
                // Drop the object
                grabbedObject.OnDropped();
                grabbedObject = null;
            }
        }
    }
}
