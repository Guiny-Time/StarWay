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
    private GameObject alumbObj;
    
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
    }

    protected override void OnStop()
    {
        count = 0;
        magicTrigger = false;
        blackboard.inAlumb = false;
    }

    protected override State OnUpdate() {
        EventCenter.GetInstance().AddEventListener("UseMagic", (GameObject o) => {
            magicTrigger = true;
            alumbObj = o;
            Debug.Log(o.name);
            startPoint = new Vector2(Mathf.Round(transform.position.z), Mathf.Round(transform.position.x));
            endPoint = new Vector2(Mathf.Round(alumbObj.transform.position.z), Mathf.Round(alumbObj.transform.position.x));
            result = AStarMgr.GetInstance().FindPathRect(startPoint,endPoint);
            count = 0;
            anim.Play("EneCh1Run");
            ChangeColor();
            Debug.Log(result.Count);
            Debug.Log("success");
        });

        if (blackboard.detectPlayer)
        {
            return State.Failure;
        }
        if (EnemyMgr.GetInstance().DetectPlayer(precision,angle,radius,transform))
        {
            blackboard.detectPlayer = true;
            ChangeColor();
            return State.Failure;
        }
        if (magicTrigger && !blackboard.inAlumb)
        {
            MoveGameObject();
            return State.Running;
        }

        if (blackboard.inAlumb)
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
