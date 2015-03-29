using UnityEngine;

// Used to store 2 linear graphs, connected by a mid point
// By default the midpoint is initialized at the bottom right of the axises
// Assumes the startpoint is bottom left while the endpoint is top right
public class LinerBiGraph
{
    Vector2 startPoint;
    Vector2 endPoint;
    Vector2 midPoint;

    public LinerBiGraph(Vector2 StartPoint, Vector2 EndPoint)
    {
        startPoint = StartPoint;
        endPoint = EndPoint;

        MoveMidPoint(endPoint.x, startPoint.y);        
    }

    public void ResetMidPoint()
    {
        MoveMidPoint(endPoint.x, startPoint.y);     
    }

    // Return the value of Y at that point on the graph
    public float Evaluate(float x)
    {
        float ratio = 0.0f;

        // Catch errors
        if (x < startPoint.x || x > endPoint.x)
        {
            Debug.LogWarning("x outside range of LinearBiGraph: " + x);
        }

        // Evaluate
        if (x == midPoint.x)
        {            
            return midPoint.y;
        }
        else if (x < midPoint.x)
        {
            // The length of x and y are proportional since it's linear
            // Find the percentage along the x line between the 2 points
            ratio = (x - startPoint.x) / (midPoint.x - startPoint.x);

            // Apply that to the length of the y and add to the bottom point            
            return ((midPoint.y - startPoint.y) * ratio) + startPoint.y;
        }
        else
        {
            ratio = (x - midPoint.x) / (endPoint.x - midPoint.x);

            return ((endPoint.y - midPoint.y) * ratio) + midPoint.y;
        }
    }

    // Move the mid point, is clamped inside the graphs
    public void MoveMidPoint(float newX, float newY)
    {
        newX = Mathf.Clamp(newX, startPoint.x, endPoint.x);
        newY = Mathf.Clamp(newY, startPoint.y, endPoint.y);

        midPoint = new Vector2(newX, newY);

        //Debug.Log("midPoint " + midPoint);
    }

    // Adjust the midpoint position based on the relative percentage given
    public void ScaleMidPoint(float newXPercent, float newYPercent)
    {
        // Find how long that percent is per axis
        float totalX = endPoint.x - startPoint.x;
        float totalY = endPoint.y - startPoint.y;

        // Find the new position of the midpoint
        float newX = midPoint.x + (totalX * newXPercent);
        float newY = midPoint.y + (totalY * newYPercent);

        MoveMidPoint(newX, newY);
    }
}
