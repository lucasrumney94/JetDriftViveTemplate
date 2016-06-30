using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public struct Option
{
    public Type type;

    public bool boolValue;
    public float floatValue;

    public Option(ref float newFloat)
    {
        type = typeof(float);
        boolValue = false;
        floatValue = newFloat;
    }

    public Option(ref bool newBool)
    {
        type = typeof(bool);
        boolValue = newBool;
        floatValue = 0f;
    }
}

public class Toolbox : MonoBehaviour {

    private ControllerInputTracker inputTracker;

    public Canvas selectionCanvas;
    public RectTransform selectionContent; //Assign to the 'content' child of the scroll rect
    public Button selectionButtonPrefab;
    public Canvas toolOptionsCanvas;

    public VRTool[] toolPrefabs;

    private VRTool activeTool;
    private VRTool[] toolInstances; //When a tool is spawned cache a reference to it, and activate/deactivate that instance instead of creating and destroying instances

    private Option[] activeToolOptions;

    void OnEnable()
    {
        inputTracker = transform.GetComponentInParent<ControllerInputTracker>();

        inputTracker.menuPressedDown += new ControllerInputDelegate(ToggleToolboxMenu);
    }

    void OnDisable()
    {
        inputTracker.menuPressedDown -= new ControllerInputDelegate(ToggleToolboxMenu);
    }

    void Start()
    {
        PopulateToolBoxMenu();
    }

    void Update()
    {
        ListenForLongPress();
    }

    #region Press length detection

    private bool listeningForLongPress = false;
    private float timeOfLastMenuDown;

    private void StartListeningForLongPress()
    {
        listeningForLongPress = true;
    }

    private void StopListeningForLongPress()
    {
        listeningForLongPress = false;
    }

    private void ListenForLongPress()
    {
        if (listeningForLongPress)
        {
            if (Time.time - timeOfLastMenuDown < 1f)
            {
                if (inputTracker.menuPressed == false)
                {
                    //Stop Listneing
                    StopListeningForLongPress();
                    //Do short press action
                    ToggleToolOptions();
                }
            }
            else
            {
                //Stop listening
                StopListeningForLongPress();
                //Do long press action
                ToggleToolboxMenu();
            }
        }
    }

    #endregion

    #region Toolbox menu

    public void ToggleToolboxMenu()
    {
        if (selectionCanvas.gameObject.activeInHierarchy)
        {
            CloseToolboxMenu();
        }
        else if (selectionCanvas.gameObject.activeInHierarchy == false)
        {
            OpenToolboxMenu();
        }
    }

    private void OpenToolboxMenu()
    {
        //Temporarily disable active tool
        activeTool.gameObject.SetActive(false);

        //Open selection canvas
        selectionCanvas.gameObject.SetActive(true);

        //Populate canvas with a mneu item for eash tool
    }

    private void CloseToolboxMenu()
    {
        //Reeanble active tool
        activeTool.gameObject.SetActive(true);

        //Disable canvas
        selectionCanvas.gameObject.SetActive(false);
    }

    public void PopulateToolBoxMenu()
    {
        foreach (VRTool tool in toolPrefabs)
        {
            VRTool newTool = Instantiate(tool).GetComponent<VRTool>();
            newTool.transform.SetParent(inputTracker != null ? inputTracker.transform : transform.parent);
            newTool.gameObject.SetActive(false);

            Button toolButton = Instantiate(selectionButtonPrefab).GetComponent<Button>();
            toolButton.transform.SetParent(selectionContent);
            toolButton.transform.localScale = Vector3.one;
            toolButton.transform.localPosition = Vector3.zero;
            Text buttonText = toolButton.transform.GetChild(0).GetComponent<Text>();
            if (buttonText != null)
            {
                buttonText.text = tool.name;
            }
            else
            {
                Debug.Log("Button prefab did not have a UI Text as its first child!");
            }
            toolButton.onClick = new Button.ButtonClickedEvent();
            toolButton.onClick.AddListener(() => ChangeActiveTool(newTool));
            Navigation navigation = new Navigation();
            navigation.mode = Navigation.Mode.Vertical;
            toolButton.navigation = navigation;
        }
    }

    public void ChangeActiveTool(VRTool newTool)
    {
        activeTool = newTool; //is this actually updating?
        CloseToolboxMenu();
        Debug.Log(newTool.name + " was set as active tool!");
    }

    #endregion

    #region Tool Options Menu

    private void ToggleToolOptions()
    {

    }

    private void OpenToolOptions()
    {
        activeToolOptions = activeTool.toolOptions;

    }

    private void CloseToolOptions()
    {

    }

    #endregion
}
