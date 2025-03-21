﻿/// <summary>
/// 格子类型
/// </summary>

/// <summary>
/// A星格子类
/// </summary>
public class AStarNode
{
    //格子对象的坐标
    public int x;
    public int y;
    //寻路消耗
    public float f;
    //离起点的距离
    public float g;
    //离终点的距离
    public float h;
    //父对象
    public AStarNode father;
    //格子的类型, 0不可通行，1可通行
    public int type;

    /// <summary>
    /// 构造函数 传入坐标和格子类型
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="type"></param>
    public AStarNode( int x, int y, int type )
    {
        this.x = x;
        this.y = y; 
        this.type = type;
    }
}