using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMgr : BaseManager<PlayerMgr>
{
    private int magic;
    private int speed;
    private int area;

    public void SetMagic(int magic)
    {
        this.magic = magic;
    }
    
    public void SetSpeed(int speed)
    {
        this.speed = speed;
    }

    public void SetArea(int area)
    {
        this.area = area;
    }

    public int GetMagic()
    {
        return magic;
    }

    public int GetSpeed()
    {
        return speed;
    }

    public int GetArea()
    {
        return area;
    }
    

    public List<AStarNode> FindPathList(Vector2 start, Vector2 end)
    {
        return AStarMgr.GetInstance().FindPath(start, end);
    }
    
    
}
