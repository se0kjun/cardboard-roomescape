using UnityEngine;
using System.Collections;

public class CandleTrick : MonoBehaviour, ITrickManager {
    private bool _grabFlag;
    private bool _prevGrabFlag;
    private GameManager gameManager;
    private ShowStatusMsg statusText;
    private ShowStatusMsg statusTextRight;

    public GameObject _targetObject;

	void Start () 
    {
        _grabFlag = false;
        _prevGrabFlag = false;
        statusText = GameObject.Find("statusText").GetComponent<ShowStatusMsg>();
        statusTextRight = GameObject.Find("statusText (1)").GetComponent<ShowStatusMsg>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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

    public bool MoveFlag
    {
        get { return false; }
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
        gameManager.NightVisionFlag = false;
        gameObject.SetActive(false);
        statusText.ShowStatusText("야간투시경을 제거하였습니다.");
        statusTextRight.ShowStatusText("야간투시경을 제거하였습니다.");
    }

    public void onTargetTrigger()
    {
        throw new System.NotImplementedException();
    }
}
