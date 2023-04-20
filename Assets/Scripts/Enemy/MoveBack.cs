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

    // 检测精度
    public int precision = 4;
    // 检测角度
    public int angle;   //45
    // 半径
    public float radius;    //2.5
    public Animator anim;

    public EnemyCh1 e_ctl;
    // 路径
    private List<AStarNode> result;
    // 路径计数
    private int count = 0;
    private bool inBack = true;
    private Transform transform;
    
    protected override void OnStart() {
        transform = context.transform;
        e_ctl = transform.GetComponent<EnemyCh1>();
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        anim.Play("EneCh1walk");
        
        startPoint = new Vector2(Mathf.Round(transform.position.z), Mathf.Round(transform.position.x));
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

        if (EnemyMgr.GetInstance().DetectPlayer(precision, angle, radius, transform))
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
        
        e_ctl.ChangeNormalColor();
    }
}