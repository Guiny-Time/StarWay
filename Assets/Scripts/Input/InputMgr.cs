using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Events;
using UnityEngine.UI;
using Object = System.Object;

public class InputMgr : SingletonMono<InputMgr>
{
    public GameObject[] pause;
    public Animator chapter;
    public Material magicMat;
    public LineRenderer lr;
    public UnityEvent<String> e;
    
    private Ray ray;
    private RaycastHit hit;
    private GameObject mapCollider;
    private GameObject chooseObj; // mouse choose obj
    private GameObject highlightBlocks = null;
    
    private bool inMagic;   // whether in magic state
    private Vector2 pl_p;   // player position
    private Vector2 bl_p;   // block position
    private float d;    // distance between p and b

    private void OnEnable()
    {
        pause = GameObject.FindGameObjectsWithTag("pause");
        chapter = GameObject.FindWithTag("AnimCanvas").GetComponent<Animator>();
        foreach (var o in pause)
        {
            o.gameObject.SetActive(false);
        }
        chapter.Play("OpenChapter");
        magicMat.SetFloat("_Magic", 0);
        inMagic = false;
    }

    private void Start()
    {
        d = PlayerMgr.GetInstance().GetArea() * Mathf.Sqrt(2);
    }

    public void UIListener()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 弹出UI
            Debug.Log("弹出设置界面");
            UICtl.GetInstance().ShowPanel();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 暂停
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                foreach (var o in pause)
                {
                    o.gameObject.SetActive(true);
                }
            }
            else
            {
                Time.timeScale = 1;
                foreach (var o in pause)
                {
                    o.gameObject.SetActive(false);
                }
            }
        }

        // 使用重力魔法
        if (Input.GetKeyDown(KeyCode.E) && magicMat.GetFloat("_Magic") > -0.6) 
        {
            if (!inMagic)
            {
                print("1、开始使用重力魔法 in InputMgr.cs");
                lr.positionCount = 0;
                GravityMagic();
            }
            else
            {
                print("退出施法状态");
                inMagic = false;
            }
            
        }

    }

    public bool DetermineDistance()
    {
        GameObject block = GetCurrentMouse();
        bl_p = new Vector2(block.transform.position.x, block.transform.position.z);
        Transform pl_t = GameObject.Find("Orion").transform;
        pl_p = new Vector2(pl_t.position.x, pl_t.position.z);
        foreach (var points in PlayerMgr.GetInstance().GetForbidPoints())   // 禁止列表
        {
            if (bl_p == points)
            {
                return false;
            }
        }
        if (Vector2.Distance(pl_p, bl_p) > d)   // 超出距离
        {
            return false;   // 不显示方块
        }

        if (bl_p.x == Mathf.Round(pl_p.x) && bl_p.y == Mathf.Round(pl_p.y) )   // 玩家所处位置
        {
            return false;
        }
        
        return true;
    }

    /// <summary>
    /// 获取当前player是否处于施法状态
    /// </summary>
    /// <returns></returns>
    public bool GetMagicState()
    {
        return inMagic;
    }

    public void SetArea(int distance)
    {
        
    }

    /// <summary>
    /// 施法状态下的输入控制
    /// </summary>
    public void InMagic()
    {
        // 右键退出施法状态
        if (Input.GetMouseButtonDown(1))
        {
            print("退出施法状态");
            inMagic = false;
        }

        // 超出距离不允许施法
        if (!DetermineDistance())
        {
            return;
        }
        
        // 合法条件，开始施法
        if(Input.GetMouseButtonDown(0))
        {
            GameObject block = GetCurrentMouse();   // 施法对象
            // if(Vector3.Distance(block,))
            print("3、对该方块施展重力魔法：" + block.name + "in InputMgr.cs");
            BlockCtl bCtl = block.GetComponent<BlockCtl>();
            int blockState = bCtl.GetState();
            
            if (blockState == 0)    // 原本不可通行,施法后变成可通行方块
            {
                print("4、原本不可通行,施法后变成可通行方块 in InputMgr.cs");
                e.Invoke(block.name);
                BlockMgr.GetInstance().UseGravityMagic(block, blockState);
                bCtl.SetState(1);
            }
            else  // 原本可通行,施法后变成不可通行方块
            {
                print("4、原本可通行,施法后变成不可通行方块 in InputMgr.cs");
                e.Invoke(block.name);
                bCtl.SetState(0);
            }

            float magicCost = (float)0.6 / PlayerMgr.GetInstance().GetMagic();
            magicMat.SetFloat("_Magic", magicMat.GetFloat("_Magic") - magicCost);
            inMagic = false;
        }
    }

    public GameObject GetCurrentMouse()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        // 仅检测地块层(Ground)
        int layerMask = 1 << 3;
        if(Physics.Raycast(ray, out hitData, 1000, layerMask))
        {
            mapCollider = hitData.collider.transform.gameObject;
        }
        return mapCollider;
    }
    
    /// <summary>
    /// 使用重力魔法
    /// </summary>
    void GravityMagic()
    {
        inMagic = true;
        print("2、Gravity Magic in InputMgr.cs");
        chooseObj = GetCurrentMouse();
        // clear highlight
        if (highlightBlocks != null)
        {
            highlightBlocks.GetComponent<Outline>().enabled = false;
        }
        highlightBlocks = null;

        if (chooseObj.GetComponent<Outline>())
        {
            if (highlightBlocks && chooseObj.name != highlightBlocks.name)
            {
                highlightBlocks.GetComponent<Outline>().enabled = false;
                highlightBlocks = chooseObj;
                highlightBlocks.GetComponent<Outline>().enabled = true;
            }
            else
            {
                highlightBlocks = chooseObj;
                highlightBlocks.GetComponent<Outline>().enabled = true;
            }
        }
    }
    
    /// <summary>
    /// 画线，用于展示自动寻路路线
    /// </summary>
    public void DrawLine(List<AStarNode> result)
    {
        if (result == null)
        {
            lr.positionCount = 0;
        }
        try
        {
            lr.positionCount = result.Count;
            for (int i = 0; i < result.Count; i++)
            {
                lr.SetPosition(i, new Vector3(result[i].y, 0, result[i].x));
            }
        }
        catch (Exception e)
        {
            lr.positionCount = 0;
        }
        
    }
}
