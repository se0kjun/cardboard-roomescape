using UnityEngine;
using System.Collections;

public interface ITrickManager {
    bool MoveFlag { get; }
    bool GrabFlag { get; set; }
    GameObject TargetObject { get; set; }
    GameObject MovingObject { get; }

    void onEventMethod();
    void onTargetTrigger();
}

