using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point
{
    public float height;
    public float temperature;

    List<Point> linkedPoints;
    
    public Point()
    {
        linkedPoints = new List<Point>();
    }
}
