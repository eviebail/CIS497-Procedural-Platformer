﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmGenerator : MonoBehaviour
{
    rType type;
    List<string> levelLayout = new List<string>();
    static int LEVEL_DURATION = 6;
    List<Block> levelBlocks = new List<Block>();
    GeometryGenerator gen;
    public int startX = -1;
    public int startY = 0;

    public Texture2D texGround;
    public Texture2D texEnemy;
    public Texture2D texSpike;
    public Texture2D texCoin;
    //private Sprite mySprite;
    //private SpriteRenderer sr;
    //GameObject gameObject;

    protected void selectType()
    {
        int val = (int) (Random.value * 3.0f);
        val = 0;
        switch (val)
        {
            case 0:
                type = rType.regular;
                break;
            case 1:
                type = rType.random;
                break;
            case 2:
                type = rType.swing;
                break;
            case 3:
                type = rType.test;
                break;
        }
        Debug.Log("Type: " + type);
    }

    protected int selectLength()
    {
        float val = 0.5f;// Random.value;
        if (val < 0.33)
        {
            return 4;
        } else if (val < 0.66)
        {
            return 8;
        }
        else
        {
            return 16;
        }
    }

    protected int selectDensity(int max)
    {
        float val = 0.5f;// Random.value;
        if (val < 0.33)
        {
            return Mathf.Min(2, max);
        }
        else if (val < 0.66)
        {
            return Mathf.Min(4, max);
        }
        else
        {
            return Mathf.Min(8, max);
        }
        //return Mathf.Min(val, max);
    }

    private RhythmBlock generateRhythmBlock()
    {
        selectType();
        int length = selectLength();
        Debug.Log("Length: " + length);
        int density = selectDensity(length);
        Debug.Log("Denisty: " + density);
        return new RhythmBlock(type, length, density);
        //density can't be b
    }

    private void generateLevel(int length)
    {
        for (int i = 0; i < length; i++)
        {
            if (Random.value < 0.5)
            {
                levelLayout.Add("r");
            } else
            {
                levelLayout.Add("r"); //TEST levelLayout.Add("p");
            }
        }
        string otp = "Level Layout: [";
        for (int i = 0; i < levelLayout.Count; i++)
        {
            otp += levelLayout[i] + ", ";
        }
        otp += "]";
        Debug.Log(otp);
    }

    // Start is called before the first frame update
    void Start()
    {
        //fill in the layout array
        generateLevel(LEVEL_DURATION);

        //populate the blocks array with the appropriate block type
        for (int i = 0; i < levelLayout.Count; i++)
        {
            if (levelLayout[i].Equals("r"))
            {
                levelBlocks.Add(generateRhythmBlock());
            } else if (levelLayout[i].Equals("p"))
            {
                //levelBlocks.Add(generatePuzzleBlock());
            }
        }

        //place geometry based on the blocks generated
        Debug.Log("START X: " + startX);
        gen = new GeometryGenerator(levelBlocks, startX, startY, texGround, texEnemy, texSpike, texCoin);
        gen.generateGeometry();
        gen.cleanUpEnemies();
        gen.cleanUpStomps();
        //mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height),
        //                         new Vector2(0.5f, 0.5f), 100.0f);
        //sr.sprite = mySprite;
    }

    //void Awake()
    //{
    //    gameObject = new GameObject();
    //    sr = gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
    //    sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
    //    gameObject.transform.position = new Vector3(-5.0f, 1.5f, 0.0f);
    //    gameObject.transform.localScale = new Vector3(4f,4f,4f);
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}