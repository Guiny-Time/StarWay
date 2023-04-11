using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnTriggerEnter(Collider collider){
        if (SaveMgr.GetInstance().LastChapter() && SaveMgr.GetInstance().LastLevel())
        {
            // 完结了，看看到时候搞个结束Timeline吧
            SceneManager.LoadScene("Open");
        }
        if (SaveMgr.GetInstance().LastLevel())
        {
            // 进入下一章
            PlayerPrefs.SetString("Progress","Chap" + PlayerPrefs.GetInt("Chapter")+"-1");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }

        int nextLevel = PlayerPrefs.GetInt("Level") + 1;
        PlayerPrefs.SetString("Progress","Chap" + PlayerPrefs.GetInt("Chapter")+"-" + nextLevel.ToString());
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }
}
