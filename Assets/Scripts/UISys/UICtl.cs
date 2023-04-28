using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UICtl : SingletonMono<UICtl>
{
    public AudioSource bgm;
    public AudioSource soundEffect;
    public GameObject[] panelElement;
    public UnityEvent bgmInit;
    public UnityEvent soundInit;
    [SerializeReference] private GameObject continueGame;   // 加载存档按钮
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
        bgm = GameObject.FindWithTag("Music").GetComponent<AudioSource>();
        soundEffect = GameObject.FindWithTag("Sound").GetComponent<AudioSource>();
        T_ChapterID = GameObject.Find("T_ChapterID").GetComponent<TMP_Text>();
        T_ChapterName = GameObject.Find("T_ChapterName").GetComponent<TMP_Text>();
    }

    public void Start()
    {
        bgmInit.Invoke();
        soundInit.Invoke();
        // init volume
        try
        {
            bgm.volume = PlayerPrefs.GetFloat("bgm");
            soundEffect.volume = PlayerPrefs.GetFloat("audio");
        }
        catch (Exception e)
        {
            bgm.volume = 0.8f;
            soundEffect.volume = 0.8f;
        }

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

    public bool GetPanelState()
    {
        return isOpen;
    }
    
    /// <summary>
    /// 在关卡中打开设置面板，暂停游戏时间
    /// </summary>
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
            print(panelElement[0].gameObject.name);
            PanelList.Push(panelElement[0]);
            PanelList.Peek().SetActive(true);
            isOpen = true;
        }
    }
    
    /// <summary>
    /// 在初始界面中打开设置面板，不需要暂停时间
    /// </summary>
    public void ShowPanelWithoutChangeTime()
    {
        // 底层面板打开时，按esc关闭
        if (isOpen)
        {
            PanelList.Peek().SetActive(false);
            PanelList.Pop();
            if(PanelList.Count == 0)
                isOpen = false;
        }
        // 底层面板关闭时，按esc打开
        else
        {
            print(panelElement[0].gameObject.name);
            PanelList.Push(panelElement[0]);
            PanelList.Peek().SetActive(true);
            isOpen = true;
        }
    }

    /// <summary>
    /// 展示第二层面板
    /// </summary>
    public void ShowSecondPanel()
    {
        PanelList.Peek().SetActive(false);
        PanelList.Push(panelElement[1]);
        PanelList.Peek().SetActive(true);
    }

    /// <summary>
    /// 关闭第二层面板
    /// </summary>
    public void CloseSecondPanel()
    {
        PanelList.Peek().SetActive(false);
        PanelList.Pop();
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
    /// 打开项目的Github页面
    /// </summary>
    public void AboutProject()
    {
        Application.OpenURL("https://github.com/Guiny-Time/StarWay");
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

    public void InitBgm(Slider s)
    {
        s.value = PlayerPrefs.GetFloat("bgm");
    }

    public void InitAudio(Slider s)
    {
        s.value = PlayerPrefs.GetFloat("audio");
    }

    /// <summary>
    /// 设置bgm音量
    /// </summary>
    /// <param name="s"></param>
    public void ChangeBgmVolum(Slider s)
    {
        // bgm.volume = Mathf.Lerp(bgm.volume, s.value, 0.005f);
        bgm.volume = s.value;
        MusicMgr.GetInstance().SetBgmVol(s.value);
    }

    /// <summary>
    /// 设置音效音量
    /// </summary>
    /// <param name="s"></param>
    public void ChangeEffectVolum(Slider s)
    {
        soundEffect.volume = s.value;
        MusicMgr.GetInstance().SetEffectVol(s.value);
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
