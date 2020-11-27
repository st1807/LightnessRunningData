using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    private void Awake()
    {
        instance=this;
    }
    Transform myTrans;
    Transform targetTrans;
    Transform targetPivot;

    float rotateSpeed = 5;
    float followSpeed= 6;

    float smoothX, smoothY , smoothVelocityX ,smoothVelocityY;
    float angleY , angleX;

    float smoothNum = 0.5f;
    
    void Start()
    {
        myTrans= transform;
    }

    public void BindTarget(Transform _target)
    {
        targetTrans = _target;
        targetPivot = transform.Find("Pivot");
    }

    public void FixedRotate()
    {
        if (targetTrans==null)
        {
            return;
        }
        float rotateX = Input.GetAxis("Mouse X");
        float rotateY = Input.GetAxis("Mouse Y");
        HandleRotation(rotateX , rotateY);
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 pos= Vector3.Lerp(myTrans.position,targetTrans.position,Time.fixedDeltaTime * followSpeed);
        myTrans.position = pos;
    }
    private void HandleRotation(float _ver, float _hor)
    {
        smoothX = Mathf.SmoothDamp(smoothX , _ver, ref smoothVelocityX, smoothNum);
        smoothY = Mathf.SmoothDamp(smoothY , _hor, ref smoothVelocityY, smoothNum);

        
        angleX += smoothX * rotateSpeed;
        myTrans.rotation = Quaternion.Euler(0 , angleX , 0);

        if (targetPivot!=null)
        {
            angleY -= smoothY * rotateSpeed;
            angleY = Mathf.Clamp(angleY ,-45f ,45f);
            targetPivot.localRotation = Quaternion.Euler(angleY , 0 , 0);
            // Debug.Log("rotation : "+targetPivot.rotation);
        }

    }
}
