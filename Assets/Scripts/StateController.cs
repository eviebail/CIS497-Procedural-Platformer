using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/*
 *Constraint ideas:
 * Jumping is for whimps! Complete the level without jumping
 * Genocide Route! Defeat all enemies before finishing level
 * No Damage! Beat the level without getting hurt
 * Difficult ones:
 * The floor is spikes! Spikes replace blocks, but need moving blocks and enemies to make it thru?
 * Find all stars! Place stars throughout level - implement branching paths [caves, clouds, bouncy things]
 */

public class StateController : MonoBehaviour
{
	Dropdown drop;
	Button generate;
    Toggle t0, t1, t2, t3, t4, t5, t6;
    Text constraints;
	public static int option = 0;
    string otp = "";
	// Start is called before the first frame update
	void Start()
	{
		drop = transform.GetChild(1).GetComponent<Dropdown>();
		drop.onValueChanged.AddListener(delegate {
			DropdownValueChanged(drop);
		});
		generate = transform.GetChild(2).GetComponent<Button>();

        t0 = transform.GetChild(4).GetComponent<Toggle>();
        t1 = transform.GetChild(5).GetComponent<Toggle>();
        t2 = transform.GetChild(6).GetComponent<Toggle>();
        t3 = transform.GetChild(7).GetComponent<Toggle>();
        t4 = transform.GetChild(8).GetComponent<Toggle>();
        t5 = transform.GetChild(9).GetComponent<Toggle>();
        t6 = transform.GetChild(10).GetComponent<Toggle>();

        constraints = transform.GetChild(11).GetComponent<Text>();

        generate.onClick.AddListener(TaskOnClick);
	}

	// Update is called once per frame
	void Update()
	{
        otp = "Active Constraints:\n";
        if (t0.isOn)
        {
            otp += "-Complete the level without jumping\n";
            RhythmGenerator.constraints[0] = 1;
        } else
        {
            RhythmGenerator.constraints[0] = 0;
        }
        if (t1.isOn)
        {
            otp += "-Defeat all enemies before finishing level\n";
            RhythmGenerator.constraints[1] = 1;
        }
        else
        {
            RhythmGenerator.constraints[1] = 0;
        }
        if (t2.isOn)
        {
            otp += "-Beat the level without getting hurt\n";
            RhythmGenerator.constraints[2] = 1;
        }
        else
        {
            RhythmGenerator.constraints[2] = 0;
        }
        if (t3.isOn)
        {
            otp += "-It's like the floor is lava, but with spikes\n";
            RhythmGenerator.constraints[3] = 1;
        }
        else
        {
            RhythmGenerator.constraints[3] = 0;
        }
        if (t4.isOn)
        {
            otp += "-Find the elusive stars before you beat the level\n";
            RhythmGenerator.constraints[4] = 1;
        }
        else
        {
            RhythmGenerator.constraints[4] = 0;
        }
        if (t5.isOn)
        {
            otp += "-Complete the level without looking left\n";
            RhythmGenerator.constraints[5] = 1;
        }
        else
        {
            RhythmGenerator.constraints[5] = 0;
        }
        if (t6.isOn)
        {
            otp += "-Complete the level before the time runs out\n";
            RhythmGenerator.constraints[6] = 1;
        }
        else
        {
            RhythmGenerator.constraints[6] = 0;
        }
        constraints.text = otp;
    }

	void TaskOnClick()
	{
		//Output this to console when Button1 or Button3 is clicked
		Debug.Log("Start the level!");
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
	}

	/*
     *EASY - shorter rhythms. less dense blocks. regular and swing. no random
     *MEDIUM - generation as it is
     *HARD - longer rhythms, and more dense blocks. bias towards random
     */
	void DropdownValueChanged(Dropdown change)
	{
		//m_Text.text = "New Value : " + change.value;
		if (change.value == 0)
		{
			Debug.Log("EASY");
			option = 0;
		}
		else if (change.value == 1)
		{
			Debug.Log("MEDIUM");
			option = 1;
		}
		else
		{
			Debug.Log("HARD");
			option = 2;
		}
	}
}
