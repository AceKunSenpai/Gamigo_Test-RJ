using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;

public class StringSorter : MonoBehaviour
{
    [Header("Input Settings")]
    [Tooltip("The string to be sorted.")]
    // public string inputString = "trion world network";
    [SerializeField] private TMP_InputField inputField;
    

    [Tooltip("The custom sort order string.")]
    // Must be unique characters and include all characters from the input string
    public string sortOrder;
    [SerializeField] private TMP_InputField sortOrderField;
    // [SerializeField] private uint testValue;

    [Header("Output")]
    [Tooltip("Result after sorting.")]
    public string sortedString;
    [SerializeField] private TMPro.TMP_Text resultText;   

    // TODO: Make Enum selection available in the inspector to choose sorting method
    public enum SortMethod
    {
        BubbleSort = 0,
        FastSort = 1
        // RJ: Additional sorting methods could be added here if needed (ex. QuickSort, MergeSort, etc.)
    }

    // SortMethod currentSortMethod = SortMethod.FastSort; // Change to BubbleSort to test that method

    void Start()
    {
        inputField.text = "trion world network";
        sortOrderField.text = " oinewkrtdl";

        // Run both versions for demonstration
        // string bubbleSorted = BubbleSortLetters(inputString, sortOrder);
        // string fastSorted = SortLetters(inputString, sortOrder);

        // Debug.Log("BubbleSort result: " + bubbleSorted);
        // Debug.Log("FastSort result: " + fastSorted);

        // Store one of them in the inspector field
        // sortedString = fastSorted;
    }

    public static string BubbleSortLetters(string input, string sortOrder)
    {
        var rank = new Dictionary<char, int>(sortOrder.Length);
        for (int i = 0; i < sortOrder.Length; i++)
            rank[sortOrder[i]] = i;

        char[] chars = input.ToCharArray();
        int n = chars.Length;
        bool swapped;
        do
        {
            swapped = false;
            for (int i = 1; i < n; i++)
            {
                if (rank[chars[i - 1]] > rank[chars[i]])
                {
                    char temp = chars[i - 1];
                    chars[i - 1] = chars[i];
                    chars[i] = temp;
                    swapped = true;
                }
            }
            n--;
        } while (swapped);

        return new string(chars);
    }

    public static string SortLetters(string input, string sortOrder)
    {
        var rank = new Dictionary<char, int>(sortOrder.Length);
        for (int i = 0; i < sortOrder.Length; i++)
            rank[sortOrder[i]] = i;

        char[] chars = input.ToCharArray();
        Array.Sort(chars, (a, b) => rank[a].CompareTo(rank[b]));
        return new string(chars);
    }

    // public void HandleSortType()

    public void checkSortOrderIfUniqueAndValid(int currentSortMethod)
    {
        string inputString = inputField.text;
        string sortOrder = sortOrderField.text;
        bool isValid = true;
        bool hasDuplicates = false;

        // Check if all characters in sortOrder are unique
        HashSet<char> uniqueChars = new HashSet<char>(sortOrder);
        if (uniqueChars.Count != sortOrder.Length)
        {
            resultText.text = "Sort order must contain unique characters.";
            hasDuplicates = true;
            return;
        }

        // Check if all characters in inputString are present in sortOrder
        foreach (char c in inputString) // RJ: For loop can be an alternative, but foreach is more concise for iterating through characters in a string
        {
            isValid = true;
            if (!uniqueChars.Contains(c))
            {
                resultText.text = $"Character '{c}' in input string is not in sort order.";
                isValid = false;
                return;
            }
        }

        // If valid, perform sorting
        if(!hasDuplicates && isValid)
        {
            resultText.text = "Sort order is valid and has unique characters.";
            // Proceed to sort the string
            switch((SortMethod)currentSortMethod)
            {
                // Traditional Bubble Sort method (inefficient for large strings, but included for demonstration)
                // RJ: This is intentionally inefficient to demonstrate the concept, but in practice, you would typically use the built-in sorting method for better performance.
                case SortMethod.BubbleSort: // Case currentSortMethod = 0
                    sortedString = BubbleSortLetters(inputString, sortOrder);
                    break;
                // Faster Method using built-in Array.Sort with custom comparer
                case SortMethod.FastSort: // Case currentSortMethod = 1
                    sortedString = SortLetters(inputString, sortOrder);
                    break;
                // RJ: Other Sorting methods could be implemented here if needed
                // (ex. QuickSort, MergeSort, etc.,) but for simplicity, we are using the built-in sort for the fast method.
            }
            resultText.text = "Sorted String: " + sortedString;
        }
        else
        {
            resultText.text = "Sort order is not valid.";
        }
    }
}
