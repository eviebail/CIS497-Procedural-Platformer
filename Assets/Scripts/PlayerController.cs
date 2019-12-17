using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO: Channel inner minecraft phyiscs to get parabolic motion for jump
//and tweak linecasting function to ensure it works
public class PlayerController : MonoBehaviour
{

    public Texture2D texIdle1;
    public Texture2D texIdle2;
    public Texture2D texIdle3;
    public Texture2D texIdle4;
    public Texture2D texIdle5;
    public Texture2D texIdle6;
    public Texture2D texIdle7;
    public Texture2D texIdle8;

    public Texture2D texRun1R;
    public Texture2D texRun2R;
    public Texture2D texRun1L;
    public Texture2D texRun2L;

    public Texture2D texCrouch1R;
    public Texture2D texCrouch2R;
    public Texture2D texCrouch1L;
    public Texture2D texCrouch2L;

    public Texture2D texJumpL;
    public Texture2D texJumpR;
    public Texture2D texFallL;
    public Texture2D texFallR;

    Sprite mySprite4;
    Sprite mySprite3;
    Sprite mySprite2;
    Sprite mySprite1;
    Sprite mySprite5;
    Sprite mySprite6;
    Sprite mySprite7;
    Sprite mySprite8;
    Sprite mySprite9;
    Sprite mySprite10;
    Sprite mySprite11;
    Sprite mySprite12;

    Sprite mySprite13;
    Sprite mySprite14;
    Sprite mySprite15;
    Sprite mySprite16;

    Sprite mySprite17;
    Sprite mySprite18;
    Sprite mySprite19;
    Sprite mySprite20;

    public float speed = 0.1f;
    Vector2 dest = Vector2.zero;
    private Vector3 start = new Vector3(-9f, 2f, -3f);
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
    public bool xCollided = false;
    public Vector2 moveDir = new Vector2();

    public static int killed = 0;
    public static int totalEnemies = 0;

    public int prevStomp = -1;
    public Vector2 prevEnemy = new Vector2(-1, -1);
    public int prevEnemy2 = -1;
    public int prevSpike = -1;
    public int prevPlatform = -1;

    public Vector2 boundBox = new Vector2(0.5f, 0.5f);

    //need to do stomps too!

    public static List<GameObject> enemies;
    public static List<GameObject> lowerEnemies;
    public static List<GameObject> upperEnemies;
    public static List<GameObject> superEnemies;
    public static List<GameObject> platforms;
    public static List<GameObject> stompers;
    public static List<GameObject> spikes;
    public static List<GameObject> coins;
    public static List<GameObject> stars;
    public static List<Vector3> lvl;
    public static List<Vector3> upperLvl;
    public static List<Vector3> lowerLvl;
    public static List<Vector4> ranges;

    public static int numLives = 5;
    public static int numCoins = 0;
    public static int numStars = 0;

    public static int totalStars = 0;

    public static int timer = 0;

    Vector2 lastValidPos;

    Vector2 preWarpPos;

    Vector2 direction = Vector2.right;

    //x = x_0 + vxt
    //y = y_0 + vyt + -ayt^2;
    //in fixedupdate, change_t should always be 1

