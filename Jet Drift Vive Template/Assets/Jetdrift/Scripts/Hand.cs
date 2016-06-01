using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Hand : MonoBehaviour {

    private VRInputManager vrInput;
    public int controllerIndex;

    public LayerMask collisionMask;

    public Rigidbody tip;
    public GameObject highlightedObject;
    public GameObject heldObject;
    

    void Start()
    {
        controllerIndex = GetComponentInParent<ControllerInputTracker>().index;
        vrInput = GameObject.FindGameObjectWithTag("VRInputManager").GetComponent<VRInputManager>();
        transform.position = tip.transform.position;
    }

    void Update()
    {
        CheckHandCollision();
    }

    private void CheckHandCollision()
    {
        if (vrInput.TriggerDown(controllerIndex))
        {
            if (heldObject != null)
            {
                Drop(heldObject);
            }
            else if (highlightedObject != null)
            {
                Pickup(highlightedObject);
            }
        }
    }
    
    private void Pickup(GameObject picked)
    {
        heldObject = picked;
        picked.transform.parent = transform;
        picked.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void Drop(GameObject held)
    {
        held.transform.parent = null;
        Rigidbody heldRigidbody = heldObject.GetComponent<Rigidbody>();
        heldRigidbody.isKinematic = false;
        heldRigidbody.velocity = vrInput.controllerVelocity(controllerIndex);
        heldRigidbody.angularVelocity = vrInput.controllerAngularVelocity(controllerIndex);
        heldRigidbody.maxAngularVelocity = heldRigidbody.angularVelocity.magnitude;
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
