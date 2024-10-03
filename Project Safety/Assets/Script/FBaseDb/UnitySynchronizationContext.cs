using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class UnitySynchronizationContext : MonoBehaviour
{
    private static SynchronizationContext _syncContext;

    void Awake()
    {
        _syncContext = SynchronizationContext.Current;
    }

    public static void ExecuteOnMainThread(System.Action action)
    {
        _syncContext.Post(_ => action(), null);
    }
}
