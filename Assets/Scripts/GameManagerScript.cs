using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public delegate void CanControl(bool canCont);
    public delegate void lightControl(float ratio);
    public delegate void CoinReset();


    public event CanControl myCanContol;
    public event lightControl myLightControl;
    public event CoinReset myCoinReset;

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

    public void CallMyCoinReset()
    {
        if (myCoinReset != null)
        {
            myCoinReset();
        }
    }



}
