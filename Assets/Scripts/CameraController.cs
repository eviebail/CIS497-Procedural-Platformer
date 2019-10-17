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
    }

    // Update is called once per frame
    void Update()
    {
        //if (prevLife != PlayerController.numLives)
        //{
            prevLife = PlayerController.numLives;
            text.text = "Lives: " + PlayerController.numLives + "      " +
            "Coins: " + PlayerController.numCoins;
        //}
    }
}
