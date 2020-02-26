using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coinScript : MonoBehaviour
{
    private GameManagerScript gameManagerScript;
    private SpriteRenderer myRenderer;
    private Collider2D myCol;



    void Start()
    {
        SetInitialReferences();
    }

    void Update()
    {
        
    }

    void SetInitialReferences() {
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        myRenderer = GetComponent<SpriteRenderer>();
        myCol = GetComponent<Collider2D>();
    }


    public void PlayerTouch()
    {
        gameManagerScript.myCoinReset += ResetAllCoins;
        myCol.enabled = false;
        myRenderer.color = Color.black;
    }

    void ResetAllCoins()
    {
        myRenderer.color = Color.yellow;
        myCol.enabled = true;
        gameManagerScript.myCoinReset -= ResetAllCoins;

    }
}
