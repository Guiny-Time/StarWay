using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class Detect : ActionNode
{
    // 检测精度
    public int precision = 4;
    // 检测角度
    public int angle;   //45
    // 半径
    public float radius;    //2.5
    public Animator anim;
    
    public EnemyCh1 e_ctl;
    private Transform transform;
    
    protected override void OnStart()
    {
        transform = context.transform;
        e_ctl = transform.GetComponent<EnemyCh1>();
        anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
    }

    protected override void OnStop()
    {
        e_ctl.ChangeFindColor();
        anim.Play("EneCh1Run");
    }

    protected override State OnUpdate() {
        if (DetectPlayer())
        {
            return State.Success;
        }
        return State.Failure;
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
