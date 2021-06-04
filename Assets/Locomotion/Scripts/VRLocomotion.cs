using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public float smoothAmount;
    public float arcHeight;
    public int numLinePoints = 10;
    public RawImage fadeImage;

    private void Start()
    {
        // Make sure we have space for all the points in our teleport beam
        teleportBeam.positionCount = numLinePoints;
    }

    void Update()
    {
        HandleTeleporting();
        //HandleMovement();
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

            // Smooth out the movement of the reticule and beam
            Vector3 startPoint = teleportReticule.transform.position;
            Vector3 desiredPosition = hit.point;
            Vector3 deltaToDesiredPosition = desiredPosition - startPoint;
            Vector3 smoothedDelta = (deltaToDesiredPosition / smoothAmount) * Time.deltaTime;
            Vector3 newPosition = startPoint + smoothedDelta;

            // Shorter version of the above
            //newPosition = Vector3.Lerp(startPoint, hit.point, 1f / smoothAmount * Time.deltaTime);

            // Move the reticule to the hit position
            teleportReticule.transform.position = newPosition;

            // Calculate the control point of the arc we're trying to create
            // This will be above the midpoint between the hand and the teleport target
            Vector3 delta = desiredPosition - startPoint;
            Vector3 midpoint = startPoint + delta / 2f;
            Vector3 controlPoint = midpoint + Vector3.up * arcHeight;

            // Shorter version of the above
            //controlPoint = startPoint + (desiredPosition - startPoint) / 2f + Vector3.up * arcHeight;

            // Calculate all the positions on the curved line
            for(int i = 0; i < numLinePoints; ++i)
            {
                // Calculate percentage
                float t = (float)i / (numLinePoints - 1);

                // Calculate the 2 component lerps
                Vector3 startToMiddlePoint = Vector3.Lerp(transform.position, controlPoint, t);
                Vector3 middleToEndPoint = Vector3.Lerp(controlPoint, newPosition, t);
                Vector3 positionOnCurve = Vector3.Lerp(startToMiddlePoint, middleToEndPoint, t);

                // Set the point on the line renderer
                teleportBeam.SetPosition(i, positionOnCurve);
            }

            // Set the start and end positions of the teleport beam
            //teleportBeam.SetPosition(0, transform.position);
            //teleportBeam.SetPosition(1, newPosition);

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
                    StartCoroutine(TeleportPlayer(hit.point));
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

    private IEnumerator TeleportPlayer(Vector3 point)
    {
        // Fade to black
        /*for(int i = 0; i < 30; ++i)
        {
            float t = (float)i / (30 - 1);
            fadeImage.color = Color.Lerp(Color.clear, Color.black, t);

            yield return new WaitForEndOfFrame();
        }*/

        float currentTime = 0f;
        while(currentTime < 1f)
        {
            fadeImage.color = Color.Lerp(Color.clear, Color.black, currentTime);

            yield return new WaitForEndOfFrame();

            currentTime += Time.deltaTime;
        }

        // Changes the position other player
        player.position = point;

        // Fade back
        currentTime = 0f;
        while (currentTime < 1f)
        {
            fadeImage.color = Color.Lerp(Color.black, Color.clear, currentTime);

            yield return new WaitForEndOfFrame();

            currentTime += Time.deltaTime;
        }

        // Fade back
        /*for(int i = 0; i < 30; ++i)
        {
            float t = (float)i / (30 - 1);
            fadeImage.color = Color.Lerp(Color.black, Color.clear, t);

            yield return new WaitForEndOfFrame();
        }*/
    }

    private void HandleMovement()
    {
        // Get the forward and strafe inputs
        var forwardInput = Input.GetAxis($"XRI_{handedness}_Primary2DAxis_Vertical");
        var strafeInput = Input.GetAxis($"XRI_{handedness}_Primary2DAxis_Horizontal");

        // Get the forward and strafe directions
        var forwardDir = Camera.main.transform.forward;
        forwardDir.y = 0;
        var strafeDir = Camera.main.transform.right;
        strafeDir.y = 0;

        // Normalize the directions
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
