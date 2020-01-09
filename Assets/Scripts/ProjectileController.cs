using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    Vector2 direction = new Vector2();
    public int srState = 0;
    public int prevState = 0;
    SpriteRenderer sr;
    GameObject player;
    public GameObject pellet;
    Vector3 arcPos;
    Vector3 arcEnd;
    Vector3 dest;
    bool start;
    public bool death = false;
    bool startDeath = false;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        InvokeRepeating("SwitchSprites", 0f, 1f);
        start = false;
        direction = new Vector2(-1,0);
    }

    void DeathAnimation()
    {
        if (srState == 8)
        {
            srState = 9;
            sr.sprite = SpriteLoader.spriteEnemyDeath1;
            return;
        }
        else if (srState == 9)
        {
            srState = 10;
            sr.sprite = SpriteLoader.spriteEnemyDeath2;
            return;
        }
        else if (srState == 10)
        {
            srState = 11;
            sr.sprite = SpriteLoader.spriteEnemyDeath3;
            return;
        }
        else if (srState == 11)
        {
            srState = 12;
            sr.sprite = SpriteLoader.spriteEnemyDeath4;
            return;
        }
        else if (srState == 12)
        {
            srState = 13;
            sr.sprite = SpriteLoader.spriteEnemyDeath5;
            return;
        }
        else if (srState == 13)
        {
            srState = 14;
            sr.sprite = SpriteLoader.spriteEnemyDeath6;
            return;
        }
        else if (srState == 14)
        {
            sr.sprite = null;
            if (pellet != null)
            {
                pellet.GetComponent<SpriteRenderer>().sprite = null;
            }
            Destroy(this);
            return;
        }
    }

    void SwitchSprites()
    {
        if (srState == 0)
        {
            sr.sprite = SpriteLoader.spriteEnemy;
            srState = 1;
        } else if (srState == 1)
        {
            sr.sprite = SpriteLoader.spriteEnemyAttackL1;
            srState = 2;
        } else if (srState == 2)
        {
            sr.sprite = SpriteLoader.spriteEnemyAttackL2;
            srState = 3;
            if (pellet == null)
            {
                pellet = new GameObject();
                SpriteRenderer srP = pellet.AddComponent<SpriteRenderer>() as SpriteRenderer;
                srP.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                srP.sprite = SpriteLoader.spriteEnemyAttackPellet;
                pellet.AddComponent<BoxCollider2D>();
                pellet.GetComponent<BoxCollider2D>().isTrigger = true;
                pellet.AddComponent<Rigidbody2D>();
                pellet.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
                start = true;
            }
        }
        else if (srState == 3)
        {
            sr.sprite = SpriteLoader.spriteEnemyAttackL1;
            srState = 0;
        } else if (srState == 4)
        {
            sr.sprite = SpriteLoader.spriteEnemy2;
            srState = 5;
        }
        else if (srState == 5)
        {
            sr.sprite = SpriteLoader.spriteEnemyAttackR1;
            srState = 6;
        }
        else if (srState == 6)
        {
            sr.sprite = SpriteLoader.spriteEnemyAttackR2;
            srState = 7;
        }
        else if (srState == 7)
        {
            sr.sprite = SpriteLoader.spriteEnemyAttackR1;
            srState = 4;
        }
    }
    private void FixedUpdate()
    {
        if (!death)
        {
            if (player != null)
            {
                if (player.transform.position.x < transform.position.x)
                {
                    direction.x = -1;
                    if (prevState != 0)
                    {
                        prevState = 0;
                        srState = 0;
                    }
                }
                else
                {
                    direction.x = 1;
                    if (prevState != 4)
                    {
                        prevState = 4;
                        srState = 4;
                    }
                }
            }
            if (pellet != null)
            {
                if (start)
                {
                    start = false;
                    dest = arcPos;
                }
                if (System.Math.Abs(pellet.transform.position.x - arcPos.x) < 0.1 &&
                    System.Math.Abs(pellet.transform.position.y - arcPos.y) < 0.1)
                {
                    //move to arcEnd
                    dest = arcEnd;
                }
                else if (System.Math.Abs(pellet.transform.position.x - arcEnd.x) < 0.1 &&
                          System.Math.Abs(pellet.transform.position.y - arcEnd.y) < 0.1)
                {
                    pellet.GetComponent<SpriteRenderer>().sprite = null;
                    Destroy(pellet);
                }

                Vector2 p = Vector2.MoveTowards(pellet.transform.position, dest, 0.03f);
                pellet.GetComponent<Rigidbody2D>().MovePosition(p);

            }
            else
            {
                Vector3 pos = transform.position;
                if (direction.x == -1)
                {
                    arcPos = new Vector3(pos.x - 1, pos.y + 1, -1);
                    arcEnd = new Vector3(pos.x - 2, pos.y - 1, -1);
                }
                else
                {
                    arcPos = new Vector3(pos.x + 1, pos.y + 1, -1);
                    arcEnd = new Vector3(pos.x + 2, pos.y - 1, -1);
                }
            }
        } else
        {
            
            if (!startDeath)
            {
                CancelInvoke("SwitchSprites");
                InvokeRepeating("DeathAnimation", 0f, 0.1f);
                startDeath = true;
            }
        }
    }
}
