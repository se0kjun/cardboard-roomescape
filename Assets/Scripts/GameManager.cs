using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;
using MyoQuaternion = Thalmic.Myo.Quaternion;

public enum MyoState
{
    GRAB,
    NONE
}

public enum FootState
{
    WALK,
    LAND,
    UNKNOWN
}

public class GameManager : MonoBehaviour
{
    #region LIGHT
    public GameObject playerLight;
    public GameObject nightvisionLight;
    #endregion

    public bool nightVisionFlag;
    public bool NightVisionFlag
    {
        get
        {
            return nightVisionFlag;
        }
        set
        {
            if (value)
            {
                playerLight.GetComponent<Light>().intensity = 0.0f;
                nightvisionLight.GetComponent<Light>().intensity = 3.0f;
            }
            else
            {
                playerLight.GetComponent<Light>().intensity = 7.0f;
                nightvisionLight.GetComponent<Light>().intensity = 0.0f;
            }

            nightVisionFlag = value;
        }
    }

    public GameObject soundEffecter;
    public GameObject objectSet;

    #region MYO
    public MyoState myoState;
    public GameObject handObject;
    public GameObject myoBaseObject;
    [HideInInspector]
    public MyoWebClient myoController;

    private Transform grabObject;
    private Transform movingObject;
    private Quaternion grabRotation;

    public Pose currentPose;
    public bool grabFlag;

    public string processingData;
    #endregion

    #region WALKING
    public GameObject cardboardController;
    private SensorWebClient walkingController;
    public Bounds walkingBound;

    private FootState leftFootPrevious;
    private FootState rightFootprevious;
    private int leftFootWalkCnt;
    private int leftFootLandCnt;
    private int rightFootWalkCnt;
    private int rightFootLandCnt;
    private const int THRESHOLD_CNT = 10;

    private Vector3 targetPos;
    private bool targetPosFlag;

    private FootState leftFootState;
    private FootState rightFootState;
    public AudioClip[] footAudioSource;
    public Text statusText;
    public Text debugText;
    #endregion

    #region PLAYER_SETTING
    public bool keyFlag;
    #endregion

    void Awake()
    {
        NightVisionFlag = true;
    }

    void Start ()
    {
        #region MYO_INIT
        myoState = MyoState.NONE;
        myoController = GetComponent<MyoWebClient>();
        myoBaseObject = GameObject.Find("BaseData");
        #endregion

        #region WALKING_INIT
        walkingController = GetComponent<SensorWebClient>();
        leftFootState = FootState.UNKNOWN;
        rightFootState = FootState.UNKNOWN;
        targetPosFlag = false;
        #endregion

        keyFlag = false;
    }

    void Update () 
    {
        WalkingControl();
        MoveCharacter();

        debugText.text = leftFootWalkCnt.ToString() + "/" + leftFootLandCnt + "/" + rightFootWalkCnt.ToString() + "/" + rightFootLandCnt.ToString();
        myoBaseObject.transform.localRotation = GetComponent<MyoWebClient>().recOrientation;
        currentPose = myoController.recPose;
        ObjectControlWithMyo();
	}

