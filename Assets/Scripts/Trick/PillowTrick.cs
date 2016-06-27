using UnityEngine;
using System.Collections;

public class PillowTrick : MonoBehaviour, ITrickManager {
    private bool _grabFlag;
    private bool _prevGrabFlag;
    private GameManager gameManager;
    private ShowStatusMsg statusText;
    private ShowStatusMsg statusTextRight;

    public GameObject _targetObject;

	// Use this for initialization
	void Start () 
    {
        _grabFlag = false;
        _prevGrabFlag = false;
        statusText = GameObject.Find("statusText").GetComponent<ShowStatusMsg>();
        statusTextRight = GameObject.Find("statusText (1)").GetComponent<ShowStatusMsg>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
    public bool MoveFlag
    {
        get { return true; }
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
            return transform.parent.gameObject;
        }
    }

    public void onEventMethod()
    {
        if (_targetObject != null)
        {
            _targetObject.transform.FindChild("default").GetComponent<ITrickManager>().onTargetTrigger();
            statusText.ShowStatusText("베게 밑에 이상한 버튼이 있다.");
            statusTextRight.ShowStatusText("베게 밑에 이상한 버튼이 있다.");
        }
        else
        {
            statusText.ShowStatusText("아무런일도 일어나지 않았다.");
            statusTextRight.ShowStatusText("아무런일도 일어나지 않았다.");
        }
    }

    public void onTargetTrigger()
    {
        throw new System.NotImplementedException();
    }
}
