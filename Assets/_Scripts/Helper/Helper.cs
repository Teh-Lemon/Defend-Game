using UnityEngine;
using System.Collections.Generic;

public static class Helper
{
    // Separates out the individual digits in an integer into a list
    // Requires a positive number
    public static List<int> SeparateDigits(int number)
    {
        var digitsList = new List<int>();
        
        while (number > 0)
        {
            digitsList.Add(number % 10);
            number = number / 10;
        }
        digitsList.Reverse();

        return digitsList;
    }
}
