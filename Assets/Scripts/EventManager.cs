﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {
    public static Action<int> OnSecondTick;

    public static void HandleOnSecondTick(int second) {
        OnSecondTick?.Invoke(second);
    }

    public static Action OnCameraShake;

    public static void HandleOnCameraShake() {
        OnCameraShake?.Invoke();
    }

    public enum EndGameType {
        Win,
        Die,
        LowBattery
    }

    public static Action<EndGameType> OnEndGame;

    public static void HandleOnEndGame(EndGameType type) {
        OnEndGame?.Invoke(type);
    }
}
