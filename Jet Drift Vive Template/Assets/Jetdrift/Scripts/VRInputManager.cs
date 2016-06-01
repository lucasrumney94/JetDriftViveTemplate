using UnityEngine;
using System.Collections;

public class VRInputManager : MonoBehaviour {

    public ControllerInputTracker controllerLeft;
    public ControllerInputTracker controllerRight;

    public ControllerInputTracker[] controllers; //Change to a more generic device input tracker?

    void Start()
    {
        controllers = new ControllerInputTracker[16];
    }

    void Update()
    {
        controllers[controllerLeft.index] = controllerLeft;
        controllers[controllerRight.index] = controllerRight;
    }

    public bool TriggerDown(int index)
    {
        if (controllers[index] != null)
        {
            return controllers[index].triggerDown;
        }
        else return false;
    }

    public bool TriggerUp(int index)
    {
        if (controllers[index] != null)
        {
            return controllers[index].triggerUp;
        }
        else return false;
    }

    public bool GripDown(int index)
    {
        if (controllers[index] != null)
        {
            return controllers[index].gripDown;
        }
        else return false;
    }

    public Vector3 controllerPosition(int index)
    {
        if (controllers[index] != null)
        {
            return controllers[index].position;
        }
        else return Vector3.zero;
    }

    public Quaternion controllerRotation(int index)
    {
        if (controllers[index] != null)
        {
            return controllers[index].rotation;
        }
        else return Quaternion.identity;
    }

    public Vector3 controllerVelocity(int index)
    {
        if (controllers[index] != null)
        {
            return controllers[index].velocity;
        }
        else return Vector3.zero;
    }

    public Vector3 controllerAngularVelocity(int index)
    {
        if (controllers[index] != null)
        {
            return controllers[index].angularVelocity;
        }
        else return Vector3.zero;
    }
}
