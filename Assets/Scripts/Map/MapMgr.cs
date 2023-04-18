using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class MapMgr : MonoBehaviour
{
    public int chapter;
    public int level;
    public int mapWidth = 1;
    public int mapHeight = 1;
    public int collectionNum = 0;
    public GameObject door;
    public AStarMgr.mapInfoWidth[] info = new AStarMgr.mapInfoWidth[8];

    private void Awake()
    {
        AStarMgr.GetInstance().InitMapInfo(info,mapWidth,mapHeight);
        SaveMgr.GetInstance().Save(chapter, level);
        PlayerPrefs.SetInt("CollectionNum", 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    } 

    // Update is called once per frame
    void Update()
    {
        if (PlayerPrefs.GetInt("CollectionNum") == collectionNum)
        {
            door.gameObject.SetActive(true);
        }
    }

}
