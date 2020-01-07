using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeometryGenerator : MonoBehaviour
{
    private List<Block> blocks; //Rhythm or puzzle block
    private int globalX;
    private int upGlobalX;
    private int lowGlobalX;
    private int globalY;
    private int upGlobalY;
    private int lowGlobalY;
    private int originalY;
    private StateMachine state;
    private Vector3 blockScale = new Vector3(6.25f,6.25f,6.25f);
    private int level = 0;

    public static List<GameObject> enemies = new List<GameObject>();
    public static List<GameObject> lowerEnemies = new List<GameObject>();
    public static List<GameObject> upperEnemies = new List<GameObject>();
    public static List<GameObject> superEnemies = new List<GameObject>();

    public static List<GameObject> platforms = new List<GameObject>();
    public static List<GameObject> upperPlatforms = new List<GameObject>();

    public static List<GameObject> stompers = new List<GameObject>();
    public static List<GameObject> lowerStompers = new List<GameObject>();
    public static List<GameObject> upperStompers = new List<GameObject>();

    public static List<GameObject> spikes = new List<GameObject>();
    public static List<GameObject> upperSpikes = new List<GameObject>();
    public static List<GameObject> lowerSpikes = new List<GameObject>();

    public static List<GameObject> coins = new List<GameObject>();
    public static List<GameObject> stars = new List<GameObject>();
    public static List<Vector4> lvlRanges = new List<Vector4>();

    public static List<List<GameObject>> ground = new List<List<GameObject>>();
    public static List<List<GameObject>> upperGround = new List<List<GameObject>>();
    public static List<List<GameObject>> lowerGround = new List<List<GameObject>>();

    int globalZ = 0;

    public static bool startLower = false;

    public static bool startUpper = false;

    //for loading 2d sprites instead of 3d objects
    void Awake()
    {
        
    }

    //Need to patch platformController
    //Add player climbing on platform much later...
    //basic set up of geometry generator working!

    public GeometryGenerator(List<Block> blocks)
    {
        this.blocks = blocks;
        globalX = -9;
        globalY = 0;
        originalY = 0;
        upGlobalX = globalX;
        upGlobalY = 15;
        lowGlobalX = globalX;
        lowGlobalY = -15;

        state = new StateMachine();
    }

    public static void reset()
    {
        enemies.Clear();
        lowerEnemies.Clear();
        upperEnemies.Clear();
        lowerSpikes.Clear();
        upperSpikes.Clear();
        lowerStompers.Clear();
        upperStompers.Clear();
        superEnemies.Clear();
        platforms.Clear();
        stompers.Clear();
        spikes.Clear();
        coins.Clear();
        for (int i = 0; i < ground.Count; i++)
        {
            ground[i].Clear();
        }
        ground.Clear();

        for (int i = 0; i < upperGround.Count; i++)
        {
            upperGround[i].Clear();
        }
        upperGround.Clear();

        for (int i = 0; i < lowerGround.Count; i++)
        {
            lowerGround[i].Clear();
        }
        lowerGround.Clear();

        lvlRanges.Clear();
        stars.Clear();
    }

    public void generateGeometry()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            int[] rhythm = blocks[i].getRhythmArray();
            List<Vector2> action = blocks[i].getActionArray();
            int groundState = 0;

            level = 0;// (int)blocks[i].getLevel();

            //break area
            for (int k = 0; k < 4; k++)
            {
                GameObject cube = new GameObject();
                SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                cube.transform.localScale = blockScale;
                Sprite mySprite = SpriteLoader.spriteGnd;
                sr.sprite = mySprite;
                cube.AddComponent<BoxCollider2D>();
                List<GameObject> l = new List<GameObject>();
                cube.transform.position = new Vector3(globalX, globalY, 0);
                l.Add(cube);
                globalX += 1;

                if (level == 0)
                {
                    ground.Add(l);
                }
                else if (level == 1)
                {
                    upperGround.Add(l);
                }
                else
                {
                    lowerGround.Add(l);
                }
            }

                for (int j = 0; j < rhythm.Length; j++)
            {
                if (rhythm[j] == 0) //this is a ground block
                {
                    groundState = state.getNextState(); //only update the state if it is not an action block!
                    if (groundState == 0) //flat_ground
                    {
                        //create sprite for flat ground
                        GameObject flat_ground = new GameObject();
                        SpriteRenderer sr = flat_ground.AddComponent<SpriteRenderer>() as SpriteRenderer;
                        flat_ground.transform.localScale = blockScale;
                        sr.sprite = SpriteLoader.spriteGround;
                        flat_ground.AddComponent<BoxCollider2D>();
                        flat_ground.transform.position = new Vector3(globalX, globalY, 0);

                        List<GameObject> l = new List<GameObject>();
                        l.Add(flat_ground);

                        if (level == 0) //==flat_ground + ground
                        {
                            ground.Add(l);
                        }
                        else if (level == 1) //==flat_ground + upperground
                        {
                            upperGround.Add(l);
                        } else //flat_ground + lowerground
                        {
                            lowerGround.Add(l);
                        }
                    } else if (groundState == 1) //slope_up
                    {
                        //create sprite for flat ground
                        GameObject flat_ground = new GameObject();
                        SpriteRenderer sr = flat_ground.AddComponent<SpriteRenderer>() as SpriteRenderer;
                        flat_ground.transform.localScale = blockScale;
                        sr.sprite = SpriteLoader.spriteUnderGnd;
                        flat_ground.AddComponent<BoxCollider2D>();
                        flat_ground.transform.position = new Vector3(globalX, globalY, 0);

                        GameObject slant_up_ground = new GameObject();
                        SpriteRenderer sr2 = slant_up_ground.AddComponent<SpriteRenderer>() as SpriteRenderer;
                        slant_up_ground.transform.localScale = blockScale;
                        sr2.sprite = SpriteLoader.spriteSlope;
                        slant_up_ground.AddComponent<BoxCollider2D>();
                        slant_up_ground.transform.position = new Vector3(globalX, globalY + 1, -0.5f);

                        List<GameObject> l = new List<GameObject>();
                        l.Add(slant_up_ground);
                        l.Add(flat_ground);

                        if (level == 0) //==up_slope + ground
                        {
                            ground.Add(l);
                        }
                        else if (level == 1) //==up_slope + upperground
                        {
                            upperGround.Add(l);
                        }
                        else //up_slope + lowerground
                        {
                            lowerGround.Add(l);
                        }
                    } else //groundState == 2 slope down
                    {
                        //create sprite for flat ground
                        GameObject flat_ground = new GameObject();
                        SpriteRenderer sr = flat_ground.AddComponent<SpriteRenderer>() as SpriteRenderer;
                        flat_ground.transform.localScale = blockScale;
                        sr.sprite = SpriteLoader.spriteUnderGnd;
                        flat_ground.AddComponent<BoxCollider2D>();
                        flat_ground.transform.position = new Vector3(globalX, globalY - 1, 0);

                        GameObject slant_down_ground = new GameObject();
                        SpriteRenderer sr2 = slant_down_ground.AddComponent<SpriteRenderer>() as SpriteRenderer;
                        slant_down_ground.transform.localScale = blockScale;
                        sr2.sprite = SpriteLoader.spriteSlope2;
                        slant_down_ground.AddComponent<BoxCollider2D>();
                        slant_down_ground.transform.position = new Vector3(globalX, globalY, -1);

                        List<GameObject> l = new List<GameObject>();
                        l.Add(slant_down_ground);
                        l.Add(flat_ground);

                        if (level == 0) //==up_slope + ground
                        {
                            ground.Add(l);
                        }
                        else if (level == 1) //==up_slope + upperground
                        {
                            upperGround.Add(l);
                        }
                        else //up_slope + lowerground
                        {
                            lowerGround.Add(l);
                        }
                    }
                } else
                { //rhythm[j] == 1 <- this is an action block
                    //investigate action array to figure out what we should do

                    //0 - cliff, 1 - enemy, 2 - wait
                    //each array index is a second.
                    //short = 1 spot (0 diff)
                    //medium = 2 spots (1 diff)
                    //long = 4 spots (3 diff)
                    //enemy = 2 spots
                    groundState = 0; //prevents the modification of globalY with action blocks
                    Vector2 act = action[0]; //[type, duration]
                    if (act.x == 0)
                    {
                        //a duration of zero corresponds to a spike
                        if (act.y == 0)
                        {
                            GameObject spike = new GameObject();
                            SpriteRenderer sr = spike.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            spike.transform.localScale = blockScale;
                            sr.sprite = SpriteLoader.spriteSpike;
                            spike.AddComponent<BoxCollider2D>();
                            spike.transform.position = new Vector3(globalX, globalY, 0);
                            List<GameObject> l = new List<GameObject>();
                            l.Add(spike);

                            if (level == 0)
                            {
                                ground.Add(l);
                                spikes.Add(spike);
                            } else if (level == 1)
                            {
                                upperGround.Add(l);
                                upperSpikes.Add(spike);
                            } else
                            {
                                lowerGround.Add(l);
                                lowerSpikes.Add(spike);
                            }
                        }
                        //a longer duration means cliff!!
                        else
                        {
                            int startingX = globalX;
                            for (int k = startingX; k < startingX + act.y; k++)
                            {
                                List<GameObject> cliff = new List<GameObject>();
                                if (level == 0)
                                {
                                    ground.Add(cliff);
                                }
                                else if (level == 1)
                                {
                                    upperGround.Add(cliff);
                                }
                                else
                                {
                                    lowerGround.Add(cliff);
                                }
                            }
                            globalX += (int)(act.y - 1); //-1 to account for globalX+=1 at end of loop
                        }
                    } else if (act.x == 1) //enemy
                    {
                        //make flat ground below froggy boi
                        GameObject flat_ground = new GameObject();
                        SpriteRenderer sr = flat_ground.AddComponent<SpriteRenderer>() as SpriteRenderer;
                        flat_ground.transform.localScale = blockScale;
                        sr.sprite = SpriteLoader.spriteGround;
                        flat_ground.AddComponent<BoxCollider2D>();
                        flat_ground.transform.position = new Vector3(globalX, globalY, 0);

                        List<GameObject> l = new List<GameObject>();
                        l.Add(flat_ground);

                        if (level == 0) //==up_slope + ground
                        {
                            ground.Add(l);
                        }
                        else if (level == 1) //==up_slope + upperground
                        {
                            upperGround.Add(l);
                        }
                        else //up_slope + lowerground
                        {
                            lowerGround.Add(l);
                        }

                        GameObject enemy = new GameObject();
                        enemy.transform.localScale = blockScale;
                        enemy.AddComponent<BoxCollider2D>();
                        enemy.AddComponent<Rigidbody2D>();
                        enemy.GetComponent<Rigidbody2D>().isKinematic = true;
                        enemy.AddComponent<EnemyController>();
                        SpriteRenderer sre = enemy.AddComponent<SpriteRenderer>() as SpriteRenderer;
                        sre.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                        sre.sprite = SpriteLoader.spriteEnemy;
                        enemy.transform.position = new Vector3(globalX, globalY + 1f, 0);

                        if (level == 0) //==up_slope + ground
                        {
                            enemies.Add(enemy);
                        }
                        else if (level == 1) //==up_slope + upperground
                        {
                            upperEnemies.Add(enemy);
                        }
                        else //up_slope + lowerground
                        {
                            lowerEnemies.Add(enemy);
                        }
                    } else if (act.x == 2) //moving platforms or smashers
                    {
                        if (Random.value < 0.5) //smasher!
                        {
                            //make flat ground for the smasher to smoosh you under
                            GameObject flat_ground = new GameObject();
                            SpriteRenderer sr = flat_ground.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            flat_ground.transform.localScale = blockScale;
                            sr.sprite = SpriteLoader.spriteGround;
                            flat_ground.AddComponent<BoxCollider2D>();
                            flat_ground.transform.position = new Vector3(globalX, globalY, 0);

                            List<GameObject> l = new List<GameObject>();
                            l.Add(flat_ground);

                            //create the smasher
                            GameObject smasher = new GameObject();
                            SpriteRenderer sr2 = smasher.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            smasher.transform.localScale = blockScale;
                            sr2.sprite = SpriteLoader.spriteMoveGnd;
                            smasher.AddComponent<BoxCollider2D>();
                            smasher.AddComponent<Rigidbody2D>();
                            smasher.name = "smasher";
                            smasher.AddComponent<SmasherController>();
                            smasher.GetComponent<Rigidbody2D>().freezeRotation = true;
                            smasher.GetComponent<Rigidbody2D>().isKinematic = false;
                            smasher.transform.position = new Vector3(globalX, globalY + 1, 0);

                            if (level == 0) //==up_slope + ground
                            {
                                ground.Add(l);
                                stompers.Add(smasher);
                            }
                            else if (level == 1) //==up_slope + upperground
                            {
                                upperGround.Add(l);
                                upperStompers.Add(smasher);
                            }
                            else //up_slope + lowerground
                            {
                                lowerGround.Add(l);
                                lowerStompers.Add(smasher);
                            }
                        } else //moving platforms --> cliff, platform, cliff
                        {
                            List<GameObject> l1 = new List<GameObject>();
                            List<GameObject> l2 = new List<GameObject>();
                            List<GameObject> l3 = new List<GameObject>();

                            GameObject mover = new GameObject();
                            SpriteRenderer sr = mover.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            mover.transform.localScale = blockScale;
                            sr.sprite = SpriteLoader.spriteMoveGnd;
                            mover.AddComponent<BoxCollider2D>();
                            mover.AddComponent<Rigidbody2D>();
                            mover.name = "platform";
                            mover.AddComponent<PlatformController>();
                            mover.GetComponent<Rigidbody2D>().freezeRotation = true;
                            mover.GetComponent<Rigidbody2D>().isKinematic = false;

                            mover.transform.position = new Vector3(globalX + 1, globalY, 0);
                            platforms.Add(mover);

                            if (level == 0)
                            {
                                ground.Add(l1);
                                ground.Add(l2);
                                ground.Add(l3);
                            } else if (level == 1)
                            {
                                upperGround.Add(l1);
                                upperGround.Add(l2);
                                upperGround.Add(l3);
                            } else
                            {
                                lowerGround.Add(l1);
                                lowerGround.Add(l2);
                                lowerGround.Add(l3);
                            }

                            globalX += 2;
                        } 
                    }
                    action.RemoveAt(0);
                }

                globalX += 1;

                if (groundState == 1)
                {
                    globalY += 1;
                }

                if (groundState == 2)
                {
                    globalY -= 1;
                }
            }
        }

        //final break area
        for (int k = 0; k < 3; k++)
        {
            GameObject cube = new GameObject();
            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            cube.transform.localScale = blockScale;
            sr.sprite = SpriteLoader.spriteGnd;
            cube.AddComponent<BoxCollider2D>();
            List<GameObject> l = new List<GameObject>();
            cube.transform.position = new Vector3(globalX, globalY, 0);
            l.Add(cube);
            globalX += 1;

            ground.Add(l);
        }

        //GOAL_TAG
        GameObject lastGround = new GameObject();
        SpriteRenderer srG = lastGround.AddComponent<SpriteRenderer>() as SpriteRenderer;
        srG.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
        lastGround.transform.localScale = blockScale;
        srG.sprite = SpriteLoader.spriteGnd;
        lastGround.AddComponent<BoxCollider2D>();

        GameObject goal = new GameObject();
        SpriteRenderer sr3 = goal.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr3.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
        goal.transform.position = new Vector3(globalX, globalY + 1.5f, 0);
        goal.transform.localScale = blockScale;
        sr3.sprite = SpriteLoader.spriteGoal;
        goal.AddComponent<BoxCollider2D>();

        List<GameObject> list = new List<GameObject>();
        lastGround.transform.position = new Vector3(globalX, globalY, 0);
        list.Add(goal);
        list.Add(lastGround);
        globalX += 1;

        ground.Add(list);

        


        //Debug.Log(":: " + ground.Count);
        //for (int i = 0; i < ground.Count; i++)
        //{
        //    Debug.Log("Pos: " + ground[i][0].transform.position.x + ", " + ground[i][0].transform.position.y);
        //    for (int j = 0; j < ground[i].Count; j++)
        //    {
        //        Debug.Log(ground[i][j].transform.position.z);
        //    }
        //}
    }
}

//state machine for flat (0), slantUp (1), and slantDown(2) platforms
public class StateMachine : MonoBehaviour
{
    public int curr = 0;
    bool freeze;


    public StateMachine()
    {
        freeze = false;
    }

    public void freezeFlat(bool isFreeze)
    {
        freeze = isFreeze;
    }

    public int getNextState()
    {
        //return 0;
        if (freeze)
        {
            return 0;
        }

        float val = Random.value;
        //can adjust these parameters later on to get the overall shape of the
        //terrain!
        
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
                if (val < 0.2)
                {
                    curr = 1;
                }
                else
                {
                    curr = 0;
                }
                break;
            case 2:
                if (val < 0.1)
                {
                    curr = 2;
                }
                else
                {
                    curr = 0;
                }
                break;
        }
        
        return curr;
    }
}
