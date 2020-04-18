using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {
    public static Action<int> OnSecondTick;

    public static void HandleOnSecondTick(int second) {
        OnSecondTick?.Invoke(second);
    }
}
