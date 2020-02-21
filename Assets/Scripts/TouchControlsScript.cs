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
    private float speedVelocity=4;
    public float maxSpeedVelocity = 7f;
    public float minSpeedVelocity = 4f;
    private float ControlEffect = 1000;


    public float goStrength = 30;
    public float accelartion = 5;

    private Vector3 vectorToTarget;
    private float angle;
    private Quaternion q;

    public float speed;
    public float speedNow;
    private float refFloat;
    private List<float> turnAngles=new List<float>(2);
    private List<float> turnSize = new List<float>(10);
    private Quaternion rot;

    private float time;

    private GameObject playerObj;
    private Rigidbody2D playerRb;

    [SerializeField]
    private Animator playerAnim;

    private bool isMove;
    private bool moveRight;


    void Start()
    {
        SetinitialReferences();
    }

    void Update()
    {
        SetToFingerPos();
        //IncreaseMaxSpeed();
        MoveToCentre();
    }

    void IncreaseMaxSpeed()
    {
       
        if (Mathf.Abs(speedNow) > 0.5f)
        {
            speedVelocity += Time.deltaTime;
        }
        else
        {
            speedVelocity -= Time.deltaTime;
        }
        speedVelocity = Mathf.Clamp(speedVelocity, minSpeedVelocity, maxSpeedVelocity);

    }

    void FixedUpdate()
    {
        //playerRb.angularVelocity = 30f;

        //MovePlayer();
        SetPlayerAnimation();
        MoveComparedToGround();
    }

    void SetinitialReferences()
    {
        speedVelocity = minSpeedVelocity;
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
        for (int l = 0; l < 10; l++)
        {
            turnSize.Add(0);
        }

    }

  
    void MoveComparedToGround()
    {
        if (isMove)
        {
            //playerRb.AddForce(playerAnim.gameObject.transform.right * (-goStrength * speedNow));
            playerRb.AddForce(Vector3.right * (-goStrength * speedNow),ForceMode2D.Force);
            //playerRb.velocity = playerAnim.gameObject.transform.right*-20*speedVelocity * speedNow*Time.deltaTime;

            playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, speedVelocity);

            Debug.Log(playerRb.velocity.magnitude);
        }


    }



    void MoveToCentre() {
        float distance = Vector2.Distance(touchCenter.transform.position, touchPos.transform.position);
        if (distance>maxCenreDistance)
        {
            touchCenter.transform.position = Vector2.SmoothDamp(touchCenter.transform.position, touchPos.transform.position, ref refCentrePos, 0.1f);
        }
    }

    void SetToFingerPos()
    {
        if (Input.GetMouseButtonDown(0))
        {
            speed = speedNow;
            
            isMove = true;
        }
            if (Input.GetMouseButton(0))
        {
            touchPos.transform.position=cam.ScreenToWorldPoint(Input.mousePosition);


            vectorToTarget = touchPos.transform.position - touchCenter.transform.position;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            q = Quaternion.AngleAxis(angle,Vector3.forward) ;
            touchCenter.transform.rotation = Quaternion.Slerp(touchCenter.transform.rotation, q, Time.deltaTime * 10f);
            //touchCenter.transform.rotation = q;






            CalculateSpeed();
            //MovePlayer();
        }

        if (Input.GetMouseButtonUp(0))
        {
            isMove = false;
            speed = 0;
            //playerRb.velocity = new Vector3(0,playerRb.velocity.y, 0);
            speedNow = 0;

            //zero floats
        }

    }



    public float rotAngle;

    void CalculateSpeed()
    {

        //Quaternion rotAngle = Quaternion.Inverse(rot) * touchCenter.transform.localRotation;
        ////float rotAngle = Quaternion.Angle(rot, touchCenter.transform.localRotation);

        //turnAngles.Insert(0, rotAngle.z);
        //turnAngles.RemoveAt(2);
        //rot = touchCenter.transform.localRotation;

        //turnSize.Insert(0, turnAngles[0] * ControlEffect - turnAngles[1] * ControlEffect);
        //turnSize.RemoveAt(10);

        //speed = 0;
        //for (int i = 0; i < turnSize.Count; i++)
        //    {
        //        speed += turnSize[i];
        //    }
        //speed = (speed / (turnSize.Count));


        //rotAngle = Quaternion.Angle(rot, touchCenter.transform.localRotation);

       
        rotAngle = Mathf.DeltaAngle(rot.z, touchCenter.transform.localRotation.z)*1000;

        
        
        rot = touchCenter.transform.localRotation;

        turnSize.Insert(0, rotAngle);
        turnSize.RemoveAt(turnSize.Count-1);


        //speedNow = Mathf.SmoothDamp(speedNow, speed, ref refFloat, 0.2f);
        speedNow = Mathf.SmoothDamp(speedNow, rotAngle, ref refFloat, 0.4f);

        if (speedNow > 30)
        {
            if (moveRight)
            {
                moveRight = false;
                goStrength = minSpeedVelocity;
            }
            goStrength += accelartion*Time.deltaTime;
            goStrength = Mathf.Clamp(goStrength, minSpeedVelocity, maxSpeedVelocity);
        }else if (speedNow < -30)
        {
            if (!moveRight)
            {
                moveRight = true;
                goStrength = minSpeedVelocity;
            }
            goStrength += accelartion*Time.deltaTime;
            goStrength = Mathf.Clamp(goStrength, minSpeedVelocity, maxSpeedVelocity);
        }else if(speedNow>-30 && speedNow < 30)
        {
            goStrength -= Time.deltaTime;
            goStrength = Mathf.Clamp(goStrength, minSpeedVelocity, maxSpeedVelocity);
        }

        //speed = 0;
        //for (int i = 0; i < turnSize.Count; i++)
        //{
        //    speed += turnSize[i];
        //}
        //speed = (speed / (turnSize.Count));


    }


    void MovePlayer() {
        if (Mathf.Abs(playerRb.angularVelocity) > speedVelocity)
        {

            playerRb.angularVelocity = speedVelocity;

            
        }
        //SetPlayerAnimation();
        playerRb.AddTorque(speedNow*20, ForceMode2D.Force);
        //Debug.Log(playerRb.velocity.magnitude);
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
                 if (Mathf.Abs(playerRb.velocity.magnitude) > 0 && Mathf.Abs(playerRb.velocity.magnitude) < (speedVelocity / 4))
                {
                    playerAnim.SetBool("isWalking", true);
                    playerAnim.SetBool("isRunning", false);
                    playerAnim.SetBool("isBall", false);
                }
                else if (Mathf.Abs(playerRb.velocity.magnitude) > (speedVelocity / 4) && Mathf.Abs(playerRb.velocity.magnitude) < (speedVelocity / 4)*2)
                {
                    playerAnim.SetBool("isRunning", true);
                    playerAnim.SetBool("isBall", false);
                    playerAnim.SetBool("isWalking", false);

                }else if (Mathf.Abs(playerRb.velocity.magnitude) >= (speedVelocity/4)*2)
                {
                    playerAnim.SetBool("isBall", true);

                }
        }
        


    }


}
