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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public GameObject GetCurrentMouse()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitData;
        if(Physics.Raycast(ray, out hitData, 1000))
        {
            mousePosition = hitData.point;
            mapCollider = hitData.collider.transform.gameObject;

        }
        
        return mapCollider;
    }
}
