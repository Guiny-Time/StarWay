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

    private Vector3 mousePosition;
    private Ray ray;
    private RaycastHit hit;
    private GameObject mapCollider;
    private GameObject chooseObj; // mouse choose obj
    private GameObject highlightBlocks = null;
    private bool inMagic;   // whether in magic state

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

    public void UIListener(List<AStarNode> result)
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

        if (Input.GetKeyDown(KeyCode.E) && magicMat.GetFloat("_Magic") > -0.6)
        {
            if (!inMagic)
            {
                print("开始使用重力魔法");
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


    public bool GetMagicState()
    {
        return inMagic;
    }

    public void InMagic()
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            GameObject block = GetCurrentMouse();
            print("对该方块施展重力魔法：" + block.name);
            BlockCtl bCtl = block.GetComponent<BlockCtl>();
            int blockState = bCtl.GetState();
            if (blockState == 0)    //原本不可通行
            {
                e.Invoke(block.name);
                BlockMgr.GetInstance().UseGravityMagic(block, blockState);
                bCtl.SetState(1);
            }
            else
            {
                e.Invoke(block.name);
                bCtl.SetState(0);
            }
            magicMat.SetFloat("_Magic", magicMat.GetFloat("_Magic") - 0.3f);
            inMagic = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            print("退出施法状态");
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
            mousePosition = hitData.point;
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
