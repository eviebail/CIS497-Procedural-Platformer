using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    TMPro.TextMeshPro text;
    int prevLife = PlayerController.numLives;
    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetChild(0).GetComponent<TMPro.TextMeshPro>();
        text.text = "Lives: " + PlayerController.numLives + "      " +
            "Coins: " + PlayerController.numCoins;

        if (RhythmGenerator.constraints[1] == 1)
        {
            text.text += "      Enemies: " + PlayerController.killed + " / " + PlayerController.totalEnemies;
        }
        if (RhythmGenerator.constraints[4] == 1)
        {
            text.text += "      Stars: " + PlayerController.numStars + " / " + PlayerController.totalStars;
        }
    }

    // Update is called once per frame
    void Update()
    {
        prevLife = PlayerController.numLives;
        text.text = "Lives: " + PlayerController.numLives + "      " +
        "Coins: " + PlayerController.numCoins;

        if (RhythmGenerator.constraints[1] == 1)
        {
            text.text += "      Enemies: " + PlayerController.killed + " / " + PlayerController.totalEnemies;
        }

        if (RhythmGenerator.constraints[4] == 1)
        {
            text.text += "      Stars: " + PlayerController.numStars + " / " + PlayerController.stars.Count;
        }

        if (RhythmGenerator.constraints[6] == 1)
        {
            text.text += "      Time Left: " + PlayerController.timer;
        }
    }
}
