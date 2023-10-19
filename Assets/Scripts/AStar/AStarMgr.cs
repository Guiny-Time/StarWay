using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
/// <summary>
/// A星寻路管理器
/// </summary>
public class AStarMgr : BaseManager<AStarMgr>
{
    //地图的宽高
    private int _mapW;
    private int _mapH;
    // 初始化地图信息
    [Serializable]
    public class mapInfoWidth
    {
        public int[] element;
    }

    //地图相关所有的格子对象容器
    public AStarNode[,] nodes;
    //开启列表
    private List<AStarNode> openList = new List<AStarNode>(); 
    //关闭列表
    private List<AStarNode> closeList = new List<AStarNode>(); 

    /// <summary>
    /// 初始化地图信息
    /// </summary>
    /// <param name="w"></param>
    /// <param name="h"></param>
    public void InitMapInfo(mapInfoWidth[] data,int w ,int h) 
    {
        this._mapW = w;
        this._mapH = h;
        nodes = new AStarNode[w, h];
        if(data!=null){
            // 装填
            for ( int i = 0; i < data.Length; i++ )
            {
                for( int j = 0; j < data[i].element.Length; j++ )
                {
                    int nodeType = data[i].element[j];
                    AStarNode node = new AStarNode(i, j, nodeType);
                    nodes[i, j] = node;
                }
            }
        }else{
            Debug.Log("map data file not found");
        }
        Debug.Log("init complete");
    }

    /// <summary>
    /// 获得格子状态数据
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <returns></returns>
    public int GetBlockState(float x, float z)
    {
        return nodes[(int)z, (int)x].type;
    }

    /// <summary>
    /// 修改格子状态数据
    /// </summary>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="state"></param>
    public void ChangeBlockState(float x, float z, int state)
    {
        nodes[(int)z, (int)x].type = state;
    }

    /// <summary>
    /// 寻路方法 提供给外部使用
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <returns></returns>
    public List<AStarNode> FindPath(Vector2 startPos, Vector2 endPos)
    {
        //首先判断 传入的两个点 是否合法
        //如果不合法 应该直接 返回null 意味着不能寻路
        //1.首先 要在地图范围内
        if (startPos.x < 0 || startPos.x >= _mapW ||
            startPos.y < 0 || startPos.y >= _mapH ||
            endPos.x < 0 || endPos.x >= _mapW ||
            endPos.y < 0 || endPos.y >= _mapH)
        {
            // Debug.Log("开始或者结束点在地图格子范围外");
            return null;
        }
        
        //2.要不是阻挡
        //应该得到起点和终点 对应的格子
        AStarNode start = nodes[(int)startPos.x, (int)startPos.y];
        AStarNode end = nodes[(int)endPos.x, (int)endPos.y];
        if (start.type == 0 ||
            end.type == 0)
        {
            // Debug.Log("开始或者结束点不可通行");
            return null;
        }

        //清空上一次相关的数据 避免他们影响 这一次的寻路计算

        //清空关闭和开启列表
        closeList.Clear();
        openList.Clear();

        //把开始点放入关闭列表中
        start.father = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        closeList.Add(start);

        while(true)
        {
            //从起点开始 找周围的点 并放入开启列表中
            //上 x  y -1
            FindNearlyNodeToOpenList(start.x, start.y - 1, 1, start, end);
            //左 x - 1 y
            FindNearlyNodeToOpenList(start.x - 1, start.y, 1, start, end);
            //右 x + 1 y
            FindNearlyNodeToOpenList(start.x + 1, start.y, 1, start, end);
            //下 x y + 1
            FindNearlyNodeToOpenList(start.x, start.y + 1, 1, start, end);
            // 左上
            FindNearlyNodeToOpenList(start.x-1, start.y - 1, 1.91f, start, end);
            // 左下
            FindNearlyNodeToOpenList(start.x - 1, start.y + 1, 1.91f, start, end);
            // 右上
            FindNearlyNodeToOpenList(start.x + 1, start.y - 1, 1.91f, start, end);
            // 右下
            FindNearlyNodeToOpenList(start.x + 1, start.y + 1, 1.91f, start, end);

            //死路判断 开启列表为空 都还没有找到终点 就认为是死路
            if (openList.Count == 0)
            {
                // Debug.Log("死路");
                return null;
            }

            //选出开启列表中 寻路消耗最小的点
            openList.Sort(SortOpenList);
            for ( int i = 0; i < openList.Count; ++i )
            {
                AStarNode node = openList[i];
                //Debug.Log("点" + node.x + "," + node.y + ":g=" + node.g + "h=" + node.h + "f=" + node.f);
            }

            //放入关闭列表中 然后再从开启列表中移除
            closeList.Add(openList[0]);
            //找得这个点 又编程新的起点 进行下一次寻路计算了
            start = openList[0];
            openList.RemoveAt(0);

            //如果这个点已经是终点了 那么得到最终结果返回出去
            //如果这个点 不是终点 那么继续寻路
            if (start == end)
            {
                //找完了 找到路径了
                List<AStarNode> path = new List<AStarNode>();
                path.Add(end);
                while(end.father != null)
                {
                    path.Add(end.father);
                    end = end.father;
                }
                //列表翻转的API
                path.Reverse();

                return path;
            }
        }
    }

