using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO:
//fix hill issue and level ending conditions <-
//add a_x??
public class PlayerController : MonoBehaviour
{
    public float speed = 0.1f;
    public Vector2 dest = Vector2.zero;
    private Vector3 start = new Vector3(-9f, 2f, -3f);
    public Texture2D tex;
    private Sprite mySprite;
    private SpriteRenderer sr;

    public static bool win = false;
    public static bool death = false;

    public bool hit = false;
    public bool prevHit = false;

    public float v_x = 0;
    public float max_vx = 0.15f;
    public float v_y = 0;
    public float a_y = 0.2f;
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

    public int spriteState = 0;

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
    public static List<List<GameObject>> ground;
    public static List<List<GameObject>> upperGround;
    public static List<List<GameObject>> lowerGround;
    public static List<Vector4> ranges;

    public static int numLives = 5;
    public static int numCoins = 0;
    public static int numStars = 0;

    public static int totalStars = 0;

    public static int timer = 0;

    Vector2 lastValidPos;

    Vector2 preWarpPos;

    Vector2 direction = Vector2.right;

    bool DISABLE_CONTROLS = false;

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

    }
    public int srState = 1;

    void SwitchSprites()
    {
        if (DISABLE_CONTROLS && direction.x == 1)
        {
            srState = 22;
        } else if (DISABLE_CONTROLS)
        {
            srState = 21;
        }

        if (hit)
        {
            if (prevSrState != 23)
            {
                srState = 23;
                prevSrState = 23;
            }
        }

        if (srState == 1)
        {
            srState = 2;
            sr.sprite = SpriteLoader.spriteIdle1;
            return;
        }
        else if (srState == 2)
        {
            srState = 3;
            sr.sprite = SpriteLoader.spriteIdle2;
            return;
        }
        else if (srState == 3)
        {
            srState = 4;
            sr.sprite = SpriteLoader.spriteIdle3;
            return;
        }
        else if (srState == 4)
        {
            srState = 1;
            sr.sprite = SpriteLoader.spriteIdle4;
            return;
        }
        if (srState == 5)
        {
            srState = 6;
            sr.sprite = SpriteLoader.spriteIdle5;
            return;
        }
        else if (srState == 6)
        {
            srState = 7;
            sr.sprite = SpriteLoader.spriteIdle6;
            return;
        }
        else if (srState == 7)
        {
            srState = 8;
            sr.sprite = SpriteLoader.spriteIdle7;
            return;
        }
        else if (srState == 8)
        {
            srState = 5;
            sr.sprite = SpriteLoader.spriteIdle8;
            return;
        }
        if (srState == 9)
        {
            srState = 10;
            sr.sprite = SpriteLoader.spriteRun1R;
            return;
        }
        else if (srState == 10)
        {
            srState = 9;
            sr.sprite = SpriteLoader.spriteRun2R;
            return;
        }
        else if (srState == 11)
        {
            srState = 12;
            sr.sprite = SpriteLoader.spriteRun1L;
            return;
        }
        else if (srState == 12)
        {
            srState = 11;
            sr.sprite = SpriteLoader.spriteRun2L;
            return;
        }
        if (srState == 13)
        {
            srState = 14;
            sr.sprite = SpriteLoader.spriteCrouch1R;
            return;
        }
        else if (srState == 14)
        {
            srState = 13;
            sr.sprite = SpriteLoader.spriteCrouch2R;
            return;
        }
        else if (srState == 15)
        {
            srState = 16;
            sr.sprite = SpriteLoader.spriteCrouch1L;
            return;
        }
        else if (srState == 16)
        {
            srState = 15;
            sr.sprite = SpriteLoader.spriteCrouch2L;
            return;
        }
        else if (srState == 17)
        {
            sr.sprite = SpriteLoader.spriteJumpL;
            //if (v_y < 0)
            //{
            //    srState = 19;
            //}
            return;
        }
        else if (srState == 18)
        {
            sr.sprite = SpriteLoader.spriteJumpR;
            //if (v_y < 0)
            //{
            //    srState = 20;
            //}
            return;
        }
        else if (srState == 19)
        {
            sr.sprite = SpriteLoader.spriteFallL;
            return;
        }
        else if (srState == 20)
        {
            sr.sprite = SpriteLoader.spriteFallR;
            return;
        }
        else if (srState == 21)
        {
            sr.sprite = SpriteLoader.spriteDashL;
            return;
        }
        else if (srState == 22)
        {
            sr.sprite = SpriteLoader.spriteDashR;
            return;
        }
        else if (srState == 23)
        {
            srState = 24;
            sr.sprite = SpriteLoader.spriteHurt1;
            return;
        }
        else if (srState == 24)
        {
            srState = 23;
            sr.sprite = SpriteLoader.spriteHurt2;
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
            numLives = 15;
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
        ground = GeometryGenerator.ground;
        upperGround = GeometryGenerator.upperGround;
        lowerGround = GeometryGenerator.lowerGround;
        ranges = GeometryGenerator.lvlRanges;

        totalEnemies = enemies.Count + superEnemies.Count + lowerEnemies.Count + upperEnemies.Count;

        totalStars = stars.Count;

        mySprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
        sr.sprite = mySprite;

        timer = (ground.Count /*+ upperLvl.Count + lowerLvl.Count*/) / 2 - 5;

        if (RhythmGenerator.constraints[6] == 1)
        {
            InvokeRepeating("AdvanceTime", 1f, 1f);
        }

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
        ground = GeometryGenerator.ground;
        upperGround = GeometryGenerator.upperGround;
        lowerGround = GeometryGenerator.lowerGround;
        ranges = GeometryGenerator.lvlRanges;
        totalStars = stars.Count;
        win = false;
        death = false;
        totalEnemies = enemies.Count + superEnemies.Count + lowerEnemies.Count + upperEnemies.Count;
        timer = (ground.Count /*+ upperLvl.Count + lowerLvl.Count*/) / 2 - 5;

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

    bool prevDisable = false;

    bool startCliff = false;

    float xDest = 0;

    private void Move()
    {
        // Move closer to Destination
        if (!DISABLE_CONTROLS && !hit)
        {
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
                ////Debug.Log("Dest: " + dest);
            }
        }
        

        if (DISABLE_CONTROLS && !prevDisable)
        {
            prevDisable = true;
            dest = transform.position;
            if (direction.y != 0)
            {
                startCliff = true;
            } else
            {
                startCliff = false;
            }
            if (direction.x == 1)
            {
                dest = new Vector2(dest.x + 3, dest.y + 3*direction.y);
            } else
            {
                dest = new Vector2(dest.x - 3, dest.y + 3 * direction.y);
            }
        }

        if (hit)
        {
            if (prevHit)
            {
                dest = new Vector2(xDest, transform.position.y + v_y - a_y);
                if (!isAirborne(dest))
                {
                    prevHit = false;
                    hit = false;
                    v_x = 0;
                    v_y = 0;
                    if (direction.x == 1)
                    {
                        srState = 1;
                    } else
                    {
                        srState = 5;
                    }
                }
            } else
            {
                dest = transform.position;
                if (direction.x == 1)
                {
                    xDest = dest.x - 1;
                    dest = new Vector2(xDest, dest.y + v_y - a_y);
                }
                else
                {
                    xDest = dest.x + 1;
                    dest = new Vector2(xDest, dest.y + v_y - a_y);
                }
            }
            
            prevHit = true;
        }

        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

        if (DISABLE_CONTROLS && !startCliff)
        {
            int posX = (int)Mathf.Round(transform.position.x + 9);
            if (ground[posX].Count > 0)
            {
                if (ground[posX][0].transform.position.z == -1 ||
                    ground[posX][0].transform.position.z == -0.5f)
                {
                    dest = transform.position; //stop sliding!
                }
            }
        }

        if (DISABLE_CONTROLS && transform.position.x == dest.x && transform.position.y == dest.y)
        {
            DISABLE_CONTROLS = false;
            startCliff = false;
            if (direction.x == 1) {
                srState = 1;
            }
            if (direction.x == -1)
            {
                srState = 5;
            }
        }

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

            a_y = 0.125f;
        }
        else
        {
            a_y = 0;
        }

        //update lastValidPos
        if (ground[(int)Mathf.Round(transform.position.x + 9)].Count > 0)
        {
            lastValidPos = ground[(int)Mathf.Round(transform.position.x + 9)][0].transform.position;
        }

        if (Input.GetKey(KeyCode.Space) && valid(Vector2.up)
                            && !isAirborne(transform.position)
                            && !DISABLE_CONTROLS && !hit)
        {
            //Debug.Log(srState + " <<<<<< ");
            if (RhythmGenerator.constraints[0] == 1)
            {
                death = true;
                end();
            }
            //dest = new Vector2(dest.x, dest.y + 2f);
            v_y = 1.5f;
            transform.parent = null;
        }


        if (Input.GetKey(KeyCode.RightArrow) && !DISABLE_CONTROLS && !hit)
        {
            Vector2 dir = Vector2.right;
            //don't care about what's above range?
            if (ground[(int)(transform.position.x + 9)].Count > 0)
            {
                if (ground[(int)(transform.position.x + 9)][0].transform.position.z == -0.5f)
                {
                    dir = new Vector2(1, 1);
                }
                if (ground[(int)(transform.position.x + 9)][0].transform.position.z == -1)
                {
                    dir = new Vector2(-1, -1);
                }
            }
            if (valid(dir))
            {
                v_x = 0.15f;//f_x = 0.05f; //
                direction.x = 1;
                direction.y = dir.y;
                transform.parent = null;
            }

        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !DISABLE_CONTROLS && !hit)
        {
            if (RhythmGenerator.constraints[5] == 1)
            {
                death = true;
                end();
            }
            Vector2 dir = Vector2.right;
            if (ground[(int)(transform.position.x + 9)].Count > 0)
            {
                if (ground[(int)(transform.position.x + 9)][0].transform.position.z == -0.5f)
                {
                    dir = new Vector2(1, 1);
                }
                if (ground[(int)(transform.position.x + 9)][0].transform.position.z == -1)
                {
                    dir = new Vector2(-1, -1);
                }
            }
            if (valid(-1 * dir))
            {
                v_x = -0.15f;//f_x = 0.05f;// 
                direction.x = -1;
                direction.y = -1*dir.y;
                transform.parent = null;
            }

        }
        else
        {
            if (!DISABLE_CONTROLS && !hit)
            {
                f_x = 0;
                v_x = 0;
            }
            
            //Debug.Log(srState + " <<<<< ");
        }

        if (Input.GetKey(KeyCode.DownArrow) && overPipe() && !DISABLE_CONTROLS && !hit)
        {
            //Debug.Log("DOWNARRRAOOWOAFEOKSG");
            //warp to destination
            if (upperGround.Count > 0)
            {
                int pos = (int)Mathf.Round(transform.position.x - ranges[1].x);
                //Debug.Log("WJOOOOOO " + pos + " , " + upperLvl.Count);
                if (pos >= 0 && pos < upperGround.Count)
                {
                    Vector3 obstacle = upperGround[pos][0].transform.position;
                    //Debug.Log("HIIIII " + obstacle.x + " , " + transform.position.x);
                    if (obstacle.x == (int)Mathf.Round(transform.position.x))
                    {
                        preWarpPos = ground[(int)Mathf.Round(transform.position.x) + 10][0].transform.position;
                        transform.position = new Vector3(obstacle.x, obstacle.y + 4, -3);
                    }
                }
            }
            if (lowerGround.Count > 0)
            {
                int pos = (int)Mathf.Round(transform.position.x - ranges[2].x);
                if (pos >= 0 && pos < lowerGround.Count)
                {
                    Vector3 obstacle = lowerGround[pos][0].transform.position;
                    ////Debug.Log("HIIIII " + obstacle.x + " , " + transform.position.x);
                    if (obstacle.x == (int)Mathf.Round(transform.position.x))
                    {
                        preWarpPos = ground[(int)Mathf.Round(transform.position.x) + 10][0].transform.position;
                        transform.position = new Vector3(obstacle.x, obstacle.y + 4, -3);
                    }
                }
            }
        }

        animateSprite();
        //Invoke("animateDash", waitTimeDash);
        spriteState = srState;
        Move();
        onCollideWithEnemy();
        onCollideWithPlatform();
        onCollideWithSmasher();
        onCollideWithSpike();
        //onCollideWithCoin();
        //onCollideWithStar();
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
            (int)Mathf.Round(transform.position.x + 9) >= (ground.Count - 1))
        {
            Reloader.win = true;
            //Debug.Log("hello??" + win);
            end();
        }
        else if (RhythmGenerator.constraints[4] == 1
          && (int)Mathf.Round(transform.position.x + 9) >= (ground.Count - 1)
          && numStars == totalStars)
        {
            Reloader.win = true;
            //Debug.Log("hello??" + win);
            end();
        }
        else if (RhythmGenerator.constraints[1] == 0 && RhythmGenerator.constraints[4] == 0
            && (int)Mathf.Round(transform.position.x + 9) >= (ground.Count - 1))
        {

            Reloader.win = true;
            //Debug.Log("hello??" + win);
            end();
        }
    }

    void animateSprite()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (direction.x == 1)
            {
                if (prevSrState != 18)
                {
                    prevSrState = 18;
                    srState = 18;
                }
            }
            else if (direction.x == -1)
            {
                if (prevSrState != 17)
                {
                    prevSrState = 17;
                    srState = 17;
                }
            }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (prevSrState != 9 && !isAir)
            {
                prevSrState = 9;
                srState = 9;
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (prevSrState != 11 && !isAir)
            {
                prevSrState = 11;
                srState = 11;
            }
        } else
        {
            if (direction.x == -1)
            {
                if (prevSrState != 5 && !isAir)
                {
                    prevSrState = 5;
                    srState = 5;
                }
            }
            else if (direction.x == 1)
            {
                if (prevSrState != 1 && !isAir)
                {
                    prevSrState = 1;
                    srState = 1;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Z) /*&& !isAir*/)
        {
            DISABLE_CONTROLS = true;
            prevDisable = false;

            if (direction.x == 1)
            {
                if (prevSrState != 22)
                {
                    prevSrState = 22;
                    srState = 22;
                }
            }
            else if (direction.x == -1)
            {
                if (prevSrState != 21)
                {
                    prevSrState = 21;
                    srState = 21;
                }
            }
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (direction.x == 1)
            {
                if (prevSrState != 13)
                {
                    prevSrState = 13;
                    srState = 13;
                }
            }
            else if (direction.x == -1)
            {
                if (prevSrState != 15)
                {
                    prevSrState = 15;
                    srState = 15;
                }
            }
        }
        if (isAir)
        {
            if (transform.position.y > transform.position.y + v_y - a_y)
            {
                if (direction.x == 1)
                {
                    if (prevSrState != 20)
                    {
                        prevSrState = 20;
                        srState = 20;
                    }
                }
                else if (direction.x == -1)
                {
                    if (prevSrState != 19)
                    {
                        prevSrState = 19;
                        srState = 19;
                    }
                }
            }
            if (transform.position.y < transform.position.y + v_y - a_y)
            {
                if (direction.x == 1)
                {
                    if (prevSrState != 18)
                    {
                        prevSrState = 18;
                        srState = 18;
                    }
                }
                else if (direction.x == -1)
                {
                    if (prevSrState != 17)
                    {
                        prevSrState = 17;
                        srState = 17;
                    }
                }
            }
        }
    }

    void end()
    {
        SceneManager.LoadScene("Death");
    }

    bool isAirborne(Vector2 pos)
    {
        bool yCollided = yCollide();
        //yCollided = yCollideUpper() || yCollided;
        //yCollided = yCollideLower() || yCollided;
        return (!yCollided);
    }

    bool valid(Vector2 dir)
    {
        xCollided = xCollide(dir);
        //xCollided = xCollideUpper(dir) && xCollided;
        //xCollided = xCollideLower(dir) && xCollided;
        //I want each function to be called to update LastValidPos!
        return (!xCollided);
    }

    bool overPipe()
    {
        int pos = (int)Mathf.Round(transform.position.x + 10);
        if (pos < 0 || pos >= ground.Count)
        {
            return false;
        }

        Vector3 obstacle = ground[pos][0].transform.position;
        if (obstacle.z == 5)
        {
            return true;
        }
        return false;
    }

    bool xCollide(Vector2 dir)
    {
        return false; //all cases should be handled by dir until
                      //block on block is implemented
    }

    bool yCollide()
    {
        //Debug.Log(transform.position.x + 9);

        int pos = (int)Mathf.Round(transform.position.x + 9);
        if (pos < 0 || pos >= ground.Count)
        {
            return false;
        }

        for (int i = 0; i < platforms.Count; i++)
        {
            Vector2 playerPos = transform.position;
            Vector2 platPos = platforms[i].transform.position;
            if (playerPos.x < platPos.x + 0.5f && playerPos.x > platPos.x - 0.5f)
            {
                if ((playerPos.y - 1) > (platPos.y - 0.1)
                    && (playerPos.y - 1) < (platPos.y + 0.1))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        if (ground[pos].Count <= 0)
        {
            return false;
        }

        Vector3 obstacle = ground[pos][0].transform.position;
        Vector3 position = transform.position;
        if (obstacle.z == 0 || obstacle.z == -0.1f)
        {
            //level ground
            if ((position.y - 0.7f) - obstacle.y < 0.5f && (position.y - 0.7f) - obstacle.y > -0.1f)
            {
                return true;
            } else
            {
                return false;
            }
        } else if (obstacle.z == -0.5f)
        {
            float x = transform.position.x - (obstacle.x - 0.5f);
            float b = (obstacle.y - 0.5f);
            ////Debug.Log("y = mx + b: " + (transform.position.y - 1) + " v " + (x + b));
            if (transform.position.y > (x + b) && transform.position.y < (x + b + 0.5)) //y=mx + b
            {
                v_y = v_x;
                return true;
            }
            //change direction which we move
            return false;
        }
        else if (obstacle.z == -1)
        {
            float x = transform.position.x - (obstacle.x - 0.5f);
            float b = (obstacle.y + 0.5f);
            ////Debug.Log("y = mx + b: " + (transform.position.y - 1) + " v " + (x + b));
            if (transform.position.y > (-x + b) && transform.position.y < (-x + b + 0.5)) //y=mx + b
            {
                v_y = -v_x;
                return true;
            }
            //change direction which we move
            return false;
        }

        return true;
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

            ProjectileController enemyPC = en.GetComponent<ProjectileController>();

            //if the enemy shoots pellets, check for collisions with the pellet
            if (enemyPC != null)
            {
                if (enemyPC.pellet != null)
                {
                    Vector3 pelletPos = enemyPC.pellet.transform.position;
                    if (pos.x < (pelletPos.x + 0.2) && pos.x > (pelletPos.x - 0.2))
                    {
                        if (pos.y < (pelletPos.y + 0.2) && pos.y > (pelletPos.y - 0.2))
                        {
                            if (!DISABLE_CONTROLS)
                            {
                                if (prevEnemy.x != i || prevEnemy.y != 0)
                                {
                                    hit = true;
                                    prevHit = false;
                                    numLives -= 1;
                                    v_y = 1f;
                                    v_x = 0f; // -0.5f * v_x;
                                    prevEnemy.x = i;
                                    prevEnemy.y = 0;
                                }
                            }
                        }
                    }
                }
            }

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
                    ////Debug.Log("Collision with enemy: " + en.name);
                    if (!DISABLE_CONTROLS)
                    {
                        if (prevEnemy.x != i || prevEnemy.y != 0)
                        {
                            hit = true;
                            prevHit = false;
                            numLives -= 1;
                            if (en.GetComponent<EnemyController>() != null)
                            {
                                en.GetComponent<EnemyController>().state = (en.GetComponent<EnemyController>().state + 1) % 2;
                            }
                            v_y = 1f;
                            v_x = 0f; // -0.5f * v_x;
                            prevEnemy.x = i;
                            prevEnemy.y = 0;
                        }
                    }
                    else
                    {
                        toDelete.Add(i);
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
        ////Debug.Log("To delete count: " + toDelete.Count);
        killed += toDelete.Count;
        for (int j = 0; j < toDelete.Count; j++)
        {
            GameObject en = enemies[toDelete[j]];
            enemies.RemoveAt(toDelete[j]);
            if (en.GetComponent<EnemyController>() != null)
            {
                EnemyController guide = en.GetComponent<EnemyController>();
                guide.death = true;
                guide.srState = 5;
            } else
            {
                ProjectileController g = en.GetComponent<ProjectileController>();
                g.srState = 8;
                g.death = true;
            }
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
                    ////Debug.Log("Collision with enemy: " + en.name);
                    if (!DISABLE_CONTROLS)
                    {
                        if (prevEnemy.x != i || prevEnemy.y != 1)
                        {
                            hit = true;
                            prevHit = false;
                            numLives -= 1;
                            en.GetComponent<EnemyController>().state = (en.GetComponent<EnemyController>().state + 1) % 2;
                            v_y = 1f;
                            v_x = 0f;
                            prevEnemy.x = i;
                            prevEnemy.y = 1;
                        }
                    }
                    else
                    {
                        toDelete.Add(i);
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
        ////Debug.Log("To delete count: " + toDelete.Count);
        killed += toDeleteL.Count;
        for (int j = 0; j < toDeleteL.Count; j++)
        {
            GameObject en = lowerEnemies[toDeleteL[j]];
            lowerEnemies.RemoveAt(toDeleteL[j]);
            EnemyController guide = en.GetComponent<EnemyController>();
            guide.death = true;
            guide.srState = 5;
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
                        hit = true;
                        prevHit = false;
                        numLives -= 1;
                        en.GetComponent<EnemyController>().state = (en.GetComponent<EnemyController>().state + 1) % 2;
                        v_y = 1f;
                        v_x = 0f;
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
        ////Debug.Log("To delete count: " + toDelete.Count);
        killed += toDeleteU.Count;
        for (int j = 0; j < toDeleteU.Count; j++)
        {
            GameObject en = upperEnemies[toDeleteU[j]];
            upperEnemies.RemoveAt(toDeleteU[j]);
            EnemyController guide = en.GetComponent<EnemyController>();
            guide.death = true;
            guide.srState = 5;
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
                    ////Debug.Log("Collision with enemy: " + en.name);
                    if (prevEnemy2 != i)
                    {
                        hit = true;
                        prevHit = false;
                        numLives -= 1;
                        en.GetComponent<EnemyController>().state = (en.GetComponent<EnemyController>().state + 1) % 2;
                        v_y = 1f;
                        v_x = 0f;
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
        ////Debug.Log("To delete count: " + toDelete.Count);
        killed += toDeleteSuper.Count;
        for (int j = 0; j < toDeleteSuper.Count; j++)
        {
            GameObject en = superEnemies[toDeleteSuper[j]];
            superEnemies.RemoveAt(toDeleteSuper[j]);
            EnemyController guide = en.GetComponent<EnemyController>();
            guide.death = true;
            guide.srState = 5;
        }
        toDeleteSuper.Clear();

    }

    void onFallOfCliff()
    {
        if (transform.position.y < -15)
        {
            numLives -= 1;
            transform.position = lastValidPos + 4f * Vector2.up;
            v_y = 0;
            a_y = 0;
            v_x = 0;
        }
    }

    bool onCollideWithPlatform()
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
        return f_ext;
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
                    ////Debug.Log(i + " OUCH FROM BLOCK");
                    hit = true;
                    prevHit = false;
                    numLives -= 1;
                    v_y = 0.5f;
                    v_x = 0f;
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
            ////Debug.Log("You: " + (cp.y - 1) + " , spike: " + (spike.y + 0.5f));
            if (cp.x < spike.x + 0.5f && cp.x > spike.x - 0.5f &&
                (cp.y - 1) < (spike.y + 0.5f) && !DISABLE_CONTROLS /*&& (cp.y-1) > spike.y + 0.5f*/)
            {
                if (prevSpike != i)
                {
                    ////Debug.Log(i + " OUCH FROM SPIKE");
                    hit = true;
                    prevHit = false;
                    numLives -= 1;
                    v_y = 1f;
                    v_x = 0f;
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
        ////Debug.Log("To delete count: " + toDelete.Count);
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
        ////Debug.Log("To delete count: " + toDelete.Count);
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
