using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMgr : MonoBehaviour
{
    public int mapWidth = 1;
    public int mapHeight = 1;
    public AStarMgr.mapInfoWidth[] info = new AStarMgr.mapInfoWidth[8];

    private void Awake()
    {
        AStarMgr.GetInstance().InitMapInfo(info,mapWidth,mapHeight);
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
