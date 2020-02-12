﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControlsScript : MonoBehaviour
{
    private GameObject touchPos;
    private GameObject touchCenter;
    private Camera cam;

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
   


    void Start()
    {
        SetinitialReferences();
    }

    void Update()
    {
        SetToFingerPos();
        speedNow = Mathf.SmoothDamp(speedNow, speed, ref refFloat, 2f);
       
    }

    void FixedUpdate()
    {
        //MovePlayer();
    }

    void SetinitialReferences()
    {
        cam = Camera.main;
        touchPos = transform.GetChild(0).gameObject;
        touchCenter = transform.GetChild(1).gameObject;
        playerObj = GameObject.Find("Player");
        playerRb = playerObj.GetComponent<Rigidbody2D>();
    }


    void SetToFingerPos()
    {

            if (Input.GetMouseButton(0))
        {
            touchPos.transform.position=cam.ScreenToWorldPoint(Input.mousePosition);


            vectorToTarget = touchPos.transform.position - touchCenter.transform.position;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            q = Quaternion.AngleAxis(angle, Vector3.forward);
            //touchCenter.transform.rotation = Quaternion.Slerp(touchCenter.transform.rotation, q, Time.deltaTime * 10f);
            touchCenter.transform.rotation = q;






            CalculateSpeed();
            MovePlayer();
        }

        if (Input.GetMouseButtonUp(0))
        {
            speed = 0;
            speedNow = 0;

            //zero floats
        }

    }

   



    void CalculateSpeed()
    {
        
        Quaternion rotAngle = Quaternion.Inverse(rot) * touchCenter.transform.localRotation;
        turnAngles.Insert(0, rotAngle.z);
        turnAngles.RemoveAt(2);
        rot = touchCenter.transform.localRotation;
        turnSize.Insert(0, turnAngles[0] * 1000 - turnAngles[1] * 1000);
        turnSize.RemoveAt(10);


        for (int i = 0; i < turnSize.Count; i++)
            {
                speed += turnSize[i];
            }
            speed = (speed / (turnSize.Count));
        
        

    }


    void MovePlayer() {
        playerRb.AddTorque(20*speedNow, ForceMode2D.Force);
        playerRb.angularVelocity = 30f;
    }


}
