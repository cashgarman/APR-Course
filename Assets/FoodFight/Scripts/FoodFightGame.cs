using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class FoodFightGame : MonoBehaviour
{
    public BoxCollider spawnArea;
    public Target targetPrefab;
    public Foodstuff[] foodstuffs;
    private int score;
    public TMP_Text scoreText;
    private float countdown;
    public float gameDuration = 30;
    public TMP_Text countdownText;
    public GameObject playAgainPrompt;
    private bool gameOver;
    public string leftTriggerName;
    public string rightTriggerName;
    public GameObject highScorePrompt;

    private void Start()
    {
        // Spawn a target
        SpawnTarget();

        // Spawn some food
        SpawnFood();

        UpdateUI();

        // Start the game countdown
        countdown = gameDuration;
    }

    public void OnTargetHit()
    {
        // Spawn a new target
        SpawnTarget();

        // Increased the score
        score += 1;
        UpdateUI();
    }

    private void Update()
    {
        // Decrease the game countdown
        countdown -= Time.deltaTime;
        UpdateUI();

        // If the game has ended
        if(countdown <= 0)
        {
            // Pause the game
            Time.timeScale = 0f;

            // Ask the player if they would like to play again?
            playAgainPrompt.SetActive(true);

            // Flag the game as over
            gameOver = true;

            // If this is a new highscore
            var prevHighscore = PlayerPrefs.GetInt("highscore", 0);
            if(score > prevHighscore)
            {
                highScorePrompt.SetActive(true);
                PlayerPrefs.SetInt("highscore", score);
            }
        }

        // Check if the player wants to play again
        if(gameOver && (Input.GetButtonDown(leftTriggerName) || Input.GetButtonDown(rightTriggerName)))
        {
            // Reload the level
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void UpdateUI()
    {
        scoreText.text = $"Score: {score}";
        countdownText.text = $"Countdown: {countdown:F1} sec";
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

    private void SpawnFood()
    {
        // Select a random food prefab
        var randomFoodstuffPrefab = foodstuffs[Random.Range(0, foodstuffs.Length)];

        // Instantiate a new foodstuff
        var newFoodstuff = Instantiate(randomFoodstuffPrefab, transform.position, transform.rotation);

        // Let the new food know about the game
        newFoodstuff.game = this;
    }

    public void OnFoodThrown()
    {
        // Spawn another food after a delay
        Invoke(nameof(SpawnFood), 1f);
    }
}
