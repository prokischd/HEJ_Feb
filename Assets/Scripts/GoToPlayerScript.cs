using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPlayerScript : MonoBehaviour
{
    private GameObject player;

    private Vector3 refPos;
    private float time=0.5f;
    private float distance;


    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        transform.position=Vector3.SmoothDamp(transform.position, player.transform.position, ref refPos, time);
        distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= 0.5f)
        {
            gameObject.SetActive(false);
        }
    }
}
