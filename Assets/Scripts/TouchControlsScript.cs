using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlsScript : MonoBehaviour
{
    private GameObject touchPos;
    private GameObject touchCenter;
    private Camera cam;
    public float maxCenreDistance = 0.1f;
    private Vector2 refCentrePos;
    public float maxAngularVelocity=120;
    public float ControlEffect = 1000;

    private Vector3 vectorToTarget;
    private float angle;
    private Quaternion q;

    private float speed;
    public float speedNow;
    private float refFloat;
    public List<float> turnAngles=new List<float>(2);
    public List<float> turnSize = new List<float>(10);
    private Quaternion rot;

    private float time;

    private GameObject playerObj;
    private Rigidbody2D playerRb;

    [SerializeField]
    private Animator playerAnim;
   


    void Start()
    {
        SetinitialReferences();
    }

    void Update()
    {
        SetToFingerPos();
        speedNow = Mathf.SmoothDamp(speedNow, speed, ref refFloat, 0.5f);
        MoveToCentre();
    }

    void FixedUpdate()
    {
        //playerRb.angularVelocity = 30f;
        MovePlayer();
        SetPlayerAnimation();
    }

    void SetinitialReferences()
    {
       
        cam = Camera.main;
        touchPos = transform.GetChild(0).gameObject;
        touchCenter = transform.GetChild(1).gameObject;
        playerObj = GameObject.Find("Player");
        playerAnim = playerObj.transform.parent.transform.GetChild(1).GetChild(0).GetComponent<Animator>();
        playerRb = playerObj.GetComponent<Rigidbody2D>();
        for (int i = 0; i < turnAngles.Count; i++)
        {
            turnAngles[i] = 0;
        }
        for (int l = 0; l < turnSize.Count; l++)
        {
            turnSize[l] = 0;
        }

    }




    void MoveToCentre() {
        float distance = Vector2.Distance(touchCenter.transform.position, touchPos.transform.position);
        if (distance>maxCenreDistance)
        {
            touchCenter.transform.position = Vector2.SmoothDamp(touchCenter.transform.position, touchPos.transform.position, ref refCentrePos, 0.05f);
        }
    }

    void SetToFingerPos()
    {
        if (Input.GetMouseButtonDown(0))
        {
            speed = speedNow;
        }
            if (Input.GetMouseButton(0))
        {
            touchPos.transform.position=cam.ScreenToWorldPoint(Input.mousePosition);


            vectorToTarget = touchPos.transform.position - touchCenter.transform.position;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            q = Quaternion.AngleAxis(angle,Vector3.forward) ;
            //touchCenter.transform.rotation = Quaternion.Slerp(touchCenter.transform.rotation, q, Time.deltaTime * 10f);
            touchCenter.transform.rotation = q;






            CalculateSpeed();
            //MovePlayer();
        }

        if (Input.GetMouseButtonUp(0))
        {
            speed = 0;
            //speedNow = 0;

            //zero floats
        }

    }

   



    void CalculateSpeed()
    {
        
        Quaternion rotAngle = Quaternion.Inverse(rot) * touchCenter.transform.localRotation;
        turnAngles.Insert(0, rotAngle.z);
        turnAngles.RemoveAt(2);
        rot = touchCenter.transform.localRotation;
        turnSize.Insert(0, turnAngles[0] * ControlEffect - turnAngles[1] * ControlEffect);
        turnSize.RemoveAt(10);


        for (int i = 0; i < turnSize.Count; i++)
            {
                speed += turnSize[i];
            }
            speed = (speed / (turnSize.Count));
        
        

    }


    void MovePlayer() {
        if (Mathf.Abs(playerRb.angularVelocity) > maxAngularVelocity)
        {

            playerRb.angularVelocity = maxAngularVelocity;

            
        }
        //SetPlayerAnimation();
        playerRb.AddTorque(speedNow*20, ForceMode2D.Force);
        Debug.Log(playerRb.velocity);
    }

    void SetPlayerAnimation() {
        if (playerRb.velocity.magnitude <= 0.1f )
        {
            playerAnim.SetBool("isWalking", false);
            playerAnim.SetBool("isRunning", false);
            playerAnim.SetBool("isBall", false);
        }
        else
        {
                 if (Mathf.Abs(playerRb.angularVelocity) > 0 && Mathf.Abs(playerRb.angularVelocity) < (maxAngularVelocity / 3))
                {
                    playerAnim.SetBool("isWalking", true);
                    playerAnim.SetBool("isRunning", false);
                    playerAnim.SetBool("isBall", false);
                }
                else if (Mathf.Abs(playerRb.angularVelocity) > (maxAngularVelocity / 3) && Mathf.Abs(playerRb.angularVelocity) < (maxAngularVelocity / 3)*2)
                {
                    playerAnim.SetBool("isRunning", true);
                    playerAnim.SetBool("isBall", false);
                    playerAnim.SetBool("isWalking", false);

                }else if (Mathf.Abs(playerRb.angularVelocity) >= (maxAngularVelocity/3)*2)
                {
                    playerAnim.SetBool("isBall", true);

                }
        }
        


    }


}
