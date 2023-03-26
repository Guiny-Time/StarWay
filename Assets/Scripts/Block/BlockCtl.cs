using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 普通地块
/// </summary>
public class BlockCtl : MonoBehaviour
{
    // 地块状态
    private int state;
    private double offset;
    // Start is called before the first frame update
    void Start()
    {
        state = AStarMgr.GetInstance().GetBlockState(transform.position.x, transform.position.z);
        offset = BlockMgr.GetInstance().NextDouble(0,1);
        InitBlocks();
    }


    void InitBlocks()
    {
        
        Vector3 oldPos = transform.position;
        // 可通行地面
        if (state == 1)
        {
            transform.position = new Vector3(oldPos.x, oldPos.y - (float)offset * 0.1f, oldPos.z);
        }
        else
        {
            transform.position = new Vector3(oldPos.x, oldPos.y + (float)offset * 0.5f, oldPos.z);
        }
    }
}
