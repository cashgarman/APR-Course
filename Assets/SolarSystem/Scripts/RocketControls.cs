using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketControls : MonoBehaviour
{
    private Rigidbody rigidbody;
    public Light engineLight;
    public float engineForce;
    public float turningForce;
    public float dragForce;
    public Rigidbody laserBoltPrefab;
    public Transform barrel;
    public float laserImpulse;
    public AudioClip pewSound;
    

    void Start()
    {
        // Store the rigidbody from this object
        rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        HandleRotation();
        HandleEngineForces();

        // Apply some drag
        rigidbody.AddForce(rigidbody.velocity * -dragForce * Time.deltaTime);

        // If the left mouse button is pressed
        if(Input.GetMouseButtonDown(0))
        {
            // Create a new laser bolt
            var laserBolt = Instantiate(laserBoltPrefab, barrel.position, Quaternion.identity);
            
            // NOTE: Up in the context of this laser bolt prefab is in-fact its forward
            laserBolt.transform.up = transform.forward;

            // Match the laser bolt's velocity with the rocket's velocity
            laserBolt.velocity = rigidbody.velocity;

            // Apply an impulse to the laser bolt
            laserBolt.AddForce(laserBolt.transform.up * laserImpulse);

            // Play a pew pew sound
            GetComponent<AudioSource>().PlayOneShot(pewSound);
        }
    }

    private void HandleEngineForces()
    {
        // Keep track of the engine
        bool engineOn = false;

        // If the W key is being held down
        if (Input.GetKey(KeyCode.W))
        {
            // Apply a forward force to the rocket
            rigidbody.AddForce(transform.forward * engineForce * Time.deltaTime);

            // The engine is on
            engineOn = true;
        }

        // If the S key is being held down
        if (Input.GetKey(KeyCode.S))
        {
            // Apply a backwards force to the rocket
            rigidbody.AddForce(transform.forward * -engineForce * Time.deltaTime);

            // The engine is on
            engineOn = true;
        }

        // If the A key is being held down
        if (Input.GetKey(KeyCode.A))
        {
            // Apply a left force to the rocket
            rigidbody.AddForce(transform.right * -engineForce * Time.deltaTime);

            // The engine is on
            engineOn = true;
        }

        // If the D key is being held down
        if (Input.GetKey(KeyCode.D))
        {
            // Apply a right force to the rocket
            rigidbody.AddForce(transform.right * engineForce * Time.deltaTime);

            // The engine is on
            engineOn = true;
        }

        // The GetAxis() approach to the above
        // float direction = Input.GetAxis("Vertical");
        // rigidbody.AddForce(transform.forward * direction * engineForce * Time.deltaTime);

        // Turn on the engine light only when the engine is running
        engineLight.enabled = engineOn;
    }

    private void HandleRotation()
    {
        // Only rotate when the right mouse button is held down
        if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.Space))
        {
            // Get the mouse inputs
            float yaw = Input.GetAxis("Mouse X");
            float pitch = -Input.GetAxis("Mouse Y");

            // Apply some rotation to the rocket based on the mouse inputs
            rigidbody.AddRelativeTorque(
                pitch * turningForce * Time.deltaTime,  // Pitch (X-Axis)
                yaw * turningForce * Time.deltaTime,    // Yaw (Y-Axis)
                0);                                     // Roll (Z-Axis, not used here)
        }
    }
}
