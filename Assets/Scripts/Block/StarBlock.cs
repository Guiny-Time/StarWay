using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StarBlock : MonoBehaviour
{
    public Material magicMat;   // 魔法条
    public UnityEvent e;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
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
        // birdge.GetComponent<Animator>().Play("Fix");
    }
}
