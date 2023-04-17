using System;
using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class PlayerCtl : MonoBehaviour
{
    public int speed;
    public int magic;
    public Animator anim;

    private List<AStarNode> result = new List<AStarNode>();
    private List<AStarNode> temp_result = new List<AStarNode>();
    private GameObject highlightBlocks = null;
    private bool moveState; //false: stand; true: moving
    private int stepCount = 1;  // foot step
    private Vector2 temp;
    private GameObject pos; // mouse choose obj
    
    
    
    private void Awake()
    {
        PlayerMgr.GetInstance().SetMagic(magic);
        PlayerMgr.GetInstance().SetSpeed(speed);
        anim.Play("stand");
    }

    // Start is called before the first frame update
    void Start()
    {
        moveState = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        InputMgr.GetInstance().UIListener(result);
        if (!UICtl.GetInstance().GetPanelState())
        {
            ChooseBlock();      // choose move-to block
        }

        if (moveState) { 
            Moving();   // moving
            anim.SetBool("running", true);
        }
        else
        {
            anim.SetBool("running", false);
        }

        // find fath
        if (pos != null)
        {
            result = AStarMgr.GetInstance().FindPath(new Vector2( Mathf.Round(transform.position.z), Mathf.Round(transform.position.x)),
                new Vector2(pos.transform.position.z,pos.transform.position.x));
        }

        // use magic
        if (InputMgr.GetInstance().GetMagicState())
        {
            InputMgr.GetInstance().InMagic();
        }
        else
        {
            InputMgr.GetInstance().DrawLine(result);
            if(Input.GetMouseButtonDown(0))
            {
                if (result.Count != 0 && !UICtl.GetInstance().GetPanelState())
                {
                    temp_result = result;
                    stepCount = 1; 
                    moveState = true;
                }
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
            if (result == null && !InputMgr.GetInstance().GetMagicState())
            {
                highlightBlocks.GetComponent<Outline>().enabled = false;
                highlightBlocks = null;
            }
            else
            {
                if (highlightBlocks && pos.name != highlightBlocks.name)
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
        catch (Exception)
        {
            return;
        }
    }
    

    void Moving()
    {
        Vector3 NextPos = new Vector3(temp_result[stepCount].y, -0.4f, temp_result[stepCount].x);
        if(transform.position == NextPos){
            stepCount++;
            if(stepCount + 1 > temp_result.Count)
            {
                moveState = false;
            }
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(NextPos - transform.position, Vector3.up); //转向
            transform.position = Vector3.MoveTowards(transform.position, NextPos, speed * Time.deltaTime);
        }
        
    }
}
