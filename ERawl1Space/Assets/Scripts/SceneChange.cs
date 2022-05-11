using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneChange : MonoBehaviour
{
    public float timerBeforeChange = 10f;
    public string sceneName;
    private float timePassed;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed > timerBeforeChange)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
