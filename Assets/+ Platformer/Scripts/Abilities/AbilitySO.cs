using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class AbilitySO : ScriptableObject
{
    public string abilityName;
    [TextArea]
    public string abilityDescription;
    public AbilityType abilityType;
}
