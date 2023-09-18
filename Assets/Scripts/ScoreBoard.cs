using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreBoard : MonoBehaviour
{
    int score;
    TMP_Text scoreboardText;

void Start()
    {
        scoreboardText = GetComponent<TMP_Text>();
        scoreboardText.text = "0";
    }

public void IncreaseScore(int amountToIncrease)
    {
        score += amountToIncrease;
        scoreboardText.text = score.ToString();
    }
}
