using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveOnce : ActionNode
{
    public Vector2 startPoint;
    public Vector2[] endPoint;
    public Animator anim;
    private Vector2 temp;
    // 速度
    public float speed;
    // 攻击范围obj
    public GameObject attack;
    
    // 检测精度
    public int precision = 4;
    // 检测角度
    public int angle;   //45
    // 半径
    public float radius;    //2.5

    // 路径
    private List<AStarNode> result = new List<AStarNode>();

    private int endNum = 0; // 第几个终点
    // 路径计数
    private int count = 0;
    protected override void OnStart() {
        var transform = context.transform;
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        attack = transform.GetChild(transform.childCount - 1).gameObject;
        temp = startPoint;  // 暂存
        result = AStarMgr.GetInstance().FindPathRect(startPoint, endPoint[endNum]);
    }

    protected override void OnStop()
    {
        count = 0;
        endNum = 0;
    }

    protected override State OnUpdate()
    {
        // Debug.Log(blackboard.detectPlayer + " in Move");
        var t = context.transform;
        if (blackboard.detectPlayer)
        {
            return State.Failure;
        }
        if (EnemyMgr.GetInstance().DetectPlayer(precision, angle, radius, t))
        {
            blackboard.detectPlayer = true;
            return State.Failure;
        }
        MoveGameObject();
        return State.Success;
    }
    
    /// <summary>
    /// 根据路径进行移动
    /// </summary>
    void MoveGameObject()
    {
        var transform = context.transform;
        Vector3 NextPos = new Vector3(result[count].y, 0, result[count].x);
        // 一般通行
        if(transform.position == NextPos){
            count++;
            if(count + 1 > result.Count)
            {
                if (endNum + 1 >= endPoint.Length)
                {
                    startPoint = endPoint[endNum];
                    result = AStarMgr.GetInstance().FindPathRect(startPoint, endPoint[0]);
                    endNum = 0;
                }
                else
                {
                    startPoint = endPoint[endNum];
                    result = AStarMgr.GetInstance().FindPathRect(startPoint, endPoint[endNum + 1]);
                }
                count = 0;
                endNum += 1;
            }
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(NextPos - transform.position, Vector3.up); //转向
            transform.position = Vector3.MoveTowards(transform.position, NextPos, speed * Time.deltaTime);
        }
        
        Material[] materials = attack.GetComponent<MeshRenderer>().materials;
        materials[0] = Resources.Load("normal") as Material;
        materials[1] = Resources.Load("normal_b") as Material;
        attack.GetComponent<MeshRenderer>().materials = materials;
    }
}
