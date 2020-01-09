using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 0.05f;
    Vector2 dest = Vector2.zero;
    private Vector3 start = new Vector3(-9f, 2f, 0f);

    public float v_x = 0;
    public float v_y = 0;
    public float a_y = 0;

    public int level = 0;

    public int state = 0;

    public int srState = 2;

    public bool death = false;

    public Vector2 xRange = new Vector2(-100,0);

    SpriteRenderer sr; //= enemy.AddComponent<SpriteRenderer>() as SpriteRenderer;

    //x = x_0 + vxt
    //y = y_0 + vyt + -ayt^2;
    //in fixedupdate, change_t should always be 1

    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos = transform.position;
        transform.position = new Vector3(pos.x, pos.y, -1);
        dest = transform.position;
        sr = GetComponent<SpriteRenderer>();
        InvokeRepeating("SwitchSprites", 0f, 0.1f);
    }

    private void Move()
    {
        // Move closer to Destination
        dest = new Vector2(transform.position.x + v_x, transform.position.y + v_y - a_y);
        //Debug.Log("ENEMY DEST: " + dest);
        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);
        //Debug.Log("vx: " + v_x);
    }

    void SwitchSprites()
    {
        if (srState == 1)
        {
            srState = 3;
            sr.sprite = SpriteLoader.spriteEnemy;
            return;
        }
        else if (srState == 2)
        {
            srState = 4;
            sr.sprite = SpriteLoader.spriteEnemy2;
            return;
        }
        else if (srState == 3)
        {
            srState = 1;
            sr.sprite = SpriteLoader.spriteEnemy3;
            return;
        }
        else if (srState == 4)
        {
            srState = 2;
            sr.sprite = SpriteLoader.spriteEnemy4;
            return;
        }
        else if (srState == 5)
        {
            srState = 6;
            sr.sprite = SpriteLoader.spriteEnemyDeath1;
            return;
        }
        else if (srState == 6)
        {
            srState = 7;
            sr.sprite = SpriteLoader.spriteEnemyDeath2;
            return;
        }
        else if (srState == 7)
        {
            srState = 8;
            sr.sprite = SpriteLoader.spriteEnemyDeath3;
            return;
        }
        else if (srState == 8)
        {
            srState = 9;
            sr.sprite = SpriteLoader.spriteEnemyDeath4;
            return;
        }
        else if (srState == 9)
        {
            srState = 10;
            sr.sprite = SpriteLoader.spriteEnemyDeath5;
            return;
        }
        else if (srState == 10)
        {
            srState = 11;
            sr.sprite = SpriteLoader.spriteEnemyDeath6;
            return;
        }
        else if (srState == 11)
        {
            sr.sprite = null;
            Destroy(this);
            return;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if (isAirborne(transform.position))
        //{
        //    //dest = new Vector2(dest.x, dest.y - 0.1f);
        //    a_y = 0.1f;
        //} else
        //{
        //    a_y = 0;
        //}
        // Check for Input if not moving
        if (!death)
        {
            if (xRange.x != -100)
            {
                if (!valid(1f * Vector2.right) || transform.position.x + 1 >= xRange.y + 1)
                {
                    state = 1;
                    //Debug.Log("YO 1");
                    //v_x = 0.15f;
                    srState = 1;
                }
                else if (!valid(-1f * Vector2.right) || transform.position.x - 1 <= xRange.x - 1)
                {
                    state = 0;
                    srState = 2;
                    //Debug.Log("YO 2");
                    //v_x = -0.15f;
                }
            }
            else
            {
                if (!valid(1f * Vector2.right))
                {
                    state = 1;
                    srState = 1;
                    //Debug.Log("YO 1");
                    //v_x = 0.15f;
                }
                else if (!valid(-1f * Vector2.right))
                {
                    state = 0;
                    srState = 2;
                    //Debug.Log("YO 2");
                    //v_x = -0.15f;
                }
            }



            if (state == 0)
            {
                v_x = 0.15f;
                //sr.sprite = mySprite2;
                //srState = 2;
            }
            else if (state == 1)
            {
                v_x = -0.15f;
                //srState = 1;
                //sr.sprite = mySprite1;
            }

            Move();
        }

    }

    bool isAirborne(Vector2 pos)
    {
        return !yCollide();
    }

    bool valid(Vector2 dir)
    {
        //return !xCollide(dir);
        bool xCollided = xCollide(dir);
        //if (level == 1)
        //{
        //    xCollided = xCollideUpper(dir);
        //} else if (level == 2)
        //{
        //    xCollided = xCollideLower(dir);
        //}
        //xCollided = xCollideUpper(dir) || xCollided;
        //xCollided = xCollideLower(dir) || xCollided;
        //I want each function to be called to update LastValidPos!
        return (!xCollided);
    }

    bool xCollide(Vector2 dir)
    {
        List<List<GameObject>> ground = GeometryGenerator.ground;
        List<Vector4> ranges = GeometryGenerator.lvlRanges;
        Vector2 pos = transform.position;
        Vector2 posFuture = pos + dir;

        int posNow = (int)Mathf.Round(pos.x + 9);
        int posFtr = (int)Mathf.Round(posFuture.x + 9);

        //if (pos.y < ranges[0].z)
        //{
        //    return false;
        //}

        //is there anything between curr x,y and future x,y?
        if (pos.x < posFuture.x)
        {
            for (int i = posNow + 1; i < posFtr + 1; i++)
            {
                if (ground[i].Count <= 0)
                {
                    return true;
                }

                if (pos.y - 1 < ground[i][0].transform.position.y)
                {
                    return true;
                }

                if (ground[i][0].transform.position.z == -1 ||
                    ground[i][0].transform.position.z == -0.5f ||
                    ground[i][0].transform.position.z == -0.1f)
                {
                    return true;
                }
            }
        }
        else
        {
            for (int i = posNow; i > posFtr - 1; i--)
            {
                if (ground[i].Count <= 0)
                {
                    return true;
                }

                if (pos.y - 1 < ground[i][0].transform.position.y)
                {
                    return true;
                }

                if (ground[i][0].transform.position.z == -1 ||
                    ground[i][0].transform.position.z == -0.5f ||
                    ground[i][0].transform.position.z == -0.1f)
                {
                    return true;
                }
            }
        }


        return false;
    }

    bool xCollideUpper(Vector2 dir)
    {
        List<Vector4> ranges = GeometryGenerator.lvlRanges;
        List<List<GameObject>> upperGround = GeometryGenerator.upperGround;
        Vector2 pos = transform.position;
        Vector2 posFuture = pos + dir;

        int posNow = (int)(Mathf.Round(pos.x - ranges[1].x));
        int posFtr = (int)(Mathf.Round(posFuture.x - ranges[1].x));

        if (posNow < 0 || posNow >= upperGround.Count || posFtr < 0 || posFtr >= upperGround.Count
            || pos.y < ranges[1].z)
        {
            return false;
        }

        //is there anything between curr x,y and future x,y?
        if (pos.x < posFuture.x)
        {
            for (int i = posNow + 1; i < (int)Mathf.Min(posFtr + 1, upperGround.Count); i++)
            {
                if (upperGround[i - 1][0].transform.position.z == 3)
                {
                    if ((pos.y - 0.3) < upperGround[i][0].transform.position.y && System.Math.Abs(upperGround[i][0].transform.position.z - -1) > 0.1)
                    {
                        return true;
                    }
                }
                else
                {
                    if ((pos.y - 1) < upperGround[i][0].transform.position.y && System.Math.Abs(upperGround[i][0].transform.position.z - -1) > 0.1
                        && upperGround[i][0].transform.position.z != 3 && upperGround[i + 1][0].transform.position.z != 3)
                    {
                        return true;
                    }
                }

                //if future position is over cliff, return true
                if (System.Math.Abs(upperGround[i][0].transform.position.z - -1) < 0.1)//isAirborne(new Vector2(i,pos.y - 1)))
                {
                    return true;
                }
                if ((pos.y - 1) > upperGround[i][0].transform.position.y) //isAirborneCondition
                {
                    return true;
                }
            }
        }
        else
        {
            for (int i = (int)Mathf.Max(posFtr, 1); i < posNow; i++)
            {
                if (upperGround[i - 1][0].transform.position.z == 3)
                {
                    if ((pos.y - 0.4) < upperGround[i][0].transform.position.y && System.Math.Abs(upperGround[i][0].transform.position.z - -1) > 0.1)
                    {
                        return true;
                    }
                }
                else
                {
                    if ((pos.y - 1) < upperGround[i][0].transform.position.y && System.Math.Abs(upperGround[i][0].transform.position.z - -1) > 0.1
                        && upperGround[i][0].transform.position.z != 3 && upperGround[i + 1][0].transform.position.z != 3)
                    {
                        return true;
                    }
                }
                //if future position is over cliff, return true
                if (System.Math.Abs(upperGround[i][0].transform.position.z - -1) < 0.1)//isAirborne(new Vector2(i,pos.y - 1)))
                {
                    return true;
                }
                if ((pos.y - 1) > upperGround[i][0].transform.position.y) //isAirborneCondition
                {
                    return true;
                }
            }
        }


        return false;
    }

    bool xCollideLower(Vector2 dir)
    {
        List<Vector4> ranges = GeometryGenerator.lvlRanges;
        List<List<GameObject>> lowerGround = GeometryGenerator.lowerGround;
        Vector2 pos = transform.position;
        Vector2 posFuture = pos + dir;

        int posNow = (int)(Mathf.Round(pos.x - ranges[2].x));
        int posFtr = (int)(Mathf.Round(posFuture.x - ranges[2].x));

        //Debug.Log("::x's " + posNow + " , " + posFtr);
        //Debug.Log("::range " + ranges[2].x + " , " + ranges[2].y);

        if (posNow < 0 || posNow >= lowerGround.Count || posFtr < 0 || posFtr >= lowerGround.Count
            || pos.y < ranges[2].z)
        {
            return false;
        }

        //Debug.Log("now vs ftr: " + pos.x + " , " + posFuture.x);

        //is there anything between curr x,y and future x,y?
        if (pos.x < posFuture.x)
        {
            for (int i = posNow + 1; i < (int)Mathf.Min(posFtr + 1, lowerGround.Count); i++)
            {
                if (lowerGround[i - 1][0].transform.position.z == 3)
                {
                    if ((pos.y - 0.3) < lowerGround[i][0].transform.position.y && System.Math.Abs(lowerGround[i][0].transform.position.z - -1) > 0.1)
                    {
                        return true;
                    }
                }
                else
                {
                    if ((pos.y - 1) < lowerGround[i][0].transform.position.y && System.Math.Abs(lowerGround[i][0].transform.position.z - -1) > 0.1
                        && lowerGround[i][0].transform.position.z != 3 && lowerGround[i + 1][0].transform.position.z != 3)
                    {
                        return true;
                    }
                }

                //if future position is over cliff, return true
                if (System.Math.Abs(lowerGround[i][0].transform.position.z - -1) < 0.1)//isAirborne(new Vector2(i,pos.y - 1)))
                {
                    return true;
                }
                if ((pos.y - 1) > lowerGround[i][0].transform.position.y) //isAirborneCondition
                {
                    return true;
                }
            }
        }
        else
        {
            for (int i = (int)Mathf.Max(posFtr, 1); i < posNow; i++)
            {
                if (lowerGround[i - 1][0].transform.position.z == 3)
                {
                    if ((pos.y - 0.4) < lowerGround[i][0].transform.position.y && System.Math.Abs(lowerGround[i][0].transform.position.z - -1) > 0.1)
                    {
                        return true;
                    }
                }
                else
                {
                    if ((pos.y - 1) < lowerGround[i][0].transform.position.y && System.Math.Abs(lowerGround[i][0].transform.position.z - -1) > 0.1
                        && lowerGround[i][0].transform.position.z != 3 && lowerGround[i + 1][0].transform.position.z != 3)
                    {
                        return true;
                    }
                }
                //if future position is over cliff, return true
                if (System.Math.Abs(lowerGround[i][0].transform.position.z - -1) < 0.1)//isAirborne(new Vector2(i,pos.y - 1)))
                {
                    return true;
                }
                if ((pos.y - 1) > lowerGround[i][0].transform.position.y) //isAirborneCondition
                {
                    return true;
                }
            }
        }


        return false;
    }

    //_want to return false if posFuture is over a cliff
    //bool xCollide(Vector2 dir)
    //{
    //    List<Vector3> lvl = GeometryGenerator.lvl;
    //    Vector2 pos = transform.position;
    //    Vector2 posFuture = pos + dir;

    //    int posNow = (int)Mathf.Round(pos.x + 10);
    //    int posFtr = (int)Mathf.Round(posFuture.x + 10);

    //    //is there anything between curr x,y and future x,y?
    //    if (pos.x < posFuture.x)
    //    {
    //        for (int i = posNow + 1; i < posFtr + 1; i++)
    //        {
    //            //if current y level is less than future position y and future position is not a cliff
    //            //return true

    //            if ((pos.y - 1) < lvl[i].y && System.Math.Abs(lvl[i].z - -1) > 0.1)
    //            {
    //                return true;
    //            }
    //            //if future position is over cliff, return true
    //            if (System.Math.Abs(lvl[i].z - -1) < 0.1)//isAirborne(new Vector2(i,pos.y - 1)))
    //            {
    //                return true;
    //            }
    //            if ((pos.y - 1) > lvl[i].y) //isAirborneCondition
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        for (int i = posFtr; i < posNow; i++)
    //        {
    //            //if block is a cliff, want xCollide = true
    //            if ((pos.y - 1) < lvl[i].y && System.Math.Abs(lvl[i].z - -1) > 0.1)
    //            {
    //                return true;
    //            }
    //            //if future position is over cliff, return true
    //            if (System.Math.Abs(lvl[i].z - -1) < 0.1)//isAirborne(new Vector2(i,pos.y - 1)))
    //            {
    //                return true;
    //            }
    //            if ((pos.y - 1) > lvl[i].y) //isAirborneCondition
    //            {
    //                return true;
    //            }
    //        }
    //    }

    //    return false;
    //}

    bool yCollide()
    {
        List<List<GameObject>> ground = GeometryGenerator.ground;
        int pos = (int)Mathf.Round(transform.position.x + 10);
        if (pos < 0 || pos >= ground.Count)
        {
            //Debug.Log("ERRRRRRR");
            return false;
        }

        Vector3 obstacle = ground[pos][0].transform.position;
        if (System.Math.Abs(obstacle.z - -1) < 0.01)
        {
            //cliff!
            //Debug.Log("-1!!!!!!!");
            return false;
        }
        //Debug.Log("Player: " + (transform.position.y - 1) + " ground " + obstacle.y);
        if (System.Math.Abs((transform.position.y - 1) - obstacle.y) < 0.1)
        {
            return true;
        }
        return false;
    }
}
