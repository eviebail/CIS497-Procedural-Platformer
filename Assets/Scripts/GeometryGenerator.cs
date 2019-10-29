using System.Collections;
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

    public Texture2D texGround;
    public Texture2D texEnemy;
    public Texture2D texSpike;
    public Texture2D texCoin;
    public Texture2D texGnd;
    public Texture2D texGoal;

    public int enemyID = 0; 

    //private Sprite mySprite;
    //private SpriteRenderer sr;
    // gameObject;

    //for loading 2d sprites instead of 3d objects
    void Awake()
    {
        
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
        platforms.Clear();
        stompers.Clear();
        spikes.Clear();
        coins.Clear();
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
        Debug.Log("BLOCKS: " + blocks.Count);
        Debug.Log("STARTX: " + globalX);
        lvl.Add(new Vector3(0,0, -1));
        for (int i = 0; i < blocks.Count; i++)
        {
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
                globalX += 1;
            }


            for (int j = 0; j < rhythm.Length; j++)
            {
                if (rhythm[j] == 0)
                {
                    //place a block at global x,y
                    if (s == 1) {

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
                        lvl.Add(new Vector3(globalX, globalY+1,0));

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
                        lvl.Add(new Vector3(globalX, globalY,0));
                    }

                    globalX += 1;
                    if (s == 1)
                    {
                        globalY += 1;
                    } else if (s == 2)
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
                            spikes.Add(spike);
                        } else
                        {
                            for (int k = globalX + 1; k < globalX + act.y + 2; k++)
                            {
                                lvl.Add(new Vector3(0, 0, -1));
                            }
                        }

                        globalX += (int) (act.y + 1);
                        

                    } else if (System.Math.Abs(act.x - 1) < 0.1)
                    {
                        //enemy

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
                        if  (Random.value < 0.5)
                        {

                            // globalX + [X+1 - m] + [X + 2 - space] + newGlobalX
                            //Debug.Log("!!!!!WAIT!!!!");
                            //WAIT - either a moving platform or stomper
                            //place a block and add a platformcontroller to it so it moves up and down

                            lvl.Add(new Vector3(globalX, globalY, -1));

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

                            lvl.Add(new Vector3(globalX+2, globalY, -1));

                            platforms.Add(cube);

                            globalX += 3;
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

        Debug.Log("ENDX: " + globalX);
        lvl.Add(new Vector3(0, 0, -1));
        addCoins();
    }
}

//state machine for flat (0), slantUp (1), and slantDown(2) platforms
public class StateMachine : MonoBehaviour
{
    private int curr = 0;
    bool first;

    public StateMachine()
    {
        first = true;
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
        return curr;
    }
}
