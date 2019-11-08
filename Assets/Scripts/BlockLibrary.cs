using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum rType { regular, random, swing, test }

public interface Block
{
    int[] getRhythmArray();
    List<Vector2> getActionArray();
}

public class RhythmBlock : MonoBehaviour, Block
{
    private int[] rhythm;
    private List<Vector2> action;
    private bool generateEnemies;
    private float enemyThreshold = 0.5f;

    public RhythmBlock(rType type, int length, int density, bool genEn, int offset)
    {
        generateEnemies = genEn;
        if (genEn)
        {
            enemyThreshold = 0.65f;
        }
        rhythm = new int[length];
        //Debug.Log(length + " " + density);
        int mult = length / density + offset;
        switch (type)
        {
            case rType.regular:
                for (int i = 1; i < length; i += mult)
                {
                    rhythm[i] = 1;
                }
                break;
            case rType.random:
                for (int i = 1; i < length; i++)
                {
                    if (rhythm[i - 1] == 1)
                    {
                        continue;
                    }
                    if (Random.value < 0.33)
                    {
                        rhythm[i] = 1;
                    }
                }
                break;
            case rType.swing:
                for (int i = 1; i < length; i += mult)
                {
                    rhythm[i] = 1;
                    if ((i + 2 + offset) < length)
                    {
                        rhythm[i + 2 + offset] = 1;
                    }
                }
                break;
            case rType.test:
                break;
        }
        string otp = "Rhythm Array: [";
        for (int i = 0; i < length; i++)
        {
            otp += rhythm[i] + ", ";
        }
        otp += "]";
        Debug.Log(otp);

        loadActionBlock();
    }

    public List<Vector2> getActionArray()
    {
        return action;
    }

    public int[] getRhythmArray()
    {
        return rhythm;
    }

    //need to add either cliff or enemy for every one I find in rhythm
    //ensure it doesn't clash with another action
    //0 - cliff, 1 - enemy, 2 - wait
    //each array index is a second.
    //short = 1 spot (0 diff)
    //medium = 2 spots (1 diff)
    //long = 4 spots (3 diff)
    //enemy = 2 spots
    public void loadActionBlock()
    {
        action = new List<Vector2>();
        int prevAction = 0;
        int currAction = 0;
        int duration = 0;
        int type = 0;
        Vector2 act = new Vector2();
        bool first = true;

        for (int i = currAction; i < rhythm.Length; i++)
        {
            if (rhythm[i] == 1 && first)
            {
                prevAction = i;
                currAction = prevAction;
                first = false;
            }
            else if (rhythm[i] == 1)
            {
                prevAction = currAction;
                currAction = i;
                //ensures duration is no bigger than the distance between actions
                if (generateEnemies)
                {
                    if (Random.value < enemyThreshold && (currAction - prevAction) > 0)
                    {
                        duration = 1;
                    }
                    else
                    {
                        duration = (int)(Random.value * (currAction - prevAction));
                    }
                } else {
                    duration = (int)(Random.value * (currAction - prevAction));
                }

                if (Random.value < enemyThreshold && generateEnemies)
                {
                    type = 1;
                }
                else
                {
                    type = (int)(Random.value * 3);//SHOULD BE 3!! TEST
                }
                 
                if (type == 1 && duration > 0)
                {
                    act = new Vector2(type, 1);
                }
                else
                {
                    if (duration == 0)
                    {
                        if (Random.value < 0.5f)
                        {
                            act = new Vector2(0, Mathf.Min(duration, 2));
                        } else
                        {
                            act = new Vector2(2, Mathf.Min(duration, 2));//TESTINGact = new Vector2(2, Mathf.Min(duration, 3));
                        }
                    }
                    else
                    {
                        act = new Vector2(type, Mathf.Min(duration, 2));
                    }
                }
                action.Add(act);
            }
        }
        duration = (int)(Random.value * (rhythm.Length - 1 - currAction));
        type = (int)(Random.value * 2);
        if (type == 1 && duration > 0)
        {
            act = new Vector2(type, 1);
        }
        else
        {
            if (duration == 0)
            {
                act = new Vector2(0, Mathf.Min(duration, 2));
            }
            else
            {
                act = new Vector2(type, Mathf.Min(duration, 2));
            }
        }
        action.Add(act);

        string otp = "Action Array: [";
        for (int i = 0; i < action.Count; i++)
        {
            otp += action[i] + ", ";
        }
        otp += "]";
        Debug.Log(otp);
    }
}
