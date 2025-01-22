using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour
{
    public void RecordHighScore(int currentScore)
    {
        // Get the current high score (default to 0 if it doesn't exist)
        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        // If the current score is greater than the high score
        if (currentScore > highScore)
        {
            // Update the high score
            PlayerPrefs.SetInt("HighScore", currentScore);
        }
    }
    
    public void DeleteHighScore()
    {
        // Check if the high score exists
        if (PlayerPrefs.HasKey("HighScore"))
        {
            // Delete the high score
            PlayerPrefs.DeleteKey("HighScore");
        }
    }
    
    public void ResetAllScores()
    {
        Debug.Log("Resetting all scores...");
        // Set all high scores to 0
        PlayerPrefs.SetInt("HighScore1", 1500);
        PlayerPrefs.SetInt("HighScore2", 1000);
        PlayerPrefs.SetInt("HighScore3", 500);
    }
    
    public int[] GetTopThreeScores()
    {
        // Get the current high scores (default to an array of three 0s if they don't exist)
        int[] highScores = new int[3];
        highScores[0] = PlayerPrefs.GetInt("HighScore1", 0);
        highScores[1] = PlayerPrefs.GetInt("HighScore2", 0);
        highScores[2] = PlayerPrefs.GetInt("HighScore3", 0);

        // Return the high scores
        return highScores;
    }
    
    public bool RecordTopThreeScores(int currentScore)
    {
        // Initialize isUpdated to false
        bool isUpdated = false;

        // Get the current high scores (default to an array of three 0s if they don't exist)
        int[] highScores = new int[3];
        highScores[0] = PlayerPrefs.GetInt("HighScore1", 0);
        highScores[1] = PlayerPrefs.GetInt("HighScore2", 0);
        highScores[2] = PlayerPrefs.GetInt("HighScore3", 0);

        // Check if the current score is greater than the lowest high score
        if (currentScore > highScores[2])
        {
            // Check if the current score is already in the high scores
            if (Array.IndexOf(highScores, currentScore) != -1)
            {
                // If the current score is already in the high scores, return false
                return true;
            }
            
            // Find the correct position for the new score
            int position = 2;
            if (currentScore > highScores[1]) position = 1;
            if (currentScore > highScores[0]) position = 0;

            // Shift down the lower scores
            for (int i = 2; i > position; i--)
            {
                highScores[i] = highScores[i - 1];
            }

            // Insert the new score
            highScores[position] = currentScore;

            // Set isUpdated to true
            isUpdated = true;
        }

        // Store the high scores back into PlayerPrefs
        PlayerPrefs.SetInt("HighScore1", highScores[0]);
        PlayerPrefs.SetInt("HighScore2", highScores[1]);
        PlayerPrefs.SetInt("HighScore3", highScores[2]);

        // Return whether the high scores were updated
        return isUpdated;
    }
}
