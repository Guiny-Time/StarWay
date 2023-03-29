using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class InputMgr : BaseManager<InputMgr>
{
    protected Vector3 mousePosition;
    private Ray ray;
    private RaycastHit hit;
    private GameObject mapCollider;

    public void UIListener()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 弹出UI
            Debug.Log("弹出设置界面");
            // Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 暂停
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

    }
    
    
    public GameObject GetCurrentMouse()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        // 仅检测地块层(Ground)
        int layerMask = 1 << 3;
        if(Physics.Raycast(ray, out hitData, 1000, layerMask))
        {
            mousePosition = hitData.point;
            mapCollider = hitData.collider.transform.gameObject;
        }
        
        return mapCollider;
    }
}
