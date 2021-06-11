using UnityEngine;

public class Grabber : MonoBehaviour
{
    public string gripInputName;
    public string triggerInputName;

    private Animator animator;
    private InteractiveObject grabbedObject;
    private InteractiveObject highlightedObject;

    void Start()
    {
        // Store the animator for future use
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the other object is a grabbable object
        InteractiveObject grabbableObject = other.GetComponent<InteractiveObject>();
        if(grabbableObject != null)
        {
            // Highlight the grabbable object
            grabbableObject.OnTouchStart();
            highlightedObject = grabbableObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the other object is a grabbable object
        InteractiveObject grabbableObject = other.GetComponent<InteractiveObject>();
        if (grabbableObject != null)
        {
            // Unhighlight the grabbable object
            grabbableObject.OnTouchEnd();
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
                if (highlightedObject.OnGrabbed(this))
                {
                    grabbedObject = highlightedObject;
                    highlightedObject = null;
                }
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

        // If the trigger is pressed and an object is being held
        if(Input.GetButtonDown(triggerInputName) && grabbedObject != null)
        {
            // Let the held object know the trigger started being held
            grabbedObject.OnTriggerStart();
        }

        // If the trigger is released and an object is being held
        if (Input.GetButtonUp(triggerInputName) && grabbedObject != null)
        {
            // Let the held object know the trigger stopped being held
            grabbedObject.OnTriggerEnd();
        }
    }
}
