using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorsManager : MonoBehaviour
{
    // Collider humanCollider;
    Camera controlCamera;
    Rigidbody humanRigid;
    // Vector3 moveX,moveY ;

    Vector3 moveDir;
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

        // if (moveRotation==null)
        // {
        //     moveRotation= Quaternion.LookRotation(myTrans.forward);
        // }

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
        canMove = animator.GetBool("canMove");
        if (jump)
        {
            if(isOnGround && canMove)
            {
                animator.CrossFade("jumpBegin",0.2f);
                humanRigid.AddForce(0,300,0);
            }
        }


        // moveX = vertical * transform.forward;
        // moveY = horizontal * transform.right;
        Vector3 moveX = vertical * controlCamera.transform.forward;
        Vector3 moveY= horizontal * controlCamera.transform.right;
        float moveSpeed= basicSpeed * speedPower;

        moveDir = ((moveX + moveY).normalized) * moveSpeed * moveLimit;
        moveDir.y=humanRigid.velocity.y;

        float m = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
        moveLimit = Mathf.Clamp01(m);

        animator.SetFloat("speed",moveLimit,0.1f,Time.fixedDeltaTime);

    }

    void FixedMove()
    {
        if (isOnGround)
        {
            if (canMove)
            {
                humanRigid.velocity=moveDir;// human move;
            }
        }

        if (canMove)
        {
                Vector3 targetDir = moveDir;
                targetDir.y = 0;
                if (targetDir==Vector3.zero)
                {
                    targetDir = transform.forward;
                }
                Quaternion targetQua= Quaternion.LookRotation(targetDir);
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
