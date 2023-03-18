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

    private GameObject pos;
    private List<AStarNode> result = new List<AStarNode>();
    private List<AStarNode> temp_result = new List<AStarNode>();
    private GameObject highlightBlocks = null;
    private bool moveState = false; //false: stand; true: moving
    private int stepCount = 1;  // foot step
    private Vector2 temp;

    
    private void Awake()
    {
        PlayerMgr.GetInstance().SetMagic(magic);
        PlayerMgr.GetInstance().SetSpeed(speed);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if (!moveState)
        //{
            ChooseBlock();
        //}
        if(moveState)
        {
            Moving();
        }
        if(Input.GetMouseButtonDown(0))
        {
            temp_result = result;
            moveState = true;
        }   
    }

    private void OnClick()
    {
        while (moveState)
        {
            Moving();
        }
    }

    /// <summary>
    /// 选择移动目标
    /// </summary>
    void ChooseBlock()
    {
        pos = InputMgr.GetInstance().GetCurrentMouse();
        try
        {
            result = AStarMgr.GetInstance().FindPath(new Vector2(transform.position.z, transform.position.x),new Vector2(pos.transform.position.z,pos.transform.position.x));
            if (result == null)
            {
                highlightBlocks.GetComponent<Outline>().enabled = false;
                highlightBlocks = null;
                lr.SetVertexCount(0);
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

                lr.positionCount = result.Count;
                for (int i = 0; i < result.Count; i++)
                {
                    lr.SetPosition(i,new Vector3(result[i].y, 0, result[i].x));
                }
            }
        }
        catch (Exception e)
        {
            // pos.GetComponent<Outline>().enabled = false;
            // highlightBlocks.GetComponent<Outline>().enabled = false;
            lr.SetVertexCount(0);
        }
    }

    void Moving()
    {
        Vector3 NextPos = new Vector3(temp_result[stepCount].y, 0.25f, temp_result[stepCount].x);
        if(transform.position == NextPos){ 
            print("currentCount" + stepCount);
            stepCount++;
            if(stepCount + 1 > result.Count)
            {
                moveState = false;
                stepCount = 0; 
            }
        }
        else
        {
            // transform.rotation = Quaternion.LookRotation(NextPos - transform.position, Vector3.forward); //转向
            transform.position = Vector3.MoveTowards(transform.position, NextPos, speed * Time.deltaTime);
        }
        

    }
}
