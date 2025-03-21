using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Mono的管理者
/// </summary>
public class MonoCtl : MonoBehaviour {

    private event UnityAction updateEvent;
    void Start () {
        DontDestroyOnLoad(this.gameObject);
    }
    void Update () {
        if (updateEvent != null)
            updateEvent();
    }


    /// <summary>
    /// 给外部提供的 添加帧更新事件的函数
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    /// <summary>
    /// 提供给外部 用于移除帧更新事件函数
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }
}