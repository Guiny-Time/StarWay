using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLogic : MonoBehaviour
{
    public GameObject root;

    private void Awake()
    {
        print(PlayerPrefs.GetString("Progress"));
        GameObject map = (GameObject)Resources.Load("Prefab/" + PlayerPrefs.GetString("Progress"));
        GameObject.Instantiate(map, root.transform);
        // SaveMgr.GetInstance().InitChapNames();
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
