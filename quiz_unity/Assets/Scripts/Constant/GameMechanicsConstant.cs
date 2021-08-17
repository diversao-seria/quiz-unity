using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class GameMechanicsConstant
{
    public enum PowerUpNames {
        Earth = 0,
        Water = 1,
        Air = 2
    }

    public static readonly int PowerUpCount = Enum.GetNames(typeof(PowerUpNames)).Length;
}
