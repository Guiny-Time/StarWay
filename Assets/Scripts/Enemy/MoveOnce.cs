using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveOnce : ActionNode
{
    public Vector2 startPoint;
    public Vector2[] endPoint;
    
    public EnemyCh1 e_ctl;
    
    // 速度
    public float speed;

    // 检测精度
    public int precision = 4;
    // 检测角度
    public int angle;   //45
    // 半径
    public float radius;    //2.5

    // 路径
    private List<AStarNode> result;

    private int endNum = 0; // 第几个终点
    // 路径计数
    private int count = 0;
    private Transform transform;
    protected override void OnStart()
    {
        transform = context.transform;
        e_ctl = transform.GetComponent<EnemyCh1>();
        result = AStarMgr.GetInstance().FindPathRect(startPoint, endPoint[endNum]);
    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        // Debug.Log(blackboard.detectPlayer + " in Move");
        if (blackboard.detectPlayer)
        {
            return State.Failure;
        }
        if (EnemyMgr.GetInstance().DetectPlayer(precision, angle, radius, transform))
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
        
        e_ctl.ChangeNormalColor();
    }
}
