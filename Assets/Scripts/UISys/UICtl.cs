using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UICtl : SingletonMono<UICtl>
{
    public AudioSource bgm;
    public AudioSource soundEffect;
    public GameObject[] panelElement;
    [SerializeReference] private GameObject continueGame;   // 加载存档按钮
    [SerializeReference] private GameObject exitConfirm;
    [SerializeReference] private GameObject volumeControl;
    [SerializeReference] private Animator UIAnim;
    
    private TMP_Text T_ChapterID;
    private TMP_Text T_ChapterName;
    
    Stack<GameObject> PanelList = new Stack<GameObject>();
    
    private GameObject[] audioSource;
    private bool isOpen = false;
    

    private void OnEnable()
    {
        UIAnim = this.gameObject.GetComponent<Animator>();
        T_ChapterID = GameObject.Find("T_ChapterID").GetComponent<TMP_Text>();
        T_ChapterName = GameObject.Find("T_ChapterName").GetComponent<TMP_Text>();
        bgm = GameObject.FindWithTag("Music").GetComponent<AudioSource>();
        bgm.volume = 0f;
    }

    public void Start()
    {
        if (continueGame)
        {
            if (SaveMgr.GetInstance().GetProgress()!="Chap0-0")
            {
                PlayerPrefs.SetInt("firstTime", 0);
                continueGame.gameObject.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt("firstTime", 1);
                continueGame.gameObject.SetActive(false);
            }
        }

        if (T_ChapterID && T_ChapterName)
        {
            T_ChapterID.text = SaveMgr.GetInstance().GetProgress();
            T_ChapterName.text = PlayerPrefs.GetString(SaveMgr.GetInstance().GetProgress());
        }
    }

    private void Update()
    {
        if (bgm.volume < 0.8f)
        {
            bgm.volume = Mathf.Lerp(bgm.volume, 1, 0.01f);
        }
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
        }
        // 底层面板关闭时，按esc打开
        else
        {
            Time.timeScale = 0;
            PanelList.Push(panelElement[0]);
            PanelList.Peek().SetActive(true);
            isOpen = true;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
    }

    /// <summary>
    /// 重载章节
    /// </summary>
    public void ReloadChapter()
    {
        Debug.Log("重新加载章节");
        Time.timeScale = 1;
        PlayerPrefs.SetString("Progress","Chap" + PlayerPrefs.GetInt("Chapter")+"-1");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name,LoadSceneMode.Single);
    }

    /// <summary>
    /// 退出到开始界面
    /// </summary>
    public void ExitToOpen()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    /// <summary>
    /// 保存并退出
    /// </summary>
    public void ExitGame()
    {
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
    }

    /// <summary>
    /// 新游戏
    /// </summary>
    public void NewGame()
    {
        // 清空存档
        SaveMgr.GetInstance().CleanSave();
        PlayButtonClick();
        UIAnim.Play("CloseChapter");
        Invoke("LoadNewGame",1.5f);
    }

    private void LoadNewGame()
    {
        SceneManager.LoadScene("Ch1");
    }

    /// <summary>
    /// 加载存档
    /// </summary>
    public void LoadSave()
    {
        PlayButtonClick();
        Invoke("LoadNewGame",1.5f);
    }
    /// <summary>
    /// hover音效
    /// </summary>
    public void PlayButtonOver()
    {
        soundEffect.clip = (AudioClip)Resources.Load("Music/button_over");
        soundEffect.Play();
    }

    public void PlayButtonClick()
    {
        soundEffect.clip = (AudioClip)Resources.Load("Music/button_click");
        soundEffect.Play();
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
