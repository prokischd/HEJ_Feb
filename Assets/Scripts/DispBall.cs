using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispBall : MonoBehaviour
{
    [SerializeField]
    private GameObject ballSprite;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetBallSpriteTrue()
    {
        ballSprite.SetActive(true);
    }

    public void SetBallSpriteFalse()
    {
        ballSprite.SetActive(false);
    }
}
