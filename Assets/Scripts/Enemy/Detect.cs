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
    protected override void OnStart() {
    }

    protected override void OnStop()
    {
        ChangeColor();
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
        var transform = context.transform;
        if (Physics.Raycast(transform.position, Vector3.Normalize(Quaternion.Euler(0, angle, 0) * transform.forward), out hit,
                radius) && hit.collider.CompareTag("Player"))
        {
            return true;
        }

        return false;

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
