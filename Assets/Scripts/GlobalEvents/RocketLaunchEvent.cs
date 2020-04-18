using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLaunchEvent : GlobalEvent {
    protected override void Execute() {
        Debug.Log("Rocket Launched");
    }
}
