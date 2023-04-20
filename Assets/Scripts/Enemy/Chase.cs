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
    
    public EnemyCh1 e_ctl;
    
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
        
        e_ctl = transform.GetComponent<EnemyCh1>();
        player = GameObject.FindWithTag("Player");
        playerAnim = player.GetComponentInParent<Animator>();
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
        
        startPoint = new Vector2(Mathf.Round(transform.position.z), Mathf.Round(transform.position.x));
        endPoint = new Vector2(Mathf.Round(player.transform.position.z), Mathf.Round(player.transform.position.x));
        result = AStarMgr.GetInstance().FindPathRect(startPoint, endPoint);
        
        count = 1;
        timer = 0;
        e_ctl.ChangeFindColor();
    }

    protected override void OnStop()
    {
        blackboard.detectPlayer = false;
        e_ctl.ChangeNormalColor();
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
}
