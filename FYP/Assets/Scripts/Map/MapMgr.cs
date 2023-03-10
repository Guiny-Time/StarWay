using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapMgr : MonoBehaviour
{
    public int mapWidth = 1;
    public int mapHeight = 1;
    public AStarMgr.mapInfoWidth[] info = new AStarMgr.mapInfoWidth[8];
    private List<AStarNode> result = new List<AStarNode>();

    // Start is called before the first frame update
    void Start()
    {
        AStarMgr AStar = new AStarMgr();
        AStar.InitMapInfo(info,mapWidth,mapHeight);
        result = AStar.FindPath(new Vector2(0, 13), new Vector2(6, 1));
        for (int i = 0; i < result.Count; i++)
        {
            print("第" + i + "个点z坐标：" + result.ElementAt(i).x);
            print("第" + i + "个点x坐标：" + result.ElementAt(i).y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
