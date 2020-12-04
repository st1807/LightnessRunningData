using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorsManager : MonoBehaviour
{
    Transform leftWeapon;
    Transform rightWeapon;
    GameObject charactorObj;

    Transform myTrans;

    // Collider humanCollider;
    Camera controlCamera;
    Rigidbody humanRigid;
    // Vector3 moveX,moveY ;

    Vector3 moveDir;
    Animator animator;
    float vertical,horizontal;
    float basicSpeed = 2;
    float speedPower = 1;

    float moveLimit=0;

    Quaternion moveRotation;
    bool canMove = false;

    bool jump;

    bool fire01;
    bool isRoll;

    bool isOnGround=true;

    bool isSpeedUp;

    bool isDizzy;

    CameraController cameraController;

    string[] attackStrs = new string[]{"Attack01","Attack02"};

    void Start()
    {
        // humanCollider=GetComponent<Collider>();
        myTrans=transform;
        humanRigid=GetComponent<Rigidbody>();

        controlCamera=Camera.main;

        charactorObj=myTrans.Find("charactor").gameObject;

        rightWeapon = myTrans.Find("charactor/root/weaponShield_r");
        leftWeapon = myTrans.Find("charactor/root/weaponShield_l");

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
    void LateUpdate()
    {
        UpdateAnimate();
    }

    void UpdateData()
    {
        vertical=Input.GetAxis("Vertical");
        horizontal=Input.GetAxis("Horizontal");
        jump = Input.GetButtonDown("Jump");
        fire01 = Input.GetButtonDown("Fire1");
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


    }

    void FixedMove()
    {
        if (isOnGround)
        {
            if (canMove)
            {
                humanRigid.velocity=moveDir;// human move;
                animator.SetFloat("speed",moveLimit,0.1f,Time.fixedDeltaTime);
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

    void UpdateAnimate()
    {

        if (fire01&&canMove)
        {
            if (isOnGround)
            {
                int temp = Random.Range(0,2);
                animator.CrossFade(attackStrs[temp],0.1f);
            }
            else
            {
                animator.CrossFade("JumpAttack01",0.1f);
            }

            fire01 = false;
        }
    }

    IEnumerator ArcherShoot()
    {
        yield return new WaitForSeconds(0.6f);

        GameObject obj = Instantiate(rightWeapon.gameObject,rightWeapon.parent);
        obj.transform.position = rightWeapon.position;
        obj.transform.rotation = rightWeapon.rotation;
        obj.AddComponent<FlyThings>();
        rightWeapon.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.2f);
        
        rightWeapon.gameObject.SetActive(true);
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
