using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public struct Option
{
    public Type type;

    public string name;

    public bool boolValue;
    public float floatValue;

    public Option(ref float newFloat, string optionName)
    {
        type = typeof(float);
        name = optionName;
        boolValue = false;
        floatValue = newFloat;
    }

    public Option(ref bool newBool, string optionName)
    {
        type = typeof(bool);
        name = optionName;
        boolValue = newBool;
        floatValue = 0f;
    }
}

public class Toolbox : MonoBehaviour {

    private ControllerInputTracker inputTracker;

    public Canvas selectionCanvas;
    public RectTransform selectionContent; //Assign to the 'content' child of the scroll rect

    public Canvas optionsCanvas;
    public RectTransform optionsContent;

    public Button selectionButtonPrefab;
    public Toggle toggleOptionPrefab;
    public Slider sliderOptionPrefab;

    public VRTool[] toolPrefabs;

    private VRTool activeTool;
    private VRTool[] toolInstances; //When a tool is spawned cache a reference to it, and activate/deactivate that instance instead of creating and destroying instances

    private Option[] activeToolOptions;

    void OnEnable()
    {
        inputTracker = transform.GetComponentInParent<ControllerInputTracker>();

        inputTracker.menuPressedDown += new ControllerInputDelegate(StartListeningForLongPress);
    }

    void OnDisable()
    {
        inputTracker.menuPressedDown -= new ControllerInputDelegate(StartListeningForLongPress);
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
        timeOfLastMenuDown = Time.time;
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
        if (activeTool != null)
        {
            activeTool.gameObject.SetActive(false);
        }

        //Open selection canvas
        selectionCanvas.gameObject.SetActive(true);
    }

    private void CloseToolboxMenu()
    {
        //Reeanble active tool
        if (activeTool != null)
        {
            activeTool.gameObject.SetActive(true);
        }

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
        activeTool = newTool;
        CloseToolboxMenu();
        Debug.Log(newTool.name + " was set as active tool!");
    }

    #endregion

    #region Tool Options Menu

    private void ToggleToolOptions()
    {
        if (optionsCanvas.gameObject.activeInHierarchy)
        {
            CloseToolOptions();
        }
        else if (optionsCanvas.gameObject.activeInHierarchy == false)
        {
            OpenToolOptions();
        }
    }

    private void OpenToolOptions()
    {
        //Temporarily disable active tool
        if (activeTool != null)
        {
            activeToolOptions = activeTool.toolOptions;

            //Remove all children of content
            Transform contentTransform = optionsContent.transform;
            int childCount = contentTransform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                Destroy(contentTransform.GetChild(i).gameObject);
            }

            //Spawn new Toggle or Slider for each option
            PopulateToolOptionsMenu();

            activeTool.gameObject.SetActive(false);
        }

        optionsCanvas.gameObject.SetActive(true);
    }

    private void CloseToolOptions()
    {
        //Reeanble active tool
        if (activeTool != null)
        {
            activeTool.gameObject.SetActive(true);
        }

        optionsCanvas.gameObject.SetActive(false);
    }

    private void PopulateToolOptionsMenu()
    {
        foreach (Option toolOption in activeToolOptions)
        {
            GameObject newOptionUIElement;
            if (toolOption.type == typeof(bool))
            {
                newOptionUIElement = Instantiate(toggleOptionPrefab.gameObject);
            }
            else if (toolOption.type == typeof(float))
            {
                newOptionUIElement = Instantiate(sliderOptionPrefab.gameObject);
            }
            else
            {
                Debug.Log("Tool option not set to valid type!");
                return;
            }

            newOptionUIElement.transform.SetParent(optionsContent.transform);
            newOptionUIElement.transform.localScale = Vector3.one;
            newOptionUIElement.transform.localPosition = Vector3.zero;

            Text optionText = newOptionUIElement.transform.GetComponentInChildren<Text>();
            if (optionText != null)
            {
                optionText.text = toolOption.name;
            }
            else
            {
                Debug.Log("Option prefab did not have a UI Text as a child!");
            }

            Navigation navigation = new Navigation();
            navigation.mode = Navigation.Mode.Vertical;

            Toggle optionToggle = newOptionUIElement.GetComponent<Toggle>();
            Slider optionSlider = newOptionUIElement.GetComponent<Slider>();

            if (optionToggle != null)
            {
                optionToggle.navigation = navigation;
            }
            else if (optionSlider != null)
            {
                optionSlider.navigation = navigation;
            }

        }
    }

    #endregion
}
