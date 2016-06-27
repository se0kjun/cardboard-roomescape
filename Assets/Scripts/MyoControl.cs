using UnityEngine;
using System.Collections;

using LockingPolicy = Thalmic.Myo.LockingPolicy;
using Pose = Thalmic.Myo.Pose;
using UnlockType = Thalmic.Myo.UnlockType;
using VibrationType = Thalmic.Myo.VibrationType;
using MyoQuaternion = Thalmic.Myo.Quaternion;

public class MyoControl : MonoBehaviour {
    private GameManager gameManager;

    public GameObject handModelObject;
    public GameObject myoGameObject;
    //[HideInInspector]
    //public ThalmicMyo myoController;

    private Quaternion antiYaw = Quaternion.identity;
    private float referenceRoll = 0.0f;
    private Pose lastPose = Pose.Unknown;
    
    private Transform previousGlowObject;

    private Pose recPose = Pose.Unknown;
    private Quaternion recOrientation = Quaternion.identity;
    private Thalmic.Myo.XDirection recXdir;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        CheckRayCasting();

        recPose = gameManager.GetComponent<MyoWebClient>().recPose;
        recOrientation = gameManager.GetComponent<MyoWebClient>().recOrientation;
        recXdir = gameManager.GetComponent<MyoWebClient>().recXdir;

        bool updateReference = false;
        if (recPose != lastPose)
        {
            lastPose = recPose;

            if (recPose == Pose.FingersSpread)
            {
                updateReference = true;
            }
        }

        if (Input.GetKeyDown("r"))
        {
            updateReference = true;
        }

        if (updateReference)
        {
            antiYaw = Quaternion.FromToRotation(
                new Vector3(myoGameObject.transform.forward.x, 0, myoGameObject.transform.forward.z),
                Camera.main.transform.forward
            );

            Vector3 referenceZeroRoll = computeZeroRollVector(myoGameObject.transform.forward);
            referenceRoll = rollFromZero(referenceZeroRoll, myoGameObject.transform.forward, myoGameObject.transform.up);
        }

        // Current zero roll vector and roll value.
        Vector3 zeroRoll = computeZeroRollVector(myoGameObject.transform.forward);
        float roll = rollFromZero(zeroRoll, myoGameObject.transform.forward, myoGameObject.transform.up);

        float relativeRoll = normalizeAngle(roll - referenceRoll);

        Quaternion antiRoll = Quaternion.AngleAxis(relativeRoll, myoGameObject.transform.forward);
        transform.rotation = antiYaw * antiRoll * Quaternion.LookRotation(myoGameObject.transform.forward);
        if (recXdir == Thalmic.Myo.XDirection.TowardWrist)
        {
            transform.rotation = new Quaternion(transform.localRotation.x,
                                                -transform.localRotation.y,
                                                transform.localRotation.z,
                                                -transform.localRotation.w);
        }
    }

    float rollFromZero(Vector3 zeroRoll, Vector3 forward, Vector3 up)
    {
        float cosine = Vector3.Dot(up, zeroRoll);
        Vector3 cp = Vector3.Cross(up, zeroRoll);
        float directionCosine = Vector3.Dot(forward, cp);
        float sign = directionCosine < 0.0f ? 1.0f : -1.0f;

        return sign * Mathf.Rad2Deg * Mathf.Acos(cosine);
    }

    Vector3 computeZeroRollVector(Vector3 forward)
    {
        Vector3 antigravity = Vector3.up;
        Vector3 m = Vector3.Cross(myoGameObject.transform.forward, antigravity);
        Vector3 roll = Vector3.Cross(m, myoGameObject.transform.forward);

        return roll.normalized;
    }

    float normalizeAngle(float angle)
    {
        if (angle > 180.0f)
        {
            return angle - 360.0f;
        }
        if (angle < -180.0f)
        {
            return angle + 360.0f;
        }
        return angle;
    }

    void CheckRayCasting()
    {
        RaycastHit hit;
        Ray ray = new Ray(Vector3.zero, transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.tag == "object")
            {
                if (previousGlowObject != null)
                {
                    previousGlowObject.GetComponent<ObjectSelection>().UnSetGlowSelection();
                }
                previousGlowObject = hit.transform;
                hit.transform.GetComponent<ObjectSelection>().SetGlowSelection();
            }
            else
            {
                if (previousGlowObject != null)
                {
                    previousGlowObject.GetComponent<ObjectSelection>().UnSetGlowSelection();
                }
            }
        }
    }
}
