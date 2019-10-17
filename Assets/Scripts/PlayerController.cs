using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: Channel inner minecraft phyiscs to get parabolic motion for jump
//and tweak linecasting function to ensure it works
public class PlayerController : MonoBehaviour
{
    public float speed = 0.1f;
    Vector2 dest = Vector2.zero;
    private Vector3 start = new Vector3(-9f,2f,0f);
    public Texture2D tex;
    private Sprite mySprite;
    private SpriteRenderer sr;

    public static bool win = false;
    public static bool death = false;

    public float v_x = 0;
    public float max_vx = 0.15f;
    public float v_y = 0;
    public float a_y = 0.1f;
    public float a_x = 0.01f;
    public float f_x = 0;
    public bool f_ext = false;
    public float f_vy = 0;
    public int pIndex = -1;
    public bool isAir = false;
    public float obstacleY = 0;
    public float obstacleY1 = 0;

    public int prevStomp = -1;
    public int prevEnemy = -1;
    public int prevSpike = -1;
    public int prevPlatform = -1;

    public Vector2 boundBox = new Vector2(0.5f,0.5f);

    public static List<GameObject> enemies;
    public static List<GameObject> platforms;
    public static List<GameObject> stompers;
    public static List<GameObject> spikes;
    public static List<GameObject> coins;

    public static int numLives = 5;
    public static int numCoins = 0;

    Vector2 lastValidPos;

    Vector2 direction = Vector2.right;

    //x = x_0 + vxt
    //y = y_0 + vyt + -ayt^2;
    //in fixedupdate, change_t should always be 1

    void Awake()
    {
        sr = this.gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
        this.gameObject.transform.position = start;
        this.gameObject.transform.localScale = new Vector3(4f, 4f, 4f);
    }

    // Start is called before the first frame update
    void Start()
    {
        dest = transform.position;
        a_x = 0f;// 0.015f;
        lastValidPos = transform.position;

        enemies = GeometryGenerator.enemies;
        platforms = GeometryGenerator.platforms;
        stompers = GeometryGenerator.stompers;
        spikes = GeometryGenerator.spikes;
        coins = GeometryGenerator.coins;

        mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        sr.sprite = mySprite;

        List<Vector3> lvl = GeometryGenerator.lvl;

        //Debug.Log("sTomPS: " + stompers.Count);
        //for (int i = 0; i < lvl.Count; i++)
        //{
        //    Debug.Log(lvl[i]);
        //}
    }

    public static void reset()
    {
        numLives = 5;
        numCoins = 0;
        enemies = GeometryGenerator.enemies;
        platforms = GeometryGenerator.platforms;
        stompers = GeometryGenerator.stompers;
        spikes = GeometryGenerator.spikes;
        coins = GeometryGenerator.coins;
        win = false;
        death = false;
    }

