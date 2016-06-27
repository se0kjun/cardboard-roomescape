using UnityEngine;
using System.Collections;

public class PictureTrick : MonoBehaviour, ITrickManager {
    private bool _grabFlag;
    private bool _prevGrabFlag;
    
    private GameManager gameManager;
    private ShowStatusMsg statusText;
    private ShowStatusMsg statusTextRight;
    private ShowPaperMsg paperText;
    public string pictureMessage;
    public bool markerPicture;

    public GameObject _targetObject;

	// Use this for initialization
	void Start () 
    {
        _grabFlag = false;
        _prevGrabFlag = false;
        statusText = GameObject.Find("statusText").GetComponent<ShowStatusMsg>();
        statusTextRight = GameObject.Find("statusText (1)").GetComponent<ShowStatusMsg>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        paperText = GameObject.Find("GameManager").GetComponent<ShowPaperMsg>();
	}

    public bool MoveFlag
    {
        get { return false; }
    }

    public bool GrabFlag
    {
        get
        {
            return _grabFlag;
        }
        set
        {
            _prevGrabFlag = _grabFlag;
            _grabFlag = value;
        }
    }

    public GameObject TargetObject
    {
        get
        {
            return _targetObject;
        }
        set
        {
            _targetObject = value;
        }
    }

    public GameObject MovingObject
    {
        get
        {
            return null;
        }
    }

    public void onEventMethod()
    {
        if (!markerPicture)
            statusText.ShowStatusText(pictureMessage);
        else
        {
            statusText.ShowStatusText("수상한 쪽지를 발견했다. 읽어본다.");
            statusTextRight.ShowStatusText("수상한 쪽지를 발견했다. 읽어본다.");
            paperText.showMessage(true, new string[] { pictureMessage });
        }
    }

    public void onTargetTrigger()
    {
        throw new System.NotImplementedException();
    }
}
