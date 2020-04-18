using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDestroyingEvent : LocalEvent
{
    protected override void Execute()
    {
        Debug.Log("Building is destroyed");
    }

}
