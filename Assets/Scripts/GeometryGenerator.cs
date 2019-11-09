﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryGenerator : MonoBehaviour
{
    private List<Block> blocks; //Rhythm or puzzle block
    private int globalX;
    private int globalY;
    private StateMachine state;
    private Vector3 blockScale = new Vector3(6.25f,6.25f,6.25f);

    public static List<Vector3> lvl = new List<Vector3>(); //x,y,type
    public static List<GameObject> enemies = new List<GameObject>();
    public static List<GameObject> platforms = new List<GameObject>();
    public static List<GameObject> stompers = new List<GameObject>();
    public static List<GameObject> spikes = new List<GameObject>();
    public static List<GameObject> coins = new List<GameObject>();
    public static List<GameObject> superEnemies = new List<GameObject>();
    public static List<List<GameObject>> ground = new List<List<GameObject>>();

    public Texture2D texGround;
    public Texture2D texEnemy;
    public Texture2D texSpike;
    public Texture2D texCoin;
    public Texture2D texGnd;
    public Texture2D texGoal;

    public int enemyID = 0;

    public int timer = 0;

    public bool dir = true;

    //private Sprite mySprite;
    //private SpriteRenderer sr;
    // gameObject;

    //for loading 2d sprites instead of 3d objects
    void Awake()
    {
        
    }

    public void jumpTerrain()
    {
        //loop over level and change accordingly
        //Vector3 terrain = new Vector3(); //prevY, currY, switchStart, 
        List<Vector2> ranges = new List<Vector2>();
        Vector2 preGap = new Vector2();
        Vector2 postGap = new Vector2();
        preGap.x = 1;
        preGap.y = -100;
        postGap.x = -100;
        postGap.y = -100;

        Debug.Log("HIIiiiiiiiiIIIIIIiiiIIII");

        for (int i = 1; i < lvl.Count; i++)
        {
            Vector3 element = lvl[i];
            if (preGap.y == -100)
            {
                if (element.z == -1)
                {
                    preGap.y = i;
                }
            } else
            {
                if (postGap.x == -100)
                {
                    if (element.z != -1)
                    {
                        postGap.x = i;
                    }
                } else 
                {
                    if (element.z == -1)
                    {
                        postGap.y = i;
                        ranges.Add(new Vector2(preGap.x, preGap.y));
                        ranges.Add(new Vector2(postGap.x, postGap.y));
                        preGap.x = postGap.x;
                        preGap.y = postGap.y;
                        postGap.x = -100;
                        postGap.y = -100;
                    }
                }
            }
        }
        postGap.x = preGap.x;
        postGap.y = lvl.Count - 1;
        ranges.Add(new Vector2(preGap.x, preGap.y));
        ranges.Add(new Vector2(postGap.x, postGap.y));

        //for (int i = 0; i < ranges.Count-2; i+=2)
        //{
        //    Debug.Log("PRE::: " + lvl[(int)ranges[i].x] + ", " + lvl[(int)ranges[i].y - 1]);
        //    Debug.Log("POST::: " + lvl[(int)ranges[i+1].x] + ", " + lvl[(int)ranges[i+1].y - 1]);
        //}

        Debug.Log("::: " + lvl[(int)ranges[ranges.Count-1].x] + ", " + lvl[(int)ranges[ranges.Count - 1].y - 1]);

        for (int i = 0; i < ranges.Count - 2; i += 2)
        {
            float preY = lvl[(int)ranges[i].y - 1].y;
            float postY = lvl[(int)ranges[i + 1].x].y;

            Debug.Log("PRE::: " + lvl[(int)ranges[i].x] + ", " + lvl[(int)ranges[i].y - 1]);
            Debug.Log("POST::: " + lvl[(int)ranges[i + 1].x] + ", " + lvl[(int)ranges[i + 1].y - 1]);

            Debug.Log("DIFF: " + System.Math.Abs(preY - postY));
            Debug.Log("PRE:: " + preY + " POST:: " + postY);

            if (System.Math.Abs(preY - postY) < 0.1)
            {
                for (int j = (int)ranges[i+1].x; j < (int)ranges[i+1].y; j++)
                {
                    lvl[j] = new Vector3(lvl[j].x, lvl[j].y - 1, lvl[j].z);
                    List<GameObject> gndTexts = ground[j];
                    for (int k = 0; k < gndTexts.Count; k++)
                    {
                        Vector3 pos = gndTexts[k].transform.position;
                        gndTexts[k].transform.position = new Vector3(pos.x, pos.y - 1, pos.z);
                    }
                }
                //ranges[i] = new Vector2(ranges[i].x, ranges[i].y - 1);
                //ranges[i + 1] = new Vector2(ranges[i + 1].x, ranges[i + 1].y - 1);
                //Debug.Log("CHANGE! " + "::: " + lvl[(int)preGap.x] + ", " + lvl[(int)preGap.y - 1]);
            }
        }

    }

    public void addCoins()
    {
        //look through the layout and add coins for extended strings
        //but not on top of spikes
        int start = 1;
        int end = 1;
        for (int l = 1; l < lvl.Count; l++)
        {
            if (lvl[l].z == -1)
            {
                end = l;
            } else if (lvl[l].z == 1)
            {
                end = l;
            } else
            {
                if (lvl[l].y != lvl[l-1].y)
                {
                    end = l;
                }
            }
            if (start != end)
            {
                if ((end - start) < 3)
                {
                    start = end + 1;
                    end = end + 1;
                } else
                {
                    if (Random.value < 0.66)
                    {
                        for (int i = start; i < Mathf.Min(end, lvl.Count - 2); i++)
                        {
                            //place coin
                            if (lvl[i].z == 1)
                            {
                                continue;
                            }
                            GameObject coin = new GameObject();
                            SpriteRenderer sr2 = coin.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            
                            coin.transform.position = new Vector3(lvl[i].x, lvl[i].y + 1f, 0);
                            
                            coin.transform.localScale = blockScale;
                            Sprite mySprite2 = Sprite.Create(texCoin, new Rect(0.0f, 0.0f, texCoin.width, texCoin.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr2.sprite = mySprite2;
                            sr2.sortingLayerName = "coin";
                            coins.Add(coin);

                            //Debug.Log("(" + lvl[i].z);// + " , " + lvl[i].x + ")");
                        }
                    }
                    start = end + 1;
                    end = end + 1;
                }
            }
        }
    }

    public void cleanUpStomps()
    {
        List<GameObject> toDelete = new List<GameObject>();
        //if these guys are near platforms that are higher
        for (int i = 0; i < stompers.Count; i++)
        {
            int position = (int)Mathf.Round(stompers[i].transform.position.x + 10);
            int start = position - 1;
            int end = position + 1;
            if (start < 0)
            {
                start = 0;
            }
            if (end > lvl.Count - 1)
            {
                end = lvl.Count - 1;
            }
            //Debug.Log("HI " + start + " , " + end + " , " + position + " , count " + stompers.Count);
            for (int j = start; j < end + 1; j++)
            {
                //Debug.Log(lvl[j].y + " , " + stompers[i].transform.position.y);
                if (System.Math.Abs(lvl[j].z - -1) > 0.1)
                {
                    if ((lvl[j].y) >= stompers[i].transform.position.y)
                    {
                        toDelete.Add(stompers[i]);
                    }
                }
            }
        }
        for (int j = 0; j < toDelete.Count; j++)
        {

            GameObject stomps = toDelete[j];
            bool success = stompers.Remove(toDelete[j]);
            Destroy(stomps);
            //Debug.Log("Success? " + success); 
        }
    }

    //searches through enemies list and throws away anyone whose path is in
    //too small or on the beginning stretch
    public void cleanUpEnemies()
    {
        List<GameObject> toDelete = new List<GameObject>();
        
        for (int i = 0; i < enemies.Count; i++)
        {
            bool death = false;
            int position = (int)Mathf.Round(enemies[i].transform.position.x + 10);
            int platformSize = 0;
            for (int j = position; j < lvl.Count; j++)
            {
                if (System.Math.Abs(lvl[j].z - -1) > 0.1)
                {
                    //Debug.Log(lvl[j].y + " , " + enemies[i].transform.position.y);
                    if (System.Math.Abs((lvl[j].y + 1) - enemies[i].transform.position.y) > 0.1)
                    {
                        break;
                    }
                    platformSize += 1;
                } else
                {
                    break;
                }
            }
            for (int j = position; j >= 0; j--)
            {
                if (j == 0) //if j is the beginning at any point, delete this guy cause his range reaches the beginning
                {
                    //Debug.Log("DEATH!");
                    death = true;
                }
                //Debug.Log("j: " + j);
                if (System.Math.Abs(lvl[j].z - -1) > 0.1)
                {
                    //Debug.Log(lvl[j].y + " , " + enemies[i].transform.position.y);
                    if (System.Math.Abs((lvl[j].y+1) - enemies[i].transform.position.y) > 0.1)
                    {
                        break;
                    }
                    platformSize += 1;
                }
                else
                {
                    break;
                }
            }
            //Debug.Log("Enemy " + i + " platformSize is " + platformSize);
            if (platformSize-1 <= 3 || death) //double counted position in forloops
            {
                toDelete.Add(enemies[i]);
            }
        }
        //Debug.Log("Deleted: " + toDelete.Count + "total: " + enemies.Count);
        for (int j = 0; j < toDelete.Count; j++)
        {
            
            GameObject en = toDelete[j];
            bool success = enemies.Remove(toDelete[j]);
            Destroy(en);
            //Debug.Log("Success? " + success); 
        }

    }

    public static void reset()
    {
        lvl.Clear();
        enemies.Clear();
        superEnemies.Clear();
        platforms.Clear();
        stompers.Clear();
        spikes.Clear();
        coins.Clear();
    }

    public void placeSuperEnemies()
    {
        //loop over spike array and add enemies
        Vector4 range = new Vector4(spikes[0].transform.position.x,0, spikes[0].transform.position.y, 1); //startX, endX, Ypos, length
        List<Vector4> ranges = new List<Vector4>();
        for (int i = 0; i < spikes.Count - 1; i++)
        {
            float x1 = spikes[i].transform.position.x;
            float x2 = spikes[i + 1].transform.position.x;
            float y1 = spikes[i].transform.position.y;
            float y2 = spikes[i + 1].transform.position.y;
            //Debug.Log(y1 + " : " + y2 + " - " + (y1-y2));
            if (System.Math.Abs(x2 - x1 - 1) < 0.1 &&
                System.Math.Abs(System.Math.Abs(y2 - y1)) < 0.1)
            {
                range.w += 1;
            } else
            {
                range.y = x1;
                //add enemies here
                ranges.Add(new Vector4(range.x, range.y, range.z, range.w));
                range.x = x2;
                range.y = x2;
                range.z = spikes[i + 1].transform.position.y;
                range.w = 1;
            }
            if (i == spikes.Count - 2)
            {
                ranges.Add(new Vector4(range.x, spikes[i+1].transform.position.x, range.z, range.w + 1));
            }
        }

        for (int i = 0; i < ranges.Count; i++)
        {
            //Debug.Log("Range" + i + " : " + ranges[i]);
            Vector4 block = ranges[i];
            if (block.w > 2)
            {
                int numEnemies = (int)(block.w / 3);
                bool dir = true;
                for (int j = 0; j < numEnemies; j++)
                {
                    //create enemy
                    //GameObject enemy = new GameObject();
                    //SpriteRenderer sr4 = enemy.AddComponent<SpriteRenderer>() as SpriteRenderer;
                    //sr4.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                    //if (dir)
                    //{
                    //    enemy.transform.position = new Vector3(block.x, block.z + 1, 0);
                    //} else
                    //{
                    //    enemy.transform.position = new Vector3(block.x + j*3, block.z + 1, 0);
                    //}
                    
                    //enemy.transform.localScale = blockScale;
                    //Sprite mySprite4 = Sprite.Create(texEnemy, new Rect(0.0f, 0.0f, texEnemy.width, texEnemy.height),
                    //         new Vector2(0.5f, 0.5f), 100.0f);
                    //sr4.sprite = mySprite4;
                    //enemy.AddComponent<BoxCollider2D>();
                    //enemy.AddComponent<Rigidbody2D>();
                    //enemy.GetComponent<Rigidbody2D>().isKinematic = true;
                    //enemy.AddComponent<EnemyController>();
                    ////Debug.Log("gbx: " + globalX + " , " + (globalX + 3));
                    //if (j == numEnemies - 1)
                    //{
                    //    enemy.GetComponent<EnemyController>().xRange = new Vector2(block.x + j * 3, block.y);
                    //} else
                    //{
                    //    enemy.GetComponent<EnemyController>().xRange = new Vector2(block.x + j * 3, block.x + j * 3 + 2);
                    //}
                    
                    ////Debug.Log("(" + (block.x + j * 3) + ", " + (block.x + j * 3 + 2) + ")");
                    ////enemy.GetComponent<EnemyController>().state = 1;
                    //superEnemies.Add(enemy);
                    //dir = !dir;
                }
            }
        }
        //Debug.Log("Num: " + superEnemies.Count);

    }

    public GeometryGenerator(List<Block> blocks, int startX, int startY, Texture2D ground,
        Texture2D enemy, Texture2D spike, Texture2D coin, Texture2D gnd, Texture2D goal)
    {
        this.blocks = blocks;
        globalX = -9;
        globalY = startY;
        state = new StateMachine();
        texGround = ground;
        texEnemy = enemy;
        texSpike = spike;
        texCoin = coin;
        texGnd = gnd;
        texGoal = goal;
    }

    //for now, assume geometry is 1x1 cubes
    public void generateGeometry()
    {
        //TODO tomorrow:
        //anytime you see a 0, call statemachine and generate
        //an appropriate cube/triangle to place in the level

        //loops over rhythm array in block to place geometry according
        //to the state machine
        //Debug.Log("BLOCKS: " + blocks.Count);
        //Debug.Log("STARTX: " + globalX);
        lvl.Add(new Vector3(0,0, -1));
        List<GameObject> l5 = new List<GameObject>();
        ground.Add(l5);
        for (int i = 0; i < blocks.Count; i++)
        {
            timer = 0;
            int[] rhythm = blocks[i].getRhythmArray();
            List<Vector2> action = blocks[i].getActionArray();
            int s = state.getNextState();
            //Debug.Log("GENERATOR RHYTHM LENGTH: " + rhythm.Length);

            //Break area:
            for (int k = 0; k < 4; k++)
            {
                GameObject cube = new GameObject();
                SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                cube.transform.position = new Vector3(globalX, globalY, 0);
                cube.transform.localScale = blockScale;
                Sprite mySprite = Sprite.Create(texGnd, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                         new Vector2(0.5f, 0.5f), 100.0f);
                sr.sprite = mySprite;
                cube.AddComponent<BoxCollider2D>();
                lvl.Add(new Vector3(globalX, globalY, 0));
                List<GameObject> l = new List<GameObject>();
                l.Add(cube);
                ground.Add(l);
                globalX += 1;
            }


            for (int j = 0; j < rhythm.Length; j++)
            {

                //don't want to move up at the end of a rhythm
                if (j == rhythm.Length - 1 || j == rhythm.Length - 2
                    || j == rhythm.Length - 3)
                {
                    state.resetClock();
                }

                //Debug.Log("S: " + s);

                if (rhythm[j] == 0)
                {
                    timer += 1;

                    //place a block at global x,y
                    if (s == 1) {
                        //make the ground spikes!!
                        if (RhythmGenerator.constraints[3] == 1)
                        {
                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube.transform.position = new Vector3(globalX, globalY, 0);
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();//BoxCollider2D bc = cube.AddComponent<BoxCollider2D>() as BoxCollider2D;
                                                               //lvl.Add(new Vector3(globalX, globalY,0));

                            GameObject cube2 = new GameObject();
                            SpriteRenderer sr2 = cube2.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube2.transform.position = new Vector3(globalX, globalY + 1, 0);
                            cube2.transform.localScale = blockScale;
                            Sprite mySprite2 = Sprite.Create(texSpike, new Rect(0.0f, 0.0f, texSpike.width, texSpike.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr2.sprite = mySprite2;
                            cube2.AddComponent<BoxCollider2D>();
                            lvl.Add(new Vector3(globalX, globalY + 1, 1));
                            List<GameObject> l = new List<GameObject>();
                            l.Add(cube2);
                            l.Add(cube);
                            ground.Add(l);
                            spikes.Add(cube);

                            //if (timer >= 3)
                            //{
                            //    GameObject enemy = new GameObject();
                            //    SpriteRenderer sr4 = enemy.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            //    sr4.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            //    enemy.transform.position = new Vector3(globalX, globalY + 2f, 0);
                            //    enemy.transform.localScale = blockScale;
                            //    Sprite mySprite4 = Sprite.Create(texEnemy, new Rect(0.0f, 0.0f, texEnemy.width, texEnemy.height),
                            //             new Vector2(0.5f, 0.5f), 100.0f);
                            //    sr4.sprite = mySprite4;
                            //    enemy.AddComponent<BoxCollider2D>();
                            //    enemy.AddComponent<Rigidbody2D>();
                            //    enemy.GetComponent<Rigidbody2D>().isKinematic = true;
                            //    enemy.AddComponent<EnemyController>();
                            //    //Debug.Log("gbx: " + globalX + " , " + (globalX + 3));
                            //    enemy.GetComponent<EnemyController>().xRange = new Vector2(globalX - 3, globalX);

                            //    if (dir)
                            //    {
                            //        enemy.GetComponent<EnemyController>().state = 1;
                            //    }
                                
                            //    superEnemies.Add(enemy);
                            //    timer = 0;
                            //    dir = !dir;
                            //}

                        } else
                        {
                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube.transform.position = new Vector3(globalX, globalY, 0);
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();//BoxCollider2D bc = cube.AddComponent<BoxCollider2D>() as BoxCollider2D;
                                                               //lvl.Add(new Vector3(globalX, globalY,0));

                            GameObject cube2 = new GameObject();
                            SpriteRenderer sr2 = cube2.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube2.transform.position = new Vector3(globalX, globalY + 1, 0);
                            cube2.transform.localScale = blockScale;
                            Sprite mySprite2 = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr2.sprite = mySprite2;
                            cube2.AddComponent<BoxCollider2D>();
                            lvl.Add(new Vector3(globalX, globalY + 1, 0));
                            List<GameObject> l = new List<GameObject>();
                            l.Add(cube2);
                            l.Add(cube);
                            ground.Add(l);
                        }
                        

                    } else
                    {
                        if (RhythmGenerator.constraints[3] == 1)
                        {
                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube.transform.position = new Vector3(globalX, globalY, 0);
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texSpike, new Rect(0.0f, 0.0f, texSpike.width, texSpike.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();
                            lvl.Add(new Vector3(globalX, globalY, 1));
                            spikes.Add(cube);

                            List<GameObject> l = new List<GameObject>();
                            l.Add(cube);
                            ground.Add(l);

                            //if (timer >= 3)
                            //{
                            //    GameObject enemy = new GameObject();
                            //    SpriteRenderer sr4 = enemy.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            //    sr4.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            //    enemy.transform.position = new Vector3(globalX, globalY + 1f, 0);
                            //    enemy.transform.localScale = blockScale;
                            //    Sprite mySprite4 = Sprite.Create(texEnemy, new Rect(0.0f, 0.0f, texEnemy.width, texEnemy.height),
                            //             new Vector2(0.5f, 0.5f), 100.0f);
                            //    sr4.sprite = mySprite4;
                            //    enemy.AddComponent<BoxCollider2D>();
                            //    enemy.AddComponent<Rigidbody2D>();
                            //    enemy.GetComponent<Rigidbody2D>().isKinematic = true;
                            //    enemy.AddComponent<EnemyController>();
                            //    //Debug.Log("gbx: " + globalX + " , " + (globalX + 3));
                            //    enemy.GetComponent<EnemyController>().xRange = new Vector2(globalX - 3, globalX);

                            //    if (dir)
                            //    {
                            //        enemy.GetComponent<EnemyController>().state = 1;
                            //    }

                            //    superEnemies.Add(enemy);
                            //    timer = 0;
                            //    dir = !dir;
                            //}
                        }
                        else
                        {
                            GameObject cube2 = new GameObject();
                            SpriteRenderer sr2 = cube2.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube2.transform.position = new Vector3(globalX, globalY, 0);
                            cube2.transform.localScale = blockScale;
                            Sprite mySprite2 = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr2.sprite = mySprite2;
                            cube2.AddComponent<BoxCollider2D>();
                            lvl.Add(new Vector3(globalX, globalY, 0));
                            List<GameObject> l = new List<GameObject>();
                            l.Add(cube2);
                            ground.Add(l);
                        }

                    }

                    globalX += 1;
                    if (s == 1)
                    {
                        globalY += 1;
                    }
                    if (s == 2)
                    {
                        globalY -= 1;
                    }
                    s = state.getNextState();
                } else if (rhythm[j] == 1)
                {
                    //investigate action array to figure out what we should do

                    //0 - cliff, 1 - enemy, 2 - wait
                    //each array index is a second.
                    //short = 1 spot (0 diff)
                    //medium = 2 spots (1 diff)
                    //long = 4 spots (3 diff)
                    //enemy = 2 spots

                    Vector2 act = action[0]; //[type, duration]
                    if (System.Math.Abs(act.x) < 0.1)
                    {
                        //cliff
                        //a duration of 0 corresponds to spike

                        if (System.Math.Abs(act.y) < 0.1)
                        {
                            //spike

                            GameObject spike = new GameObject();
                            SpriteRenderer sr = spike.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            spike.transform.position = new Vector3(globalX, globalY, 0);
                            spike.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texSpike, new Rect(0.0f, 0.0f, texSpike.width, texSpike.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            spike.AddComponent<BoxCollider2D>();
                            lvl.Add(new Vector3(globalX, globalY, 1));
                            List<GameObject> l = new List<GameObject>();
                            l.Add(spike);
                            ground.Add(l);
                            spikes.Add(spike);
                        } else
                        {
                            for (int k = globalX + 1; k < globalX + act.y + 2; k++)
                            {
                                lvl.Add(new Vector3(0, 0, -1));
                                List<GameObject> l2 = new List<GameObject>();
                                ground.Add(l2);
                            }

                            if (RhythmGenerator.constraints[3] == 1)
                            {
                                state.resetClock();
                                globalY -= 1;
                            }
                        }

                        globalX += (int) (act.y + 1);


                    } else if (System.Math.Abs(act.x - 1) < 0.1)
                    {
                        //enemy

                        if (RhythmGenerator.constraints[3] == 1)
                        {
                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube.transform.position = new Vector3(globalX, globalY, 0);
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texSpike, new Rect(0.0f, 0.0f, texSpike.width, texSpike.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();
                            lvl.Add(new Vector3(globalX, globalY, 1));
                            spikes.Add(cube);
                            List<GameObject> l = new List<GameObject>();
                            l.Add(cube);
                            ground.Add(l);
                        } else
                        {
                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube.transform.position = new Vector3(globalX, globalY, 0);
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();
                            lvl.Add(new Vector3(globalX, globalY, 0));
                            List<GameObject> l = new List<GameObject>();
                            l.Add(cube);
                            ground.Add(l);
                        }

                        GameObject enemy = new GameObject();
                        SpriteRenderer sr2 = enemy.AddComponent<SpriteRenderer>() as SpriteRenderer;
                        sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                        enemy.transform.position = new Vector3(globalX, globalY + 1f, 0);
                        enemy.transform.localScale = blockScale;
                        Sprite mySprite2 = Sprite.Create(texEnemy, new Rect(0.0f, 0.0f, texEnemy.width, texEnemy.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
                        sr2.sprite = mySprite2;
                        enemy.AddComponent<BoxCollider2D>();
                        enemy.AddComponent<Rigidbody2D>();
                        enemy.GetComponent<Rigidbody2D>().isKinematic = true;
                        enemy.name = "" + enemyID;//enemy.GetComponent<BoxCollider2D>().isTrigger = true;
                        enemy.AddComponent<EnemyController>();
                        enemyID += 1;
                        enemies.Add(enemy);

                        //Debug.Log("!!!!!ENEMY!!!!");

                        globalX += 1;
                    } else if (System.Math.Abs(act.x - 2) < 0.1)
                    {
                        if  (Random.value < 0.5 && RhythmGenerator.constraints[3] != 1)
                        {

                            // globalX + [X+1 - m] + [X + 2 - space] + newGlobalX
                            //Debug.Log("!!!!!WAIT!!!!");
                            //WAIT - either a moving platform or stomper
                            //place a block and add a platformcontroller to it so it moves up and down

                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube.transform.position = new Vector3(globalX, globalY, 0);
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();
                            lvl.Add(new Vector3(globalX, globalY, 0));
                            List<GameObject> l = new List<GameObject>();
                            l.Add(cube);
                            ground.Add(l);

                            GameObject cube2 = new GameObject();
                            SpriteRenderer sr2 = cube2.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube2.transform.position = new Vector3(globalX, globalY + 1, 0);
                            cube2.transform.localScale = blockScale;
                            Sprite mySprite2 = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr2.sprite = mySprite2;
                            cube2.AddComponent<BoxCollider2D>();
                            cube2.GetComponent<BoxCollider2D>().isTrigger = true;
                            BoxCollider2D b = cube2.GetComponent<BoxCollider2D>();
                            b.size = b.bounds.size * 0.1f;
                            cube2.AddComponent<Rigidbody2D>();
                            cube2.name = "smasher";
                            cube2.AddComponent<SmasherController>();
                            cube2.GetComponent<Rigidbody2D>().freezeRotation = true;
                            cube2.GetComponent<Rigidbody2D>().isKinematic = false;
                            stompers.Add(cube2);
                            globalX += 1;
                            
                        } else
                        {
                            state.resetClock();

                            lvl.Add(new Vector3(globalX, globalY, -1));
                            List<GameObject> l = new List<GameObject>();
                            ground.Add(l);

                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube.transform.position = new Vector3(globalX + 1, globalY, 0);
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            lvl.Add(new Vector3(globalX + 1, globalY, 2));
                            cube.AddComponent<BoxCollider2D>();
                            cube.AddComponent<Rigidbody2D>();
                            cube.name = "platform";
                            cube.AddComponent<PlatformController>();
                            cube.GetComponent<Rigidbody2D>().freezeRotation = true;
                            cube.GetComponent<Rigidbody2D>().isKinematic = false;
                            List<GameObject> l2 = new List<GameObject>();
                            l2.Add(cube);
                            ground.Add(l2);

                            lvl.Add(new Vector3(globalX + 2, globalY, -1));
                            ground.Add(l);

                            platforms.Add(cube);

                            globalX += 3;
                        }
                    }
                    action.RemoveAt(0);
                }
                
            }
        }

        //Break area:
        for (int k = 0; k < 4; k++)
        {
            GameObject cube = new GameObject();
            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            cube.transform.position = new Vector3(globalX, globalY, 0);
            cube.transform.localScale = blockScale;
            Sprite mySprite = Sprite.Create(texGnd, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                     new Vector2(0.5f, 0.5f), 100.0f);
            sr.sprite = mySprite;
            cube.AddComponent<BoxCollider2D>();
            lvl.Add(new Vector3(globalX, globalY, 0));
            List<GameObject> l = new List<GameObject>();
            l.Add(cube);
            ground.Add(l);
            globalX += 1;
        }


        //add goal sprite at last block!
        GameObject cube3 = new GameObject();
        SpriteRenderer sr3 = cube3.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr3.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
        cube3.transform.position = new Vector3(globalX - 1, globalY + 1.5f, 0);
        cube3.transform.localScale = blockScale;
        Sprite mySprite3 = Sprite.Create(texGoal, new Rect(0.0f, 0.0f, texGoal.width, texGoal.height),
                 new Vector2(0.5f, 0.5f), 100.0f);
        sr3.sprite = mySprite3;
        cube3.AddComponent<BoxCollider2D>();

        //add impenetrable invisible wall to keep players from falling off the map
        lvl.Add(new Vector3(globalX, globalY + 3, 0));
        lvl.Add(new Vector3(globalX, globalY + 4, 0));

        //Debug.Log("ENDX: " + globalX);
        lvl.Add(new Vector3(0, 0, -1));

        List<GameObject> l3 = new List<GameObject>();
        ground.Add(l3);
        ground.Add(l3);
        ground.Add(l3);

        addCoins();
    }
}

//state machine for flat (0), slantUp (1), and slantDown(2) platforms
public class StateMachine : MonoBehaviour
{
    public int curr = 0;
    private int stateTimer = 0;
    private int last = -1;
    bool first;

    public StateMachine()
    {
        first = true;
    }

    public int getTime()
    {
        return stateTimer;
    }

    public void resetClock()
    {
        stateTimer = 0;
    }

    public int getNextState()
    {
        if (first)
        {
            first = false;
            return curr;
        }

        float val = Random.value;
        //can adjust these parameters later on to get the overall shape of the
        //terrain!
        if (RhythmGenerator.constraints[3] == 1 && stateTimer < 3)
        {
            stateTimer += 1;
            curr = 0;
            last = curr;
            return curr;
        }
        switch (curr)
        {
            case 0:
                if (val < 0.8)
                {
                    curr = 0;
                } else if (val < 0.9)
                {
                    curr = 1;
                } else
                {
                    curr = 2;
                }
                break;
            case 1:
                if (val < 0.3)
                {
                    curr = 1;
                }
                else
                {
                    curr = 0;
                }
                break;
            case 2:
                if (val < 0.2)
                {
                    curr = 2;
                }
                else
                {
                    curr = 0;
                }
                break;
        }
        if (RhythmGenerator.constraints[3] == 1)
        {
            if (last != curr)
            {
                last = curr;
                stateTimer = 0;
            }
        }
        return curr;
    }
}