    #region MYO_CONTROL
    void ObjectControlWithMyo()
    {
        if (myoController.recPose == Pose.Fist)
        {
            if (grabObject != null)
            {
                //moving object
                if (movingObject != null)
                    movingObject.transform.localRotation = myoBaseObject.transform.rotation;
                if (grabObject.GetComponent<ITrickManager>().MoveFlag)
                    movingObject.transform.parent = handObject.transform;
            }
            else
            {
                RaycastHit hit;
                Ray ray = new Ray(Camera.main.transform.position, handObject.transform.forward);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "object" && Vector3.Distance(hit.transform.position, Camera.main.transform.position) < 5.0f)
                    {
                        Debug.Log(hit.transform);
                        Debug.Log(hit.transform.GetComponent<ITrickManager>());
                        hit.transform.GetComponent<ObjectSelection>().SetGlowGrab();
                        grabObject = hit.transform;
                        if (hit.transform.GetComponent<ITrickManager>().MovingObject != null)
                            movingObject = hit.transform.GetComponent<ITrickManager>().MovingObject.transform;
                        else
                            movingObject = null;
                        myoState = MyoState.GRAB;
                    }
                }
                else
                {
                }
            }
        }
        else if (myoController.recPose == Pose.WaveIn)
        {
            GetComponent<ShowPaperMsg>().nextMessage();
        }
        else if (myoController.recPose == Pose.WaveOut)
        {
            GetComponent<ShowPaperMsg>().prevMessage();
        }
        else
        {
            myoState = MyoState.NONE;
            if (grabObject != null && movingObject != null)
            {
                movingObject.transform.parent = null;
                grabObject.GetComponent<ObjectSelection>().ReleaseObject();
            }
            grabObject = null;
        }
    }
    #endregion

    #region WALKING_CONTROL
    void WalkingControl()
    {
        if (leftFootPrevious != walkingController.tempLeftFoot && leftFootPrevious != FootState.UNKNOWN)
        {
            if (walkingController.tempLeftFoot == FootState.LAND)
                leftFootWalkCnt = 0;
            else if (walkingController.tempLeftFoot == FootState.WALK)
                leftFootLandCnt = 0;
            targetPosFlag = false;
        }
        else if (leftFootPrevious == walkingController.tempLeftFoot && leftFootPrevious != FootState.UNKNOWN)
        {
            if (walkingController.tempLeftFoot == FootState.LAND)
                leftFootLandCnt++;
            else if (walkingController.tempLeftFoot == FootState.WALK)
                leftFootWalkCnt++;
        }

        if (rightFootprevious != walkingController.tempRightFoot && rightFootprevious != FootState.UNKNOWN)
        {
            if (walkingController.tempRightFoot == FootState.LAND)
                rightFootWalkCnt = 0;
            else if (walkingController.tempRightFoot == FootState.WALK)
                rightFootLandCnt = 0;
            targetPosFlag = false;
        }
        else if (rightFootprevious == walkingController.tempRightFoot && rightFootprevious != FootState.UNKNOWN)
        {
            if (walkingController.tempRightFoot == FootState.LAND)
                rightFootLandCnt++;
            else if (walkingController.tempRightFoot == FootState.WALK)
                rightFootWalkCnt++;
        }

        if (leftFootWalkCnt > THRESHOLD_CNT)
            leftFootState = FootState.WALK;
        if (leftFootLandCnt > THRESHOLD_CNT)
            leftFootState = FootState.LAND;
        if (rightFootWalkCnt > THRESHOLD_CNT)
            rightFootState = FootState.WALK;
        if (rightFootLandCnt > THRESHOLD_CNT)
            rightFootState = FootState.LAND;

        //leftFootState = (leftFootWalkCnt > THRESHOLD_CNT) ? FootState.WALK : FootState.LAND;
        //rightFootState = (rightFootWalkCnt > THRESHOLD_CNT) ? FootState.WALK : FootState.LAND;
        //leftFootState = (leftFootLandCnt > THRESHOLD_CNT) ? FootState.LAND : FootState.WALK;
        //rightFootState = (rightFootLandCnt > THRESHOLD_CNT) ? FootState.LAND : FootState.WALK;

        if (!targetPosFlag)
        {
            Vector3 directionVector = Camera.main.transform.forward.normalized;
            if (directionVector.y > cardboardController.transform.position.y)
                directionVector.y = 0;

            targetPos = cardboardController.transform.position + (Camera.main.transform.forward.normalized);
        }

        leftFootPrevious = walkingController.tempLeftFoot;
        rightFootprevious = walkingController.tempRightFoot;
    }

    void MoveCharacter()
    {
        if (leftFootState == FootState.WALK && rightFootState == FootState.WALK)
        {
            //JUMP
            statusText.text = "JUMP";
        }
        else if (leftFootState != rightFootState && leftFootState != FootState.UNKNOWN && rightFootState != FootState.UNKNOWN)
        {
            //WALK
            statusText.text = "WALK";
            if (walkingBound.Contains(targetPos))
            {
                AudioSource tmp = soundEffecter.GetComponent<AudioSource>();
                int random_sel = UnityEngine.Random.Range(0, 4);
                tmp.clip = footAudioSource[random_sel];
                tmp.Play();

                cardboardController.transform.position = Vector3.MoveTowards(cardboardController.transform.position, targetPos, Time.deltaTime);
                handObject.transform.position = Vector3.MoveTowards(Camera.main.transform.position, targetPos, Time.deltaTime);
            }
            targetPosFlag = true;
        }
        else
        {
            //STOP
            statusText.text = "STOP";
        }
    }
    #endregion
}
