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
    private Vector2 temp;
    
    // 追击参数
    public float speed;
    private bool magicTrigger;
    private GameObject alumbObj;
    
    // 路径
    private List<AStarNode> result = new List<AStarNode>();
    // 路径计数
    private int count = 0;

    protected override void OnStart()
    {
        var transform = context.transform;
        EventCenter.GetInstance().AddEventListener("UseMagic", (GameObject o) => { 
            magicTrigger = true;
            alumbObj = o;
            startPoint = new Vector2(Mathf.Round(transform.position.z), Mathf.Round(transform.position.x));
            endPoint = new Vector2(Mathf.Round(alumbObj.transform.position.z), Mathf.Round(alumbObj.transform.position.x));
            result = AStarMgr.GetInstance().FindPath(startPoint,endPoint);
        });
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (magicTrigger)
        {
            var transform = context.transform;
            MoveGameObject();
            return State.Running;
        }
 
        return State.Failure;
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
            if(count + 1 >= result.Count)
            {
                // temp = startPoint;
                // startPoint = endPoint;
                // endPoint = temp;
                // result = AStarMgr.GetInstance().FindPath(startPoint, endPoint);
                count = 0; 
                magicTrigger = false;
            }
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(NextPos - transform.position, Vector3.up); //转向
            transform.position = Vector3.MoveTowards(transform.position, NextPos, speed * Time.deltaTime);
        }

    }
}
