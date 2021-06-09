using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bitgun : ThrowableObject
{
    public Rigidbody projectilePrefab;
    public Transform barrel;
    public float projectileImpulse;

    public override void OnTriggerStart()
    {
        base.OnTriggerStart();

        // Create a new projectile at the gun's barrel
        Rigidbody projectile = Instantiate(projectilePrefab, barrel.position, Quaternion.identity);

        // Apply a force to the projectile
        projectile.AddForce(barrel.forward * projectileImpulse);

        // Destroy the projectile after some time
        Destroy(projectile.gameObject, 5f);
    }
}
