using UnityEngine;
using System.Collections;

public class CubeSpawner : MonoBehaviour {

    public VRInputManager vrInput;

    public GameObject cubePrefab;

    public float initialForce;

    void Start()
    {
        vrInput = GameObject.FindGameObjectWithTag("VRInputManager").GetComponent<VRInputManager>();
    }

    void Update()
    {
        if (vrInput.trigger1Down)
        {
            GameObject newCube = Instantiate(cubePrefab, vrInput.controller1Position, vrInput.controller1Rotation) as GameObject;
            newCube.GetComponent<Rigidbody>().velocity = newCube.transform.forward * initialForce;
        }

        if (vrInput.trigger2Down)
        {
            GameObject newCube = Instantiate(cubePrefab, vrInput.controller2Position, vrInput.controller2Rotation) as GameObject;
            newCube.GetComponent<Rigidbody>().velocity = newCube.transform.forward * initialForce;
        }
    }
}
