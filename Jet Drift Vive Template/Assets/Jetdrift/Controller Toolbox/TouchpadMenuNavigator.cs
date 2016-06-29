using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TouchpadMenuNavigator : MonoBehaviour {

    public Button testButton;
    public StandaloneInputModule inputModule;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            testButton.onClick.Invoke();
        }
    }
}
