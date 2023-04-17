using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class MoveBack : ActionNode
{
    private Vector2 startPoint;
    public Vector2 endPoint;
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
    public Animator anim;

    // 路径
    private List<AStarNode> result;
    // 路径计数
    private int count = 0;
    private bool inBack = true;
    protected override void OnStart() {
        var transform = context.transform;
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        anim.Play("EneCh1walk");
        
        startPoint = new Vector2(Mathf.Round(transform.position.z), Mathf.Round(transform.position.x));
        attack = transform.GetChild(transform.childCount - 1).gameObject;
        result = AStarMgr.GetInstance().FindPathRect(startPoint, endPoint);
        
        blackboard.inBack = false;
        count = 0;
    }

    protected override void OnStop()
    {
        inBack = true;
        blackboard.inBack = false;
    }

    protected override State OnUpdate()
    {
        if (result == null)
        {
            count = 0;
            blackboard.inBack = true;
        }
        var t = context.transform;

        if (EnemyMgr.GetInstance().DetectPlayer(precision, angle, radius, t))
        {
            blackboard.detectPlayer = true;
            return State.Failure;
        }
        if ( inBack && !blackboard.inBack)
        {
            MoveGameObject();
            return State.Running;
        }

        if (blackboard.inBack)
        {
            return State.Success;
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
            if(count + 1 > result.Count)
            {
                count = 0;
                blackboard.inBack = true;
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