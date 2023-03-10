using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMgr : MonoBehaviour
{
    [Header("起止点")]
    public Vector2 startPoint;
    public Vector2 endPoint;
    private Vector2 temp;
    
    [Header("个性化参数")]
    public float speed;
    // 路径
    private List<AStarNode> result = new List<AStarNode>();
    // 路径计数
    private int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        EnermyMoveing();
    }
    
    /// <summary>
    /// 移动过程中遇到的事情（碰到我方单位/碰到防御点
    /// </summary>
    void EnermyMoveing(){

        result = AStarMgr.GetInstance().FindPath(startPoint, endPoint);
        RaycastHit hit;
        
        MoveGameObject();
    }
    
    /// <summary>
    /// 根据路径进行移动
    /// </summary>
    void MoveGameObject(){
        
        Vector3 NextPos = new Vector3(result[count].y, 0, result[count].x);

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
            transform.rotation = Quaternion.LookRotation(NextPos - transform.position, Vector3.forward); //转向
            transform.position = Vector3.MoveTowards(transform.position, NextPos, speed * Time.deltaTime);
        }

    }
}
