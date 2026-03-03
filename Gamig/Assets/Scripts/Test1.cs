using UnityEngine;
using UnityEngine.UI;
using TMPro;


/*
Write a function that takes an unsigned integer as input and returns true if all the digits in the base 10 representation of that number are unique. 

public static bool AllDigitsUnique(uint value) { } 

Example: 
AllDigitsUnique(48778584) returns false 
AllDigitsUnique(17308459) returns true 
*/

public class Test1 : MonoBehaviour
{
    [SerializeField] private uint testValue;
    [SerializeField] private TMPro.TMP_Text resultText;
    // [SerializeField] private InputField inputField;
    [SerializeField] private TMP_InputField inputField;


    public static bool AllDigitsUnique(uint value)
    {
        // Boolean array to track digits 0–9
        bool[] seen = new bool[10];

        while (value > 0)
        {
            int digit = (int)(value % 10); // Extract last digit via modulo operation
            if (seen[digit]) 
                return false; // Digit already encountered
            seen[digit] = true;
            value /= 10; // Remove last digit via integer division by 10 (remaining digits shift right)
        }

        return true; // No duplicates found
    }

    public void CheckDigits() // Use this to test for answers
    {
        // Parse input value from the input field
        if (uint.TryParse(inputField.text, out testValue))
        {
            if (AllDigitsUnique(testValue))
            {
                resultText.text = "All digits are unique.";
            }
            else
            {
                resultText.text = "Digits are not unique.";
            }
        }
        else
        {
            resultText.text = "Invalid input. Please enter a valid unsigned integer.";
        }
    }
}
