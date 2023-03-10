using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : BaseManager<PlayerMgr>
{
    private int magic;
    private int speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetState(int magic, int speed)
    {
        this.magic = magic;
        this.speed = speed;
    }

    public int GetMagic()
    {
        return magic;
    }

    public int GetSpeed()
    {
        return speed;
    }
    

    public List<AStarNode> FindPathList(Vector2 start, Vector2 end)
    {
        return AStarMgr.GetInstance().FindPath(start, end);
    }
}
