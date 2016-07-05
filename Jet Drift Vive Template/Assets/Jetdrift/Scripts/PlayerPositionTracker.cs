using UnityEngine;
using System.Collections;

public class PlayerPositionTracker : MonoBehaviour {

    public SteamVR_TrackedObject headCamera;

    public Vector3 CameraRigPosition
    {
        get
        {
            return rigTransform.position;
        }
        set
        {
            rigTransform.position = value;
        }
    }
    public Vector3 HeadPosition
    {
        get
        {
            return cameraTransform.position;
        }
        set
        {
            rigTransform.position = value - (new Vector3(HeadPosition.x, 0f, HeadPosition.z) - CameraRigPosition);
        }
    }
    public Vector3 HeadRotationEulerAngles
    {
        get
        {
            return cameraTransform.eulerAngles;
        }
    }
    public Quaternion HeadRotation
    {
        get
        {
            return cameraTransform.rotation;
        }
    }

    private Transform rigTransform;
    private Transform cameraTransform;

    void Start()
    {
        rigTransform = transform;
        cameraTransform = headCamera.transform;
    }
}
