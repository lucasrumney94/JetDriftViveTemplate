using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Hand : MonoBehaviour {

    private VRInputManager vrInput;
    public int deviceIndex;

    public LayerMask collisionMask;

    public Rigidbody tip;
    public GameObject highlightedObject;
    public GameObject heldObject;

    private FixedJoint joint;

    void Start()
    {
        deviceIndex = GetComponentInParent<ControllerInputTracker>().index;
        vrInput = GameObject.FindGameObjectWithTag("VRInputManager").GetComponent<VRInputManager>();
        transform.position = tip.transform.position;
    }

    void Update()
    {
        CheckInputs();
    }

    private void CheckInputs()
    {
        if (vrInput.TriggerDown(deviceIndex))
        {
            if(heldObject != null)
            {
                heldObject.SendMessage("Activate", SendMessageOptions.DontRequireReceiver);
            }
            else if (highlightedObject != null)
            {
                Pickup(highlightedObject);
            }
        }
        if (vrInput.TriggerUp(deviceIndex))
        {
            if(heldObject != null)
            {
                heldObject.SendMessage("DeActivate", SendMessageOptions.DontRequireReceiver);
            }
        }
        if (vrInput.GripDown(deviceIndex))
        {
            if (heldObject != null)
            {
                Drop(heldObject);
            }
        }
    }
    
    private void Pickup(GameObject picked)
    {
        picked.SendMessage("Pickup", deviceIndex, SendMessageOptions.DontRequireReceiver);
        joint = picked.AddComponent<FixedJoint>();
        joint.connectedBody = tip;
        heldObject = picked;
    }

    private void Drop(GameObject held)
    {
        Rigidbody heldRigidbody = held.GetComponent<Rigidbody>();
        heldRigidbody.velocity = vrInput.controllerVelocity(deviceIndex);
        heldRigidbody.angularVelocity = vrInput.controllerAngularVelocity(deviceIndex);
        heldRigidbody.maxAngularVelocity = heldRigidbody.angularVelocity.magnitude;
        Destroy(joint);
        joint = null;
        heldObject = null;
    }

    void OnTriggerEnter(Collider other)
    {
        highlightedObject = other.gameObject;
    }

    void OnTriggerExit(Collider other)
    {
        if (highlightedObject == other.gameObject)
        {
            highlightedObject = null;
        }
    }
}