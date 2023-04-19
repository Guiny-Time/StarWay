using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using UnityEngine.SceneManagement;

public class Chase : ActionNode
{
    public Vector2 startPoint;
    public Vector2 endPoint;
    public Animator anim;
    private Vector2 temp;
    // 速度
    public float speed;
    // 追击半径
    public float radius;

    private GameObject player;
    private Animator playerAnim;

    // 路径
    private List<AStarNode> result = new List<AStarNode>();
    // 路径计数
    private int count = 1;
    private float timer;    // 加载场景的计时器
    private Transform transform;
    protected override void OnStart() {
        transform = context.transform;
        player = GameObject.FindWithTag("Player");
        playerAnim = player.GetComponentInParent<Animator>();
        
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        startPoint = new Vector2(Mathf.Round(transform.position.z), Mathf.Round(transform.position.x));
        endPoint = new Vector2(Mathf.Round(player.transform.position.z), Mathf.Round(player.transform.position.x));
        result = AStarMgr.GetInstance().FindPathRect(startPoint, endPoint);
        count = 1;
        timer = 0;
        ChangeColorRed(transform);
    }

    protected override void OnStop()
    {
        blackboard.detectPlayer = false;
        ChangeColorNormal(transform);
    }

    protected override State OnUpdate() {
        if (blackboard.detectPlayer)
        {
            startPoint = new Vector2(Mathf.Round(transform.position.z), Mathf.Round(transform.position.x));
            endPoint = new Vector2(Mathf.Round(player.transform.position.z), Mathf.Round(player.transform.position.x));
            result = AStarMgr.GetInstance().FindPathRect(startPoint, endPoint);

            if (Vector3.Distance(transform.position, player.transform.position) <= 1)
            {
                anim.SetBool("attack",true);
                // anim.Play("EneCh1Attack");
                playerAnim.SetBool("beAttack", true);
                if (timer < 1.0f)
                {
                    timer += Time.deltaTime;
                }

                if (timer > 1.0f)
                {
                    LoadScene();
                }
            }

            if (Vector3.Distance(transform.position, player.transform.position) > radius + 1.5f)
            {
                blackboard.detectPlayer = false;
                return State.Success;
            }
            MoveGameObject();
        }

        return State.Running;
    }
    
    
    void LoadScene()
    {
        blackboard.detectPlayer = false;
        timer = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
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
    
    public void ChangeColorRed(Transform transform)
    {
        GameObject attack = transform.GetChild(transform.childCount - 1).gameObject;
        Material[] materials = attack.GetComponent<MeshRenderer>().materials;
        materials[0] = Resources.Load("find") as Material;
        materials[1] = Resources.Load("find_b") as Material;
        attack.GetComponent<MeshRenderer>().materials = materials;
    }
    
    public void ChangeColorNormal(Transform transform)
    {
        GameObject attack = transform.GetChild(transform.childCount - 1).gameObject;
        Material[] materials = attack.GetComponent<MeshRenderer>().materials;
        materials[0] = Resources.Load("normal") as Material;
        materials[1] = Resources.Load("normal_b") as Material;
        attack.GetComponent<MeshRenderer>().materials = materials;
    }
}
