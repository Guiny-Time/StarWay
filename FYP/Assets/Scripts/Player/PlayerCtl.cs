using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtl : MonoBehaviour
{
    public int speed;
    public int magic;
    public LineRenderer lr;

    private GameObject pos;
    private List<AStarNode> result = new List<AStarNode>();

    
    private void Awake()
    {
        PlayerMgr.GetInstance().SetState(magic, speed);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pos = InputMgr.GetInstance().GetCurrentMouse();
        try
        {
            result = AStarMgr.GetInstance().FindPath(new Vector2(transform.position.z, transform.position.x),new Vector2(pos.transform.position.z,pos.transform.position.x));
            lr.positionCount = result.Count;
            for (int i = 0; i < result.Count; i++)
            {
                lr.SetPosition(i,new Vector3(result[i].y, 1, result[i].x));
            }
            
            pos.GetComponent<Outline>().enabled = true;
        }
        catch (Exception e)
        {
            // do nothing
        }
    }
}
