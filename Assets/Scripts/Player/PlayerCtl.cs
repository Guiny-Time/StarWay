using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCtl : MonoBehaviour
{
    public int speed;
    public int magic;
    public LineRenderer lr;

    
    private List<AStarNode> result = new List<AStarNode>();
    private List<AStarNode> temp_result = new List<AStarNode>();
    private GameObject highlightBlocks = null;
    private bool moveState = false; //false: stand; true: moving
    private int stepCount = 1;  // foot step
    private Vector2 temp;
    private GameObject pos; // mouse choose obj
    private bool inMagic;   // whether in magic state

    
    private void Awake()
    {
        PlayerMgr.GetInstance().SetMagic(magic);
        PlayerMgr.GetInstance().SetSpeed(speed);
    }

    // Start is called before the first frame update
    void Start()
    {
        inMagic = false;
    }

    // Update is called once per frame
    void Update()
    {
        ChooseBlock();
        if (pos != null)
        {
            result = AStarMgr.GetInstance().FindPath(new Vector2( Mathf.Round(transform.position.z), Mathf.Round(transform.position.x)),
                new Vector2(pos.transform.position.z,pos.transform.position.x));
        }
        
        if(moveState) { Moving(); }     // moving

        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("开始使用重力魔法");
            inMagic = true;
            GravityMagic();
        }

        if (inMagic)
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
        else
        {
            DrawLine();
            if(Input.GetMouseButtonDown(0))
            {
                if (result.Count != 0)
                {
                    temp_result = result;
                    stepCount = 0; 
                    moveState = true;
                }
            }
        }
    }

    /// <summary>
    /// 使用重力魔法
    /// </summary>
    void GravityMagic()
    {
        pos = InputMgr.GetInstance().GetCurrentMouse();
        // clear highlight
        if (highlightBlocks != null)
        {
            highlightBlocks.GetComponent<Outline>().enabled = false;
        }
        highlightBlocks = null;
        lr.positionCount = 0;
        
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
    /// 选择目标
    /// </summary>
    void ChooseBlock()
    {
        pos = InputMgr.GetInstance().GetCurrentMouse();
        try
        {
            if (result == null && !inMagic)
            {
                highlightBlocks.GetComponent<Outline>().enabled = false;
                highlightBlocks = null;
            }
            else
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
        catch (Exception e)
        {
            return;
        }
    }

    /// <summary>
    /// 画线，用于展示自动寻路路线
    /// </summary>
    void DrawLine()
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

    void Moving()
    {
        Vector3 NextPos = new Vector3(temp_result[stepCount].y, 0.25f, temp_result[stepCount].x);
        if(transform.position == NextPos){
            stepCount++;
            if(stepCount + 1 > temp_result.Count)
            {
                moveState = false;
            }
        }
        else
        {
            // transform.rotation = Quaternion.LookRotation(NextPos - transform.position, Vector3.forward); //转向
            transform.position = Vector3.MoveTowards(transform.position, NextPos, speed * Time.deltaTime);
        }
        

    }
}
