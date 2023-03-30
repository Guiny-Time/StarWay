using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class InputMgr : SingletonMono<InputMgr>
{
    public GameObject pause1;
    public GameObject pause2;
    public Animator chapter;
    public LineRenderer lr;
    
    protected Vector3 mousePosition;
    private Ray ray;
    private RaycastHit hit;
    private GameObject mapCollider;
    private GameObject pos; // mouse choose obj
    private GameObject highlightBlocks = null;
    private bool inMagic;   // whether in magic state

    private void Start()
    {
        pause1.gameObject.SetActive(false);
        pause2.gameObject.SetActive(false);
        chapter.Play("OpenChapter");
        inMagic = false;
    }

    public void UIListener(List<AStarNode> result)
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 弹出UI
            Debug.Log("弹出设置界面");
            // Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 暂停
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                pause1.gameObject.SetActive(true);
                pause2.gameObject.SetActive(true);
            }
            else
            {
                Time.timeScale = 1;
                pause1.gameObject.SetActive(false);
                pause2.gameObject.SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
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
            print("对该方块施展重力魔法：" + pos.name);
            GameObject block = InputMgr.GetInstance().GetCurrentMouse();
            BlockCtl bCtl = block.GetComponent<BlockCtl>();
            int blockState = bCtl.GetState();
            // gravity magic
            BlockMgr.GetInstance().UseGravityMagic(block, blockState);
            bCtl.SetState(1^blockState);
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
        pos = GetCurrentMouse();
        // clear highlight
        if (highlightBlocks != null)
        {
            highlightBlocks.GetComponent<Outline>().enabled = false;
        }
        highlightBlocks = null;

        if (pos.GetComponent<Outline>())
        {
            if (highlightBlocks != null && pos.name != highlightBlocks.name)
            {
                highlightBlocks.GetComponent<Outline>().enabled = false;
                highlightBlocks = pos;
                highlightBlocks.GetComponent<Outline>().enabled = true;
            }
            else
            {
                highlightBlocks = pos;
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
