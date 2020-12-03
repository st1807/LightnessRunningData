using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyThings : MonoBehaviour
{
    public float flySpeed=10f;
    public float terminateTime= 3f;
    private float fliedTime=0f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime *flySpeed);
        fliedTime+=Time.deltaTime;
        if (fliedTime>=terminateTime)
        {
            Destroy(this.gameObject);
        }
    }
}
