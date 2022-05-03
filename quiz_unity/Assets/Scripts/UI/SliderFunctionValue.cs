using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SliderFunctionValue 
{

    // After y = F(X) is calulated, we need to multiply by number tom kae sure that F(1.5) = 1.0;
    private static double normalizingFactor;


    // Linear? Exponential? Polynomial?
    private static GameMechanicsConstant.VisualSliderChange currentMode = GameMechanicsConstant.VisualSliderChange.Linear;

    public static void SetCalculationMode(GameMechanicsConstant.VisualSliderChange mode)
    {
        currentMode = mode;
        SetNormalizingFactor();
    }

    public static float ProgressCalculation(float totalTimeElapsed)
    {
        switch (currentMode)
        {
            case GameMechanicsConstant.VisualSliderChange.Linear:
                return (float) LineFunctionCalculation(totalTimeElapsed);
            case GameMechanicsConstant.VisualSliderChange.Polynomial:
                return (float) PolynomialFunctionCalculation(totalTimeElapsed);
            case GameMechanicsConstant.VisualSliderChange.Exponential:
                return (float) ExponentialFunctionCalculation(totalTimeElapsed);
            default:
                Debug.Log("ERRO: Modo não reconhecido. Aplicando modo linear...");
                return (float) LineFunctionCalculation(totalTimeElapsed);
        }
    }

    private static void SetNormalizingFactor()
    {
        switch (currentMode)
        {
            case GameMechanicsConstant.VisualSliderChange.Linear:
                normalizingFactor = (2.0f / 3.0f);
                break;
            case GameMechanicsConstant.VisualSliderChange.Polynomial:
                normalizingFactor = (8.0f / 21.0f);
                break;
            case GameMechanicsConstant.VisualSliderChange.Exponential:
                normalizingFactor = (0.54691816067);
                Debug.Log("");
                break;
            default:
                normalizingFactor = (2.0f / 3.0f);
                Debug.Log("ERRO: Modo não reconhecido. Aplicando modo linear...");
                break;
        }

    }

    private static double LineFunctionCalculation(float totalTimeElapsed)
    {
        // F(X) = (X) * 1.5)

        return (totalTimeElapsed) * normalizingFactor;
    }

    private static double PolynomialFunctionCalculation(float totalTimeElapsed)
    {
        // (8/21) * (X^3 - X^2 + X)

        return (
            ((totalTimeElapsed) * (totalTimeElapsed) * (totalTimeElapsed)) -
            ((totalTimeElapsed) * (totalTimeElapsed)) +
             (totalTimeElapsed)
            ) * (normalizingFactor);

    }

    private static double ExponentialFunctionCalculation(float totalTimeElapsed)
    {
        // (2^x - 1) * 0.54691816067

        return (Mathf.Pow(2, totalTimeElapsed) - 1) * (normalizingFactor);
    }
}
