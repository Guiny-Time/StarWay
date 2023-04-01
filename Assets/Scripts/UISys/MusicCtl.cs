using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicCtl : MonoBehaviour
{
    private GameObject[] audioSource;
    private void Awake()
    {
        audioSource = GameObject.FindGameObjectsWithTag("Music");
        if (audioSource.Length>1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
