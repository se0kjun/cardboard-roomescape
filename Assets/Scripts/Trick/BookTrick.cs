using UnityEngine;
using System.Collections;

public class BookTrick : MonoBehaviour, ITrickManager {
    private bool _grabFlag;
    private bool _prevGrabFlag;
    private GameManager gameManager;

    public GameObject _targetObject;
    private ShowPaperMsg paperText;

    public string[] bookArticle;

    // Use this for initialization
    void Start()
    {
        _grabFlag = false;
        _prevGrabFlag = false;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        paperText = GameObject.Find("GameManager").GetComponent<ShowPaperMsg>();
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
            _targetObject.GetComponent<ITrickManager>().onTargetTrigger();
        paperText.showMessage(false, bookArticle);
    }

    public void onTargetTrigger()
    {
        throw new System.NotImplementedException();
    }
}
