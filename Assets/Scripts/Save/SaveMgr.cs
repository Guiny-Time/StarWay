using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 存档管理
/// </summary>
public class SaveMgr : BaseManager<SaveMgr>
{
    /// <summary>
    /// 初始化所有章节名字
    /// </summary>
    public void InitChapNames()
    {
        PlayerPrefs.SetString("Chap1-1","Wet Swampy");
        PlayerPrefs.SetString("Chap1-2","Misty Swampy");
        PlayerPrefs.SetString("Chap1-3","? Swampy");
    }
    /// <summary>
    /// 检测是否为最后一章，由于现在只有一个章节，所以是1
    /// </summary>
    /// <returns></returns>
    public bool LastChapter()
    {
        if (PlayerPrefs.GetInt("Chapter") == 1)
        {
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// 检测是否为章节最后一关
    /// </summary>
    /// <returns></returns>
    public bool LastLevel()
    {
        if (PlayerPrefs.GetInt("Level") == 3)
        {
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// 获取当前进度
    /// </summary>
    /// <returns></returns>
    public string GetProgress()
    {
        return PlayerPrefs.GetString("Progress");
    }
    
    /// <summary>
    /// 记录关卡推进进度
    /// </summary>
    /// <param name="chapter"></param>
    /// <param name="level"></param>
    public void Save(int chapter, int level)
    {
        PlayerPrefs.SetInt("Chapter", chapter);
        PlayerPrefs.SetInt("Level", level);
        
        string progress = chapter.ToString() + "-" + level.ToString();
        PlayerPrefs.SetString("Progress","Chap" + progress);    // e.g. chap1-1，对应prefab名称
    }

    /// <summary>
    /// 清除存档，回到初始1-1
    /// </summary>
    public void CleanSave()
    {
        PlayerPrefs.SetInt("IsFirst", 0);   // 第一次玩，触发过场动画教学
        PlayerPrefs.SetInt("Chapter", 1);
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetString("Progress","Chap1-1");
    }
}
