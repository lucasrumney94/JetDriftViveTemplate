using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour {

    private VRInputManager vrInput;
    public int controllerIndex;

    public GameObject cubePrefab;

    public float initialForce;

    void Start()
    {
        controllerIndex = GetComponentInParent<ControllerInputTracker>().index;
        vrInput = GameObject.FindGameObjectWithTag("VRInputManager").GetComponent<VRInputManager>();
    }

    void Update()
    {
        if (vrInput.TriggerDown(controllerIndex))
        {
            Debug.Log("controller #" + controllerIndex + " spawned a cube!");
            GameObject newCube = Instantiate(cubePrefab, vrInput.controllerPosition(controllerIndex), 
                vrInput.controllerRotation(controllerIndex)) as GameObject;
            newCube.transform.position += newCube.transform.forward * 0.1f;
            newCube.GetComponent<Rigidbody>().velocity = newCube.transform.forward * initialForce;
        }
    }
}
