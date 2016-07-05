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

    public void SetHeadPosition(Vector3 worldPosition)
    {
        Vector3 newPosition;
        newPosition.x = worldPosition.x - (HeadPosition.x - CameraRigPosition.x);
        newPosition.y = worldPosition.y;
        newPosition.z = worldPosition.z - (HeadPosition.z - CameraRigPosition.z);

        CameraRigPosition = newPosition;
    }

    private Transform rigTransform;
    private Transform cameraTransform;

    void Start()
    {
        rigTransform = transform;
        cameraTransform = headCamera.transform;
    }
}
