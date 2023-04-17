using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class EnemyCh1 : MonoBehaviour
{
    public BehaviourTreeRunner bt;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Alumb(string s)
    {
        print("2: 触发警报");
        Debug.Log(s);
        
        bt.tree.treeState = Node.State.Success;
        bt.tree.blackboard.inBack = true;
        bt.tree.blackboard.alumbObj = GameObject.Find(s).transform;
        if (AStarMgr.GetInstance().GetBlockState(bt.tree.blackboard.alumbObj.position.x, bt.tree.blackboard.alumbObj.position.z) == 1)
        {
            bt.tree.blackboard.fuck = true;
        }
        bt.tree.blackboard.alumbTrig = true;
    }

    public void Test(string s)
    {
        Debug.Log(s);
    }
}
