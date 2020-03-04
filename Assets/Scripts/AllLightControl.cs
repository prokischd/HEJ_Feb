using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine;

public class AllLightControl : MonoBehaviour
{

    private GameManagerScript gameManagerScript;
    private Light2D myLight;
    private float lightMax;
    private float lightNow;
    private float refFloat;

    void OnEnable()
    {
        SetInitialReferences();
        gameManagerScript.myLightControl += SetLightIntensity;
    }

    void OnDisabel() {
        gameManagerScript.myLightControl -= SetLightIntensity;

    }


    void Update()
    {
        if (lightNow != myLight.intensity)
        {
            myLight.intensity = Mathf.SmoothDamp(myLight.intensity, lightNow, ref refFloat, 0.3f);
        }
    }

    void SetInitialReferences() {
        lightNow = 0;
        myLight = GetComponent<Light2D>();
        lightMax = myLight.intensity;
        myLight.intensity = 0;
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
    }

    void SetLightIntensity(float ratio)
    {
        //myLight.intensity=(ratio*lightMax);
        lightNow = ratio*lightMax;
    }



}
