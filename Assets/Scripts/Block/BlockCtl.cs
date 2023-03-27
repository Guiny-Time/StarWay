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
        // 可通行地面
        if (state == 1)
        {
            transform.position -= new Vector3(0, (float)offset * 0.2f, 0);
        }
        else
        {
            transform.position += new Vector3(0, (float)offset * 0.5f, 0);
        }
    }
    
    public int GetState()
    {
        return state;
    }

    public void SetState(int s)
    {
        state = s;
    }
}