    private void Move()
    {
        // Move closer to Destination
        if (!f_ext)
        {
            dest = new Vector2(transform.position.x + v_x, transform.position.y + v_y - a_y);
        } else
        {
            obstacleY = platforms[pIndex].transform.position.y;
            obstacleY1 = obstacleY + 1;
            dest = new Vector2(transform.position.x + v_x, platforms[pIndex].transform.position.y + 1 + v_y);
            if (isAir)
            {
                dest = new Vector2(transform.position.x + v_x, transform.position.y + v_y - a_y);
            }
            //fext is no longer true when we move up
            //Debug.Log("Dest: " + dest);
        }
        
        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

        v_y = Mathf.Max(v_y - a_y * 2f, 0);

        if (direction.x == 1)
        {
            v_x = Mathf.Max(v_x + f_x - a_x, 0);
            v_x = Mathf.Min(v_x, max_vx);
        }
        else if (direction.x == -1)
        {
            v_x = Mathf.Min(v_x - f_x + a_x, 0);
            v_x = Mathf.Max(v_x, -max_vx);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        isAir = isAirborne(transform.position); //TESTING PURPOSES
        if (isAirborne(transform.position))
        {
            //dest = new Vector2(dest.x, dest.y - 0.1f);

            a_y = 0.1f;
        } else
        {
            a_y = 0;
        }

        if (Input.GetKey(KeyCode.Space) && valid(Vector2.up)
                            && !isAirborne(transform.position))
        {
            //dest = new Vector2(dest.x, dest.y + 2f);
            v_y = 1.5f;
            transform.parent = null;
        }
        if (Input.GetKey(KeyCode.RightArrow) && valid(0.5f*Vector2.right))
        {
            v_x = 0.15f;//f_x = 0.05f; //
            direction.x = 1;
            transform.parent = null;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && valid(-0.5f*Vector2.right))
        {
            v_x = -0.15f;//f_x = 0.05f;// 
            direction.x = -1;
            transform.parent = null;
        } else
        {
            f_x = 0;
            v_x = 0;
        }

        Move();
        onCollideWithEnemy();
        onCollideWithPlatform();
        onCollideWithSmasher();
        onCollideWithSpike();
        onCollideWithCoin();
        onFallOfCliff();

        if (numCoins >= 5)
        {
            numLives += 1;
            numCoins = 0;
        }

        if (numLives <= 0)
        {
            //death
            death = true;
            SceneManager.LoadScene("Death");
        }
        if ((int)Mathf.Round(transform.position.x + 10) == (GeometryGenerator.lvl.Count - 1))
        {
            win = true;
            SceneManager.LoadScene("Death");
        }
    }

    bool isAirborne(Vector2 pos)
    {
        return !yCollide();
    }

    bool valid(Vector2 dir)
    {
        return !xCollide(dir);
    }

    bool xCollide(Vector2 dir)
    {
        List<Vector3> lvl = GeometryGenerator.lvl;
        Vector2 pos = transform.position;
        Vector2 posFuture = pos + dir;

        int posNow = (int)Mathf.Round(pos.x + 10);
        int posFtr = (int)Mathf.Round(posFuture.x + 10);

        //is there anything between curr x,y and future x,y?
        if (pos.x < posFuture.x)
        {
            for (int i = posNow + 1; i < posFtr + 1; i++)
            {
                if ((pos.y - 1) < lvl[i].y && System.Math.Abs(lvl[i].z - -1) > 0.1)
                {
                    return true;
                }
                if (System.Math.Abs(lvl[i].z - -1) < 0.01)
                {
                    //==-1
                    if (lvl[posNow].z != -1)
                    {
                        lastValidPos = lvl[posNow];//new Vector2(posNow, pos.y);
                        //Debug.Log("LAST VALID: " + lastValidPos);
                    }
                }
            }
        } else
        {
            for (int i = posFtr; i < posNow; i++)
            {
                if ((pos.y - 1) < lvl[i].y)
                {
                    return true;
                }
                if (System.Math.Abs(lvl[i].z - -1) < 0.01)
                {
                    //==-1
                    if (lvl[posNow].z != -1)
                    {
                        lastValidPos = lvl[posNow];//new Vector2(posNow, pos.y);
                        //Debug.Log("LAST VALID: " + lastValidPos);
                    }
                }
            }
        }
        

        return false;
    }

    bool yCollide()
    {
        List<Vector3> lvl = GeometryGenerator.lvl;
        int pos = (int) Mathf.Round(transform.position.x + 10);
        if (pos < 0 || pos >= lvl.Count)
        {
            //Debug.Log("ERRRRRRR");
            return false;
        }

        Vector3 obstacle = lvl[pos];
        if (System.Math.Abs(obstacle.z - -1) < 0.01)
        {
            //cliff!
            //Debug.Log("-1!!!!!!!");
            return false;
        }
        if (System.Math.Abs(obstacle.z - 2) < 0.1) //this means its a moving platform
        {

            for (int i = 0; i < platforms.Count; i++)
            {
                Vector2 playerPos = transform.position;
                Vector2 platPos = platforms[i].transform.position;
                if (playerPos.x < platPos.x + 0.5f && playerPos.x > platPos.x - 0.5f &&
                (playerPos.y - 1) > (platPos.y - 0.1) && (playerPos.y - 1) < (platPos.y + 0.1))
                {
                    return true;
                }
            }
            return false;
        }
        //Debug.Log("Player: " + (transform.position.y - 1) + " ground " + obstacle.y);
        if (System.Math.Abs((transform.position.y - 1) - obstacle.y) < 0.1)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// guys randomly move out of bounds now??? , kill count seems ok now
    /// </summary>

    void onCollideWithEnemy()
    {
        List<int> toDelete = new List<int>();
        for (int i = 0; i < enemies.Count; i++)
        {
            GameObject en = enemies[i];
            Vector2 pos = transform.position;
            Vector2 enemy = en.transform.position;
            //want to know if transform.pos is in the bounding box of the enemy position
            //pos.x < enemy.x + 1 && pos.x > enemy.x - 1
            if (pos.x < (enemy.x + 0.5) && pos.x > (enemy.x - 0.5) /*&&
                pos.y < (enemy.y + 1) && pos.y >= (enemy.y - 1)*/)
            {
                if (pos.y - enemy.y < 1.0 && pos.y - enemy.y > 0.5)
                {
                    toDelete.Add(i);
                    v_y = 1.0f;
                } else if (pos.y < (enemy.y + 0.5) && pos.y > (enemy.y - 0.5))
                {
                    //Debug.Log("Collision with enemy: " + en.name);
                    if (prevEnemy != i)
                    {
                        numLives -= 1;
                        v_x = -0.5f * v_x;
                        prevEnemy = i;
                    }
                }
            }
            else
            {
                if (prevEnemy == i)
                {
                    prevEnemy = -1;
                }
            }
        }
        //Debug.Log("To delete count: " + toDelete.Count);
        for (int j = 0; j < toDelete.Count; j++)
        {
            GameObject en = enemies[toDelete[j]];
            enemies.RemoveAt(toDelete[j]);
            Destroy(en);
        }
        toDelete.Clear();
    }

    void onFallOfCliff()
    {
        if (transform.position.y < -6)
        {
            numLives -= 1;
            transform.position = lastValidPos + 4f*Vector2.up;
            v_y = 0;
            a_y = 0;
            v_x = 0;
        }
    }

    void onCollideWithPlatform()
    {
        f_ext = false;
        for (int i = 0; i < platforms.Count; i++)
        {
            GameObject plat = platforms[i];
            Vector2 platPos = plat.transform.position;
            Vector2 playerPos = transform.position;

            if (playerPos.x < platPos.x + 0.5f && playerPos.x > platPos.x - 0.5f &&
                (playerPos.y - 1) > (platPos.y - 1) && (playerPos.y - 1) < (platPos.y + 2))
            {
                //if (prevPlatform != i)
                //{
                //    transform.parent = platforms[i].transform;
                //    prevPlatform = i;
                //}

                f_ext = f_ext || true;
                f_vy = 0.1f;
                pIndex = i;
                a_y = 0;
            }
            else
            {
                //transform.parent = null;
                //if (prevPlatform == i)
                //{
                //    prevPlatform = -1;
                //}
                f_ext = f_ext || false;
            }
        }
    }

    void onCollideWithSmasher()
    {
        for (int i = 0; i < stompers.Count; i++)
        {
            Vector2 stp= stompers[i].transform.position;
            Vector2 cp = transform.position;
            if (cp.x < stp.x + 0.5f && cp.x > stp.x - 0.5f &&
                cp.y < stp.y + 0.5f && cp.y > stp.y - 0.5f)
            {
                if (prevStomp != i) {
                    //Debug.Log(i + " OUCH FROM BLOCK");
                    numLives -= 1;
                    prevStomp = i;
                }
            } else
            {
                if (prevStomp == i)
                {
                    prevStomp = -1;
                }
            }
        }
    }

    void onCollideWithSpike()
    {
        for (int i = 0; i < spikes.Count; i++)
        {
            Vector2 spike = spikes[i].transform.position;
            Vector2 cp = transform.position;
            //Debug.Log("You: " + (cp.y - 1) + " , spike: " + (spike.y + 0.5f));
            if (cp.x < spike.x + 0.5f && cp.x > spike.x - 0.5f &&
                (cp.y-1) < (spike.y + 0.5f) /*&& (cp.y-1) > spike.y + 0.5f*/)
            {
                if (prevSpike != i)
                {
                    //Debug.Log(i + " OUCH FROM SPIKE");
                    numLives -= 1;
                    prevSpike = i;
                }
            }
            else
            {
                if (prevSpike == i)
                {
                    prevSpike = -1;
                }
            }
        }
    }

    void onCollideWithCoin()
    {
        List<int> toDelete = new List<int>();
        for (int i = 0; i < coins.Count; i++)
        {
            GameObject c = coins[i];
            Vector2 pos = transform.position;
            Vector2 coin = c.transform.position;
            //want to know if transform.pos is in the bounding box of the enemy position
            //pos.x < enemy.x + 1 && pos.x > enemy.x - 1
            if (pos.x < (coin.x + 0.5) && pos.x > (coin.x - 0.5) &&
                pos.y < (coin.y + 0.5) && pos.y > (coin.y - 0.5))
            {
                toDelete.Add(i);
                numCoins += 1;
            }
        }
        //Debug.Log("To delete count: " + toDelete.Count);
        for (int j = 0; j < toDelete.Count; j++)
        {
            GameObject c = coins[toDelete[j]];
            coins.Remove(c);
            Destroy(c);
        }
        toDelete.Clear();
    }
}
