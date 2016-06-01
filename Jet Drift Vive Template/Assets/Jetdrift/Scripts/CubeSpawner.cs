using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour {

    private VRInputManager vrInput;
    public int deviceIndex;

    public GameObject cubePrefab;

    public float initialForce;

    void Start()
    {
        deviceIndex = GetComponentInParent<ControllerInputTracker>().index;
        vrInput = GameObject.FindGameObjectWithTag("VRInputManager").GetComponent<VRInputManager>();
    }

    void Update()
    {
        if (vrInput.TriggerDown(deviceIndex))
        {
            Debug.Log("controller #" + deviceIndex + " spawned a cube!");
            GameObject newCube = Instantiate(cubePrefab, vrInput.controllerPosition(deviceIndex), 
                vrInput.controllerRotation(deviceIndex)) as GameObject;
            newCube.transform.position += newCube.transform.forward * 0.1f;
            newCube.GetComponent<Rigidbody>().velocity = newCube.transform.forward * initialForce;
        }
    }
}
