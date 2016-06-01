using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class HoldableObject : MonoBehaviour {

    public VRInputManager vrInput;

    public bool held;
    public int heldDeviceIndex = -1;

    public Vector3 anchor;

    void Start()
    {
        vrInput = GameObject.FindGameObjectWithTag("VRInputManager").GetComponent<VRInputManager>();
    }

    public virtual void Pickup(int deviceIndex)
    {
        held = true;
        heldDeviceIndex = deviceIndex;
        transform.localPosition = vrInput.controllerPosition(deviceIndex);
        transform.localRotation = vrInput.controllerRotation(deviceIndex);
        transform.position += transform.TransformDirection(anchor);
        Debug.Log("Picked up by controller #" + heldDeviceIndex);
    }

    public virtual void Drop()
    {
        held = false;
        heldDeviceIndex = -1;
    }
}
