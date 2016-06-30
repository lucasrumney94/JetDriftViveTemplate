using UnityEngine;
using System.Collections;

public class VRTool : MonoBehaviour {

    public Option[] toolOptions;

    public virtual void InitializeOptions()
    {
        throw new System.NotImplementedException("Tried to call InitializeOptions on the base class of VRTool! All descendants of VRTool should override this function.");
    }
}
