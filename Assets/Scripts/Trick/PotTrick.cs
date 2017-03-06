using UnityEngine;
using System.Collections;

public class PotTrick : MonoBehaviour, ITrickManager {
    private bool _grabFlag;
    private bool _prevGrabFlag;
    private GameManager gameManager;
    private ShowStatusMsg statusText;
    private ShowStatusMsg statusTextRight;

    #region KEY_STATE
    private bool keyState;
    private bool keyFlag;
    #endregion

    public GameObject _targetObject;
    public bool fireFlag;

	// Use this for initialization
	void Start () 
    {
        keyState = false;
        keyFlag = false;
        _grabFlag = false;
        _prevGrabFlag = false;
        fireFlag = false;
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
        if (keyState)
        {
            if (!keyFlag)
            {
                if (!fireFlag)
                {
                    //GET KEY
                    statusText.ShowStatusText("키를 획득하였습니다.");
                    statusTextRight.ShowStatusText("키를 획득하였습니다.");
                    gameManager.keyFlag = true;
                    GameObject.Find("pPlane3").GetComponent<MeshRenderer>().enabled = true;
                }
                else
                {
                    statusText.ShowStatusText("너무 뜨겁습니다.");
                    statusTextRight.ShowStatusText("너무 뜨겁습니다.");
                }
            }
        }
        else
        {
            statusText.ShowStatusText("아무런일도 일어나지 않았습니다.");
            statusTextRight.ShowStatusText("아무런일도 일어나지 않았습니다.");
        }
    }

    public void onTargetTrigger()
    {
        ParticleSystem particle = GameObject.Find("PotParticle").GetComponent<ParticleSystem>();
        var em = particle.emission;
        em.enabled = true;

        keyState = true;
        fireFlag = true;

        GameObject.Find("barrel").GetComponent<BarrelTrick>().barrelFlag = true;
        GameObject.Find("barrel (1)").GetComponent<BarrelTrick>().barrelFlag = true;
        GameObject.Find("barrel (2)").GetComponent<BarrelTrick>().barrelFlag = true;
        GameObject.Find("barrel (3)").GetComponent<BarrelTrick>().barrelFlag = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BarrelTrick>() != null)
        {
            collision.gameObject.GetComponent<BarrelTrick>().onTargetTrigger();
            ParticleSystem particle = GameObject.Find("PotParticle").GetComponent<ParticleSystem>();
            var em = particle.emission;
            em.enabled = false;
            fireFlag = false;
            statusText.ShowStatusText("불이 꺼졌습니다.");
            statusTextRight.ShowStatusText("불이 꺼졌습니다.");
        }
    }
}
