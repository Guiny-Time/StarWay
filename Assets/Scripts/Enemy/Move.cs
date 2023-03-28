using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class Move : ActionNode
{
    public Vector2 startPoint;
    public Vector2 endPoint;
    private Vector2 temp;
    // 速度
    public float speed;
    // 检测精度
    public int precision = 1;
    // 检测角度
    public int angle;
    // 半径
    public float radius;
    // 攻击范围obj
    public GameObject attack;

    // 路径
    private List<AStarNode> result = new List<AStarNode>();
    // 路径计数
    private int count = 0;
    protected override void OnStart() {
        var transform = context.transform;
        attack = transform.GetChild(transform.childCount - 1).gameObject;
        Debug.Log(attack.name);
        result = AStarMgr.GetInstance().FindPath(startPoint, endPoint);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate()
    {
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
                temp = startPoint;
                startPoint = endPoint;
                endPoint = temp;
                result = AStarMgr.GetInstance().FindPath(startPoint, endPoint);
                count = 0; 
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
