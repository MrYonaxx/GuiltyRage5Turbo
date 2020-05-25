using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ControllerConfiguration", menuName = "ControllerConfigurationData", order = 1)]
public class ControllerConfigurationData : ScriptableObject
{

    [SerializeField]
    [ValueDropdown("controllerValues")]
    public string controllerA;
    [SerializeField]
    [ValueDropdown("controllerValues")]
    public string controllerB;
    [SerializeField]
    [ValueDropdown("controllerValues")]
    public string controllerX;
    [SerializeField]
    [ValueDropdown("controllerValues")]
    public string controllerY;


    private ValueDropdownList<string> controllerValues = new ValueDropdownList<string>()
    {
        {"ControllerA", "ControllerA" },
        {"ControllerB", "ControllerB" },
        {"ControllerX", "ControllerX" },
        {"ControllerY", "ControllerY" },
    };
}
