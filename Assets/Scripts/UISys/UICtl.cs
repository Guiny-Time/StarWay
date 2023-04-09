using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICtl : SingletonMono<UICtl>
{
    public AudioSource bgm;
    public GameObject[] panelElement;
    private bool isOpen = false;
    [SerializeReference] private GameObject menuPanel;
    [SerializeReference] private GameObject exitConfirm;
    [SerializeReference] private GameObject volumeControl;
    Stack<GameObject> PanelList = new Stack<GameObject>();
    private GameObject[] audioSource;

    private void Start()
    {
        // Cursor.lockState = CursorLockMode.Confined;
        bgm = GameObject.Find("Music").GetComponent<AudioSource>();
        MusicMgr.GetInstance().SetBKObject(bgm);
    }

    public bool GetPanelState()
    {
        return isOpen;
    }
    
    public void ShowPanel()
    {
        // 底层面板打开时，按esc关闭
        if (isOpen)
        {
            PanelList.Peek().SetActive(false);
            PanelList.Pop();
            Time.timeScale = 1;
            if(PanelList.Count == 0)
                isOpen = false;
            // Cursor.lockState = CursorLockMode.Confined;
        }
        // 底层面板关闭时，按esc打开
        else
        {
            Time.timeScale = 0;
            PanelList.Push(panelElement[0]);
            PanelList.Peek().SetActive(true);
            isOpen = true;
            // Cursor.lockState = CursorLockMode.None;
        }
    }

    // 展示退出面板面板
    public void ShowExitConfirmPanel()
    {
        PanelList.Push(exitConfirm);
        PanelList.Peek().SetActive(true);
    }

    // 展示音量调节面板
    public void ShowVolumePanel()
    {
        PanelList.Push(volumeControl);
        PanelList.Peek().SetActive(true);
    }

    /// <summary>
    /// 重载关卡
    /// </summary>
    public void ReloadLevel()
    {
        Time.timeScale = 1;
        EventCenter.GetInstance().Clear();
        SceneManager.LoadScene(0,LoadSceneMode.Single);
    }

    /// <summary>
    /// 重载章节
    /// </summary>
    public void ReloadChapter()
    {
        Debug.Log("重新加载章节");
    }

    /// <summary>
    /// 保存并退出
    /// </summary>
    public void ExitGame()
    {
        // PlayerPrefs ......
        Debug.Log("退出游戏");
        Application.Quit();
    }
    
    /// <summary>
    /// 返回游戏
    /// </summary>
    public void BackGame()
    {
        PanelList.Peek().SetActive(false);
        PanelList.Pop();
        isOpen = false;
        Time.timeScale = 1;
        // Cursor.lockState = CursorLockMode.Confined;        
    }

    //静音
    public void AudioStopper(){
        MusicMgr.GetInstance().PauseBKMusic();
    }

    //重新播放音效
    public void AudioBeginner(){
        MusicMgr.GetInstance().PlayBkMusic();
    }
    
}
