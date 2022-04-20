using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preface : MonoBehaviour
{
    public float delay;
    
    void Start()
    {
        StartCoroutine(TimeDown());
    }

    IEnumerator TimeDown()
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Title Screen");
    }
}