    public List<AStarNode> FindPathRect(Vector2 startPos, Vector2 endPos)
    {
        //首先判断 传入的两个点 是否合法
        //如果不合法 应该直接 返回null 意味着不能寻路
        //1.首先 要在地图范围内
        if (startPos.x < 0 || startPos.x >= _mapW ||
            startPos.y < 0 || startPos.y >= _mapH ||
            endPos.x < 0 || endPos.x >= _mapW ||
            endPos.y < 0 || endPos.y >= _mapH)
        {
            Debug.Log("开始或者结束点在地图格子范围外");
            return null;
        }
        
        //2.要不是阻挡
        //应该得到起点和终点 对应的格子
        AStarNode start = nodes[(int)startPos.x, (int)startPos.y];
        AStarNode end = nodes[(int)endPos.x, (int)endPos.y];
        if (start.type == 0 ||
            end.type == 0)
        {
            Debug.Log("开始或者结束点不可通行");
            return null;
        }

        //清空上一次相关的数据 避免他们影响 这一次的寻路计算

        //清空关闭和开启列表
        closeList.Clear();
        openList.Clear();

        //把开始点放入关闭列表中
        start.father = null;
        start.f = 0;
        start.g = 0;
        start.h = 0;
        closeList.Add(start);

        while(true)
        {
            //从起点开始 找周围的点 并放入开启列表中
            //上 x  y -1
            FindNearlyNodeToOpenList(start.x, start.y - 1, 1, start, end);
            //左 x - 1 y
            FindNearlyNodeToOpenList(start.x - 1, start.y, 1, start, end);
            //右 x + 1 y
            FindNearlyNodeToOpenList(start.x + 1, start.y, 1, start, end);
            //下 x y + 1
            FindNearlyNodeToOpenList(start.x, start.y + 1, 1, start, end);

            //死路判断 开启列表为空 都还没有找到终点 就认为是死路
            if (openList.Count == 0)
            {
                Debug.Log("死路");
                return null;
            }

            //选出开启列表中 寻路消耗最小的点
            openList.Sort(SortOpenList);
            for ( int i = 0; i < openList.Count; ++i )
            {
                AStarNode node = openList[i];
                //Debug.Log("点" + node.x + "," + node.y + ":g=" + node.g + "h=" + node.h + "f=" + node.f);
            }

            //放入关闭列表中 然后再从开启列表中移除
            closeList.Add(openList[0]);
            //找得这个点 又编程新的起点 进行下一次寻路计算了
            start = openList[0];
            openList.RemoveAt(0);

            //如果这个点已经是终点了 那么得到最终结果返回出去
            //如果这个点 不是终点 那么继续寻路
            if (start == end)
            {
                //找完了 找到路径了
                List<AStarNode> path = new List<AStarNode>();
                path.Add(end);
                while(end.father != null)
                {
                    path.Add(end.father);
                    end = end.father;
                }
                //列表翻转的API
                path.Reverse();

                return path;
            }
        }
    }


    /// <summary>
    /// 排序函数
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    private int SortOpenList(AStarNode a, AStarNode b)
    {
        if (a.f > b.f)
            return 1;
        else if (a.f == b.f)
            return 1;
        else
            return -1;
    }

    /// <summary>
    /// 把临近的点放入开启列表中的函数
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void FindNearlyNodeToOpenList(int x, int y, float g, AStarNode father, AStarNode end)
    {
        //边界判断
        if (x < 0 || x >= _mapW ||
            y < 0 || y >= _mapH)
            return;
        //在范围内 再去取点
        AStarNode node = nodes[x, y];
        //判断这些点 是否是边界 是否是阻挡  是否在开启或者关闭列表 如果都不是 才放入开启列表
        if (node == null||
            node.type == 0 ||
            closeList.Contains(node) ||
            openList.Contains(node) )
            return;
        //计算f值
        //f = g + h
        //记录父对象
        node.father = father;
        //计算g  我离起点的距离 就是我父亲离起点的距离 + 我离我父亲的距离
        node.g = father.g + g;
        node.h = Mathf.Abs(end.x - node.x) + Mathf.Abs(end.y - node.y);
        node.f = node.g + node.h;

        //Debug.Log("点" + node.x + "," + node.y + ":g=" + node.g + "h=" + node.h);

        //如果通过了上面的合法验证 就存到开启列表中
        openList.Add(node);
    }

}
