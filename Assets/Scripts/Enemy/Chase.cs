using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.SceneManagement;

public class Chase : ActionNode
{
    public Vector2 startPoint;
    public Vector2 endPoint;
    private Vector2 temp;
    // 速度
    public float speed;
    // 追击半径
    public float radius;

    private GameObject player;

    // 路径
    private List<AStarNode> result = new List<AStarNode>();
    // 路径计数
    private int count = 0;
    protected override void OnStart() {
        var transform = context.transform;
        player = GameObject.FindWithTag("Player");
        startPoint = new Vector2(Mathf.Round(transform.position.z), Mathf.Round(transform.position.x));
        endPoint = new Vector2(Mathf.Round(player.transform.position.z), Mathf.Round(player.transform.position.x));
        result = AStarMgr.GetInstance().FindPath(startPoint, endPoint);
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        var transform = context.transform;
        startPoint = new Vector2(Mathf.Round(transform.position.z), Mathf.Round(transform.position.x));
        endPoint = new Vector2(Mathf.Round(player.transform.position.z), Mathf.Round(player.transform.position.x));
        result = AStarMgr.GetInstance().FindPath(startPoint, endPoint);

        if (Vector3.Distance(transform.position, player.transform.position) <= 1)
        {
            EventCenter.GetInstance().Clear();
            SceneManager.LoadScene(0,LoadSceneMode.Single);
            return State.Success;
        }

        if (Vector3.Distance(transform.position, player.transform.position) > radius + 1.5f)
        {
            return State.Success;
        }
        MoveGameObject();
        return State.Running;
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

    }
}
