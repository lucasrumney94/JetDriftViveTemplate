using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class ReferenceValue<T> //Taken from Eric Lippert's answer to this question: http://stackoverflow.com/questions/2256048/store-a-reference-to-a-value-type
{
    private Func<T> getter;
    private Action<T> setter;
    public ReferenceValue(Func<T> getter, Action<T> setter)
    {
        this.getter = getter;
        this.setter = setter;
    }
    public T Value
    {
        get { return getter(); }
        set { setter(value); }
    }
}

[Serializable]
public struct Option
{
    public Type type;

    public string name;

    private ReferenceValue<bool> boolReference;
    private ReferenceValue<float> floatReference;

    public bool boolValue
    {
        get
        {
            return boolReference.Value;
        }
        set
        {
            boolReference.Value = value;
        }
    }
    public float floatValue
    {
        get
        {
            return floatReference.Value;
        }
        set
        {
            floatReference.Value = value;
        }
    }
    public float minValue;
    public float maxValue;

    public Option(ReferenceValue<float> floatReference, string optionName, float min = 0f, float max = 1f)
    {
        type = typeof(float);
        name = optionName;
        this.boolReference = new ReferenceValue<bool>(() => false, v => { });
        this.floatReference = floatReference;
        minValue = min;
        maxValue = max;
    }

    public Option(ReferenceValue<bool> boolReference, string optionName)
    {
        type = typeof(bool);
        name = optionName;
        this.boolReference = boolReference;
        this.floatReference = new ReferenceValue<float>(() => 0f, v => { });
        minValue = 0f;
        maxValue = 0f;
    }
}

public class Toolbox : MonoBehaviour {

    private ControllerInputTracker inputTracker;

    public Canvas selectionCanvas;
    public RectTransform selectionContent; //Assign to the 'content' child of the scroll rect

    public Canvas optionsCanvas;
    public RectTransform optionsContent;

    private GameObject[] optionsUIElements;

    public Button selectionButtonPrefab;
    public Toggle toggleOptionPrefab;
    public Slider sliderOptionPrefab;

    public VRTool[] toolPrefabs;

    public VRTool activeTool;

    public Option[] activeToolOptions;

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
        UpdateOptionsUI();
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
            toolButton.transform.localRotation = Quaternion.identity;
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
        optionsUIElements = new GameObject[activeToolOptions.Length];
        for (int i = 0; i < activeToolOptions.Length; i++)
        {
            GameObject newOptionUIElement;
            if (activeToolOptions[i].type == typeof(bool))
            {
                newOptionUIElement = Instantiate(toggleOptionPrefab.gameObject);
            }
            else if (activeToolOptions[i].type == typeof(float))
            {
                newOptionUIElement = Instantiate(sliderOptionPrefab.gameObject);
            }
            else
            {
                Debug.Log("Tool option not set to valid type!");
                return;
            }

            optionsUIElements[i] = newOptionUIElement;

            newOptionUIElement.transform.SetParent(optionsContent.transform);
            newOptionUIElement.transform.localScale = Vector3.one;
            newOptionUIElement.transform.localPosition = Vector3.zero;
            newOptionUIElement.transform.localRotation = Quaternion.identity;

            Text optionText = newOptionUIElement.transform.GetComponentInChildren<Text>();
            if (optionText != null)
            {
                optionText.text = activeToolOptions[i].name;
            }
            else
            {
                Debug.Log("Option prefab did not have a UI Text as a child!");
            }

            Navigation navigation = new Navigation();
            navigation.mode = Navigation.Mode.Vertical;

            Toggle optionToggle = newOptionUIElement.GetComponent<Toggle>();
            Slider optionSlider = newOptionUIElement.GetComponent<Slider>();

            int currentIndex = i; //Necessary for some reason to do with delegates / lambda

            if (optionToggle != null)
            {
                optionToggle.isOn = activeToolOptions[i].boolValue;
                optionToggle.navigation = navigation;

                optionToggle.onValueChanged.AddListener((value) => { SetBoolOption(currentIndex, value); });
            }
            else if (optionSlider != null)
            {
                optionSlider.minValue = activeToolOptions[i].minValue;
                optionSlider.maxValue = activeToolOptions[i].maxValue;
                optionSlider.value = activeToolOptions[i].floatValue;
                optionSlider.navigation = navigation;

                optionSlider.onValueChanged.AddListener((value) => { SetFloatOption(currentIndex, value); });
            }
        }
    }

    private void UpdateOptionsUI()
    {
        if (optionsUIElements != null)
        {
            for (int i = 0; i < optionsUIElements.Length; i++)
            {
                Toggle optionToggle = optionsUIElements[i].GetComponent<Toggle>();
                Slider optionSlider = optionsUIElements[i].GetComponent<Slider>();

                if (optionToggle != null)
                {
                    optionToggle.isOn = activeToolOptions[i].boolValue;
                }
                else if (optionSlider != null)
                {
                    optionSlider.minValue = activeToolOptions[i].minValue;
                    optionSlider.maxValue = activeToolOptions[i].maxValue;
                    optionSlider.value = activeToolOptions[i].floatValue;
                }
            }
        }
    }

    public void SetFloatOption(int optionIndex, float newValue)
    {
        activeToolOptions[optionIndex].floatValue = newValue;
    }

    public void SetBoolOption(int optionIndex, bool newValue)
    {
        activeToolOptions[optionIndex].boolValue = newValue;
        Debug.Log("Set option #" + optionIndex + " to " + newValue);
    }

    #endregion
}
