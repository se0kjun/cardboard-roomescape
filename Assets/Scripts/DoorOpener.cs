using UnityEngine;
using System.Collections;
using System;

public class DoorOpener : MonoBehaviour, ITrickManager {
    private bool _grabFlag;

    public GameObject doorObject;
    public bool openedDoor;
    private Vector3 grabAngle;

    private GameManager gameManager;
    private ShowStatusMsg statusText;
    private ShowStatusMsg statusTextRight;

    public bool MoveFlag
    {
        get
        {
            return false;
        }
    }

    public bool GrabFlag
    {
        get
        {
            return _grabFlag;
        }
        set
        {
            _grabFlag = value;
        }
    }

    public GameObject TargetObject
    {
        get
        {
            throw new NotImplementedException();
        }

        set
        {
            throw new NotImplementedException();
        }
    }

    public GameObject MovingObject
    {
        get
        {
            return null;
        }
    }

    void Start ()
    {
        _grabFlag = false;
        statusText = GameObject.Find("statusText").GetComponent<ShowStatusMsg>();
        statusTextRight = GameObject.Find("statusText (1)").GetComponent<ShowStatusMsg>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        openedDoor = true;
	}
	
	void Update ()
    {
        Debug.Log(string.Format("keyflag: {0} // opendoor: {1} // grab: {2}" ,gameManager.keyFlag, openedDoor, _grabFlag));
        if (gameManager.keyFlag && openedDoor && _grabFlag)
        {
            if (Mathf.Abs(grabAngle.x - gameManager.myoBaseObject.transform.eulerAngles.x) > 90.0f)
            {
                doorObject.GetComponent<Animation>().Play();
                statusText.ShowStatusText("탈출 성공!");
                statusTextRight.ShowStatusText("탈출 성공!");
                GameObject.Find("OutsideLight").GetComponent<Light>().intensity = 8.0f;
                openedDoor = false;
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        statusText.ShowStatusText("손을 회전해주세요.");
        statusTextRight.ShowStatusText("손을 회전해주세요.");
    }

    public void onEventMethod()
    {
        grabAngle = GameObject.Find("BaseData").transform.rotation.eulerAngles;
    }

    public void onTargetTrigger()
    {
        throw new NotImplementedException();
    }
}
