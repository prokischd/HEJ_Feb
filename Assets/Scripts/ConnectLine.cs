using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectLine : MonoBehaviour
{
    private LineRenderer myLine;
    private GameObject[] dotPoses;


    void Start()
    {
        myLine = GetComponent<LineRenderer>();
        dotPoses = new GameObject[transform.childCount];
        myLine.positionCount = dotPoses.Length;
        for (int i = 0; i < dotPoses.Length; i++)
        {
            dotPoses[i] = transform.GetChild(i).gameObject;
            myLine.SetPosition(i, dotPoses[i].transform.position);
        }
    }

    void Update()
    {
        for (int i = 0; i < dotPoses.Length; i++)
        {
            myLine.SetPosition(i, dotPoses[i].transform.position);
        }
    }
}