    void Awake()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, -3);
        sr = this.gameObject.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
        this.gameObject.transform.position = start;
        this.gameObject.transform.localScale = new Vector3(4f, 4f, 4f);

        mySprite4 = Sprite.Create(texIdle4, new Rect(0.0f, 0.0f, texIdle4.width, texIdle4.height),
              new Vector2(0.5f, 0.5f), 100.0f);
        mySprite3 = Sprite.Create(texIdle3, new Rect(0.0f, 0.0f, texIdle3.width, texIdle3.height),
                      new Vector2(0.5f, 0.5f), 100.0f);
        mySprite2 = Sprite.Create(texIdle2, new Rect(0.0f, 0.0f, texIdle2.width, texIdle2.height),
                      new Vector2(0.5f, 0.5f), 100.0f);
        mySprite1 = Sprite.Create(texIdle1, new Rect(0.0f, 0.0f, texIdle1.width, texIdle1.height),
                             new Vector2(0.5f, 0.5f), 100.0f);

        mySprite5 = Sprite.Create(texIdle5, new Rect(0.0f, 0.0f, texIdle5.width, texIdle5.height),
              new Vector2(0.5f, 0.5f), 100.0f);
        mySprite6 = Sprite.Create(texIdle6, new Rect(0.0f, 0.0f, texIdle3.width, texIdle3.height),
                      new Vector2(0.5f, 0.5f), 100.0f);
        mySprite7 = Sprite.Create(texIdle7, new Rect(0.0f, 0.0f, texIdle2.width, texIdle2.height),
                      new Vector2(0.5f, 0.5f), 100.0f);
        mySprite8 = Sprite.Create(texIdle8, new Rect(0.0f, 0.0f, texIdle1.width, texIdle1.height),
                             new Vector2(0.5f, 0.5f), 100.0f);

        mySprite9 = Sprite.Create(texRun1R, new Rect(0.0f, 0.0f, texRun1R.width, texRun1R.height),
              new Vector2(0.5f, 0.5f), 100.0f);
        mySprite10 = Sprite.Create(texRun2R, new Rect(0.0f, 0.0f, texRun2R.width, texRun2R.height),
                      new Vector2(0.5f, 0.5f), 100.0f);
        mySprite11 = Sprite.Create(texRun1L, new Rect(0.0f, 0.0f, texRun1L.width, texRun1L.height),
                      new Vector2(0.5f, 0.5f), 100.0f);
        mySprite12 = Sprite.Create(texRun2L, new Rect(0.0f, 0.0f, texRun2L.width, texRun2L.height),
                             new Vector2(0.5f, 0.5f), 100.0f);

        mySprite13 = Sprite.Create(texCrouch1R, new Rect(0.0f, 0.0f, texCrouch1R.width, texCrouch1R.height),
              new Vector2(0.5f, 0.5f), 100.0f);
        mySprite14 = Sprite.Create(texCrouch2R, new Rect(0.0f, 0.0f, texRun2R.width, texRun2R.height),
                      new Vector2(0.5f, 0.5f), 100.0f);
        mySprite15 = Sprite.Create(texCrouch1L, new Rect(0.0f, 0.0f, texRun1L.width, texRun1L.height),
                      new Vector2(0.5f, 0.5f), 100.0f);
        mySprite16 = Sprite.Create(texCrouch2L, new Rect(0.0f, 0.0f, texRun2L.width, texRun2L.height),
                             new Vector2(0.5f, 0.5f), 100.0f);

        mySprite17 = Sprite.Create(texJumpL, new Rect(0.0f, 0.0f, texJumpL.width, texJumpL.height),
              new Vector2(0.5f, 0.5f), 100.0f);
        mySprite18 = Sprite.Create(texJumpR, new Rect(0.0f, 0.0f, texJumpR.width, texJumpR.height),
                      new Vector2(0.5f, 0.5f), 100.0f);
        mySprite19 = Sprite.Create(texFallL, new Rect(0.0f, 0.0f, texFallL.width, texFallL.height),
                      new Vector2(0.5f, 0.5f), 100.0f);
        mySprite20 = Sprite.Create(texFallR, new Rect(0.0f, 0.0f, texFallR.width, texFallR.height),
                             new Vector2(0.5f, 0.5f), 100.0f);

    }
    public int srState = 1;

    void SwitchSprites()
    {
        if (srState == 1)
        {
            srState = 2;
            sr.sprite = mySprite1;
            return;
        }
        else if (srState == 2)
        {
            srState = 3;
            sr.sprite = mySprite2;
            return;
        }
        else if (srState == 3)
        {
            srState = 4;
            sr.sprite = mySprite3;
            return;
        }
        else if (srState == 4)
        {
            srState = 1;
            sr.sprite = mySprite4;
            return;
        }
        if (srState == 5)
        {
            srState = 6;
            sr.sprite = mySprite5;
            return;
        }
        else if (srState == 6)
        {
            srState = 7;
            sr.sprite = mySprite6;
            return;
        }
        else if (srState == 7)
        {
            srState = 8;
            sr.sprite = mySprite7;
            return;
        }
        else if (srState == 8)
        {
            srState = 5;
            sr.sprite = mySprite8;
            return;
        }
        if (srState == 9)
        {
            srState = 10;
            sr.sprite = mySprite9;
            return;
        }
        else if (srState == 10)
        {
            srState = 9;
            sr.sprite = mySprite10;
            return;
        }
        else if (srState == 11)
        {
            srState = 12;
            sr.sprite = mySprite11;
            return;
        }
        else if (srState == 12)
        {
            srState = 11;
            sr.sprite = mySprite12;
            return;
        }
        if (srState == 13)
        {
            srState = 14;
            sr.sprite = mySprite13;
            return;
        }
        else if (srState == 14)
        {
            srState = 13;
            sr.sprite = mySprite14;
            return;
        }
        else if (srState == 15)
        {
            srState = 16;
            sr.sprite = mySprite15;
            return;
        }
        else if (srState == 16)
        {
            srState = 15;
            sr.sprite = mySprite16;
            return;
        }
        else if (srState == 17)
        {
            sr.sprite = mySprite17;
            //if (v_y < 0)
            //{
            //    srState = 19;
            //}
            return;
        }
        else if (srState == 18)
        {
            sr.sprite = mySprite18;
            //if (v_y < 0)
            //{
            //    srState = 20;
            //}
            return;
        }
        else if (srState == 19)
        {
            sr.sprite = mySprite19;
            return;
        }
        else if (srState == 20)
        {
            sr.sprite = mySprite20;
            return;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SwitchSprites", 0f, 0.1f);

        if (RhythmGenerator.constraints[2] == 1)
        {
            numLives = 1;
        }
        else
        {
            numLives = 5;
        }

        dest = transform.position;
        a_x = 0f;// 0.015f;
        lastValidPos = transform.position;
        preWarpPos = transform.position;

        enemies = GeometryGenerator.enemies;
        upperEnemies = GeometryGenerator.upperEnemies;
        lowerEnemies = GeometryGenerator.lowerEnemies;
        superEnemies = GeometryGenerator.superEnemies;
        platforms = GeometryGenerator.platforms;
        stompers = GeometryGenerator.stompers;
        spikes = GeometryGenerator.spikes;
        coins = GeometryGenerator.coins;
        stars = GeometryGenerator.stars;
        lvl = GeometryGenerator.lvl;
        upperLvl = GeometryGenerator.upperLvl;
        lowerLvl = GeometryGenerator.lowerLvl;
        ranges = GeometryGenerator.lvlRanges;

        totalEnemies = enemies.Count + superEnemies.Count + lowerEnemies.Count + upperEnemies.Count;

        totalStars = stars.Count;

        mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        sr.sprite = mySprite;

        timer = (lvl.Count /*+ upperLvl.Count + lowerLvl.Count*/) / 2 - 5;

        if (RhythmGenerator.constraints[6] == 1)
        {
            InvokeRepeating("AdvanceTime", 1f, 1f);
        }

        Debug.Log("PLATFORMS: " + platforms.Count);
        //Debug.Log("LowEr lEveL");
        //for (int i = 0; i < lowerLvl.Count; i++)
        //{
        //    Debug.Log(lowerLvl[i]);
        //}
        //Debug.Log("sTomPS: " + stompers.Count);
        //for (int i = 0; i < lvl.Count; i++)
        //{
        //    Debug.Log(lvl[i]);
        //}
    }

    public static void reset()
    {
        if (RhythmGenerator.constraints[2] == 1)
        {
            numLives = 1;
        }
        else
        {
            numLives = 5;
        }
        numCoins = 0;
        killed = 0;
        numStars = 0;
        enemies = GeometryGenerator.enemies;
        upperEnemies = GeometryGenerator.upperEnemies;
        lowerEnemies = GeometryGenerator.lowerEnemies;
        superEnemies = GeometryGenerator.superEnemies;
        platforms = GeometryGenerator.platforms;
        stompers = GeometryGenerator.stompers;
        spikes = GeometryGenerator.spikes;
        coins = GeometryGenerator.coins;
        stars = GeometryGenerator.stars;
        lvl = GeometryGenerator.lvl;
        upperLvl = GeometryGenerator.upperLvl;
        lowerLvl = GeometryGenerator.lowerLvl;
        ranges = GeometryGenerator.lvlRanges;
        totalStars = stars.Count;
        win = false;
        death = false;
        totalEnemies = enemies.Count + superEnemies.Count + lowerEnemies.Count + upperEnemies.Count;
        timer = (lvl.Count /*+ upperLvl.Count + lowerLvl.Count*/) / 2 - 5;

    }

    void AdvanceTime()
    {
        timer = (int)Mathf.Max(timer - 1, 0);
        if (timer == 0)
        {
            death = true;
            end();
        }
    }

    private void Move()
    {
        // Move closer to Destination
        if (!f_ext)
        {
            dest = new Vector2(transform.position.x + v_x, transform.position.y + v_y - a_y);
        }
        else
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

    int prevSrState = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        isAir = isAirborne(transform.position); //TESTING PURPOSES
        if (isAirborne(transform.position))
        {
            //dest = new Vector2(dest.x, dest.y - 0.1f);

            a_y = 0.1f;
        }
        else
        {
            a_y = 0;
        }

        if (Input.GetKey(KeyCode.Space) && valid(Vector2.up)
                            && !isAirborne(transform.position))
        {
            if (direction.x == 1)
            {
                if (prevSrState != 18)
                {
                    prevSrState = 18;
                    srState = 18;
                }
            } else if (direction.x == -1)
            {
                if (prevSrState != 17)
                {
                    prevSrState = 17;
                    srState = 17;
                }
            }

            Debug.Log(srState + " <<<<<< ");
            if (RhythmGenerator.constraints[0] == 1)
            {
                death = true;
                end();
            }
            //dest = new Vector2(dest.x, dest.y + 2f);
            v_y = 1.5f;
            transform.parent = null;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (prevSrState != 9 && !isAir)
            {
                prevSrState = 9;
                srState = 9;
            }
            
            Vector2 dir = Vector2.right;
            //don't care about what's above range?
            if (lvl[(int)(transform.position.x + 10)].z == 3)
            {
                dir = new Vector2(1, 1);
            }
            if (lvl[(int)(transform.position.x + 10)].z == 4)
            {
                dir = new Vector2(-1, -1);
            }
            if (valid(0.5f * dir))
            {
                v_x = 0.15f;//f_x = 0.05f; //
                direction.x = 1;
                transform.parent = null;
            }

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (prevSrState != 11 && !isAir)
            {
                prevSrState = 11;
                srState = 11;
            }
            if (RhythmGenerator.constraints[5] == 1)
            {
                death = true;
                end();
            }
            Vector2 dir = Vector2.right;
            if (lvl[(int)(transform.position.x + 10)].z == 3)
            {
                dir = new Vector2(1, 1);
            }
            if (lvl[(int)(transform.position.x + 10)].z == 4)
            {
                dir = new Vector2(-1, -1);
            }
            if (valid(-0.5f * dir))
            {
                v_x = -0.15f;//f_x = 0.05f;// 
                direction.x = -1;
                transform.parent = null;
            }

        }
        else
        {
            if (direction.x == -1)
            {
                if (prevSrState != 5 && !isAir)
                {
                    prevSrState = 5;
                    srState = 5;
                }
            } else if (direction.x == 1)
            {
                if (prevSrState != 1 && !isAir)
                {
                    prevSrState = 1;
                    srState = 1;
                }
            }
            f_x = 0;
            v_x = 0;
            Debug.Log(srState + " <<<<< ");
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (direction.x == 1)
            {
                if (prevSrState != 13 && !isAir)
                {
                    prevSrState = 13;
                    srState = 13;
                }
            }
            else if (direction.x == -1)
            {
                if (prevSrState != 15 && !isAir)
                {
                    prevSrState = 15;
                    srState = 15;
                }
            }
        }

        if (Input.GetKey(KeyCode.DownArrow) && overPipe())
        {
            Debug.Log("DOWNARRRAOOWOAFEOKSG");
            //warp to destination
            if (upperLvl.Count > 0)
            {
                int pos = (int)Mathf.Round(transform.position.x - ranges[1].x);
                Debug.Log("WJOOOOOO " + pos + " , " + upperLvl.Count);
                if (pos >= 0 && pos < upperLvl.Count)
                {
                    Vector3 obstacle = upperLvl[pos];
                    Debug.Log("HIIIII " + obstacle.x + " , " + transform.position.x);
                    if (obstacle.x == (int)Mathf.Round(transform.position.x))
                    {
                        preWarpPos = lvl[(int)Mathf.Round(transform.position.x) + 10];
                        transform.position = new Vector3(obstacle.x, obstacle.y + 4, -3);
                    }
                }
            }
            if (lowerLvl.Count > 0)
            {
                int pos = (int)Mathf.Round(transform.position.x - ranges[2].x);
                if (pos >= 0 && pos < lowerLvl.Count)
                {
                    Vector3 obstacle = lowerLvl[pos];
                    Debug.Log("HIIIII " + obstacle.x + " , " + transform.position.x);
                    if (obstacle.x == (int)Mathf.Round(transform.position.x))
                    {
                        preWarpPos = lvl[(int)Mathf.Round(transform.position.x) + 10];
                        transform.position = new Vector3(obstacle.x, obstacle.y + 4, -3);
                    }
                }
            }
        }

        Move();
        onCollideWithEnemy();
        onCollideWithPlatform();
        onCollideWithSmasher();
        //onCollideWithSpike();
        onCollideWithCoin();
        onCollideWithStar();
        onFallOfCliff();

        if (numCoins >= 5)
        {
            if (RhythmGenerator.constraints[2] == 0)
            {
                numLives += 1;
            }
            numCoins = 0;
        }

        if (numLives <= 0)
        {
            //death
            death = true;
            end();
        }

        if (RhythmGenerator.constraints[1] == 1 && killed == totalEnemies &&
            (int)Mathf.Round(transform.position.x + 10) >= (lvl.Count - 4))
        {
            Reloader.win = true;
            Debug.Log("hello??" + win);
            end();
        }
        else if (RhythmGenerator.constraints[4] == 1
          && (int)Mathf.Round(transform.position.x + 10) >= (lvl.Count - 4)
          && numStars == totalStars)
        {
            Reloader.win = true;
            Debug.Log("hello??" + win);
            end();
        }
        else if (RhythmGenerator.constraints[1] == 0 && RhythmGenerator.constraints[4] == 0
            && (int)Mathf.Round(transform.position.x + 10) >= (lvl.Count - 4))
        {

            Reloader.win = true;
            Debug.Log("hello??" + win);
            end();
        }
    }

    void end()
    {
        SceneManager.LoadScene("Death");
    }

    bool isAirborne(Vector2 pos)
    {
        bool yCollided = yCollide();
        yCollided = yCollideUpper() || yCollided;
        yCollided = yCollideLower() || yCollided;
        return (!yCollided);
    }

    bool valid(Vector2 dir)
    {
        xCollided = xCollide(dir);
        xCollided = xCollideUpper(dir) && xCollided;
        xCollided = xCollideLower(dir) && xCollided;
        //I want each function to be called to update LastValidPos!
        return (!xCollided);
    }

    bool overPipe()
    {
        int pos = (int)Mathf.Round(transform.position.x + 10);
        if (pos < 0 || pos >= lvl.Count)
        {
            return false;
        }

        Vector3 obstacle = lvl[pos];
        if (obstacle.z == 5)
        {
            return true;
        }
        return false;
    }

    //bool overPipeLower()
    //{
    //    int pos = (int)Mathf.Round(transform.position.x - ranges[1].x);
    //    if (pos < 0 || pos >= upperLvl.Count)
    //    {
    //        return false;
    //    }

    //    Vector3 obstacle = upperLvl[pos];
    //    if (obstacle.z == 5)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    //bool overPipeUpper()
    //{
    //    int pos = (int)Mathf.Round(transform.position.x - ranges[2].x);
    //    if (pos < 0 || pos >= lowerLvl.Count)
    //    {
    //        return false;
    //    }

    //    Vector3 obstacle = lowerLvl[pos];
    //    if (obstacle.z == 5)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    bool xCollide(Vector2 dir)
    {
        Vector2 pos = transform.position;
        Vector2 posFuture = pos + dir;

        int posNow = (int)Mathf.Round(pos.x + 10);
        int posFtr = (int)Mathf.Round(posFuture.x + 10);

        if (pos.y < ranges[0].z)
        {
            return false;
        }

        //is there anything between curr x,y and future x,y?
        if (pos.x < posFuture.x)
        {
            for (int i = posNow + 1; i < posFtr + 1; i++)
            {
                if (lvl[i - 1].z == 3)
                {
                    if ((pos.y - 0.3) < lvl[i].y && System.Math.Abs(lvl[i].z - -1) > 0.1)
                    {
                        return true;
                    }
                }
                else
                {
                    if ((pos.y - 1) < lvl[i].y && System.Math.Abs(lvl[i].z - -1) > 0.1
                        && lvl[i].z != 3 && lvl[i + 1].z != 3)
                    {
                        return true;
                    }
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
        else
        {
            for (int i = posFtr; i < posNow; i++)
            {
                if (lvl[i - 1].z == 3)
                {
                    if ((pos.y - 0.4) < lvl[i].y && System.Math.Abs(lvl[i].z - -1) > 0.1)
                    {
                        return true;
                    }
                }
                else
                {
                    if ((pos.y - 1) < lvl[i].y && System.Math.Abs(lvl[i].z - -1) > 0.1
                        && lvl[i].z != 3 && lvl[i + 1].z != 3)
                    {
                        return true;
                    }
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

    bool xCollideUpper(Vector2 dir)
    {
        Vector2 pos = transform.position;
        Vector2 posFuture = pos + dir;

        int posNow = (int)(Mathf.Round(pos.x - ranges[1].x));
        int posFtr = (int)(Mathf.Round(posFuture.x - ranges[1].x));

        if (posNow < 0 || posNow >= upperLvl.Count || posFtr < 0 || posFtr >= upperLvl.Count
            || pos.y < ranges[1].z)
        {
            return false;
        }

        //is there anything between curr x,y and future x,y?
        if (pos.x < posFuture.x)
        {
            for (int i = posNow + 1; i < (int)Mathf.Min(posFtr + 1, upperLvl.Count); i++)
            {
                if (upperLvl[i - 1].z == 3)
                {
                    if ((pos.y - 0.3) < upperLvl[i].y && System.Math.Abs(upperLvl[i].z - -1) > 0.1)
                    {
                        return true;
                    }
                }
                else
                {
                    if ((pos.y - 1) < upperLvl[i].y && System.Math.Abs(upperLvl[i].z - -1) > 0.1
                        && upperLvl[i].z != 3 && upperLvl[i + 1].z != 3)
                    {
                        return true;
                    }
                }

                if (System.Math.Abs(upperLvl[i].z - -1) < 0.01)
                {
                    //==-1
                    if (upperLvl[posNow].z != -1)
                    {
                        lastValidPos = upperLvl[posNow];//new Vector2(posNow, pos.y);
                        //Debug.Log("LAST VALID: " + lastValidPos);
                    }
                }
            }
        }
        else
        {
            for (int i = (int)Mathf.Max(posFtr, 1); i < posNow; i++)
            {
                if (upperLvl[i - 1].z == 3)
                {
                    if ((pos.y - 0.4) < upperLvl[i].y && System.Math.Abs(upperLvl[i].z - -1) > 0.1)
                    {
                        return true;
                    }
                }
                else
                {
                    if ((pos.y - 1) < upperLvl[i].y && System.Math.Abs(upperLvl[i].z - -1) > 0.1
                        && upperLvl[i].z != 3 && upperLvl[i + 1].z != 3)
                    {
                        return true;
                    }
                }
                if (System.Math.Abs(upperLvl[i].z - -1) < 0.01)
                {
                    //==-1
                    if (upperLvl[posNow].z != -1)
                    {
                        lastValidPos = upperLvl[posNow];//new Vector2(posNow, pos.y);
                        //Debug.Log("LAST VALID: " + lastValidPos);
                    }
                }
            }
        }


        return false;
    }

    bool xCollideLower(Vector2 dir)
    {
        Vector2 pos = transform.position;
        Vector2 posFuture = pos + dir;

        int posNow = (int)(Mathf.Round(pos.x - ranges[2].x));
        int posFtr = (int)(Mathf.Round(posFuture.x - ranges[2].x));

        //Debug.Log("::x's " + posNow + " , " + posFtr);
        //Debug.Log("::range " + ranges[2].x + " , " + ranges[2].y);

        if (posNow < 0 || posNow >= lowerLvl.Count || posFtr < 0 || posFtr >= lowerLvl.Count
            || pos.y < ranges[2].z)
        {
            return false;
        }

        //Debug.Log("now vs ftr: " + pos.x + " , " + posFuture.x);

        //is there anything between curr x,y and future x,y?
        if (pos.x < posFuture.x)
        {
            for (int i = posNow + 1; i < (int)Mathf.Min(posFtr + 1, lowerLvl.Count); i++)
            {
                if (lowerLvl[i - 1].z == 3)
                {
                    if ((pos.y - 0.3) < lowerLvl[i].y && System.Math.Abs(lowerLvl[i].z - -1) > 0.1)
                    {
                        return true;
                    }
                }
                else
                {
                    if ((pos.y - 1) < lowerLvl[i].y && System.Math.Abs(lowerLvl[i].z - -1) > 0.1
                        && lowerLvl[i].z != 3 && lowerLvl[i + 1].z != 3)
                    {
                        return true;
                    }
                }

                if (System.Math.Abs(lowerLvl[i].z - -1) < 0.01)
                {
                    //==-1
                    if (lowerLvl[posNow].z != -1)
                    {
                        lastValidPos = lowerLvl[posNow];//new Vector2(posNow, pos.y);
                        //Debug.Log("LAST VALID: " + lastValidPos);
                    }
                }
            }
        }
        else
        {
            for (int i = (int)Mathf.Max(posFtr, 1); i < posNow; i++)
            {
                if (lowerLvl[i - 1].z == 3)
                {
                    if ((pos.y - 0.4) < lowerLvl[i].y && System.Math.Abs(lowerLvl[i].z - -1) > 0.1)
                    {
                        return true;
                    }
                }
                else
                {
                    if ((pos.y - 1) < lowerLvl[i].y && System.Math.Abs(lowerLvl[i].z - -1) > 0.1
                        && lowerLvl[i].z != 3 && lowerLvl[i + 1].z != 3)
                    {
                        return true;
                    }
                }
                if (System.Math.Abs(lowerLvl[i].z - -1) < 0.01)
                {
                    //==-1
                    Debug.Log("Am I -1??? " + lowerLvl[i].z);
                    if (lowerLvl[posNow].z != -1)
                    {
                        lastValidPos = lowerLvl[posNow];//new Vector2(posNow, pos.y);
                        //Debug.Log("LAST VALID: " + lastValidPos);
                    }
                }
            }
        }


        return false;
    }

    bool yCollide()
    {
        int pos = (int)Mathf.Round(transform.position.x + 10);
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
        if (obstacle.z == 3)
        {
            float x = transform.position.x - (obstacle.x - 0.5f);
            float b = (obstacle.y - 0.5f);
            //Debug.Log("y = mx + b: " + (transform.position.y - 1) + " v " + (x + b));
            if (transform.position.y > (x + b) && transform.position.y < (x + b + 0.5)) //y=mx + b
            {
                v_y = v_x;
                return true;
            }
            //change direction which we move
            return false;
        }
        if (obstacle.z == 4)
        {
            float x = transform.position.x - (obstacle.x - 0.5f);
            float b = (obstacle.y + 0.5f);
            //Debug.Log("y = mx + b: " + (transform.position.y - 1) + " v " + (x + b));
            if (transform.position.y > (-x + b) && transform.position.y < (-x + b + 0.5)) //y=mx + b
            {
                v_y = -v_x;
                return true;
            }
            //change direction which we move
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

    bool yCollideUpper()
    {
        int pos = (int)Mathf.Round(transform.position.x - ranges[1].x); //DOESNT WORK BECAUSE SIZE IS TOO SMALL
                                                                        //NEED TO ADJUST FOR SIZE DIFF
        if (pos < 0 || pos >= upperLvl.Count)
        {
            //Debug.Log("ERRRRRRR at pos " + pos + " , " + (pos - ranges[1].x));
            //Debug.Log("RANGES " + (ranges[1].x) + " , " + ranges[1].y);
            return false;
        }

        Vector3 obstacle = upperLvl[pos];
        if (System.Math.Abs(obstacle.z - -1) < 0.01)
        {
            //cliff!
            return false;
        }
        if (obstacle.z == 3)
        {
            float x = transform.position.x - (obstacle.x - 0.5f);
            float b = (obstacle.y - 0.5f);
            //Debug.Log("y = mx + b: " + (transform.position.y - 1) + " v " + (x + b));
            if (transform.position.y > (x + b) && transform.position.y < (x + b + 0.5)) //y=mx + b
            {
                v_y = v_x;
                return true;
            }
            //change direction which we move
            return false;
        }
        if (obstacle.z == 4)
        {
            float x = transform.position.x - (obstacle.x - 0.5f);
            float b = (obstacle.y + 0.5f);
            //Debug.Log("y = mx + b: " + (transform.position.y - 1) + " v " + (x + b));
            if (transform.position.y > (-x + b) && transform.position.y < (-x + b + 0.5)) //y=mx + b
            {
                v_y = -v_x;
                return true;
            }
            //change direction which we move
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

    bool yCollideLower()
    {
        int pos = (int)Mathf.Round(transform.position.x - ranges[2].x);
        if (pos < 0 || pos >= lowerLvl.Count)
        {
            //Debug.Log("ERRRRRRR");
            return false;
        }

        Vector3 obstacle = lowerLvl[pos];
        if (System.Math.Abs(obstacle.z - -1) < 0.01)
        {
            //cliff!
            //Debug.Log("-1!!!!!!!");
            return false;
        }
        if (obstacle.z == 3)
        {
            float x = transform.position.x - (obstacle.x - 0.5f);
            float b = (obstacle.y - 0.5f);
            //Debug.Log("y = mx + b: " + (transform.position.y - 1) + " v " + (x + b));
            if (transform.position.y > (x + b) && transform.position.y < (x + b + 0.5)) //y=mx + b
            {
                v_y = v_x;
                return true;
            }
            //change direction which we move
            return false;
        }
        if (obstacle.z == 4)
        {
            float x = transform.position.x - (obstacle.x - 0.5f);
            float b = (obstacle.y + 0.5f);
            //Debug.Log("y = mx + b: " + (transform.position.y - 1) + " v " + (x + b));
            if (transform.position.y > (-x + b) && transform.position.y < (-x + b + 0.5)) //y=mx + b
            {
                v_y = -v_x;
                return true;
            }
            //change direction which we move
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
            if (pos.x < (enemy.x + 0.7) && pos.x > (enemy.x - 0.7) /*&&
                pos.y < (enemy.y + 1) && pos.y >= (enemy.y - 1)*/)
            {
                if (pos.y - enemy.y < 1.0 && pos.y - enemy.y > 0.5)
                {
                    toDelete.Add(i);
                    v_y = 2.2f;
                }
                else if (pos.y < (enemy.y + 0.5) && pos.y > (enemy.y - 0.5))
                {
                    //Debug.Log("Collision with enemy: " + en.name);
                    if (prevEnemy.x != i || prevEnemy.y != 0)
                    {
                        numLives -= 1;
                        v_x = -0.5f * v_x;
                        prevEnemy.x = i;
                        prevEnemy.y = 0;
                    }
                }
            }
            else
            {
                if (prevEnemy.x == i && prevEnemy.y == 0)
                {
                    prevEnemy.x = -1;
                    prevEnemy.y = -1;
                }
            }
        }
        //Debug.Log("To delete count: " + toDelete.Count);
        killed += toDelete.Count;
        for (int j = 0; j < toDelete.Count; j++)
        {
            GameObject en = enemies[toDelete[j]];
            enemies.RemoveAt(toDelete[j]);
            Destroy(en);
        }
        toDelete.Clear();


        //FOR LOWER ENEMIES
        List<int> toDeleteL = new List<int>();
        for (int i = 0; i < lowerEnemies.Count; i++)
        {
            GameObject en = lowerEnemies[i];
            Vector2 pos = transform.position;
            Vector2 enemy = en.transform.position;
            //want to know if transform.pos is in the bounding box of the enemy position
            //pos.x < enemy.x + 1 && pos.x > enemy.x - 1
            if (pos.x < (enemy.x + 0.7) && pos.x > (enemy.x - 0.7) /*&&
                pos.y < (enemy.y + 1) && pos.y >= (enemy.y - 1)*/)
            {
                if (pos.y - enemy.y < 1.0 && pos.y - enemy.y > 0.5)
                {
                    toDeleteL.Add(i);
                    v_y = 2.2f;
                }
                else if (pos.y < (enemy.y + 0.5) && pos.y > (enemy.y - 0.5))
                {
                    //Debug.Log("Collision with enemy: " + en.name);
                    if (prevEnemy.x != i || prevEnemy.y != 1)
                    {
                        numLives -= 1;
                        v_x = -0.5f * v_x;
                        prevEnemy.x = i;
                        prevEnemy.y = 1;
                    }
                }
            }
            else
            {
                if (prevEnemy.x == i && prevEnemy.y == 1)
                {
                    prevEnemy.x = -1;
                    prevEnemy.y = -1;
                }
            }
        }
        //Debug.Log("To delete count: " + toDelete.Count);
        killed += toDeleteL.Count;
        for (int j = 0; j < toDeleteL.Count; j++)
        {
            GameObject en = lowerEnemies[toDeleteL[j]];
            lowerEnemies.RemoveAt(toDeleteL[j]);
            Destroy(en);
        }
        toDeleteL.Clear();

        //FOR UPPER ENEMIES
        List<int> toDeleteU = new List<int>();
        for (int i = 0; i < upperEnemies.Count; i++)
        {
            GameObject en = upperEnemies[i];
            Vector2 pos = transform.position;
            Vector2 enemy = en.transform.position;
            //want to know if transform.pos is in the bounding box of the enemy position
            //pos.x < enemy.x + 1 && pos.x > enemy.x - 1
            if (pos.x < (enemy.x + 0.7) && pos.x > (enemy.x - 0.7) /*&&
                pos.y < (enemy.y + 1) && pos.y >= (enemy.y - 1)*/)
            {
                if (pos.y - enemy.y < 1.0 && pos.y - enemy.y > 0.5)
                {
                    toDeleteU.Add(i);
                    v_y = 2.2f;
                }
                else if (pos.y < (enemy.y + 0.5) && pos.y > (enemy.y - 0.5))
                {
                    if (prevEnemy.x != i || prevEnemy.y != 2)
                    {
                        numLives -= 1;
                        v_x = -0.5f * v_x;
                        prevEnemy.x = i;
                        prevEnemy.y = 2;
                    }
                }
            }
            else
            {
                if (prevEnemy.x == i && prevEnemy.y == 2)
                {
                    prevEnemy.x = -1;
                    prevEnemy.y = -1;
                }
            }
        }
        //Debug.Log("To delete count: " + toDelete.Count);
        killed += toDeleteU.Count;
        for (int j = 0; j < toDeleteU.Count; j++)
        {
            GameObject en = upperEnemies[toDeleteU[j]];
            upperEnemies.RemoveAt(toDeleteU[j]);
            Destroy(en);
        }
        toDeleteU.Clear();

        //FOR SUPERENEMIES

        List<int> toDeleteSuper = new List<int>();
        for (int i = 0; i < superEnemies.Count; i++)
        {
            GameObject en = superEnemies[i];
            Vector2 pos = transform.position;
            Vector2 enemy = en.transform.position;
            //want to know if transform.pos is in the bounding box of the enemy position
            //pos.x < enemy.x + 1 && pos.x > enemy.x - 1
            if (pos.x < (enemy.x + 0.7) && pos.x > (enemy.x - 0.7) /*&&
                pos.y < (enemy.y + 1) && pos.y >= (enemy.y - 1)*/)
            {
                if (pos.y - enemy.y < 1.0 && pos.y - enemy.y > 0.5)
                {
                    toDeleteSuper.Add(i);
                    v_y = 2.0f;
                }
                else if (pos.y < (enemy.y + 0.5) && pos.y > (enemy.y - 0.5))
                {
                    //Debug.Log("Collision with enemy: " + en.name);
                    if (prevEnemy2 != i)
                    {
                        numLives -= 1;
                        v_x = -0.5f * v_x;
                        prevEnemy2 = i;
                    }
                }
            }
            else
            {
                if (prevEnemy2 == i)
                {
                    prevEnemy2 = -1;
                }
            }
        }
        //Debug.Log("To delete count: " + toDelete.Count);
        killed += toDeleteSuper.Count;
        for (int j = 0; j < toDeleteSuper.Count; j++)
        {
            GameObject en = superEnemies[toDeleteSuper[j]];
            superEnemies.RemoveAt(toDeleteSuper[j]);
            Destroy(en);
        }
        toDeleteSuper.Clear();

    }

    void onFallOfCliff()
    {
        if (transform.position.y < ranges[2].z - 5)
        {
            numLives -= 1;
            transform.position = lastValidPos + 4f * Vector2.up;
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
            Vector2 stp = stompers[i].transform.position;
            Vector2 cp = transform.position;
            if (cp.x < stp.x + 0.5f && cp.x > stp.x - 0.5f &&
                cp.y < stp.y + 0.5f && cp.y > stp.y - 0.5f)
            {
                if (prevStomp != i)
                {
                    //Debug.Log(i + " OUCH FROM BLOCK");
                    numLives -= 1;
                    prevStomp = i;
                }
            }
            else
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
                (cp.y - 1) < (spike.y + 0.5f) /*&& (cp.y-1) > spike.y + 0.5f*/)
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

    void onCollideWithStar()
    {
        List<int> toDelete = new List<int>();
        for (int i = 0; i < stars.Count; i++)
        {
            GameObject s = stars[i];
            Vector2 pos = transform.position;
            Vector2 star = s.transform.position;
            //want to know if transform.pos is in the bounding box of the enemy position
            //pos.x < enemy.x + 1 && pos.x > enemy.x - 1
            if (pos.x < (star.x + 0.5) && pos.x > (star.x - 0.5) &&
                pos.y < (star.y + 0.5) && pos.y > (star.y - 0.5))
            {
                transform.position = preWarpPos + 4f * Vector2.up;
                numStars += 1;
                toDelete.Add(i);
            }
        }
        //Debug.Log("To delete count: " + toDelete.Count);
        for (int j = 0; j < toDelete.Count; j++)
        {
            GameObject s = stars[toDelete[j]];
            stars.Remove(s);
            Destroy(s);
        }
        toDelete.Clear();
    }
}

//warp between areas
//place star in main level?
//integrate with other constraints