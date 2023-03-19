using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Polybrush;
using UnityEngine.SceneManagement;

public class EnemyMgr : MonoBehaviour
{
    [Header("起止点")]
    public Vector2 startPoint;
    public Vector2 endPoint;
    private Vector2 temp;
    
    [Header("个性化参数")]
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

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /// <summary>
    /// 移动过程中遇到的事情（碰到我方单位/碰到防御点
    /// </summary>
    public void EnermyMoveing(){

        result = AStarMgr.GetInstance().FindPath(startPoint, endPoint);

        MoveGameObject();
    }
    
    /// <summary>
    /// 根据路径进行移动
    /// </summary>
    void MoveGameObject(){
        
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
        
        // 遇见玩家
        if (DetectPlayer())
        {
            // 重载
            // SceneManager.LoadScene(0);
            Material[] materials = attack.GetComponent<MeshRenderer>().materials;
            materials[0] = Resources.Load("find") as Material;
            materials[1] = Resources.Load("find_b") as Material;
            attack.GetComponent<MeshRenderer>().materials = materials;
        }
        else
        {
            Material[] materials = attack.GetComponent<MeshRenderer>().materials;
            materials[0] = Resources.Load("normal") as Material;
            materials[1] = Resources.Load("normal_b") as Material;
            attack.GetComponent<MeshRenderer>().materials = materials;
        }

    }

    public bool DetectPlayer()
    {
        if (GenerateRay(0))
        {
            return true;
            
        }
        for (int i = 1; i < precision; i++)
        {
            if (GenerateRay(angle / (2 * i)) || GenerateRay(-1 * angle / (2 * i)))
            {
                return true;
            }
        }
        return false;
    }

    public bool GenerateRay(int angle)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.Normalize(Quaternion.Euler(0, angle, 0) * transform.forward), out hit,
                radius) && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;

    }
    
}
 