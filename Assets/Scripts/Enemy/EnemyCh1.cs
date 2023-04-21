using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class EnemyCh1 : MonoBehaviour
{
    public BehaviourTreeRunner bt;

    public Material normal;
    public Material normal_b;

    public Material find;
    public Material find_b;
    
    private GameObject attack;
    private Material[] materials;
    
    // Start is called before the first frame update
    void Start()
    {
        // 攻击范围
        attack = transform.GetChild(transform.childCount - 1).gameObject;
        materials = attack.GetComponent<MeshRenderer>().materials;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 敌对生物攻击范围变蓝（静息状态）
    /// </summary>
    public void ChangeNormalColor()
    {
        materials[0] = normal;
        materials[1] = normal_b;
        attack.GetComponent<MeshRenderer>().materials = materials;
    }

    /// <summary>
    /// 敌对生物攻击范围变红（警戒状态）
    /// </summary>
    public void ChangeFindColor()
    {
        materials[0] = find;
        materials[1] = find_b;
        attack.GetComponent<MeshRenderer>().materials = materials;
    }

    /// <summary>
    /// UnityEvent触发警报，执行该方法
    /// </summary>
    /// <param name="s"></param>
    public void Alumb(string s)
    {
        print("5、触发UnityEvent in EnemyCh1.cs");
        bt.tree.treeState = Node.State.Failure;
        bt.tree.blackboard.inBack = true;
        bt.tree.blackboard.alumbObj = GameObject.Find(s).transform;
        if (AStarMgr.GetInstance().GetBlockState(bt.tree.blackboard.alumbObj.position.x, bt.tree.blackboard.alumbObj.position.z) == 1)
        {
            bt.tree.blackboard.fuck = true;
        }
        bt.tree.blackboard.alumbTrig = true;
    }
}
