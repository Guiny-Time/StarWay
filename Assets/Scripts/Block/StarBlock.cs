using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StarBlock : MonoBehaviour
{
    public Material magicMat;   // 魔法条
    public UnityEvent e;

    //触发开始 只调用一次
    public void OnTriggerEnter(Collider collider){
        print(collider.name);
        e.Invoke();
        PlayerPrefs.SetInt("CollectionNum", PlayerPrefs.GetInt("CollectionNum") + 1);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 白色星光事件：回满魔法槽
    /// </summary>
    public void WhiteStar()
    {
        PlayerMgr.GetInstance().SetMagic(2);
        magicMat.SetFloat("_Magic", 0);
    }

    /// <summary>
    /// 红色星光事件：修复断桥
    /// </summary>
    /// <param name="birdge"></param>
    public void RedStar(GameObject birdge)
    {
        Vector3 pos = birdge.transform.position;
        BlockCtl bCtl = birdge.GetComponent<BlockCtl>();
        birdge.GetComponent<Animator>().Play("fix");
        // 修改状态为可通行
        Debug.Log("修改断桥状态");
        BlockMgr.GetInstance().UseGravityMagic(birdge, 0);
        bCtl.SetState(1);
    }

    /// <summary>
    /// 蓝色星光事件：陨星术，砸死一个enemy
    /// </summary>
    public void BlueStar()
    {
        // 
    }
}
