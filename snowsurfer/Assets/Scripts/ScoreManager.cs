using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int flipScore = 0;

    public void AddScore(int additionalScore)
    {
        flipScore +=  additionalScore;
        scoreText.text = "Score: " + flipScore;
    }
}
