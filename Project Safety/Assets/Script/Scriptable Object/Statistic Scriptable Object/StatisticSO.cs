using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Statistic Additional Information")]
public class StatisticSO : ScriptableObject
{
    [TextArea(3,10)]
    public string englishStatisticAdditionalInformation;

    [TextArea(3, 10)]
    public string tagalogStatisticAdditionalInformation;
}
