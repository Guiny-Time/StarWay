using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCh1 : EnemyMgr
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        EnermyMoveing();
        if (DetectPlayer())
        {
            print("find player!");
        }

    }
}
