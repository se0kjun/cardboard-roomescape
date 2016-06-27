using UnityEngine;
using System.Collections;

public class BarrelTrick : MonoBehaviour, ITrickManager {
    private bool _grabFlag;
    private bool _prevGrabFlag;
    private GameManager gameManager;
    private ShowStatusMsg statusText;
    private ShowStatusMsg statusTextRight;

    public GameObject _targetObject;
    public bool barrelFlag;

    void Start()
    {
        _grabFlag = false;
        _prevGrabFlag = false;
        barrelFlag = false;
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
        get { return true; }
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
            return gameObject;
        }
    }

    public void onEventMethod()
    {
        if (barrelFlag)
        {
            statusText.ShowStatusText("항아리에 불을 끌 수 있을 것 같다.");
            statusTextRight.ShowStatusText("항아리에 불을 끌 수 있을 것 같다.");
        }
        else
        {
            statusText.ShowStatusText("아직 물통은 필요 없다.");
            statusTextRight.ShowStatusText("아직 물통은 필요 없다.");
        }
    }

    public void onTargetTrigger()
    {
        Destroy(gameObject);
    }
}
