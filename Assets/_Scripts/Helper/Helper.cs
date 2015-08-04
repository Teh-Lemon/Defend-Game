using UnityEngine;
using System.Collections.Generic;

public static class Helper
{
    /// <summary>
    /// Separates out the individual digits in an integer into a list
    /// Requires a positive number
    /// </summary>
    /// <param name="number"></param>
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

    /// <summary>
    /// Returns either -1.0f or 1.0f
    /// </summary>
    public static float RandomSign()
    {
        return Random.value < .5 ? 1.0f : -1.0f;
    }
 
    /// <summary>
    /// Returns whether the number is positive or negative
    /// </summary>
    /// <returns>1 or -1</returns>
    public static int GetSign(float number)
    {
        if ( number >= 0 )
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }
}
