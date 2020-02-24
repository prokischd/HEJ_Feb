using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public delegate void CanControl(bool canCont);
    public delegate void lightControl(float ratio);

    public event CanControl myCanContol;
    public event lightControl myLightControl;

    public void CallMyCanControl(bool canCont)
    {
        if (myCanContol != null)
        {
            myCanContol(canCont);
        }
    }

    public void CallMyLightControl(float ratio)
    {
        if (myCanContol != null)
        {
            myLightControl(ratio);
        }
    }



}
