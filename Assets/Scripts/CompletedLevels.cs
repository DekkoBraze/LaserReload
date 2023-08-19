using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletedLevels : MonoBehaviour
{
    Dictionary<string, bool> LevelsManagment = new Dictionary<string, bool>()
    {
        ["Level1"] = true,
        ["Level2"] = false,
        ["Level3"] = false,
        ["Level4"] = false,
        ["Level5"] = false,
        ["Level6"] = false,
        ["Level7"] = false,
        ["Level8"] = false,
        ["Level9"] = false,
    };
}
