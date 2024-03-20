using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Drop : MonoBehaviour
{
    public enum MathOperationType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division
    }

    public delegate void DropHandler(Drop drop);
    public event DropHandler OnDestroyed;
    public event DropHandler OnThresholdReached;

    private string equation;
    public TextMeshPro equationText;
    private float result;

    public float fallSpeed = 2.0f;
    public float startingFallSpeed;

    public bool isGoldenDrop = false;

    public int difficultyLevel = 1;
    private GameObject waterLevel;

    private void Start()
    {
        startingFallSpeed = fallSpeed;
        InvokeRepeating("IncreaseDifficulty", 10.0f, 10.0f);
    }

    private void OnEnable()
    {
        fallSpeed += UnityEngine.Random.Range(-0.2f, 0.2f);

        waterLevel = GameController.Instance.waterLevel;

        // Check which game mode was selected
        switch (GameController.Instance.GetCurrentGameMode())
        {
            case GameController.GameMode.Normal:
                GenerateOperation(1+difficultyLevel, 20+difficultyLevel);
                break;
            case GameController.GameMode.Binary:
                GenerateBinaryOperation(1+difficultyLevel, 20 + difficultyLevel);
                break;
            case GameController.GameMode.Nightmare:
                GenerateOperation(20 + difficultyLevel, 40 + difficultyLevel);
                break;
        }
        SetEquationText(equation);
    }

    void Update()
    {
        MoveDown();
    }

    void IncreaseDifficulty() 
    {
        difficultyLevel++;
    }

    void MoveDown()
    {
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        float dropBottom = transform.position.y - (GetComponent<Renderer>().bounds.size.y / 2);

        float waterTop = waterLevel.transform.position.y + (waterLevel.GetComponent<Renderer>().bounds.size.y / 2);

        // Check if drop touches the water
        if (dropBottom <= waterTop)
        {
            OnThresholdReached?.Invoke(this);
        }
    }

    // Generate the operation
    public void GenerateOperation(int minRange, int maxRange)
    {
        // Take two numbers between minRange and maxRange
        int operand1 = UnityEngine.Random.Range(minRange, maxRange);
        int operand2 = UnityEngine.Random.Range(minRange, maxRange);

        // Select a random operation from the enum
        MathOperationType mathOperationType = (MathOperationType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(MathOperationType)).Length);

        // Make sure that operand1 is bigger
        if (operand1 < operand2)
            (operand1, operand2) = (operand2, operand1);

        // For division and multiplication, simplify (don't go over 8)
        if (mathOperationType == MathOperationType.Division || mathOperationType == MathOperationType.Multiplication)
        {
            operand1 = UnityEngine.Random.Range(minRange, Mathf.Min(8, maxRange));
            operand2 = UnityEngine.Random.Range(1, Mathf.Min(8, maxRange));
        }

        // Based on the enum value, sum, subtract, multiplicate or divide
        switch (mathOperationType)
        {
            case MathOperationType.Addition:
                equation = $"{operand1} + {operand2}";
                result = operand1 + operand2;
                break;
            case MathOperationType.Subtraction:
                equation = $"{operand1} - {operand2}";
                result = operand1 - operand2;
                break;
            case MathOperationType.Multiplication:
                equation = $"{operand1} * {operand2}";
                result = operand1 * operand2;
                break;
            case MathOperationType.Division:
                operand1 = operand1 * operand2; // To avoid remainder
                equation = $"{operand1} / {operand2}";
                result = operand1 / operand2;
                break;
        }

        SetEquationText(equation);
    }

    //This function need to be optimized
    public void GenerateBinaryOperation(int minRange, int maxRange)
    {
        minRange = Mathf.Max(minRange, 1);
        maxRange = Mathf.Min(maxRange, 3); // 3 (11 in binary) will be the max value

        // Almost same as GenerateOperation
        int operand1 = UnityEngine.Random.Range(minRange, maxRange + 1);
        int operand2 = UnityEngine.Random.Range(minRange, maxRange + 1);

        MathOperationType operationType = (MathOperationType)UnityEngine.Random.Range(0, System.Enum.GetValues(typeof(MathOperationType)).Length);

        if (operationType == MathOperationType.Division || operationType == MathOperationType.Multiplication)
        {
            operand1 = UnityEngine.Random.Range(minRange, maxRange);
            operand2 = UnityEngine.Random.Range(1, maxRange);
        }

        int resultInt = 0;
        switch (operationType)
        {
            case MathOperationType.Addition:
                resultInt = operand1 + operand2;
                break;
            case MathOperationType.Subtraction:
                resultInt = operand1 - operand2;
                break;
            case MathOperationType.Multiplication:
                resultInt = operand1 * operand2;
                break;
            case MathOperationType.Division:
                operand2 = operand2 == 0 ? 1 : operand2; // If operand2 is 0, put 1 instead to avoid division by 0
                resultInt = operand1 / operand2;
                break;
        }

        // Convert operands and strings into binary
        string binaryOperand1 = Convert.ToString(operand1, 2).PadLeft(2, '0'); // Make sure to have always 2 numbers
        string binaryOperand2 = Convert.ToString(operand2, 2).PadLeft(2, '0');
        
        //string binaryResult = Convert.ToString(resultInt, 2).PadLeft(2, '0'); //If we want the result in binary

        // Set the equation
        equation = $"{binaryOperand1} {GetOperationSymbol(operationType)} {binaryOperand2}";
        result = resultInt;

        SetEquationText(equation);
    }

    // Get symbol based on operationType
    private string GetOperationSymbol(MathOperationType operationType)
    {
        switch (operationType)
        {
            case MathOperationType.Addition:
                return "+";
            case MathOperationType.Subtraction:
                return "-";
            case MathOperationType.Multiplication:
                return "*";
            case MathOperationType.Division:
                return "/";
            default:
                return "?";
        }
    }

    public void DeactivateDrop()
    {
        gameObject.SetActive(false);
        OnDestroyed?.Invoke(this);
    }

    public void AdjustFallSpeed(float factor)
    {
        fallSpeed *= factor; // Slow down drop
    }

    public void ResetFallSpeed()
    {
        fallSpeed = startingFallSpeed;
    }

    // Set UI inside the drop
    public void SetEquationText(string equation)
    {
        if (equationText != null)
            equationText.text = equation;
        else
            Debug.LogError("EquationText not assigned");
    }
    public float GetResult()
    {
        return result;
    }
}