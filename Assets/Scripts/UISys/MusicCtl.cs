using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicCtl : MonoBehaviour
{
    private GameObject[] audioSource;
    // 场景名
    private string sceneName;
    private void Awake()
    {
        audioSource = GameObject.FindGameObjectsWithTag("Music");
        if (audioSource.Length>1)
        {
            Destroy(this.gameObject);
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == sceneName)
        {
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
            sceneName = SceneManager.GetActiveScene().name;
        }
    }
}
