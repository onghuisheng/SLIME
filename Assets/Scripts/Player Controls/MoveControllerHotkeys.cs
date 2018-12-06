using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveControllerButton
{
    NONE,
    X,
    Circle,
    Square,
    Triangle,
    BackTrigger,
    MiddleButton,
    Start
}

public class MoveControllerHotkeys
{
    public static MoveControllerButton buttonGrab = MoveControllerButton.BackTrigger;
    public static MoveControllerButton buttonUse = MoveControllerButton.MiddleButton;
    public static MoveControllerButton buttonReset = MoveControllerButton.Square;
}