using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public UnityEvent levelOverAnim;
    public Animator chapter;
    public AudioSource music;

    private bool flag;  // 音乐开始减小
    // Start is called before the first frame update
    void Start()
    {
        chapter = GameObject.FindWithTag("AnimCanvas").GetComponent<Animator>();
        music = GameObject.FindWithTag("Music").GetComponent<AudioSource>();
        music.volume = 1;
        flag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (flag)
        {
            music.volume = Mathf.Lerp(music.volume, 0, 0.005f);
        }
    }
    
    public void OnTriggerEnter(Collider collider){
        print("door catch " + collider.name);
        levelOverAnim.Invoke();
        flag = true;
        chapter.Play("CloseChapter");
        if (SaveMgr.GetInstance().LastChapter() && SaveMgr.GetInstance().LastLevel())
        {
            // 完结了，看看到时候搞个结束Timeline吧
            print(PlayerPrefs.GetInt("Chapter") + "-" +  PlayerPrefs.GetInt("Level"));
            SceneManager.LoadScene("Open");
        }else if (SaveMgr.GetInstance().LastLevel())
        {
            // 进入下一章
            PlayerPrefs.SetString("Progress","Chap" + PlayerPrefs.GetInt("Chapter")+"-1");
            Invoke(nameof(LoadScene),2.0f);
        }
        else
        {
            int nextLevel = PlayerPrefs.GetInt("Level") + 1;
            PlayerPrefs.SetString("Progress","Chap" + PlayerPrefs.GetInt("Chapter")+"-" + nextLevel.ToString());
            Invoke(nameof(LoadScene),2.0f);
        }
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
