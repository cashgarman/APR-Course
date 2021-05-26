using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float speed;
    public float range;
    public FoodFightGame game;

    private float initialXPosition;

    private void Start()
    {
        initialXPosition = transform.position.x;
    }

    void Update()
    {
        // Calculate the new X position of the target
        Vector3 newPosition = transform.position;
        newPosition.x = initialXPosition + Mathf.Sin(Time.time * speed) * range;
        transform.position = newPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Was the target hit by food
        Foodstuff foodstuff = collision.gameObject.GetComponent<Foodstuff>();
        if(foodstuff != null)
        {
            // Destroying the target and foodstuff
            Destroy(gameObject);
            Destroy(foodstuff.gameObject);

            // Let the game know a target was destroyed
            game.OnTargetHit();
        }
    }
}
