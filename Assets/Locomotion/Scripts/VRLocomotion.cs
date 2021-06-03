using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Handedness
{
    Left,
    Right
}

public class VRLocomotion : MonoBehaviour
{
    public Handedness handedness;
    public Transform player;
    public float movementSpeed;
    public LineRenderer teleportBeam;
    public MeshRenderer teleportReticule;
    public float teleportRange;
    public Color invalidTeleportColour;
    public Color validTeleportColour;

    void Update()
    {
        HandleTeleporting();
        HandleMovement();
        HandleRotation();
    }

    private void HandleTeleporting()
    {
        // If a raycast from the hand hits anything
        if(Physics.Raycast(transform.position, transform.forward, out var hit, teleportRange))
        {
            // Show the beam and reticule
            teleportBeam.enabled = true;
            teleportReticule.gameObject.SetActive(true);

            // Move the reticule to the hit position
            teleportReticule.transform.position = hit.point;

            // Set the start and end positions of the teleport beam
            teleportBeam.SetPosition(0, transform.position);
            teleportBeam.SetPosition(1, hit.point);

            // Check if the object we hit is an invalid teleport target
            if(hit.transform.gameObject.layer == LayerMask.NameToLayer("InvalidTeleport"))
            {
                // Set the beam and reticule to the invalid colour
                teleportBeam.material.color = invalidTeleportColour;
                teleportReticule.material.color = invalidTeleportColour;
            }
            else
            {
                // Set the beam and reticule to the valid colour
                teleportBeam.material.color = validTeleportColour;
                teleportReticule.material.color = validTeleportColour;

                // If the trigger is pressed
                if (Input.GetButtonDown($"XRI_{handedness}_TriggerButton"))
                {
                    // Teleport the player
                    player.position = hit.point;
                }
            }
        }
        else
        {
            // Hide the beam and reticule
            teleportBeam.enabled = false;
            teleportReticule.gameObject.SetActive(false);
        }
    }

    private void HandleMovement()
    {
        // Get the forward and strafe inputs
        var forwardInput = Input.GetAxis($"XRI_{handedness}_Primary2DAxis_Vertical");
        var strafeInput = Input.GetAxis($"XRI_{handedness}_Primary2DAxis_Horizontal");

        /*// Step 1:

        // Get the forward and strafe directions
        var forwardDir = player.forward;
        var strafeDir = player.right;

        // Change the player's position
        player.position +=
            -forwardDir *   // Direction
            forwardInput *  // Input
            movementSpeed * // Speed
            Time.deltaTime; // Frame-rate independence

        player.position +=
            strafeDir *     // Direction
            strafeInput *   // Input
            movementSpeed * // Speed
            Time.deltaTime; // Frame-rate independence*/

        /*// Step 2

        // Get the forward and strafe directions
        var forwardDir = Camera.main.transform.forward;
        var strafeDir = Camera.main.transform.right;

        // Change the player's position
        player.position +=
            -forwardDir *   // Direction
            forwardInput *  // Input
            movementSpeed * // Speed
            Time.deltaTime; // Frame-rate independence

        player.position +=
            strafeDir *     // Direction
            strafeInput *   // Input
            movementSpeed * // Speed
            Time.deltaTime; // Frame-rate independence*/

        // Step 3

        // Get the forward and strafe directions
        var forwardDir = Camera.main.transform.forward;
        forwardDir.y = 0;
        var strafeDir = Camera.main.transform.right;
        strafeDir.y = 0;

        // Step 4
        forwardDir.Normalize();
        strafeDir.Normalize();

        // Change the player's position
        player.position +=
            -forwardDir *   // Direction
            forwardInput *  // Input
            movementSpeed * // Speed 
            Time.deltaTime; // Frame-rate independence

        player.position +=
            strafeDir *     // Direction
            strafeInput *   // Input
            movementSpeed * // Speed
            Time.deltaTime; // Frame-rate independence
    }

    private void HandleRotation()
    {
        // If the thumbstick has been pressed
        if(Input.GetButtonDown($"XRI_{handedness}_Primary2DAxisClick"))
        {
           // Determine which direction to rotate
            float rotateAngle = Input.GetAxis($"XRI_{handedness}_Primary2DAxis_Horizontal") < 0 ? -30 : 30;

            // Rotate the player
            player.Rotate(0, rotateAngle, 0);
        }
    }
}
