using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.Events;

public class Alumb : ActionNode
{
    // 运动通用参数
    public Vector2 startPoint;
    public Vector2 endPoint;
    public Animator anim;
    private Vector2 temp;
    
    // 追击参数
    public float speed;
    private bool magicTrigger;
    private Transform alumbObj;
    
    // 检测精度
    public int precision = 4;
    // 检测角度
    public int angle;   //45
    // 半径
    public float radius;    //2.5
    
    // 路径
    private List<AStarNode> result = new List<AStarNode>();
    // 路径计数
    private int count = 0;
    
    private Transform transform;

    protected override void OnStart()
    {
        transform = context.transform;
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        count = 0;
        /*if (result == null)
        {
            // 切换状态
            blackboard.inAlumb = true;
        }*/
    }

    protected override void OnStop()
    {
        count = 0;
        magicTrigger = false;
        blackboard.inAlumb = false;
    }

    protected override State OnUpdate() {
        // 魔法，触发一次特殊寻路事件（确保有result）
        if (blackboard.alumbTrig)
        {
            EventTrig(blackboard.alumbObj);
            blackboard.alumbTrig = false;
        }

        // 发现玩家，直接进入chase状态，这个bool到下一个move节点也会导致节点失败
        if (blackboard.detectPlayer)
        {
            return State.Failure;
        }
        
        // 射线发现玩家
        if (EnemyMgr.GetInstance().DetectPlayer(precision,angle,radius,transform))
        {
            blackboard.detectPlayer = true;
            ChangeColor();
            return State.Failure;
        }
        
        // 处于被魔法惊动的状态，且还没走到寻路尽头
        if (magicTrigger && !blackboard.inAlumb)
        {
            MoveGameObject();
            return State.Running;
        }

        // 走到寻路尽头了，状态Success
        if (blackboard.inAlumb)
        {
            return State.Success;
        }
        
        return State.Failure;
    }

    /// <summary>
    /// 根据魔法传来的block位置的state的不同，设置不同的执行顺序（改状态-寻路或寻路-改状态），返回路径result
    /// </summary>
    /// <param name="o"></param>
    public void EventTrig(Transform o)
    {
        magicTrigger = true;
        transform = context.transform;
        startPoint = new Vector2(Mathf.Round(transform.position.z), Mathf.Round(transform.position.x));
        endPoint = new Vector2(o.position.z, o.position.x);
        if (blackboard.fuck)    //之前是1，很可能被改成0了
        {
            BlockMgr.GetInstance().UseGravityMagic(blackboard.alumbObj.gameObject, 0);  // 先设成1
            result = AStarMgr.GetInstance().FindPathRect(startPoint,endPoint);
            if (result == null)
            {
                Debug.Log("dead road.");
                // 切换状态
                count = 0;
                BlockMgr.GetInstance().UseGravityMagic(blackboard.alumbObj.gameObject, 1);
                magicTrigger = false;
                // return State.Failure;
            }
            // BlockMgr.GetInstance().UseGravityMagic(blackboard.alumbObj.gameObject, 1);
            blackboard.fuck = false;
        }
        else
        {
            result = AStarMgr.GetInstance().FindPathRect(startPoint,endPoint);
            if (result == null)
            {
                // 切换状态
                Debug.Log("dead road.");
                count = 0;
                magicTrigger = false;
                // return State.Failure;
            }
        }
        count = 0;
        anim.Play("EneCh1Run");
        ChangeColor();
        // return State.Running;
    }

    /// <summary>
    /// 根据路径进行移动
    /// </summary>
    void MoveGameObject()
    {
        Vector3 NextPos = new Vector3(result[count].y, 0, result[count].x);
        // 一般通行
        if(transform.position == NextPos){
            count++;
            if(count + 1 >= result.Count)
            {
                count = 0;
                blackboard.inAlumb = true;
            }
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(NextPos - transform.position, Vector3.up); //转向
            transform.position = Vector3.MoveTowards(transform.position, NextPos, speed * Time.deltaTime);
        }

    }
    
    public void ChangeColor()
    {
        var transform = context.transform;
        GameObject attack = transform.GetChild(transform.childCount - 1).gameObject;
        Material[] materials = attack.GetComponent<MeshRenderer>().materials;
        materials[0] = Resources.Load("find") as Material;
        materials[1] = Resources.Load("find_b") as Material;
        attack.GetComponent<MeshRenderer>().materials = materials;
    }
}
