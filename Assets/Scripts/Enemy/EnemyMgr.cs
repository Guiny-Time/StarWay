using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Polybrush;
using UnityEngine.SceneManagement;

public class EnemyMgr : BaseManager<EnemyMgr>
{
    [Header("起止点")]
    public Vector2 startPoint;
    public Vector2 endPoint;
    private Vector2 temp;
    
    [Header("个性化参数")]
    // 速度
    public float speed;
    
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
    /// 检测是否发现玩家
    /// </summary>
    /// <returns></returns>
    public bool DetectPlayer(int precision, int angle, float radius, Transform t)
    {
        if (GenerateRay(0, radius, t))
        {
            return true;
            
        }
        for (int i = 1; i < precision; i++)
        {
            if (GenerateRay(angle / (2 * i), radius, t) || GenerateRay(-1 * angle / (2 * i), radius, t))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 生成射线检测玩家
    /// </summary>
    /// <param name="angle"></param>
    /// <returns></returns>
    public bool GenerateRay(int angle, float radius, Transform t)
    {
        RaycastHit hit;
        if (Physics.Raycast(t.position, Vector3.Normalize(Quaternion.Euler(0, angle, 0) * t.forward), out hit,
                radius) && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;

    }
    
}
 