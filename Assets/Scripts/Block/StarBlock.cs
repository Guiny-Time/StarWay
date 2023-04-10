using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBlock : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //触发开始 只调用一次
    public void OnTriggerEnter(Collider collider){
        print(collider.name);
        PlayerPrefs.SetInt("CollectionNum", PlayerPrefs.GetInt("CollectionNum") + 1);
        gameObject.SetActive(false);
    }
}
