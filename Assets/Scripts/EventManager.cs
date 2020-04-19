using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {
    public static Action<int> OnSecondTick;

    public static Action OnCameraShake;

    public static void HandleOnSecondTick(int second) {
        OnSecondTick?.Invoke(second);
    }

    public static void HandleOnCameraShake ()
    {
        OnCameraShake?.Invoke();
    }
}
