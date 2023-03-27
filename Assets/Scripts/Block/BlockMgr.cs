using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// 地块管理器
/// </summary>
public class BlockMgr : BaseManager<BlockMgr>
{
    private System.Random random;
    private double mean;
    private double stdDev;


    /// <summary>
    /// 生成随机数
    /// </summary>
    /// <param name="mean"></param>
    /// <param name="stdDev"></param>
    /// <returns></returns>
    public double NextDouble(double mean, double stdDev)
    {
        random = new System.Random();
        this.mean = mean;
        this.stdDev = stdDev;
        double u1 = 1.0 - random.NextDouble();
        double u2 = 1.0 - random.NextDouble();
        double randStdNormal = Math.Sqrt(-0.5 * Math.Log(u1)) * Math.Sin(0.5 * Math.PI * u2);
        return mean + stdDev * randStdNormal;
    }



    /// <summary>
    /// 重力魔法修改当前方块的状态，并移动方块纵向位置
    /// </summary>
    public void UseGravityMagic(GameObject block, int state)
    {
        if (state == 1)
        {
            block.transform.position += new Vector3(0,1,0);
            AStarMgr.GetInstance().ChangeBlockState(block.transform.position.x, block.transform.position.z,0);
        }
        else
        {
            block.transform.position -= new Vector3(0,1,0);
            AStarMgr.GetInstance().ChangeBlockState(block.transform.position.x, block.transform.position.z,1);
        }
    }
}
