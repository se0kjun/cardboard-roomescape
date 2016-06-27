using UnityEngine;
using System.Collections;

public class ObjectSelection : MonoBehaviour {
    public Material defaultMat;
    public Material glowMat;
    public Material grabMat;

    public void SetGlowSelection()
    {
        //GetComponent<Renderer>().material = glowMat;
    }

    public void UnSetGlowSelection()
    {
        //GetComponent<Renderer>().material = defaultMat;
    }

    public void SetGlowGrab()
    {
        if (GetComponent<BookTrick>() == null)
            GetComponent<ITrickManager>().onEventMethod();
        GetComponent<ITrickManager>().GrabFlag = true;
        //GetComponent<Renderer>().material = grabMat;
    }

    public void ReleaseObject()
    {
        if (GetComponent<BookTrick>() != null)
            GetComponent<ITrickManager>().onEventMethod();

        GetComponent<ITrickManager>().GrabFlag = false;
    }
}
