using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class GameMechanicsConstant
{
    public enum PowerUpNames 
    {
        Earth = 0,
        Water = 1,
        Air = 2
    }

    public static readonly int PowerUpCount = Enum.GetNames(typeof(PowerUpNames)).Length;

    public static readonly float AnswerConfirmationTimeinSeconds = 2.0f;
    public static readonly float FeedbackPowerUpAnimationTimeiNSeconds = 2.0f;

    public static readonly float IceAnimationTimeinSeconds = 1.5f;
    public static readonly float WindAnimationTimeinSeconds = 1.0f;
    public static readonly float LeafAnimationTimeinSeconds = 2.0f;
}
