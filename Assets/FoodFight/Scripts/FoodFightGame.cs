using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FoodFightGame : MonoBehaviour
{
    public BoxCollider spawnArea;
    public Target targetPrefab;

    public void OnTargetHit()
    {
        // Spawn a new target
        SpawnTarget();
    }

    private void SpawnTarget()
    {
        // Instantiate a new target
        Target newTarget = Instantiate(targetPrefab);

        // Generate a random position in the spawn area
        newTarget.transform.position = GetRandomSpawnPosition();

        // Let the new target know about the game
        newTarget.game = this;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Calculate the random X, Y, and Z coordinates of the position
        float x = Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
        float y = Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
        float z = Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);

        // Return a new vector with those coordinates
        return new Vector3(x, y, z);
    }
}
