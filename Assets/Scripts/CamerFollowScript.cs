using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFollowScript : MonoBehaviour
{
    private float speed=0.1f;
    private GameManagerScript gameManager;
    private GameObject playerObj;
    private Vector3 refPos;




    void Start()
    {
        SetInitialReferences();
    }

    void Update()
    {
        FollowPlayer();   
    }

    void SetInitialReferences() {
        playerObj = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        transform.position = new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, transform.position.z);

    }


    void FollowPlayer() {
        transform.position = Vector3.SmoothDamp(transform.position, new Vector3(playerObj.transform.position.x, playerObj.transform.position.y, transform.position.z), ref refPos, speed);
    }
}
