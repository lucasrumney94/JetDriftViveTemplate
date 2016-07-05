using UnityEngine;
using System.Collections;

public class VRTool : MonoBehaviour {

    public Option[] toolOptions;

    public virtual void InitializeOptions()
    {
        Debug.LogWarning("Tried to call InitializeOptions on the base class of VRTool! Did you mean to override this function?");
        toolOptions = new Option[0];
    }
}
