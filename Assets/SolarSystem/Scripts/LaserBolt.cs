using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBolt : MonoBehaviour
{
    public float lifespan = 5f;

    [SerializeField] private float JustForUnityInspector;

    private void Start()
    {
        Destroy(gameObject, lifespan); 
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

        var rocketShip = collision.gameObject.GetComponent<RocketControls>();
        if(rocketShip != null)
        {
            rocketShip.engineForce = 1;
        }

        // TODO: Blow stuff up!
        /*EnemyShip enemyShip = collision.gameObject.GetComponent<EnemyShip>();
        if(enemyShip != null)
        {
            enemyShip.health -= 10;
            if(enemyShip.health <= 0)
            {

            }
        }*/
    }
}
