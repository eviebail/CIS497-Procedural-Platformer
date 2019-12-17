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

    public static List<Vector3> lvl = new List<Vector3>(); //x,y,type
    public static List<Vector3> upperLvl = new List<Vector3>();
    public static List<Vector3> lowerLvl = new List<Vector3>();
    public static List<GameObject> enemies = new List<GameObject>();
    public static List<GameObject> lowerEnemies = new List<GameObject>();
    public static List<GameObject> upperEnemies = new List<GameObject>();

    public static List<GameObject> platforms = new List<GameObject>();
    public static List<GameObject> upperPlatforms = new List<GameObject>();

    public static List<GameObject> stompers = new List<GameObject>();
    public static List<GameObject> lowerStompers = new List<GameObject>();
    public static List<GameObject> upperStompers = new List<GameObject>();
    public static List<GameObject> spikes = new List<GameObject>();
    public static List<GameObject> upperSpikes = new List<GameObject>();
    public static List<GameObject> lowerSpikes = new List<GameObject>();
    public static List<GameObject> coins = new List<GameObject>();
    public static List<GameObject> superEnemies = new List<GameObject>();
    public static List<GameObject> stars = new List<GameObject>();
    public static List<Vector4> lvlRanges = new List<Vector4>();

    public static List<List<GameObject>> ground = new List<List<GameObject>>();
    public static List<List<GameObject>> upperGround = new List<List<GameObject>>();
    public static List<List<GameObject>> lowerGround = new List<List<GameObject>>();

    public Texture2D texGround;
    public Texture2D texEnemy;
    public Texture2D texEnemy2;
    public Texture2D texEnemy3;
    public Texture2D texEnemy4;
    public Texture2D texSpike;
    public Texture2D texCoin;
    public Texture2D texGnd;
    public Texture2D texSlope;
    public Texture2D texSlope2;
    public Texture2D texGoal;
    public Texture2D texPipe;
    public Texture2D texUnderGnd;
    public Texture2D texMoveGnd;

    public int enemyID = 0;

    public int timer = 0;

    public bool dir = true;

    public bool ledgesExist = true;

    public static bool startLower = false;

    public static bool startUpper = false;

    int offset = 2;

    //private Sprite mySprite;
    //private SpriteRenderer sr;
    // gameObject;

    //for loading 2d sprites instead of 3d objects
    void Awake()
    {
        
    }

    public GeometryGenerator(List<Block> blocks, int startX, int startY, Texture2D ground,
    Texture2D enemy, Texture2D spike, Texture2D coin, Texture2D gnd, Texture2D slope,
    Texture2D slope2, Texture2D goal, Texture2D pipe, Texture2D texUnderGnd, Texture2D texMoveGnd,
    Texture2D texEnemy2, Texture2D texEnemy3, Texture2D texEnemy4)
    {
        this.blocks = blocks;
        globalX = -9;
        globalY = startY;
        originalY = startY;
        upGlobalX = globalX;
        upGlobalY = startY + 15;
        lowGlobalX = globalX;
        lowGlobalY = startY - 15;
        state = new StateMachine();
        texGround = ground;
        texEnemy = enemy;
        texSpike = spike;
        texCoin = coin;
        texGnd = gnd;
        texSlope = slope;
        texSlope2 = slope2;
        texGoal = goal;
        texPipe = pipe;
        this.texUnderGnd = texUnderGnd;
        this.texMoveGnd = texMoveGnd;
        this.texEnemy2 = texEnemy2;
        this.texEnemy3 = texEnemy3;
        this.texEnemy4 = texEnemy4;
    }

    public void cleanUpSpikyLedgesUpper()
    {
        List<Vector2> spikeus = new List<Vector2>();
        int spikeStart = -100;
        int spikeEnd = -100;
        for (int i = 1; i < upperLvl.Count; i++)
        {
            Vector3 element = upperLvl[i];
            if (element.z == 1 && spikeStart == -100)
            {
                spikeStart = i;
            }
            if (element.z != 1 && spikeStart != -100)
            {
                spikeEnd = i;
                spikeus.Add(new Vector2(spikeStart, spikeEnd));
                spikeStart = -100;
                spikeEnd = -100;
            }
        }

        for (int k = 0; k < spikeus.Count; k++)
        {
            Vector2 range = spikeus[k];
            Vector3 level = upperLvl[(int)range.x - 1];
            if (level.z == 0 && level.y == upperLvl[(int)range.x].y)
            {
                //lower the spikes
                for (int i = (int)range.x; i < range.y; i++)
                {
                    for (int j = 0; j < upperSpikes.Count; j++)
                    {
                        if (upperSpikes[j].transform.position.x == upperLvl[i].x)
                        {
                            Vector3 pos = upperSpikes[j].transform.position;
                            upperSpikes[j].transform.position = new Vector3(pos.x, pos.y - 1, pos.z);
                        }
                    }
                }
            }

            if (level.z == 0 && level.y < upperLvl[(int)range.x].y)
            {
                //lower the spikes
                for (int i = (int)range.x; i < range.y; i++)
                {
                    for (int j = 0; j < upperSpikes.Count; j++)
                    {
                        if (upperSpikes[j].transform.position.x == upperLvl[i].x)
                        {
                            Vector3 pos = upperSpikes[j].transform.position;
                            upperSpikes[j].transform.position = new Vector3(pos.x,
                                                pos.y + (level.y - upperLvl[i].y - 1), pos.z);
                        }
                    }
                }
            }

        }
    }

    public void cleanUpSpikyLedges()
    {
        List<Vector2> spikeus = new List<Vector2>();
        int spikeStart = -100;
        int spikeEnd = -100;
        for (int i = 1; i < lvl.Count; i++)
        {
            Vector3 element = lvl[i];
            if (element.z == 1 && spikeStart == -100)
            {
                spikeStart = i;
            }
            if (element.z != 1 && spikeStart != -100)
            {
                spikeEnd = i;
                spikeus.Add(new Vector2(spikeStart, spikeEnd));
                spikeStart = -100;
                spikeEnd = -100;
            }
        }

        cleanUpSpikes();

        for (int k = 0; k < spikeus.Count; k++)
        {
            Vector2 range = spikeus[k];
            Vector3 level = lvl[(int)range.x - 1];
            if (level.z == 0 && level.y == lvl[(int)range.x].y)
            {
                //lower the spikes
                for (int i = (int)range.x; i < range.y; i++)
                {
                    for (int j = 0; j < spikes.Count; j++)
                    {
                        if (spikes[j].transform.position.x == lvl[i].x)
                        {
                            Vector3 pos = spikes[j].transform.position;
                            spikes[j].transform.position = new Vector3(pos.x, pos.y - 1, pos.z);
                        }
                    }
                }
            }

            if (level.z == 0 && level.y < lvl[(int)range.x].y)
            {
                //lower the spikes
                for (int i = (int)range.x; i < range.y; i++)
                {
                    for (int j = 0; j < spikes.Count; j++)
                    {
                        if (spikes[j].transform.position.x == lvl[i].x)
                        {
                            Vector3 pos = spikes[j].transform.position;
                            spikes[j].transform.position = new Vector3(pos.x,
                                                pos.y + (level.y - lvl[i].y - 1), pos.z);
                        }
                    }
                }
            }

        }

        cleanUpSpikyLedgesUpper();

        //for (int s = 0; s < spikeus.Count; s++)
        //{
        //    //Debug.Log("SPIKES: " + spikeus[s]);
        //}

        //for (int i = 0; i < lvl.Count; i++)
        //{
        //    if (lvl[i].z == -1)
        //    {

        //    }
        //}
    }

    public void cleanUpSpikes()
    {
        List<Vector2> spikeus = new List<Vector2>();
        int spikeStart = -100;
        int spikeEnd = -100;
        for (int i = 1; i < lvl.Count; i++)
        {
            Vector3 element = lvl[i];
            if (element.z == 1 && spikeStart == -100)
            {
                spikeStart = i;
            }
            if (element.z != 1 && spikeStart != -100)
            {
                spikeEnd = i;
                spikeus.Add(new Vector2(spikeStart, spikeEnd));
                spikeStart = -100;
                spikeEnd = -100;
            }
        }

        List<GameObject> toDelete = new List<GameObject>();

        for (int k = 0; k < spikeus.Count; k++)
        {
            Vector2 range = spikeus[k];
            if (range.y - range.x < 3)
            {
                for (int i = (int)range.x; i < range.y; i++)
                {
                    Vector3 slot = lvl[i];
                    lvl[i] = new Vector3(slot.x, slot.y, 0);
                    for (int j = 0; j < spikes.Count; j++)
                    {
                        if (spikes[j].transform.position.x == slot.x)
                        {
                            toDelete.Add(spikes[j]);

                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube.transform.position = new Vector3(slot.x, slot.y, 0);
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();

                        }
                    }
                }
            }
        }

        for (int i = 0; i < toDelete.Count; i++)
        {
            GameObject sp = toDelete[i];
            spikes.Remove(sp);
            Destroy(sp);
        }
        cleanUpSpikesUpper();
        cleanUpSpikesLower();
    }

    public void cleanUpSpikesUpper()
    {
        List<Vector2> spikeus = new List<Vector2>();
        int spikeStart = -100;
        int spikeEnd = -100;
        for (int i = 1; i < upperLvl.Count; i++)
        {
            Vector3 element = upperLvl[i];
            if (element.z == 1 && spikeStart == -100)
            {
                spikeStart = i;
            }
            if (element.z != 1 && spikeStart != -100)
            {
                spikeEnd = i;
                spikeus.Add(new Vector2(spikeStart, spikeEnd));
                spikeStart = -100;
                spikeEnd = -100;
            }
        }

        List<GameObject> toDelete = new List<GameObject>();

        for (int k = 0; k < spikeus.Count; k++)
        {
            Vector2 range = spikeus[k];
            if (range.y - range.x < 3)
            {
                for (int i = (int)range.x; i < range.y; i++)
                {
                    Vector3 slot = upperLvl[i];
                    upperLvl[i] = new Vector3(slot.x, slot.y, 0);
                    for (int j = 0; j < upperSpikes.Count; j++)
                    {
                        if (upperSpikes[j].transform.position.x == slot.x)
                        {
                            toDelete.Add(upperSpikes[j]);

                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube.transform.position = new Vector3(slot.x, slot.y, 0);
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();

                        }
                    }
                }
            }
        }

        for (int i = 0; i < toDelete.Count; i++)
        {
            GameObject sp = toDelete[i];
            upperSpikes.Remove(sp);
            Destroy(sp);
        }
    }

    public void cleanUpSpikesLower()
    {
        List<Vector2> spikeus = new List<Vector2>();
        int spikeStart = -100;
        int spikeEnd = -100;
        for (int i = 1; i < lowerLvl.Count; i++)
        {
            Vector3 element = lowerLvl[i];
            if (element.z == 1 && spikeStart == -100)
            {
                spikeStart = i;
            }
            if (element.z != 1 && spikeStart != -100)
            {
                spikeEnd = i;
                spikeus.Add(new Vector2(spikeStart, spikeEnd));
                spikeStart = -100;
                spikeEnd = -100;
            }
        }

        List<GameObject> toDelete = new List<GameObject>();

        for (int k = 0; k < spikeus.Count; k++)
        {
            Vector2 range = spikeus[k];
            if (range.y - range.x < 3)
            {
                for (int i = (int)range.x; i < range.y; i++)
                {
                    Vector3 slot = lowerLvl[i];
                    lowerLvl[i] = new Vector3(slot.x, slot.y, 0);
                    for (int j = 0; j < lowerSpikes.Count; j++)
                    {
                        if (lowerSpikes[j].transform.position.x == slot.x)
                        {
                            toDelete.Add(lowerSpikes[j]);

                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube.transform.position = new Vector3(slot.x, slot.y, 0);
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();

                        }
                    }
                }
            }
        }

        for (int i = 0; i < toDelete.Count; i++)
        {
            GameObject sp = toDelete[i];
            lowerSpikes.Remove(sp);
            Destroy(sp);
        }
    }

    public void jumpLedgesUpper()
    {
        List<Vector3> ledges = new List<Vector3>();
        int prevY = 0;
        int postY = -100;
        int gapStart = -100;
        for (int i = 1; i < upperLvl.Count; i++)
        {
            Vector3 element = upperLvl[i];
            if (element.z == 0 && gapStart == -100)
            {
                prevY = (int)element.y;
            }
            if (element.z == -1 && gapStart == -100)
            {
                gapStart = i;
            }
            if (element.z == 0 && gapStart != -100)
            {
                postY = (int)element.y;
                if (prevY < postY)
                {
                    ledges.Add(new Vector3(gapStart, prevY, postY));
                }
                prevY = postY;
                postY = -100;
                gapStart = -100;
            }
        }
        for (int i = 0; i < ledges.Count; i++)
        {
            Vector3 ps = ledges[i];
            ledges[i] = new Vector3(ps.x + 3 * i, ps.y, ps.z);
            Vector3 ranges = ledges[i];

            int baseX = (int)upperLvl[(int)ledges[i].x - 1].x;
            int baseY = (int)upperLvl[(int)ledges[i].x - 1].y; //ranges.y;
            //make and insert three cubes
            GameObject cube1 = new GameObject();
            SpriteRenderer sr = cube1.AddComponent<SpriteRenderer>() as SpriteRenderer;
            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            cube1.transform.position = new Vector3(baseX + 3, baseY - 1, 0);
            cube1.transform.localScale = blockScale;
            Sprite mySprite = Sprite.Create(texCoin, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                     new Vector2(0.5f, 0.5f), 100.0f);
            sr.sprite = mySprite;
            cube1.AddComponent<BoxCollider2D>();
            upperLvl.Insert((int)ranges.x, new Vector3(baseX + 3, baseY - 1, 0));

            List<GameObject> l = new List<GameObject>();
            l.Add(cube1);
            upperGround.Insert((int)ranges.x, l);

            GameObject cube2 = new GameObject();
            SpriteRenderer sr2 = cube2.AddComponent<SpriteRenderer>() as SpriteRenderer;
            sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            cube2.transform.position = new Vector3(baseX + 2, baseY - 1, 0);
            cube2.transform.localScale = blockScale;
            Sprite mySprite2 = Sprite.Create(texCoin, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                     new Vector2(0.5f, 0.5f), 100.0f);
            sr2.sprite = mySprite2;
            cube2.AddComponent<BoxCollider2D>();
            upperLvl.Insert((int)ranges.x, new Vector3(baseX + 2, baseY - 1, 0));

            List<GameObject> l2 = new List<GameObject>();
            l2.Add(cube2);
            upperGround.Insert((int)ranges.x, l2);

            GameObject cube3 = new GameObject();
            SpriteRenderer sr3 = cube3.AddComponent<SpriteRenderer>() as SpriteRenderer;
            sr3.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            cube3.transform.position = new Vector3(baseX + 1, baseY - 1, 0);
            cube3.transform.localScale = blockScale;
            Sprite mySprite3 = Sprite.Create(texCoin, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                     new Vector2(0.5f, 0.5f), 100.0f);
            sr3.sprite = mySprite3;
            cube3.AddComponent<BoxCollider2D>();
            upperLvl.Insert((int)ranges.x, new Vector3(baseX + 1, baseY - 1, 0));
            List<GameObject> l3 = new List<GameObject>();
            l.Add(cube3);
            upperGround.Insert((int)ranges.x, l3);

            //Add enemy to bounce off o 
            GameObject enemy = new GameObject();
            SpriteRenderer sre = enemy.AddComponent<SpriteRenderer>() as SpriteRenderer;
            sre.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            enemy.transform.position = new Vector3(baseX + 2, baseY, 0);
            enemy.transform.localScale = blockScale;
            Sprite mySpriteE = Sprite.Create(texEnemy, new Rect(0.0f, 0.0f, texEnemy.width, texEnemy.height),
                     new Vector2(0.5f, 0.5f), 100.0f);
            sre.sprite = mySpriteE;
            enemy.AddComponent<BoxCollider2D>();
            enemy.AddComponent<Rigidbody2D>();
            enemy.GetComponent<Rigidbody2D>().isKinematic = true;
            enemy.name = "" + enemyID;//enemy.GetComponent<BoxCollider2D>().isTrigger = true;
            enemy.AddComponent<EnemyController>();
            enemy.GetComponent<EnemyController>().texEnemy = texEnemy;
            enemy.GetComponent<EnemyController>().level = 1;
            enemy.GetComponent<EnemyController>().texEnemy2 = texEnemy2;
            enemy.GetComponent<EnemyController>().texEnemy3 = texEnemy3;
            enemy.GetComponent<EnemyController>().texEnemy4 = texEnemy4;
            enemyID += 1;
            upperEnemies.Add(enemy);
            enemy.GetComponent<EnemyController>().xRange = new Vector2(baseX + 1, baseX + 3);

            ////Debug.Log("::: " + (ranges.x + 4) + " vs " + (baseX + 4));

            //offset += 3;
            //start = baseX + 4;

            for (int j = (int)ranges.x + 4; j < upperLvl.Count; j++)
            {
                Vector3 pos = upperLvl[j];
                upperLvl[j] = new Vector3(pos.x + 3, pos.y - 2, pos.z);

                List<GameObject> gndTexts = upperGround[j];

                for (int k = 0; k < gndTexts.Count; k++)
                {
                    Vector3 p = gndTexts[k].transform.position;
                    gndTexts[k].transform.position = new Vector3(p.x + 3, p.y - 2, p.z);
                }
            }
            ////Debug.Log("CUTOFF: " + (baseX + 4));
            for (int k = 0; k < upperStompers.Count; k++)
            {
                ////Debug.Log("STOMPS: " + stompers[k].transform.position);
                Vector3 stompPos = upperStompers[k].transform.position;
                if (stompPos.x >= (baseX + 4))
                {
                    upperStompers[k].transform.position = new Vector3(stompPos.x + 3, stompPos.y - 2, stompPos.z);
                }
            }
        }
    }

    public void jumpLedges()
    {
        List<Vector3> ledges = new List<Vector3>();
        int prevY = 0;
        int postY = -100;
        int gapStart = -100;
        for (int i = 1; i < lvl.Count; i++)
        {
            Vector3 element = lvl[i];
            if (element.z == 0 && gapStart == -100)
            {
                prevY = (int)element.y;
            }
            if (element.z == -1 && gapStart == -100)
            {
                gapStart = i;
            }
            if (element.z == 0 && gapStart != -100)
            {
                postY = (int)element.y;
                if (prevY < postY)
                {
                    ledges.Add(new Vector3(gapStart, prevY, postY));
                }
                prevY = postY;
                postY = -100;
                gapStart = -100;
            }
        }
        //for (int i = 0; i < ledges.Count; i++)
        //{
        //    ////Debug.Log("ORIGNAL::: " + lvl[(int)(ledges[i].x - 1)]);
        //}

        //int offset = 0;
        //int start = 0;
        for (int i = 0; i < ledges.Count; i++)
        {
            Vector3 ps = ledges[i];
            ledges[i] = new Vector3(ps.x + 3*i, ps.y, ps.z);
            Vector3 ranges = ledges[i];

            ////Debug.Log("NEW::: " + lvl[(int)(ledges[i].x - 1)]);

            int baseX = (int)lvl[(int)ledges[i].x - 1].x;
            int baseY = (int)lvl[(int)ledges[i].x - 1].y; //ranges.y;
            //make and insert three cubes
            GameObject cube1 = new GameObject();
            SpriteRenderer sr = cube1.AddComponent<SpriteRenderer>() as SpriteRenderer;
            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            cube1.transform.position = new Vector3(baseX + 3, baseY - 1, 0);
            cube1.transform.localScale = blockScale;
            Sprite mySprite = Sprite.Create(texCoin, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                     new Vector2(0.5f, 0.5f), 100.0f);
            sr.sprite = mySprite;
            cube1.AddComponent<BoxCollider2D>();
            lvl.Insert((int)ranges.x, new Vector3(baseX + 3, baseY - 1, 0));

            List<GameObject> l = new List<GameObject>();
            l.Add(cube1);
            ground.Insert((int)ranges.x, l);

            GameObject cube2 = new GameObject();
            SpriteRenderer sr2 = cube2.AddComponent<SpriteRenderer>() as SpriteRenderer;
            sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            cube2.transform.position = new Vector3(baseX + 2, baseY - 1, 0);
            cube2.transform.localScale = blockScale;
            Sprite mySprite2 = Sprite.Create(texCoin, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                     new Vector2(0.5f, 0.5f), 100.0f);
            sr2.sprite = mySprite2;
            cube2.AddComponent<BoxCollider2D>();
            lvl.Insert((int)ranges.x, new Vector3(baseX + 2, baseY - 1, 0));

            List<GameObject> l2 = new List<GameObject>();
            l2.Add(cube2);
            ground.Insert((int)ranges.x, l2);

            GameObject cube3 = new GameObject();
            SpriteRenderer sr3 = cube3.AddComponent<SpriteRenderer>() as SpriteRenderer;
            sr3.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            cube3.transform.position = new Vector3(baseX + 1, baseY - 1, 0);
            cube3.transform.localScale = blockScale;
            Sprite mySprite3 = Sprite.Create(texCoin, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                     new Vector2(0.5f, 0.5f), 100.0f);
            sr3.sprite = mySprite3;
            cube3.AddComponent<BoxCollider2D>();
            lvl.Insert((int)ranges.x, new Vector3(baseX + 1, baseY - 1, 0));
            List<GameObject> l3 = new List<GameObject>();
            l.Add(cube3);
            ground.Insert((int)ranges.x, l3);

            //Add enemy to bounce off o 
            GameObject enemy = new GameObject();
            SpriteRenderer sre = enemy.AddComponent<SpriteRenderer>() as SpriteRenderer;
            sre.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            enemy.transform.position = new Vector3(baseX + 2, baseY, 0);
            enemy.transform.localScale = blockScale;
            Sprite mySpriteE = Sprite.Create(texEnemy, new Rect(0.0f, 0.0f, texEnemy.width, texEnemy.height),
                     new Vector2(0.5f, 0.5f), 100.0f);
            sre.sprite = mySpriteE;
            enemy.AddComponent<BoxCollider2D>();
            enemy.AddComponent<Rigidbody2D>();
            enemy.GetComponent<Rigidbody2D>().isKinematic = true;
            enemy.name = "" + enemyID;//enemy.GetComponent<BoxCollider2D>().isTrigger = true;
            enemy.AddComponent<EnemyController>();
            enemy.GetComponent<EnemyController>().texEnemy = texEnemy;
            enemy.GetComponent<EnemyController>().level = 0;
            enemy.GetComponent<EnemyController>().texEnemy2 = texEnemy2;
            enemy.GetComponent<EnemyController>().texEnemy3 = texEnemy3;
            enemy.GetComponent<EnemyController>().texEnemy4 = texEnemy4;
            enemyID += 1;
            enemies.Add(enemy);
            enemy.GetComponent<EnemyController>().xRange = new Vector2(baseX + 1, baseX + 3);

            ////Debug.Log("::: " + (ranges.x + 4) + " vs " + (baseX + 4));

            //offset += 3;
            //start = baseX + 4;

            for (int j = (int)ranges.x + 4; j < lvl.Count; j++)
            {
                Vector3 pos = lvl[j];
                lvl[j] = new Vector3(pos.x + 3, pos.y - 2, pos.z);

                List<GameObject> gndTexts = ground[j];
                for (int k = 0; k < gndTexts.Count; k++)
                {
                    Vector3 p = gndTexts[k].transform.position;
                    gndTexts[k].transform.position = new Vector3(p.x + 3, p.y - 2, p.z);
                }
            }
            ////Debug.Log("CUTOFF: " + (baseX + 4));
            for (int k = 0; k < stompers.Count; k++)
            {
                ////Debug.Log("STOMPS: " + stompers[k].transform.position);
                Vector3 stompPos = stompers[k].transform.position;
                if (stompPos.x >= (baseX + 4))
                {
                    stompers[k].transform.position = new Vector3(stompPos.x + 3, stompPos.y - 2, stompPos.z);
                }
            }
        }
        jumpLedgesUpper();
        cleanUpSpikyLedges();
        fixPlatforms();
        
    }

    public void fixPlatformsUpper()
    {
        for (int i = 2; i < upperLvl.Count - 2; i++)
        {
            Vector3 component = upperLvl[i];
            if ((int)component.z == 2)
            {
                float y = (upperLvl[i - 2].y + upperLvl[i + 2].y) / 2f;
                upperLvl[i] = new Vector3(component.x, y, component.z);
                //how to find platform in platforms array?
                for (int k = 0; k < platforms.Count; k++)
                {
                    Vector3 platPos = platforms[k].transform.position;
                    if (platPos.x == upperLvl[i].x)
                    {
                        //Debug.Log("Platform acquired!");
                        platforms[k].transform.position = new Vector3(platPos.x, y, platPos.z);
                    }
                }
            }
        }
    }

    public void fixPlatforms()
    {
        for (int i = 2; i < lvl.Count - 2; i++)
        {
            Vector3 component = lvl[i];
            if ((int)component.z == 2)
            {
                float y = (lvl[i - 2].y + lvl[i+2].y)/2f;
                lvl[i] = new Vector3(component.x, y, component.z);
                //how to find platform in platforms array?
                for (int k = 0; k < platforms.Count; k++)
                {
                    Vector3 platPos = platforms[k].transform.position;
                    if (platPos.x == lvl[i].x)
                    {
                        //Debug.Log("Platform acquired!");
                        platforms[k].transform.position = new Vector3(platPos.x, y, platPos.z);
                    }
                }
            }
        }
        fixPlatformsUpper();
    }

    public void jumpTerrainUpper()
    {
        //loop over level and change accordingly
        //Vector3 terrain = new Vector3(); //prevY, currY, switchStart, 
        List<Vector2> ranges = new List<Vector2>();
        Vector2 preGap = new Vector2();
        Vector2 postGap = new Vector2();
        preGap.x = 1;
        preGap.y = -100;
        postGap.x = -100;
        postGap.y = -100;

        for (int i = 1; i < upperLvl.Count; i++)
        {
            Vector3 element = upperLvl[i];
            if (preGap.y == -100)
            {
                if (element.z == -1)
                {
                    preGap.y = i;
                }
            }
            else
            {
                if (postGap.x == -100)
                {
                    if (element.z != -1)
                    {
                        postGap.x = i;
                    }
                }
                else
                {
                    if (element.z == -1)
                    {
                        postGap.y = i;
                        ranges.Add(new Vector2(preGap.x, preGap.y));
                        ranges.Add(new Vector2(postGap.x, postGap.y));
                        preGap.x = postGap.x;
                        preGap.y = postGap.y;
                        postGap.x = -100;
                        postGap.y = -100;
                    }
                }
            }
        }
        postGap.x = preGap.x;
        postGap.y = upperLvl.Count - 1;
        ranges.Add(new Vector2(preGap.x, preGap.y));
        ranges.Add(new Vector2(postGap.x, postGap.y));


        for (int i = 0; i < ranges.Count - 2; i += 2)
        {
            float preY = upperLvl[(int)ranges[i].y - 1].y;
            float postY = upperLvl[(int)ranges[i + 1].x].y;

            if (System.Math.Abs(preY - postY) < 0.1)
            {
                for (int j = (int)ranges[i + 1].x; j < (int)ranges[i + 1].y; j++)
                {
                    upperLvl[j] = new Vector3(upperLvl[j].x, upperLvl[j].y - 2, upperLvl[j].z);
                    List<GameObject> gndTexts = upperGround[j];
                    for (int k = 0; k < gndTexts.Count; k++)
                    {
                        Vector3 pos = gndTexts[k].transform.position;
                        gndTexts[k].transform.position = new Vector3(pos.x, pos.y - 2, pos.z);
                    }
                }
                for (int k = 0; k < stompers.Count; k++)
                {
                    Vector3 stompPos = stompers[k].transform.position;
                    if (stompPos.x > upperLvl[(int)ranges[i + 1].x].x &&
                        stompPos.x < upperLvl[(int)ranges[i + 1].y - 1].x)
                    {
                        stompers[k].transform.position = new Vector3(stompPos.x, stompPos.y - 2, stompPos.z);
                    }
                }

            }
            else if (preY - postY == 1)
            {
                for (int j = (int)ranges[i + 1].x; j < (int)ranges[i + 1].y; j++)
                {
                    upperLvl[j] = new Vector3(upperLvl[j].x, upperLvl[j].y - 1, upperLvl[j].z);
                    List<GameObject> gndTexts = upperGround[j];
                    for (int k = 0; k < gndTexts.Count; k++)
                    {
                        Vector3 pos = gndTexts[k].transform.position;
                        gndTexts[k].transform.position = new Vector3(pos.x, pos.y - 1, pos.z);
                    }
                }
                for (int k = 0; k < stompers.Count; k++)
                {
                    Vector3 stompPos = stompers[k].transform.position;
                    if (stompPos.x > upperLvl[(int)ranges[i + 1].x].x &&
                        stompPos.x < upperLvl[(int)ranges[i + 1].y - 1].x)
                    {
                        stompers[k].transform.position = new Vector3(stompPos.x, stompPos.y - 1, stompPos.z);
                    }
                }
            }
        }
    }

    public void jumpTerrain()
    {
        //Debug.Log("Upper Lvl: " + upperLvl.Count + " , " + upperGround.Count);
        //Debug.Log("Middle Lvl: " + lvl.Count + " , " + ground.Count);
        //Debug.Log("Lower Lvl: " + lowerLvl.Count + " , " + lowerGround.Count);
        //loop over level and change accordingly
        //Vector3 terrain = new Vector3(); //prevY, currY, switchStart, 
        List<Vector2> ranges = new List<Vector2>();
        Vector2 preGap = new Vector2();
        Vector2 postGap = new Vector2();
        preGap.x = 1;
        preGap.y = -100;
        postGap.x = -100;
        postGap.y = -100;

        for (int i = 1; i < lvl.Count; i++)
        {
            Vector3 element = lvl[i];
            if (preGap.y == -100)
            {
                if (element.z == -1)
                {
                    preGap.y = i;
                }
            } else
            {
                if (postGap.x == -100)
                {
                    if (element.z != -1)
                    {
                        postGap.x = i;
                    }
                } else 
                {
                    if (element.z == -1)
                    {
                        postGap.y = i;
                        ranges.Add(new Vector2(preGap.x, preGap.y));
                        ranges.Add(new Vector2(postGap.x, postGap.y));
                        preGap.x = postGap.x;
                        preGap.y = postGap.y;
                        postGap.x = -100;
                        postGap.y = -100;
                    }
                }
            }
        }
        postGap.x = preGap.x;
        postGap.y = lvl.Count - 1;
        ranges.Add(new Vector2(preGap.x, preGap.y));
        ranges.Add(new Vector2(postGap.x, postGap.y));


        for (int i = 0; i < ranges.Count - 2; i += 2)
        {
            float preY = lvl[(int)ranges[i].y - 1].y;
            float postY = lvl[(int)ranges[i + 1].x].y;

            if (System.Math.Abs(preY - postY) < 0.1)
            {
                for (int j = (int)ranges[i+1].x; j < (int)ranges[i+1].y; j++)
                {
                    lvl[j] = new Vector3(lvl[j].x, lvl[j].y - 2, lvl[j].z);
                    List<GameObject> gndTexts = ground[j];
                    for (int k = 0; k < gndTexts.Count; k++)
                    {
                        Vector3 pos = gndTexts[k].transform.position;
                        gndTexts[k].transform.position = new Vector3(pos.x, pos.y - 2, pos.z);
                    }
                }
                for (int k = 0; k < stompers.Count; k++)
                {
                    Vector3 stompPos = stompers[k].transform.position;
                    if (stompPos.x > lvl[(int)ranges[i + 1].x].x &&
                        stompPos.x < lvl[(int)ranges[i + 1].y - 1].x)
                    {
                        stompers[k].transform.position = new Vector3(stompPos.x, stompPos.y - 2, stompPos.z);
                    }
                }

            } else if (preY - postY == 1)
            {
                for (int j = (int)ranges[i + 1].x; j < (int)ranges[i + 1].y; j++)
                {
                    lvl[j] = new Vector3(lvl[j].x, lvl[j].y - 1, lvl[j].z);
                    List<GameObject> gndTexts = ground[j];
                    for (int k = 0; k < gndTexts.Count; k++)
                    {
                        Vector3 pos = gndTexts[k].transform.position;
                        gndTexts[k].transform.position = new Vector3(pos.x, pos.y - 1, pos.z);
                    }
                }
                for (int k = 0; k < stompers.Count; k++)
                {
                    Vector3 stompPos = stompers[k].transform.position;
                    if (stompPos.x > lvl[(int)ranges[i + 1].x].x &&
                        stompPos.x < lvl[(int)ranges[i + 1].y - 1].x)
                    {
                        stompers[k].transform.position = new Vector3(stompPos.x, stompPos.y - 1, stompPos.z);
                    }
                }
            }
        }
        jumpTerrainUpper();
        jumpLedges();
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

                            ////Debug.Log("(" + lvl[i].z);// + " , " + lvl[i].x + ")");
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
            ////Debug.Log("HI " + start + " , " + end + " , " + position + " , count " + stompers.Count);
            for (int j = start; j < end + 1; j++)
            {
                ////Debug.Log(lvl[j].y + " , " + stompers[i].transform.position.y);
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
            ////Debug.Log("Success? " + success); 
        }
        cleanUpUpperStomps();
        cleanUpLowerStomps();
    }

    public void cleanUpUpperStomps()
    {
        List<GameObject> toDelete = new List<GameObject>();
        //if these guys are near platforms that are higher
        for (int i = 0; i < upperStompers.Count; i++)
        {
            int position = (int)Mathf.Round(upperStompers[i].transform.position.x + 10);
            int start = position - 1;
            int end = position + 1;
            if (start < 0)
            {
                start = 0;
            }
            if (end > upperLvl.Count - 1)
            {
                end = upperLvl.Count - 1;
            }
            ////Debug.Log("HI " + start + " , " + end + " , " + position + " , count " + stompers.Count);
            for (int j = start; j < end + 1; j++)
            {
                ////Debug.Log(lvl[j].y + " , " + stompers[i].transform.position.y);
                if (System.Math.Abs(upperLvl[j].z - -1) > 0.1)
                {
                    if ((upperLvl[j].y) >= upperStompers[i].transform.position.y)
                    {
                        toDelete.Add(upperStompers[i]);
                    }
                }
            }
        }
        for (int j = 0; j < toDelete.Count; j++)
        {

            GameObject stomps = toDelete[j];
            bool success = upperStompers.Remove(toDelete[j]);
            Destroy(stomps);
            ////Debug.Log("Success? " + success); 
        }
    }

    public void cleanUpLowerStomps()
    {
        List<GameObject> toDelete = new List<GameObject>();
        //if these guys are near platforms that are higher
        for (int i = 0; i < lowerStompers.Count; i++)
        {
            int position = (int)Mathf.Round(lowerStompers[i].transform.position.x + 10);
            int start = position - 1;
            int end = position + 1;
            if (start < 0)
            {
                start = 0;
            }
            if (end > lowerLvl.Count - 1)
            {
                end = lowerLvl.Count - 1;
            }
            ////Debug.Log("HI " + start + " , " + end + " , " + position + " , count " + stompers.Count);
            for (int j = start; j < end + 1; j++)
            {
                ////Debug.Log(lvl[j].y + " , " + stompers[i].transform.position.y);
                if (System.Math.Abs(lowerLvl[j].z - -1) > 0.1)
                {
                    if ((lowerLvl[j].y) >= lowerStompers[i].transform.position.y)
                    {
                        toDelete.Add(lowerStompers[i]);
                    }
                }
            }
        }
        for (int j = 0; j < toDelete.Count; j++)
        {

            GameObject stomps = toDelete[j];
            bool success = lowerStompers.Remove(toDelete[j]);
            Destroy(stomps);
            ////Debug.Log("Success? " + success); 
        }
    }

    public void cosmetics()
    {
        float lowestY = 100;
        for (int i = 0; i < lvl.Count; i++)
        {
            if (lvl[i].y < lowestY && lvl[i].z != -1 && lvl[i].z != 2)
            {
                lowestY = lvl[i].y;
            }
        }

        for (int i = 0; i < lvl.Count - 3; i++)
        {
            for (int j = (int)lvl[i].y - 1; j > lowestY - 1; j--)
            {
                if (lvl[i].z == -1 || lvl[i].z == 2)
                {
                    continue;
                }
                GameObject cube = new GameObject();
                SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                cube.transform.localScale = blockScale;
                Sprite mySprite = Sprite.Create(texUnderGnd, new Rect(0.0f, 0.0f, texUnderGnd.width, texUnderGnd.height),
                         new Vector2(0.5f, 0.5f), 100.0f);
                sr.sprite = mySprite;
                cube.AddComponent<BoxCollider2D>();
                cube.transform.position = new Vector3(lvl[i].x, j, 0);
            }
        }
        cosmetics2();
        cosmetics3();
    }
    public void cosmetics2()
    {
        float lowestY = 100;
        for (int i = 0; i < upperLvl.Count; i++)
        {
            if (upperLvl[i].y < lowestY && upperLvl[i].z != -1 && upperLvl[i].z != 2)
            {
                lowestY = upperLvl[i].y;
            }
        }
        //Debug.Log("LowestY: " + lowestY);
        for (int i = 0; i < upperLvl.Count; i++)
        {
            for (int j = (int)upperLvl[i].y - 1; j > lowestY - 1; j--)
            {
                if (upperLvl[i].z == -1 || upperLvl[i].z == 2)
                {
                    continue;
                }
                GameObject cube = new GameObject();
                SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                cube.transform.localScale = blockScale;
                Sprite mySprite = Sprite.Create(texUnderGnd, new Rect(0.0f, 0.0f, texUnderGnd.width, texUnderGnd.height),
                         new Vector2(0.5f, 0.5f), 100.0f);
                sr.sprite = mySprite;
                cube.AddComponent<BoxCollider2D>();
                cube.transform.position = new Vector3(upperLvl[i].x, j, 0);
            }
        }
    }
    public void cosmetics3()
    {
        float lowestY = 100;
        for (int i = 0; i < lowerLvl.Count; i++)
        {
            if (lowerLvl[i].y < lowestY && lowerLvl[i].z != -1 && lowerLvl[i].z != 2)
            {
                lowestY = lowerLvl[i].y;
            }
        }

        for (int i = 0; i < lowerLvl.Count; i++)
        {
            for (int j = (int)lowerLvl[i].y - 1; j > lowestY - 1; j--)
            {
                if (lowerLvl[i].z == -1 || lowerLvl[i].z == 2)
                {
                    continue;
                }
                GameObject cube = new GameObject();
                SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                cube.transform.localScale = blockScale;
                Sprite mySprite = Sprite.Create(texUnderGnd, new Rect(0.0f, 0.0f, texUnderGnd.width, texUnderGnd.height),
                         new Vector2(0.5f, 0.5f), 100.0f);
                sr.sprite = mySprite;
                cube.AddComponent<BoxCollider2D>();
                cube.transform.position = new Vector3(lowerLvl[i].x, j, 0);
            }
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
                    ////Debug.Log(lvl[j].y + " , " + enemies[i].transform.position.y);
                    if (System.Math.Abs((lvl[j].y + 1) - enemies[i].transform.position.y) > 0.1)
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
            for (int j = position; j >= 0; j--)
            {
                if (j == 0) //if j is the beginning at any point, delete this guy cause his range reaches the beginning
                {
                    ////Debug.Log("DEATH!");
                    death = true;
                }
                ////Debug.Log("j: " + j);
                if (System.Math.Abs(lvl[j].z - -1) > 0.1)
                {
                    ////Debug.Log(lvl[j].y + " , " + enemies[i].transform.position.y);
                    if (System.Math.Abs((lvl[j].y + 1) - enemies[i].transform.position.y) > 0.1)
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
            ////Debug.Log("Enemy " + i + " platformSize is " + platformSize);
            if (platformSize - 1 <= 2 || death) //double counted position in forloops
            {
                toDelete.Add(enemies[i]);
            }
        }
        ////Debug.Log("Deleted: " + toDelete.Count + "total: " + enemies.Count);
        for (int j = 0; j < toDelete.Count; j++)
        {

            GameObject en = toDelete[j];
            bool success = enemies.Remove(toDelete[j]);
            Destroy(en);
            ////Debug.Log("Success? " + success); 
        }
        cleanUpUpperEnemies();
        cleanUpLowerEnemies();
    }

    public void cleanUpUpperEnemies()
    {
        List<GameObject> toDelete = new List<GameObject>();

        for (int i = 0; i < upperEnemies.Count; i++)
        {
            bool death = false;
            int position = (int)Mathf.Round(upperEnemies[i].transform.position.x - lvlRanges[1].x);
            //Debug.Log("Position " + i + " " + position);
            int platformSize = 0;
            for (int j = position; j < upperLvl.Count; j++)
            {
                if (System.Math.Abs(upperLvl[j].z - -1) > 0.1)
                {
                    ////Debug.Log(lvl[j].y + " , " + enemies[i].transform.position.y);
                    if (System.Math.Abs((upperLvl[j].y + 1) - upperEnemies[i].transform.position.y) > 0.1)
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
            for (int j = position; j >= 0; j--)
            {
                if (j == 0) //if j is the beginning at any point, delete this guy cause his range reaches the beginning
                {
                    ////Debug.Log("DEATH!");
                    death = true;
                }
                ////Debug.Log("j: " + j);
                if (System.Math.Abs(upperLvl[j].z - -1) > 0.1)
                {
                    ////Debug.Log(lvl[j].y + " , " + enemies[i].transform.position.y);
                    if (System.Math.Abs((upperLvl[j].y + 1) - upperEnemies[i].transform.position.y) > 0.1)
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
            ////Debug.Log("Enemy " + i + " platformSize is " + platformSize);
            if (platformSize - 1 <= 2 || death) //double counted position in forloops
            {
                toDelete.Add(upperEnemies[i]);
            }
        }
        ////Debug.Log("Deleted: " + toDelete.Count + "total: " + enemies.Count);
        for (int j = 0; j < toDelete.Count; j++)
        {

            GameObject en = toDelete[j];
            bool success = upperEnemies.Remove(toDelete[j]);
            Destroy(en);
            ////Debug.Log("Success? " + success); 
        }

    }

    public void cleanUpLowerEnemies()
    {
        List<GameObject> toDelete = new List<GameObject>();

        for (int i = 0; i < lowerEnemies.Count; i++)
        {
            bool death = false;
            int position = (int)Mathf.Round(lowerEnemies[i].transform.position.x - lvlRanges[2].x);
            int platformSize = 0;
            for (int j = position; j < lowerLvl.Count; j++)
            {
                if (System.Math.Abs(lowerLvl[j].z - -1) > 0.1)
                {
                    ////Debug.Log(lvl[j].y + " , " + enemies[i].transform.position.y);
                    if (System.Math.Abs((lowerLvl[j].y + 1) - lowerEnemies[i].transform.position.y) > 0.1)
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
            for (int j = position; j >= 0; j--)
            {
                if (j == 0) //if j is the beginning at any point, delete this guy cause his range reaches the beginning
                {
                    ////Debug.Log("DEATH!");
                    death = true;
                }
                ////Debug.Log("j: " + j);
                if (System.Math.Abs(lowerLvl[j].z - -1) > 0.1)
                {
                    ////Debug.Log(lvl[j].y + " , " + enemies[i].transform.position.y);
                    if (System.Math.Abs((lowerLvl[j].y + 1) - lowerEnemies[i].transform.position.y) > 0.1)
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
            ////Debug.Log("Enemy " + i + " platformSize is " + platformSize);
            if (platformSize - 1 <= 2 || death) //double counted position in forloops
            {
                toDelete.Add(lowerEnemies[i]);
            }
        }
        ////Debug.Log("Deleted: " + toDelete.Count + "total: " + enemies.Count);
        for (int j = 0; j < toDelete.Count; j++)
        {

            GameObject en = toDelete[j];
            bool success = lowerEnemies.Remove(toDelete[j]);
            Destroy(en);
            ////Debug.Log("Success? " + success); 
        }

    }

    public static void reset()
    {
        lvl.Clear();
        upperLvl.Clear();
        lowerLvl.Clear();
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

        startLower = false;
        startUpper = false;
        lvlRanges.Clear();
        stars.Clear();
    }

    public void placeSuperEnemies()
    {
        //loop over spike array and add enemies
        Vector4 range = new Vector4(spikes[0].transform.position.x,0, spikes[0].transform.position.y, 1); //startX, endX, Ypos, length
        List<Vector4> ranges = new List<Vector4>();
        for (int i = 0; i < spikes.Count - 1; i++)
        {
            float x1 = spikes[i].transform.position.x;
            float x2 = spikes[i + 1].transform.position.x;
            float y1 = spikes[i].transform.position.y;
            float y2 = spikes[i + 1].transform.position.y;
            ////Debug.Log(y1 + " : " + y2 + " - " + (y1-y2));
            if (System.Math.Abs(x2 - x1 - 1) < 0.1 &&
                System.Math.Abs(System.Math.Abs(y2 - y1)) < 0.1)
            {
                range.w += 1;
            } else
            {
                range.y = x1;
                //add enemies here
                ranges.Add(new Vector4(range.x, range.y, range.z, range.w));
                range.x = x2;
                range.y = x2;
                range.z = spikes[i + 1].transform.position.y;
                range.w = 1;
            }
            if (i == spikes.Count - 2)
            {
                ranges.Add(new Vector4(range.x, spikes[i+1].transform.position.x, range.z, range.w + 1));
            }
        }

        for (int i = 0; i < ranges.Count; i++)
        {
            ////Debug.Log("Range" + i + " : " + ranges[i]);
            Vector4 block = ranges[i];
            if (block.w >= 2)
            {
                int numEnemies = (int)Mathf.Round(block.w / 3f);
                bool dir = true;
                for (int j = 0; j < numEnemies; j++)
                {
                    //create enemy
                    GameObject enemy = new GameObject();
                    SpriteRenderer sr4 = enemy.AddComponent<SpriteRenderer>() as SpriteRenderer;
                    sr4.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                    if (dir)
                    {
                        enemy.transform.position = new Vector3(block.x, block.z + 1, 0);
                    }
                    else
                    {
                        enemy.transform.position = new Vector3(block.x + j * 2, block.z + 1, 0);
                    }

                    enemy.transform.localScale = blockScale;
                    Sprite mySprite4 = Sprite.Create(texEnemy, new Rect(0.0f, 0.0f, texEnemy.width, texEnemy.height),
                             new Vector2(0.5f, 0.5f), 100.0f);
                    sr4.sprite = mySprite4;
                    enemy.AddComponent<BoxCollider2D>();
                    enemy.AddComponent<Rigidbody2D>();
                    enemy.GetComponent<Rigidbody2D>().isKinematic = true;
                    enemy.AddComponent<EnemyController>();
                    enemy.GetComponent<EnemyController>().level = 0;
                    ////Debug.Log("gbx: " + globalX + " , " + (globalX + 3));
                    if (j == numEnemies - 1)
                    {
                        enemy.GetComponent<EnemyController>().xRange = new Vector2(block.x + j * 2, block.y);
                    }
                    else
                    {
                        enemy.GetComponent<EnemyController>().xRange = new Vector2(block.x + j * 2, block.x + j * 2 + 2);
                    }

                    ////Debug.Log("(" + (block.x + j * 3) + ", " + (block.x + j * 3 + 2) + ")");
                    //enemy.GetComponent<EnemyController>().state = 1;
                    enemy.GetComponent<EnemyController>().texEnemy = texEnemy;
                    enemy.GetComponent<EnemyController>().texEnemy2 = texEnemy2;
                    enemy.GetComponent<EnemyController>().texEnemy3 = texEnemy3;
                    enemy.GetComponent<EnemyController>().texEnemy4 = texEnemy4;
                    superEnemies.Add(enemy);
                    dir = !dir;
                }
            }
        }
        ////Debug.Log("Num: " + superEnemies.Count);
        placeSuperEnemiesUp();
        placeSuperEnemiesLow();
    }

    public void placeSuperEnemiesUp()
    {
        if (upperSpikes.Count == 0)
        {
            return;
        }
        //loop over spike array and add enemies
        Vector4 range = new Vector4(upperSpikes[0].transform.position.x, 0, upperSpikes[0].transform.position.y, 1); //startX, endX, Ypos, length
        List<Vector4> ranges = new List<Vector4>();
        for (int i = 0; i < upperSpikes.Count - 1; i++)
        {
            float x1 = upperSpikes[i].transform.position.x;
            float x2 = upperSpikes[i + 1].transform.position.x;
            float y1 = upperSpikes[i].transform.position.y;
            float y2 = upperSpikes[i + 1].transform.position.y;
            ////Debug.Log(y1 + " : " + y2 + " - " + (y1-y2));
            if (System.Math.Abs(x2 - x1 - 1) < 0.1 &&
                System.Math.Abs(System.Math.Abs(y2 - y1)) < 0.1)
            {
                range.w += 1;
            }
            else
            {
                range.y = x1;
                //add enemies here
                ranges.Add(new Vector4(range.x, range.y, range.z, range.w));
                range.x = x2;
                range.y = x2;
                range.z = upperSpikes[i + 1].transform.position.y;
                range.w = 1;
            }
            if (i == upperSpikes.Count - 2)
            {
                ranges.Add(new Vector4(range.x, upperSpikes[i + 1].transform.position.x, range.z, range.w + 1));
            }
        }

        for (int i = 0; i < ranges.Count; i++)
        {
            ////Debug.Log("Range" + i + " : " + ranges[i]);
            Vector4 block = ranges[i];
            if (block.w >= 2)
            {
                int numEnemies = (int)Mathf.Round(block.w / 3f);
                bool dir = true;
                for (int j = 0; j < numEnemies; j++)
                {
                    //create enemy
                    GameObject enemy = new GameObject();
                    SpriteRenderer sr4 = enemy.AddComponent<SpriteRenderer>() as SpriteRenderer;
                    sr4.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                    if (dir)
                    {
                        enemy.transform.position = new Vector3(block.x, block.z + 1, 0);
                    }
                    else
                    {
                        enemy.transform.position = new Vector3(block.x + j * 2, block.z + 1, 0);
                    }

                    enemy.transform.localScale = blockScale;
                    Sprite mySprite4 = Sprite.Create(texEnemy, new Rect(0.0f, 0.0f, texEnemy.width, texEnemy.height),
                             new Vector2(0.5f, 0.5f), 100.0f);
                    sr4.sprite = mySprite4;
                    enemy.AddComponent<BoxCollider2D>();
                    enemy.AddComponent<Rigidbody2D>();
                    enemy.GetComponent<Rigidbody2D>().isKinematic = true;
                    enemy.AddComponent<EnemyController>();
                    enemy.GetComponent<EnemyController>().level = 1;
                    ////Debug.Log("gbx: " + globalX + " , " + (globalX + 3));
                    if (j == numEnemies - 1)
                    {
                        enemy.GetComponent<EnemyController>().xRange = new Vector2(block.x + j * 2, block.y);
                    }
                    else
                    {
                        enemy.GetComponent<EnemyController>().xRange = new Vector2(block.x + j * 2, block.x + j * 2 + 2);
                    }

                    ////Debug.Log("(" + (block.x + j * 3) + ", " + (block.x + j * 3 + 2) + ")");
                    //enemy.GetComponent<EnemyController>().state = 1;
                    enemy.GetComponent<EnemyController>().texEnemy = texEnemy;
                    enemy.GetComponent<EnemyController>().texEnemy2 = texEnemy2;
                    enemy.GetComponent<EnemyController>().texEnemy3 = texEnemy3;
                    enemy.GetComponent<EnemyController>().texEnemy4 = texEnemy4;
                    superEnemies.Add(enemy);
                    dir = !dir;
                }
            }
        }
        ////Debug.Log("Num: " + superEnemies.Count);

    }

    //CHANGE INSIDE THE FUNCTION!!!!
    public void placeSuperEnemiesLow()
    {
        if (lowerSpikes.Count == 0)
        {
            return;
        }
        //loop over spike array and add enemies
        Vector4 range = new Vector4(lowerSpikes[0].transform.position.x, 0, lowerSpikes[0].transform.position.y, 1); //startX, endX, Ypos, length
        List<Vector4> ranges = new List<Vector4>();
        for (int i = 0; i < lowerSpikes.Count - 1; i++)
        {
            float x1 = lowerSpikes[i].transform.position.x;
            float x2 = lowerSpikes[i + 1].transform.position.x;
            float y1 = lowerSpikes[i].transform.position.y;
            float y2 = lowerSpikes[i + 1].transform.position.y;
            ////Debug.Log(y1 + " : " + y2 + " - " + (y1-y2));
            if (System.Math.Abs(x2 - x1 - 1) < 0.1 &&
                System.Math.Abs(System.Math.Abs(y2 - y1)) < 0.1)
            {
                range.w += 1;
            }
            else
            {
                range.y = x1;
                //add enemies here
                ranges.Add(new Vector4(range.x, range.y, range.z, range.w));
                range.x = x2;
                range.y = x2;
                range.z = lowerSpikes[i + 1].transform.position.y;
                range.w = 1;
            }
            if (i == lowerSpikes.Count - 2)
            {
                ranges.Add(new Vector4(range.x, lowerSpikes[i + 1].transform.position.x, range.z, range.w + 1));
            }
        }

        for (int i = 0; i < ranges.Count; i++)
        {
            ////Debug.Log("Range" + i + " : " + ranges[i]);
            Vector4 block = ranges[i];
            if (block.w >= 2)
            {
                int numEnemies = (int)Mathf.Round(block.w / 3f);
                bool dir = true;
                for (int j = 0; j < numEnemies; j++)
                {
                    //create enemy
                    GameObject enemy = new GameObject();
                    SpriteRenderer sr4 = enemy.AddComponent<SpriteRenderer>() as SpriteRenderer;
                    sr4.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                    if (dir)
                    {
                        enemy.transform.position = new Vector3(block.x, block.z + 1, 0);
                    }
                    else
                    {
                        enemy.transform.position = new Vector3(block.x + j * 2, block.z + 1, 0);
                    }

                    enemy.transform.localScale = blockScale;
                    Sprite mySprite4 = Sprite.Create(texEnemy, new Rect(0.0f, 0.0f, texEnemy.width, texEnemy.height),
                             new Vector2(0.5f, 0.5f), 100.0f);
                    sr4.sprite = mySprite4;
                    enemy.AddComponent<BoxCollider2D>();
                    enemy.AddComponent<Rigidbody2D>();
                    enemy.GetComponent<Rigidbody2D>().isKinematic = true;
                    enemy.AddComponent<EnemyController>();
                    enemy.GetComponent<EnemyController>().level = 2;
                    ////Debug.Log("gbx: " + globalX + " , " + (globalX + 3));
                    if (j == numEnemies - 1)
                    {
                        enemy.GetComponent<EnemyController>().xRange = new Vector2(block.x + j * 2, block.y);
                    }
                    else
                    {
                        enemy.GetComponent<EnemyController>().xRange = new Vector2(block.x + j * 2, block.x + j * 2 + 2);
                    }

                    ////Debug.Log("(" + (block.x + j * 3) + ", " + (block.x + j * 3 + 2) + ")");
                    //enemy.GetComponent<EnemyController>().state = 1;
                    enemy.GetComponent<EnemyController>().texEnemy = texEnemy;
                    enemy.GetComponent<EnemyController>().texEnemy2 = texEnemy2;
                    enemy.GetComponent<EnemyController>().texEnemy3 = texEnemy3;
                    enemy.GetComponent<EnemyController>().texEnemy4 = texEnemy4;
                    superEnemies.Add(enemy);
                    dir = !dir;
                }
            }
        }
        ////Debug.Log("Num: " + superEnemies.Count);

    }

    //for now, assume geometry is 1x1 cubes
    public void generateGeometry()
    {
        //TODO tomorrow:
        //anytime you see a 0, call statemachine and generate
        //an appropriate cube/triangle to place in the level

        //loops over rhythm array in block to place geometry according
        //to the state machine
        ////Debug.Log("BLOCKS: " + blocks.Count);
        ////Debug.Log("STARTX: " + globalX);
        lvl.Add(new Vector3(0,0, -1));
        Vector4 lvlRange = new Vector4();
        Vector4 upRange = new Vector4();
        Vector4 lowRange = new Vector4();

        lvlRange.x = globalX;
        lvlRange.z = globalY;
        lvlRange.w = globalY;

        upRange.z = upGlobalY;
        upRange.w = upGlobalY;

        lowRange.z = lowGlobalY;
        lowRange.w = lowGlobalY;

        List<GameObject> l5 = new List<GameObject>();
        ground.Add(l5);

        bool prevStartU = false;
        bool prevStartL = false;

        for (int i = 0; i < blocks.Count; i++)
        {
            level = (int)blocks[i].getLevel();
            //bool pair = blocks[i].isPair();
            if (!startLower)
            {
                ////Debug.Log("ㅇㅏㄴㅣㅇㅛ??");
                lowGlobalX = globalX;
                lowRange.x = globalX;
            }
            if (!startUpper)
            {
                upGlobalX = globalX;
                upRange.x = globalX;
            }
            if (level == 1 && !startUpper)
            {
                startUpper = true;
                //Debug.Log("Start upper: " + upGlobalX);
            }
            if (level == -1 && !startLower)
            {
                startLower = true;
                //Debug.Log("Start lower: " + lowGlobalX);
            }

            timer = 0;
            int[] rhythm = blocks[i].getRhythmArray();
            List<Vector2> action = blocks[i].getActionArray();
            int s = state.getNextState();
            ////Debug.Log("GENERATOR RHYTHM LENGTH: " + rhythm.Length);

            //Break area:
            for (int k = 0; k < 4; k++)
            {
                GameObject cube = new GameObject();
                SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                cube.transform.localScale = blockScale;
                Sprite mySprite = Sprite.Create(texGnd, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                         new Vector2(0.5f, 0.5f), 100.0f);
                sr.sprite = mySprite;
                cube.AddComponent<BoxCollider2D>();
                if (level == 0)
                {
                    if (!prevStartU && startUpper)
                    {
                        prevStartU = true;
                        mySprite = Sprite.Create(texPipe, new Rect(0.0f, 0.0f, texPipe.width, texPipe.height),
                                                 new Vector2(0.5f, 0.5f), 100.0f);
                        sr.sprite = mySprite;
                        lvl.Add(new Vector3(globalX, globalY, 5));
                    } else if (!prevStartL && startLower)
                    {
                        prevStartL = true;
                        mySprite = Sprite.Create(texPipe, new Rect(0.0f, 0.0f, texPipe.width, texPipe.height),
                                                 new Vector2(0.5f, 0.5f), 100.0f);
                        sr.sprite = mySprite;
                        lvl.Add(new Vector3(globalX, globalY, 5));
                    } else
                    {
                        lvl.Add(new Vector3(globalX, globalY, 0));
                    }

                    cube.transform.position = new Vector3(globalX, globalY, 0);
                    List<GameObject> l = new List<GameObject>();
                    l.Add(cube);
                    ground.Add(l);
                } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                {
                    if (upperLvl.Count == 0)
                    {
                        mySprite = Sprite.Create(texPipe, new Rect(0.0f, 0.0f, texPipe.width, texPipe.height),
                                                 new Vector2(0.5f, 0.5f), 100.0f);
                        sr.sprite = mySprite;
                        upperLvl.Add(new Vector3(upGlobalX, upGlobalY, 5));
                    } else
                    {
                        upperLvl.Add(new Vector3(upGlobalX, upGlobalY, 0));
                    }
                    
                    cube.transform.position = new Vector3(upGlobalX, upGlobalY, 0);
                    List<GameObject> l = new List<GameObject>();
                    l.Add(cube);
                    upperGround.Add(l);
                }
                else if (RhythmGenerator.constraints[4] == 1)
                {
                    if (lowerLvl.Count == 0)
                    {
                        mySprite = Sprite.Create(texPipe, new Rect(0.0f, 0.0f, texPipe.width, texPipe.height),
                                                 new Vector2(0.5f, 0.5f), 100.0f);
                        sr.sprite = mySprite;
                        lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY, 0));
                    } else
                    {
                        lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY, 0));
                    }
                    
                    cube.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);
                    List<GameObject> l = new List<GameObject>();
                    l.Add(cube);
                    lowerGround.Add(l);
                }

                if (level == 0)
                {
                    globalX += 1;
                }
                else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                {
                    upGlobalX += 1;
                }
                else if (RhythmGenerator.constraints[4] == 1)
                {
                    lowGlobalX += 1;
                }
            }


            for (int j = 0; j < rhythm.Length; j++)
            {
                if (level == 0)
                {
                    if (lvlRange.z > globalY)
                    {
                        lvlRange.z = globalY;
                    }
                    if (lvlRange.w < globalY)
                    {
                        lvlRange.w = globalY;
                    }
                } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                {
                    if (upRange.z > upGlobalY)
                    {
                        upRange.z = upGlobalY;
                    }
                    if (upRange.w < upGlobalY)
                    {
                        upRange.w = upGlobalY;
                    }
                } else if (RhythmGenerator.constraints[4] == 1)
                {
                    if (lowRange.z > lowGlobalY)
                    {
                        lowRange.z = lowGlobalY;
                    }
                    if (lowRange.w < lowGlobalY)
                    {
                        lowRange.w = lowGlobalY;
                    }
                }

                //don't want to move up at the end of a rhythm
                if (j == rhythm.Length - 1 || j == rhythm.Length - 2
                    || j == rhythm.Length - 3)
                {
                    state.resetClock();
                }

                ////Debug.Log("S: " + s);

                if (rhythm[j] == 0)
                {
                    timer += 1;

                    //place a block at global x,y
                    if (s == 1) {
                        //make the ground spikes!!
                        if (RhythmGenerator.constraints[3] == 1)
                        {
                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();//BoxCollider2D bc = cube.AddComponent<BoxCollider2D>() as BoxCollider2D;
                                                               //lvl.Add(new Vector3(globalX, globalY,0));

                            GameObject cube2 = new GameObject();
                            SpriteRenderer sr2 = cube2.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube2.transform.localScale = blockScale;
                            Sprite mySprite2 = Sprite.Create(texSpike, new Rect(0.0f, 0.0f, texSpike.width, texSpike.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr2.sprite = mySprite2;
                            cube2.AddComponent<BoxCollider2D>();

                            if (level == 0)
                            {
                                lvl.Add(new Vector3(globalX, globalY + 1, 1));
                                cube.transform.position = new Vector3(globalX, globalY, 0);
                                cube2.transform.position = new Vector3(globalX, globalY + 1, 0);
                                spikes.Add(cube);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube2);
                                l.Add(cube);
                                ground.Add(l);
                            } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                            {
                                upperLvl.Add(new Vector3(upGlobalX, upGlobalY + 1, 1));
                                cube.transform.position = new Vector3(upGlobalX, upGlobalY, 0);
                                cube2.transform.position = new Vector3(upGlobalX, upGlobalY + 1, 0);
                                upperSpikes.Add(cube);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube2);
                                l.Add(cube);
                                upperGround.Add(l);
                            } else if (RhythmGenerator.constraints[4] == 1)
                            {
                                lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY + 1, 1));
                                cube.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);
                                cube2.transform.position = new Vector3(lowGlobalX, lowGlobalY + 1, 0);
                                lowerSpikes.Add(cube);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube2);
                                l.Add(cube);
                                lowerGround.Add(l);
                            }
                        } else
                        {
                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texGnd, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();//BoxCollider2D bc = cube.AddComponent<BoxCollider2D>() as BoxCollider2D;
                                                               //lvl.Add(new Vector3(globalX, globalY,0));

                            GameObject cube2 = new GameObject();
                            SpriteRenderer sr2 = cube2.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            
                            cube2.transform.localScale = blockScale;
                            Sprite mySprite2 = Sprite.Create(texSlope, new Rect(0.0f, 0.0f, texSlope.width, texSlope.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr2.sprite = mySprite2;
                            cube2.AddComponent<BoxCollider2D>();

                            if (level == 0)
                            {
                                lvl.Add(new Vector3(globalX, globalY + 1, 3));
                                cube.transform.position = new Vector3(globalX, globalY, 0);
                                cube2.transform.position = new Vector3(globalX, globalY + 1, 0);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube2);
                                l.Add(cube);
                                ground.Add(l);
                            } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                            {
                                upperLvl.Add(new Vector3(upGlobalX, upGlobalY + 1, 3));
                                cube.transform.position = new Vector3(upGlobalX, upGlobalY, 0);
                                cube2.transform.position = new Vector3(upGlobalX, upGlobalY + 1, 0);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube2);
                                l.Add(cube);
                                upperGround.Add(l);
                            } else if (RhythmGenerator.constraints[4] == 1)
                            {
                                lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY + 1, 3));
                                cube.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);
                                cube2.transform.position = new Vector3(lowGlobalX, lowGlobalY + 1, 0);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube2);
                                l.Add(cube);
                                lowerGround.Add(l);
                            }
                        }
                        

                    } else
                    {
                        if (RhythmGenerator.constraints[3] == 1)
                        {
                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texSpike, new Rect(0.0f, 0.0f, texSpike.width, texSpike.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();

                            if (level == 0)
                            {
                                lvl.Add(new Vector3(globalX, globalY, 1));
                                cube.transform.position = new Vector3(globalX, globalY, 0);
                                spikes.Add(cube);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube);
                                ground.Add(l);
                            } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                            {
                                upperLvl.Add(new Vector3(upGlobalX, upGlobalY, 1));
                                cube.transform.position = new Vector3(upGlobalX, upGlobalY, 0);
                                upperSpikes.Add(cube);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube);
                                upperGround.Add(l);
                            } else if (RhythmGenerator.constraints[4] == 1)
                            {
                                lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY, 1));
                                cube.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);
                                lowerSpikes.Add(cube);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube);
                                lowerGround.Add(l);
                            }
                        }
                        else
                        {
                            if (s == 2)
                            {
                                if (level == 0)
                                {
                                    if (originalY - 5 < globalY - 1)
                                    {
                                        GameObject cube2 = new GameObject();
                                        SpriteRenderer sr2 = cube2.AddComponent<SpriteRenderer>() as SpriteRenderer;
                                        sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                                        cube2.transform.localScale = blockScale;
                                        Sprite mySprite2 = Sprite.Create(texSlope2, new Rect(0.0f, 0.0f, texSlope2.width, texSlope2.height),
                                                 new Vector2(0.5f, 0.5f), 100.0f);
                                        sr2.sprite = mySprite2;
                                        cube2.AddComponent<BoxCollider2D>();

                                        lvl.Add(new Vector3(globalX, globalY, 4));
                                        cube2.transform.position = new Vector3(globalX, globalY, 0);
                                        List<GameObject> l = new List<GameObject>();
                                        l.Add(cube2);
                                        ground.Add(l);
                                        //globalY -= 1;
                                    }
                                }
                                else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                                {
                                    if (originalY + 5 < upGlobalY - 1)
                                    {
                                        GameObject cube2 = new GameObject();
                                        SpriteRenderer sr2 = cube2.AddComponent<SpriteRenderer>() as SpriteRenderer;
                                        sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                                        cube2.transform.localScale = blockScale;
                                        Sprite mySprite2 = Sprite.Create(texSlope2, new Rect(0.0f, 0.0f, texSlope2.width, texSlope2.height),
                                                 new Vector2(0.5f, 0.5f), 100.0f);
                                        sr2.sprite = mySprite2;
                                        cube2.AddComponent<BoxCollider2D>();
                                        upperLvl.Add(new Vector3(upGlobalX, upGlobalY, 4));
                                        cube2.transform.position = new Vector3(upGlobalX, upGlobalY, 0);
                                        List<GameObject> l = new List<GameObject>();
                                        l.Add(cube2);
                                        upperGround.Add(l);
                                        //upGlobalY -= 1;
                                    }
                                }
                                else if (RhythmGenerator.constraints[4] == 1)
                                {
                                    GameObject cube2 = new GameObject();
                                    SpriteRenderer sr2 = cube2.AddComponent<SpriteRenderer>() as SpriteRenderer;
                                    sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                                    cube2.transform.localScale = blockScale;
                                    Sprite mySprite2 = Sprite.Create(texSlope2, new Rect(0.0f, 0.0f, texSlope2.width, texSlope2.height),
                                             new Vector2(0.5f, 0.5f), 100.0f);
                                    sr2.sprite = mySprite2;
                                    cube2.AddComponent<BoxCollider2D>();

                                    lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY, 4));
                                    cube2.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);
                                    List<GameObject> l = new List<GameObject>();
                                    l.Add(cube2);
                                    lowerGround.Add(l);
                                    //lowGlobalY -= 1;
                                } else
                                {
                                    GameObject cube2 = new GameObject();
                                    SpriteRenderer sr2 = cube2.AddComponent<SpriteRenderer>() as SpriteRenderer;
                                    sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                                    cube2.transform.localScale = blockScale;
                                    Sprite mySprite2 = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                             new Vector2(0.5f, 0.5f), 100.0f);
                                    sr2.sprite = mySprite2;
                                    cube2.AddComponent<BoxCollider2D>();

                                    if (level == 0)
                                    {
                                        lvl.Add(new Vector3(globalX, globalY, 0));
                                        cube2.transform.position = new Vector3(globalX, globalY, 0);
                                        List<GameObject> l = new List<GameObject>();
                                        l.Add(cube2);
                                        ground.Add(l);
                                    }
                                    else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                                    {
                                        upperLvl.Add(new Vector3(upGlobalX, upGlobalY, 0));
                                        cube2.transform.position = new Vector3(upGlobalX, upGlobalY, 0);
                                        List<GameObject> l = new List<GameObject>();
                                        l.Add(cube2);
                                        upperGround.Add(l);
                                    }
                                    else if (RhythmGenerator.constraints[4] == 1)
                                    {
                                        lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY, 0));
                                        cube2.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);
                                        List<GameObject> l = new List<GameObject>();
                                        l.Add(cube2);
                                        lowerGround.Add(l);
                                    }
                                }

                            } else
                            {
                                GameObject cube2 = new GameObject();
                                SpriteRenderer sr2 = cube2.AddComponent<SpriteRenderer>() as SpriteRenderer;
                                sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                                cube2.transform.localScale = blockScale;
                                Sprite mySprite2 = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                         new Vector2(0.5f, 0.5f), 100.0f);
                                sr2.sprite = mySprite2;
                                cube2.AddComponent<BoxCollider2D>();

                                if (level == 0)
                                {
                                    lvl.Add(new Vector3(globalX, globalY, 0));
                                    cube2.transform.position = new Vector3(globalX, globalY, 0);
                                    List<GameObject> l = new List<GameObject>();
                                    l.Add(cube2);
                                    ground.Add(l);
                                } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                                {
                                    upperLvl.Add(new Vector3(upGlobalX, upGlobalY, 0));
                                    cube2.transform.position = new Vector3(upGlobalX, upGlobalY, 0);
                                    List<GameObject> l = new List<GameObject>();
                                    l.Add(cube2);
                                    upperGround.Add(l);
                                } else if (RhythmGenerator.constraints[4] == 1)
                                {
                                    lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY, 0));
                                    cube2.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);
                                    List<GameObject> l = new List<GameObject>();
                                    l.Add(cube2);
                                    lowerGround.Add(l);
                                }
                            }
                        }

                    }

                    if (level == 0)
                    {
                        globalX += 1;
                    }
                    else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                    {
                        upGlobalX += 1;
                    }
                    else if (RhythmGenerator.constraints[4] == 1)
                    {
                        lowGlobalX += 1;
                    }

                    if (s == 1)
                    {
                        if (level == 0)
                        {
                            if (originalY + 10 > globalY + 1)
                            {
                                globalY += 1;
                            }
                        }
                        else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                        {
                            upGlobalY += 1;
                        }
                        else if (RhythmGenerator.constraints[4] == 1)
                        {
                            if (originalY - 5 > lowGlobalY + 1)
                            {
                                lowGlobalY += 1;
                            }
                        }
                    }
                    if (s == 2)
                    {
                        if (level == 0)
                        {
                            if (originalY - 5 < globalY - 1)
                            {
                                globalY -= 1;
                            }
                        }
                        else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                        {
                            if (originalY + 5 < upGlobalY - 1)
                            {
                                upGlobalY -= 1;
                            }
                        }
                        else if (RhythmGenerator.constraints[4] == 1)
                        {
                            lowGlobalY -= 1;
                        }
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
                            if (RhythmGenerator.constraints[0] == 1)
                            {
                                GameObject cube = new GameObject();
                                SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                                sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                                cube.transform.localScale = blockScale;
                                Sprite mySprite = Sprite.Create(texGnd, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                                         new Vector2(0.5f, 0.5f), 100.0f);
                                sr.sprite = mySprite;
                                cube.AddComponent<BoxCollider2D>();

                                if (level == 0)
                                {
                                    lvl.Add(new Vector3(globalX, globalY, 0));
                                    cube.transform.position = new Vector3(globalX, globalY, 0);
                                    List<GameObject> l = new List<GameObject>();
                                    l.Add(cube);
                                    ground.Add(l);
                                } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                                {
                                    upperLvl.Add(new Vector3(upGlobalX, upGlobalY, 0));
                                    cube.transform.position = new Vector3(upGlobalX, upGlobalY, 0);
                                    List<GameObject> l = new List<GameObject>();
                                    l.Add(cube);
                                    upperGround.Add(l);
                                } else if (RhythmGenerator.constraints[4] == 1)
                                {
                                    lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY, 0));
                                    cube.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);
                                    List<GameObject> l = new List<GameObject>();
                                    l.Add(cube);
                                    lowerGround.Add(l);
                                }
                                
                            } else
                            {
                                GameObject spike = new GameObject();
                                SpriteRenderer sr = spike.AddComponent<SpriteRenderer>() as SpriteRenderer;
                                sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                                spike.transform.localScale = blockScale;
                                Sprite mySprite = Sprite.Create(texSpike, new Rect(0.0f, 0.0f, texSpike.width, texSpike.height),
                                         new Vector2(0.5f, 0.5f), 100.0f);
                                sr.sprite = mySprite;
                                spike.AddComponent<BoxCollider2D>();

                                if (level == 0)
                                {
                                    lvl.Add(new Vector3(globalX, globalY, 1));
                                    spike.transform.position = new Vector3(globalX, globalY, 0);
                                    spikes.Add(spike);
                                    List<GameObject> l = new List<GameObject>();
                                    l.Add(spike);
                                    ground.Add(l);
                                } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                                {
                                    upperLvl.Add(new Vector3(upGlobalX, upGlobalY, 1));
                                    spike.transform.position = new Vector3(upGlobalX, upGlobalY, 0);
                                    //Debug.Log("AM I UPPPP HERE?");
                                    upperSpikes.Add(spike);
                                    List<GameObject> l = new List<GameObject>();
                                    l.Add(spike);
                                    upperGround.Add(l);
                                } else if (RhythmGenerator.constraints[4] == 1)
                                {
                                    lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY, 1));
                                    spike.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);
                                    lowerSpikes.Add(spike);
                                    List<GameObject> l = new List<GameObject>();
                                    l.Add(spike);
                                    lowerGround.Add(l);
                                }
                            }
                            
                        } else
                        {
                            ////Debug.Log("DUrATioN: " + (globalX + 1) + " v " + (globalX + act.y + 2) + " : " + act.y);
                            
                            if (RhythmGenerator.constraints[0] == 1)
                            {
                                offset = 1;
                            }
                            int startingX = globalX;
                            if (level == 1 && RhythmGenerator.constraints[4] == 1)
                            {
                                startingX = upGlobalX;
                            }
                            else if (RhythmGenerator.constraints[4] == 1)
                            {
                                startingX = lowGlobalX;
                            }
                            for (int k = startingX + 1; k < startingX + act.y + offset; k++)
                            {
                                ////Debug.Log("hI");
                                if (level == 0)
                                {
                                    lvl.Add(new Vector3(0, 0, -1));
                                    List<GameObject> l2 = new List<GameObject>();
                                    ground.Add(l2);
                                } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                                {
                                    upperLvl.Add(new Vector3(0, 0, -1));
                                    List<GameObject> l2 = new List<GameObject>();
                                    upperGround.Add(l2);
                                } else if (RhythmGenerator.constraints[4] == 1)
                                {
                                    lowerLvl.Add(new Vector3(0, 0, -1));
                                    List<GameObject> l2 = new List<GameObject>();
                                    lowerGround.Add(l2);
                                }
                            }

                            //if (RhythmGenerator.constraints[3] == 1)
                            //{
                            //    state.resetClock();
                            //    globalY -= 1;
                            //}
                        }
                        if (level == 0)
                        {
                            globalX += (int)(act.y + (offset - 1));
                        } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                        {
                            upGlobalX += (int)(act.y + (offset - 1));
                        } else if  (RhythmGenerator.constraints[4] == 1)
                        {
                            lowGlobalX += (int)(act.y + (offset - 1));
                        }

                    } else if (System.Math.Abs(act.x - 1) < 0.1)
                    {
                        //enemy

                        if (RhythmGenerator.constraints[3] == 1)
                        {
                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texSpike, new Rect(0.0f, 0.0f, texSpike.width, texSpike.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();

                            if (level == 0)
                            {
                                lvl.Add(new Vector3(globalX, globalY, 1));
                                cube.transform.position = new Vector3(globalX, globalY, 0);
                                spikes.Add(cube);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube);
                                ground.Add(l);
                            } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                            {
                                upperLvl.Add(new Vector3(upGlobalX, upGlobalY, 1));
                                cube.transform.position = new Vector3(upGlobalX, upGlobalY, 0);
                                upperSpikes.Add(cube);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube);
                                upperGround.Add(l);
                            } else if (RhythmGenerator.constraints[4] == 1)
                            {
                                lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY, 1));
                                cube.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);
                                lowerSpikes.Add(cube);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube);
                                lowerGround.Add(l);
                            }
                        } else
                        {
                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();

                            if (level == 0)
                            {
                                lvl.Add(new Vector3(globalX, globalY, 0));
                                cube.transform.position = new Vector3(globalX, globalY, 0);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube);
                                ground.Add(l);
                            } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                            {
                                upperLvl.Add(new Vector3(upGlobalX, upGlobalY, 0));
                                cube.transform.position = new Vector3(upGlobalX, upGlobalY, 0);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube);
                                upperGround.Add(l);
                            } else if (RhythmGenerator.constraints[4] == 1)
                            {
                                lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY, 0));
                                cube.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube);
                                lowerGround.Add(l);
                            }
                        }

                        GameObject enemy = new GameObject();
                        SpriteRenderer sr2 = enemy.AddComponent<SpriteRenderer>() as SpriteRenderer;
                        sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);

                        if (level == 0)
                        {
                            enemy.transform.position = new Vector3(globalX, globalY + 1f, 0);
                        } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                        {
                            enemy.transform.position = new Vector3(upGlobalX, upGlobalY + 1f, 0);
                            //Debug.Log("AM I UP HERE?? ");
                        } else if (RhythmGenerator.constraints[4] == 1)
                        {
                            enemy.transform.position = new Vector3(lowGlobalX, lowGlobalY + 1f, 0);
                            //Debug.Log("AM I DOWN HERE?? ");
                        }
                        

                        enemy.transform.localScale = blockScale;
                        Sprite mySprite2 = Sprite.Create(texEnemy, new Rect(0.0f, 0.0f, texEnemy.width, texEnemy.height),
                                 new Vector2(0.5f, 0.5f), 100.0f);
                        sr2.sprite = mySprite2;
                        enemy.AddComponent<BoxCollider2D>();
                        enemy.AddComponent<Rigidbody2D>();
                        enemy.GetComponent<Rigidbody2D>().isKinematic = true;
                        enemy.name = "" + enemyID;//enemy.GetComponent<BoxCollider2D>().isTrigger = true;
                        enemy.AddComponent<EnemyController>();
                        enemy.GetComponent<EnemyController>().texEnemy = texEnemy;
                        enemy.GetComponent<EnemyController>().texEnemy2 = texEnemy2;
                        enemy.GetComponent<EnemyController>().texEnemy3 = texEnemy3;
                        enemy.GetComponent<EnemyController>().texEnemy4 = texEnemy4;
                        enemyID += 1;
                        

                        ////Debug.Log("!!!!!ENEMY!!!!");

                        if (level == 0)
                        {
                            enemy.GetComponent<EnemyController>().level = 0;
                            enemies.Add(enemy);
                            globalX += 1;
                        }
                        else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                        {
                            enemy.GetComponent<EnemyController>().level = 1;
                            upperEnemies.Add(enemy);
                            upGlobalX += 1;
                        }
                        else if (RhythmGenerator.constraints[4] == 1)
                        {
                            enemy.GetComponent<EnemyController>().level = 2;
                            lowerEnemies.Add(enemy);
                            lowGlobalX += 1;
                        }
                    } else if (System.Math.Abs(act.x - 2) < 0.1)
                    {
                        if  (Random.value < 0.5 && RhythmGenerator.constraints[3] != 1)
                        {

                            // globalX + [X+1 - m] + [X + 2 - space] + newGlobalX
                            ////Debug.Log("!!!!!WAIT!!!!");
                            //WAIT - either a moving platform or stomper
                            //place a block and add a platformcontroller to it so it moves up and down

                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texGround, new Rect(0.0f, 0.0f, texGround.width, texGround.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;
                            cube.AddComponent<BoxCollider2D>();

                            GameObject cube2 = new GameObject();
                            SpriteRenderer sr2 = cube2.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube2.transform.localScale = blockScale;
                            Sprite mySprite2 = Sprite.Create(texMoveGnd, new Rect(0.0f, 0.0f, texMoveGnd.width, texMoveGnd.height),
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
                            

                            if (level == 0)
                            {
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube);
                                ground.Add(l);
                                lvl.Add(new Vector3(globalX, globalY, 0));
                                cube.transform.position = new Vector3(globalX, globalY, 0);
                                cube2.transform.position = new Vector3(globalX, globalY + 1, 0);
                                stompers.Add(cube2);
                            }
                            else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                            {
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube);
                                upperGround.Add(l);
                                upperLvl.Add(new Vector3(upGlobalX, upGlobalY, 0));
                                cube.transform.position = new Vector3(upGlobalX, upGlobalY, 0);
                                cube2.transform.position = new Vector3(upGlobalX, upGlobalY + 1, 0);
                                lowerStompers.Add(cube2);
                            }
                            else if (RhythmGenerator.constraints[4] == 1)
                            {
                                List<GameObject> l = new List<GameObject>();
                                l.Add(cube);
                                lowerGround.Add(l);
                                lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY, 0));
                                cube.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);
                                cube2.transform.position = new Vector3(lowGlobalX, lowGlobalY + 1, 0);
                                upperStompers.Add(cube2);
                            }

                            if (level == 0)
                            {
                                globalX += 1;
                            }
                            else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                            {
                                upGlobalX += 1;
                            }
                            else if (RhythmGenerator.constraints[4] == 1)
                            {
                                lowGlobalX += 1;
                            }

                        } else
                        {
                            state.resetClock();

                            if (level == 0)
                            {
                                lvl.Add(new Vector3(globalX, globalY, -1));
                                List<GameObject> l = new List<GameObject>();
                                ground.Add(l);
                            } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                            {
                                upperLvl.Add(new Vector3(upGlobalX, upGlobalY, -1));
                                List<GameObject> l = new List<GameObject>();
                                upperGround.Add(l);
                            } else if (RhythmGenerator.constraints[4] == 1)
                            {
                                lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY, -1));
                                List<GameObject> l = new List<GameObject>();
                                lowerGround.Add(l);
                            }
                            
                            GameObject cube = new GameObject();
                            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
                            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                            cube.transform.localScale = blockScale;
                            Sprite mySprite = Sprite.Create(texMoveGnd, new Rect(0.0f, 0.0f, texMoveGnd.width, texMoveGnd.height),
                                     new Vector2(0.5f, 0.5f), 100.0f);
                            sr.sprite = mySprite;

                            if (level == 0)
                            {
                                lvl.Add(new Vector3(globalX + 1, globalY, 2));
                                cube.transform.position = new Vector3(globalX + 1, globalY, 0);
                                List<GameObject> l2 = new List<GameObject>();
                                l2.Add(cube);
                                ground.Add(l2);
                            } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                            {
                                upperLvl.Add(new Vector3(upGlobalX + 1, upGlobalY, 2));
                                cube.transform.position = new Vector3(upGlobalX + 1, upGlobalY, 0);
                                List<GameObject> l2 = new List<GameObject>();
                                l2.Add(cube);
                                upperGround.Add(l2);
                            } else if (RhythmGenerator.constraints[4] == 1)
                            {
                                lowerLvl.Add(new Vector3(lowGlobalX + 1, lowGlobalY, 2));
                                cube.transform.position = new Vector3(lowGlobalX + 1, lowGlobalY, 0);
                                List<GameObject> l2 = new List<GameObject>();
                                l2.Add(cube);
                                lowerGround.Add(l2);
                            }

                            cube.AddComponent<BoxCollider2D>();
                            cube.AddComponent<Rigidbody2D>();
                            cube.name = "platform";
                            cube.AddComponent<PlatformController>();
                            cube.GetComponent<Rigidbody2D>().freezeRotation = true;
                            cube.GetComponent<Rigidbody2D>().isKinematic = false;
                            

                            if (level == 0)
                            {
                                lvl.Add(new Vector3(globalX + 2, globalY, -1));
                                List<GameObject> l = new List<GameObject>();
                                ground.Add(l);
                                platforms.Add(cube);
                            } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                            {
                                upperLvl.Add(new Vector3(upGlobalX + 2, upGlobalY, -1));
                                List<GameObject> l = new List<GameObject>();
                                upperGround.Add(l);
                                platforms.Add(cube);
                            } else if (RhythmGenerator.constraints[4] == 1)
                            {
                                lowerLvl.Add(new Vector3(lowGlobalX + 2, lowGlobalY, -1));
                                List<GameObject> l = new List<GameObject>();
                                lowerGround.Add(l);
                                platforms.Add(cube);
                            }
                            if (level == 0)
                            {
                                globalX += 3;
                            }
                            else if (level == 1 && RhythmGenerator.constraints[4] == 1)
                            {
                                upGlobalX += 3;
                            }
                            else if (RhythmGenerator.constraints[4] == 1)
                            {
                                lowGlobalX += 3;
                            }
                        }
                    }
                    action.RemoveAt(0);
                }
                
            }
        }

        //Break area:
        for (int k = 0; k < 3; k++)
        {
            GameObject cube = new GameObject();
            SpriteRenderer sr = cube.AddComponent<SpriteRenderer>() as SpriteRenderer;
            sr.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            cube.transform.localScale = blockScale;
            Sprite mySprite = Sprite.Create(texGnd, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                     new Vector2(0.5f, 0.5f), 100.0f);
            sr.sprite = mySprite;
            cube.AddComponent<BoxCollider2D>();

            if (level == 0)
            {
                lvl.Add(new Vector3(globalX, globalY, 0));
                cube.transform.position = new Vector3(globalX, globalY, 0);
                List<GameObject> l = new List<GameObject>();
                l.Add(cube);
                ground.Add(l);
            } else if (level == 1 && RhythmGenerator.constraints[4] == 1)
            {
                upperLvl.Add(new Vector3(upGlobalX, upGlobalY, 0));
                cube.transform.position = new Vector3(upGlobalX, upGlobalY, 0);
                List<GameObject> l = new List<GameObject>();
                l.Add(cube);
                upperGround.Add(l);
            } else if (RhythmGenerator.constraints[4] == 1)
            {
                lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY, 0));
                cube.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);
                List<GameObject> l = new List<GameObject>();
                l.Add(cube);
                lowerGround.Add(l);
            }
            
            
            if (level == 0)
            {
                globalX += 1;
            }
            else if (level == 1 && RhythmGenerator.constraints[4] == 1)
            {
                upGlobalX += 1;
            }
            else if (RhythmGenerator.constraints[4] == 1)
            {
                lowGlobalX += 1;
            }
        }

        lvlRange.y = globalX;
        upRange.y = upGlobalX;
        lowRange.y = lowGlobalX;

        lvlRanges.Add(lvlRange);
        lvlRanges.Add(upRange);
        lvlRanges.Add(lowRange);

        GameObject lastCube = new GameObject();
        SpriteRenderer lastSR = lastCube.AddComponent<SpriteRenderer>() as SpriteRenderer;
        lastSR.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
        lastCube.transform.localScale = blockScale;
        Sprite lastMySprite = Sprite.Create(texGnd, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                 new Vector2(0.5f, 0.5f), 100.0f);
        lastSR.sprite = lastMySprite;
        lastCube.AddComponent<BoxCollider2D>();

        if (level == 0)
        {
            lvl.Add(new Vector3(globalX, globalY, 0));
            lastCube.transform.position = new Vector3(globalX, globalY, 0);
        }
        //else if (level == 1 && RhythmGenerator.constraints[4] == 1)
        //{
        //    //Debug.Log("Hello?????");
        //    upperLvl.Add(new Vector3(upGlobalX, upGlobalY, 0));
        //    lastCube.transform.position = new Vector3(upGlobalX, upGlobalY, 0);
        //} else if (RhythmGenerator.constraints[4] == 1)
        //{
        //    //Debug.Log("Halllo?????");
        //    lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY, 0));
        //    lastCube.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);
        //}
        
        List<GameObject> last = new List<GameObject>();
        last.Add(lastCube);
        ground.Add(last);

        //add goal sprite at last block!
        GameObject cube3 = new GameObject();
        SpriteRenderer sr3 = cube3.AddComponent<SpriteRenderer>() as SpriteRenderer;
        sr3.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
        cube3.transform.position = new Vector3(globalX, globalY + 1.5f, 0);
        cube3.transform.localScale = blockScale;
        Sprite mySprite3 = Sprite.Create(texGoal, new Rect(0.0f, 0.0f, texGoal.width, texGoal.height),
                 new Vector2(0.5f, 0.5f), 100.0f);
        sr3.sprite = mySprite3;
        cube3.AddComponent<BoxCollider2D>();
        last.Add(cube3);

        

        //add stars to the end of upper and lower!!
        if (lowerLvl.Count > 0)
        {
            //lowerGround.Add(last);
            GameObject lastCubeL = new GameObject();
            SpriteRenderer lastSRL = lastCubeL.AddComponent<SpriteRenderer>() as SpriteRenderer;
            lastSRL.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            lastCubeL.transform.localScale = blockScale;
            Sprite lastMySpriteL = Sprite.Create(texGnd, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                     new Vector2(0.5f, 0.5f), 100.0f);
            lastSRL.sprite = lastMySpriteL;
            lastCubeL.AddComponent<BoxCollider2D>();
            lastCubeL.transform.position = new Vector3(lowGlobalX, lowGlobalY, 0);

            GameObject star = new GameObject();
            SpriteRenderer starSR = star.AddComponent<SpriteRenderer>() as SpriteRenderer;
            starSR.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            star.transform.localScale = blockScale;
            Sprite starSprite = Sprite.Create(texCoin, new Rect(0.0f, 0.0f, texCoin.width, texCoin.height),
                     new Vector2(0.5f, 0.5f), 100.0f);
            starSR.sprite = starSprite;
            star.AddComponent<BoxCollider2D>();
            star.transform.position = new Vector3(lowGlobalX, lowGlobalY + 1, 0);
            stars.Add(star);
            //lowerLvl.Add(new Vector3(lowGlobalX, lowGlobalY + 1, 5));
        }

        if (upperLvl.Count > 0)
        {
            GameObject lastCubeL = new GameObject();
            SpriteRenderer lastSRL = lastCubeL.AddComponent<SpriteRenderer>() as SpriteRenderer;
            lastSRL.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            lastCubeL.transform.localScale = blockScale;
            Sprite lastMySpriteL = Sprite.Create(texGnd, new Rect(0.0f, 0.0f, texGnd.width, texGnd.height),
                     new Vector2(0.5f, 0.5f), 100.0f);
            lastSRL.sprite = lastMySpriteL;
            lastCubeL.AddComponent<BoxCollider2D>();
            lastCubeL.transform.position = new Vector3(upGlobalX, upGlobalY, 0);

            GameObject star2 = new GameObject();
            SpriteRenderer starSR2 = star2.AddComponent<SpriteRenderer>() as SpriteRenderer;
            starSR2.color = new Color(0.9f, 0.9f, 0.9f, 1.0f);
            star2.transform.localScale = blockScale;
            Sprite starSprite2 = Sprite.Create(texCoin, new Rect(0.0f, 0.0f, texCoin.width, texCoin.height),
                     new Vector2(0.5f, 0.5f), 100.0f);
            starSR2.sprite = starSprite2;
            star2.AddComponent<BoxCollider2D>();
            star2.transform.position = new Vector3(upGlobalX, upGlobalY + 1, 0);
            stars.Add(star2);

            List<GameObject> lastU = new List<GameObject>();
            lastU.Add(lastCubeL);
            upperGround.Add(lastU);

            lastU.Add(star2);

        }

        //add impenetrable invisible wall to keep players from falling off the map
        lvl.Add(new Vector3(globalX, globalY + 3, 0));
        lvl.Add(new Vector3(globalX, globalY + 4, 0));

        ////Debug.Log("ENDX: " + globalX);
        lvl.Add(new Vector3(0, 0, -1));
        List<GameObject> l3 = new List<GameObject>();
        ground.Add(l3);
        ground.Add(l3);
        ground.Add(l3);

        //Debug.Log("LVL RANGES: " + lvlRange);
        //Debug.Log("          : " + upRange);
        //Debug.Log("          : " + lowRange);

        //addCoins();
    }
}

//state machine for flat (0), slantUp (1), and slantDown(2) platforms
public class StateMachine : MonoBehaviour
{
    public int curr = 0;
    private int stateTimer = 0;
    private int last = -1;
    bool first;

    public StateMachine()
    {
        first = true;
    }

    public int getTime()
    {
        return stateTimer;
    }

    public void resetClock()
    {
        stateTimer = 0;
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
        if (RhythmGenerator.constraints[3] == 1 && stateTimer < 4)
        {
            stateTimer += 1;
            curr = 0;
            last = curr;
            return curr;
        }
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
        if (RhythmGenerator.constraints[3] == 1)
        {
            if (last != curr)
            {
                last = curr;
                stateTimer = 0;
            }
        }
        return curr;
    }
}
