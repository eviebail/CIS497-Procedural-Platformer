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

    public int state = 0;

    //x = x_0 + vxt
    //y = y_0 + vyt + -ayt^2;
    //in fixedupdate, change_t should always be 1

    // Start is called before the first frame update
    void Start()
    {
        dest = transform.position;
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
        
        if (!valid(1f * Vector2.right))
        {
            state = 1;
            //Debug.Log("YO 1");
            //v_x = 0.15f;
        }
        else if (!valid(-1f * Vector2.right))
        {
            state = 0;
            //Debug.Log("YO 2");
            //v_x = -0.15f;
        }

        if (state == 0)
        {
            v_x = 0.15f;
        } else if (state == 1)
        {
            v_x = -0.15f;
        }

        Move();
    }

    bool isAirborne(Vector2 pos)
    {
        return !yCollide();
    }

    bool valid(Vector2 dir)
    {
        return !xCollide(dir);
    }

    //_want to return false if posFuture is over a cliff
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
                //if current y level is less than future position y and future position is not a cliff
                //return true

                if ((pos.y - 1) < lvl[i].y && System.Math.Abs(lvl[i].z - -1) > 0.1)
                {
                    return true;
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
                //if block is a cliff, want xCollide = true
                if ((pos.y - 1) < lvl[i].y && System.Math.Abs(lvl[i].z - -1) > 0.1)
                {
                    return true;
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

        return false;
    }

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
