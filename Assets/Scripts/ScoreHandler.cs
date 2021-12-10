using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreHandler {
    private static int score = 0;
    private static int max = 0;

    public int Increment() {
        Debug.Log("Incrementing score");
        score++;
        return score;
    }

    public int Decrement() {
        score--;
        if(score < 0) {
            score = 0;
        }
        return score;
    }

    public int GetScore() {
        return score;
    }

    public void SetMax(int newMax) {
        max = newMax;
    }

    public int GetMax() {
        return max;
    }

    public void Reset() {
        score = 0;
    }

}
