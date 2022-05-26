using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityFoundation.FirstPersonModeSystem;
using Zenject;

public class FirstPersonPOVExtensionBinder : FirstPersonPOVExtension
{
    [Inject]
    public void InjectEntry(FirstPersonInputs inputs)
    {
        Init(inputs);
    }
}
