using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlsScript : MonoBehaviour
{
    private GameManagerScript gameManagerMaster;

    public GameObject energyPrefab;
    private GameObject energyGroup;
    private float time;
    public float energySpawnOffset = 20f;

    private GameObject touchPos;
    private GameObject touchCenter;
    private Camera cam;
    public float maxCenreDistance = 0.1f;
    private Vector2 refCentrePos;
    private float speedVelocity=4;
    public float maxSpeedVelocity = 7f;
    public float minSpeedVelocity = 4f;


    private float goStrength = 30;
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


    private GameObject playerObj;
    private Rigidbody2D playerRb;

    [SerializeField]
    private Animator playerAnim;

    private bool isMove;
    private bool moveRight;
    private bool canControl=true;

    private float turnFloat;


    void OnEnable()
    {
        SetinitialReferences();
        gameManagerMaster.myCanContol += CanControl;
    }

    void OnDisable()
    {
        gameManagerMaster.myCanContol -= CanControl;

    }

    void LateUpdate()
    {
        SetToFingerPos();
        //IncreaseMaxSpeed();
        MoveToCentre();
    }

   void CanControl(bool canCont)
    {
        canControl = canCont;   
    }

    void FixedUpdate()
    {
        //playerRb.angularVelocity = 30f;

        //MovePlayer();
        SetPlayerAnimation();
        if (canControl)
        {
            MoveComparedToGround();
        }
    }

    void SetinitialReferences()
    {
        energyGroup = new GameObject();


        gameManagerMaster = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
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
            playerRb.AddForce(playerAnim.gameObject.transform.right * (-goStrength * speedNow));
            //playerRb.AddForce(Vector3.right * (-goStrength * speedNow),ForceMode2D.Force);
            //playerRb.velocity = playerAnim.gameObject.transform.right*-20*speedVelocity * speedNow*Time.deltaTime;

            playerRb.velocity = Vector3.ClampMagnitude(playerRb.velocity, speedVelocity);

            gameManagerMaster.CallMyLightControl(playerRb.velocity.magnitude / speedVelocity);

            //Debug.Log(playerRb.velocity.magnitude);
        }
       


    }



    void MoveToCentre() {
        float distance = Vector2.Distance(touchCenter.transform.position, touchPos.transform.position);
        if (distance>maxCenreDistance)
        {
            touchCenter.transform.position = Vector2.SmoothDamp(touchCenter.transform.position, touchPos.transform.position, ref refCentrePos, 0.1f);
        }
        //else if (distance < maxCenreDistance/3)
        //{
        //    touchCenter.transform.position = Vector2.SmoothDamp(touchCenter.transform.position, -touchCenter.transform.up*(maxCenreDistance/3), ref refCentrePos, 1f);

        //}
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


            vectorToTarget = touchPos.transform.localPosition - touchCenter.transform.localPosition;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            q = Quaternion.AngleAxis(angle,Vector3.forward) ;
            touchCenter.transform.rotation = Quaternion.Slerp(touchCenter.transform.rotation, q, Time.deltaTime * 10f);
            //touchCenter.transform.rotation = q;



            CalculateSpeed();
            //MovePlayer();
        }

        if (Input.GetMouseButtonUp(0))
        {
            gameManagerMaster.CallMyLightControl(0);

            goStrength = minSpeedVelocity;
            isMove = false;
            speed = 0;
            //playerRb.velocity = new Vector3(0,playerRb.velocity.y, 0);
            speedNow = 0;
            rotAngle = 0;
            rot = new Quaternion(0,0,0,0);
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

       
        rotAngle = Mathf.DeltaAngle(rot.z, touchCenter.transform.localRotation.z)*500;
        rotAngle = Mathf.Clamp(rotAngle, -30, 30);
        
        
        rot = touchCenter.transform.localRotation;

        turnSize.Insert(0, rotAngle);
        turnSize.RemoveAt(turnSize.Count-1);


        //speedNow = Mathf.SmoothDamp(speedNow, speed, ref refFloat, 0.2f);
        speedNow = Mathf.SmoothDamp(speedNow, rotAngle, ref refFloat, 0.4f);

        if (speedNow > 10)
        {
            if (moveRight)
            {
                moveRight = false;
                goStrength = minSpeedVelocity;
                playerRb.velocity = new Vector3(0, playerRb.velocity.y, 0);
            }
            goStrength += accelartion*Time.deltaTime;
            goStrength = Mathf.Clamp(goStrength, minSpeedVelocity, maxSpeedVelocity);
        }else if (speedNow < -10)
        {
            if (!moveRight)
            {
                moveRight = true;
                goStrength = minSpeedVelocity;
                playerRb.velocity = new Vector3(0, playerRb.velocity.y, 0);
            }
            goStrength += accelartion*Time.deltaTime;
            goStrength = Mathf.Clamp(goStrength, minSpeedVelocity, maxSpeedVelocity);
        }else if(speedNow>-10 && speedNow < 10)
        {
            goStrength -= Time.deltaTime;
            goStrength = Mathf.Clamp(goStrength, minSpeedVelocity, maxSpeedVelocity);
        }


        


        


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
        if (speedNow < -0.1f)
        {
            if (turnFloat != -1)
            {
                turnFloat = -1;
                playerObj.transform.localScale = new Vector3(turnFloat * playerObj.transform.localScale.x, playerObj.transform.localScale.y, 1);
                playerAnim.transform.localScale = new Vector3(turnFloat * playerAnim.transform.localScale.x, playerAnim.transform.localScale.y, 1);

            }
        }
        else if(speedNow > 0.1f )
        {
            if(turnFloat != 1)
            {
                turnFloat = 1;
                playerObj.transform.localScale = new Vector3(turnFloat * playerObj.transform.localScale.x, playerObj.transform.localScale.y, 1);
                playerAnim.transform.localScale = new Vector3(turnFloat * playerAnim.transform.localScale.x, playerAnim.transform.localScale.y, 1);

            }
        }

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


    //void SpawnEnergiesGameObject() {
    //    for (int i = 0; i < energyGroup.transform.childCount; i++)
    //    {
    //        if (!energyGroup.transform.GetChild(i).gameObject.activeSelf)
    //        {
    //            GameObject go = energyGroup.transform.GetChild(i).gameObject;
    //            go.transform.position = touchCenter.transform.position;
    //            go.SetActive(true);
    //            return;
    //        }
    //    }
    //    Instantiate(energyPrefab, touchCenter.transform.position, Quaternion.identity, energyGroup.transform);
    //}

    //void SpawnEnergy()
    //{
    //    time += goStrength*Time.deltaTime;
    //    if (time >= energySpawnOffset)
    //    {
    //        SpawnEnergiesGameObject();
    //        time = 0;
    //    }
    //}

}
