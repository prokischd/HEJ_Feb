using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class playerScript : MonoBehaviour
{
    private GameObject spriteObj;
    private bool isGrounded;
    private Vector3 vectorToTarget;
    private float angle;
    private Quaternion q;
    private Vector3 colPos;
    private Rigidbody2D myBody;
    private AudioManager audioManager;
    public float pitchCounter=3;
    private float coinTimeOffset=0.5f;
    public float coinTime;
    

    public RaycastHit2D hit;

    public LayerMask hitLayers;

    private GameManagerScript gameManagerScript;

    void Start()
    {
        SetInitialReferences();
    }

    void Update()
    {
        DoRaycasting();
        spriteObj.transform.position = transform.position;
        CountCoins();
    }

    void SetInitialReferences() {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        myBody = GetComponent<Rigidbody2D>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        spriteObj = transform.parent.GetChild(1).gameObject;
    }

    public float distance = 3f;

    void DoRaycasting()
    {
        // Cast a ray straight down.
        hit = Physics2D.Raycast(transform.position, -spriteObj.transform.up, distance, hitLayers);




        // If it hits something...
        if (hit.collider != null)
        {

            Debug.DrawRay(hit.point, hit.normal, Color.red);
            //Debug.DrawRay(transform.position, -transform.up * distance, Color.blue);
            //Debug.DrawLine(hit.point, hit.normal, Color.red);
            Debug.DrawLine(transform.position, hit.point, Color.blue);




            if (!isGrounded)
            {
                isGrounded = true;
                gameManagerScript.CallMyCanControl(true);
            }

            colPos = hit.point + hit.normal * -distance;

            vectorToTarget = colPos - transform.position;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            q = Quaternion.AngleAxis(angle, transform.forward);
            q = Quaternion.Euler(0, 0, 90) * q;
            spriteObj.transform.rotation = Quaternion.Slerp(spriteObj.transform.rotation, q, Time.deltaTime * 10f);
            //transform.rotation = Quaternion.Slerp(spriteObj.transform.rotation, q, Time.deltaTime * 10f);

            float distanceGround = Vector3.Distance(transform.position, hit.point);

            //myBody.AddForce(hit.normal * (-40f * (distance - distanceGround)), ForceMode2D.Force);
            myBody.AddForce(hit.normal * (-10f * myBody.velocity.magnitude), ForceMode2D.Force);

        }
        else
        {
            if (isGrounded)
            {
                isGrounded = false;
                gameManagerScript.CallMyCanControl(false);
            }
            colPos = transform.position + Vector3.forward * -distance;
            vectorToTarget = colPos - transform.position;
            angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            q = Quaternion.AngleAxis(angle, transform.forward);
            q = Quaternion.Euler(0, 0, 0) * q;
            spriteObj.transform.rotation = Quaternion.Slerp(spriteObj.transform.rotation, q, Time.deltaTime * 10f);
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == 11)
        {
            Debug.Log("coin");
            col.GetComponent<coinScript>().PlayerTouch();
            audioManager.PlayPitch("Coin", pitchCounter);
            coinTime = 0;
            pitchCounter -= 0.4f;
            pitchCounter = Mathf.Clamp(pitchCounter, 0.8f, 3f);

        }
    }

    void CountCoins() {
        if (coinTime <= coinTimeOffset)
        {
            coinTime += Time.deltaTime;
        }
        if (coinTime >= coinTimeOffset)
        {

            pitchCounter = 3;
            gameManagerScript.CallMyCoinReset();
            
        }
    }


}
