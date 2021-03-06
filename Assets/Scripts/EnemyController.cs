﻿using System.Collections;
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

    public Texture2D texEnemy;
    public Texture2D texEnemy2;
    public Texture2D texEnemy3;
    public Texture2D texEnemy4;

    Sprite mySprite4;
    Sprite mySprite3;
    Sprite mySprite2;
    Sprite mySprite1;

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
        mySprite4 = Sprite.Create(texEnemy4, new Rect(0.0f, 0.0f, texEnemy4.width, texEnemy4.height),
              new Vector2(0.5f, 0.5f), 100.0f);
        mySprite3 = Sprite.Create(texEnemy3, new Rect(0.0f, 0.0f, texEnemy3.width, texEnemy3.height),
                      new Vector2(0.5f, 0.5f), 100.0f);
        mySprite2 = Sprite.Create(texEnemy2, new Rect(0.0f, 0.0f, texEnemy2.width, texEnemy2.height),
                      new Vector2(0.5f, 0.5f), 100.0f);
        mySprite1 = Sprite.Create(texEnemy, new Rect(0.0f, 0.0f, texEnemy.width, texEnemy.height),
                             new Vector2(0.5f, 0.5f), 100.0f);
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
            sr.sprite = mySprite1;
            return;
        }
        else if (srState == 2)
        {
            srState = 4;
            sr.sprite = mySprite2;
            return;
        }
        else if (srState == 3)
        {
            srState = 1;
            sr.sprite = mySprite3;
            return;
        }
        else if (srState == 4)
        {
            srState = 2;
            sr.sprite = mySprite4;
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
        } else
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
        } else if (state == 1)
        {
            v_x = -0.15f;
            //srState = 1;
            //sr.sprite = mySprite1;
        }

        Move();

    }

    bool isAirborne(Vector2 pos)
    {
        return !yCollide();
    }

    bool valid(Vector2 dir)
    {
        //return !xCollide(dir);
        bool xCollided = xCollide(dir);
        if (level == 1)
        {
            xCollided = xCollideUpper(dir);
        } else if (level == 2)
        {
            xCollided = xCollideLower(dir);
        }
        //xCollided = xCollideUpper(dir) || xCollided;
        //xCollided = xCollideLower(dir) || xCollided;
        //I want each function to be called to update LastValidPos!
        return (!xCollided);
    }

    bool xCollide(Vector2 dir)
    {
        List<Vector3> lvl = GeometryGenerator.lvl;
        List<Vector4> ranges = GeometryGenerator.lvlRanges;
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
                //if future position is over cliff, return true
                if (System.Math.Abs(lvl[i].z - -1) < 0.1)//isAirborne(new Vector2(i,pos.y - 1)))
                {
                    return true;
                }
                if ((pos.y - 1) > lvl[i].y) //isAirborneCondition
                {
                    return true;
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
                if (System.Math.Abs(lvl[i].z - -1) < 0.1)//isAirborne(new Vector2(i,pos.y - 1)))
                {
                    return true;
                }
                if ((pos.y - 1) > lvl[i].y) //isAirborneCondition
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
        List<Vector3> upperLvl = GeometryGenerator.upperLvl;
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

                //if future position is over cliff, return true
                if (System.Math.Abs(upperLvl[i].z - -1) < 0.1)//isAirborne(new Vector2(i,pos.y - 1)))
                {
                    return true;
                }
                if ((pos.y - 1) > upperLvl[i].y) //isAirborneCondition
                {
                    return true;
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
                //if future position is over cliff, return true
                if (System.Math.Abs(upperLvl[i].z - -1) < 0.1)//isAirborne(new Vector2(i,pos.y - 1)))
                {
                    return true;
                }
                if ((pos.y - 1) > upperLvl[i].y) //isAirborneCondition
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
        List<Vector3> lowerLvl = GeometryGenerator.lowerLvl;
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

                //if future position is over cliff, return true
                if (System.Math.Abs(lowerLvl[i].z - -1) < 0.1)//isAirborne(new Vector2(i,pos.y - 1)))
                {
                    return true;
                }
                if ((pos.y - 1) > lowerLvl[i].y) //isAirborneCondition
                {
                    return true;
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
                //if future position is over cliff, return true
                if (System.Math.Abs(lowerLvl[i].z - -1) < 0.1)//isAirborne(new Vector2(i,pos.y - 1)))
                {
                    return true;
                }
                if ((pos.y - 1) > lowerLvl[i].y) //isAirborneCondition
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
        List<Vector3> lvl = GeometryGenerator.lvl;
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
        //Debug.Log("Player: " + (transform.position.y - 1) + " ground " + obstacle.y);
        if (System.Math.Abs((transform.position.y - 1) - obstacle.y) < 0.1)
        {
            return true;
        }
        return false;
    }
}
