using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reloader : MonoBehaviour
{
    public static bool win = false;
    // Start is called before the first frame update
    void Start()
    {
        GeometryGenerator.reset();
        PlayerController.reset();
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        Debug.Log("IWON! " + win);
        yield return new WaitForSeconds(3);
        if (win)
        {
            win = false;
            SceneManager.LoadScene("Start", LoadSceneMode.Single);
        } else
        {
            SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
        }
        
    }
}
