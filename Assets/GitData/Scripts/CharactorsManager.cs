using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorsManager : MonoBehaviour
{
    // Collider humanCollider;
    Camera controlCamera;
    Rigidbody humanRigid;
    Vector3 moveX,moveY , moveDir;

    GameObject charactorObj;

    Animator animator;
    bool jump;
    float vertical,horizontal;
    float basicSpeed = 2;
    float speedPower = 1;

    float moveLimit=0;

    Quaternion moveRotation;
    bool canMove = false;

    bool isJump;

    bool isRoll;

    bool isOnGround=true;

    bool isSpeedUp;

    bool isDizzy;

    Transform myTrans;

    CameraController cameraController;

    void Start()
    {
        // humanCollider=GetComponent<Collider>();
        myTrans=transform;
        humanRigid=GetComponent<Rigidbody>();

        controlCamera=Camera.main;

        charactorObj=transform.Find("charactor").gameObject;
        animator=charactorObj.GetComponent<Animator>();

        if (moveRotation==null)
        {
            moveRotation= Quaternion.LookRotation(myTrans.forward);
        }

        cameraController = CameraController.instance;
        cameraController.BindTarget(myTrans);

    }

    void FixedUpdate()
    {
        FixedMove();

        cameraController.FixedRotate();
    }
    void Update()
    {
        UpdateData();
        UpdateState();
    }

    void UpdateData()
    {
        vertical=Input.GetAxis("Vertical");
        horizontal=Input.GetAxis("Horizontal");
        jump = Input.GetButtonDown("Jump");
    }

    void UpdateState()
    {
        if (jump)
        {
            if(isOnGround)
            {
                animator.CrossFade("jumpBegin",0.2f);
                humanRigid.AddForce(0,300,0);
            }
        }


        // moveX = vertical * transform.forward;
        // moveY = horizontal * transform.right;
        moveX = vertical * controlCamera.transform.forward;
        moveY= horizontal * controlCamera.transform.right;
        float moveSpeed= basicSpeed * speedPower;

        moveDir = ((moveX + moveY).normalized) * moveSpeed * moveLimit;
        moveDir.y=humanRigid.velocity.y;

        float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        moveLimit = Mathf.Clamp01(m);

        animator.SetFloat("speed",moveLimit,0.1f,Time.fixedDeltaTime);

    }

    void FixedMove()
    {

        if (moveDir.x!=0 ||moveDir.z!=0)
        {
            // Debug.Log(moveDir);

            // animator.SetBool("canMove",true);
            canMove=true;
        }
        else
        {
            // animator.SetBool("canMove",false);
            canMove=false;
        }

        // canMove=animator.GetBool("canMove");
        if (canMove)
        {
            humanRigid.velocity=moveDir;// human move;

            Quaternion targetQua= Quaternion.LookRotation(moveDir);
            moveRotation = Quaternion.Slerp(myTrans.rotation,targetQua,Time.fixedDeltaTime * 5);
            myTrans.rotation=moveRotation;
        }

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag.Equals("Ground"))
        {
            isOnGround = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.tag.Equals("Ground"))
        {
            isOnGround = false;
        }
    }

}
