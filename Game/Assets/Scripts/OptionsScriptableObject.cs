using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Options", menuName = "ScriptableObjects/OptionsSO")]
public class OptionsScriptableObject : ScriptableObject
{
    public int populationSize;
    public float mutationRate;
    public int elitism;
    public int blockSize;

}
