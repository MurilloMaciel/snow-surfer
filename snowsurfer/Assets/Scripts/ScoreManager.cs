using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    private int _flipScore = 0;

    public void AddScore(int additionalScore)
    {
        _flipScore +=  additionalScore;
        scoreText.text = "Score: " + _flipScore;
    }
}
