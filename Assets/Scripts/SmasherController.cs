﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmasherController : MonoBehaviour
{
    public float speed = 0.05f;
    public Vector2 dest = Vector2.zero;

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

    // Update is called once per frame
    void FixedUpdate()
    {
        if (vEquivalence(transform.position, dest))
        {
            if (state == 0)
            {
                dest += 2 * Vector2.up;
                state = 1;
            }
            else
            {
                dest += -2 * Vector2.up;
                state = 0;
            }
        }

        Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Debug.Log("OUCH FROM BLOCK");
        }
    }

    public bool vEquivalence(Vector3 v1, Vector3 v2)
    {
        return (System.Math.Abs(v1.x - v2.x) < 0.1
                && System.Math.Abs(v1.y - v2.y) < 0.1
                && System.Math.Abs(v1.z - v2.z) < 0.1);
    }
}
