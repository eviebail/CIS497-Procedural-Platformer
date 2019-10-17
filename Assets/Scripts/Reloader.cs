using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reloader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GeometryGenerator.reset();
        PlayerController.reset();
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
}
