using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    private GameObject spriteObj;
    private bool isGrounded;
    private Vector3 vectorToTarget;
    private float angle;
    private Quaternion q;
    private Vector3 colPos;


    void Start()
    {
        SetInitialReferences();
    }

    void FixedUpdate()
    {
        spriteObj.transform.position = transform.position;
    }

    void SetInitialReferences() {
        spriteObj = transform.parent.GetChild(1).gameObject;
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (col.gameObject.layer == 8)
        {
            colPos = col.contacts[0].point;

            vectorToTarget = colPos - transform.position;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            q = Quaternion.AngleAxis(angle, Vector3.forward);
            spriteObj.transform.rotation = Quaternion.Slerp(spriteObj.transform.rotation, q, Time.deltaTime * 10f);

            Debug.Log(colPos);

        }

    }
}
