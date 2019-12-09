using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcGenerator 
{
    public Vector3[] GenerationZwei(float angle,float distance, Vector3 toAdd,int resolution)
    {
        float g =  Mathf.Abs(Physics.gravity.y); //gravity on y
        float velocity;

        Vector3[] arrArray = new Vector3[resolution + 1];

        float radianAngle = Mathf.Deg2Rad * angle;
        velocity = Mathf.Sqrt(distance / ((Mathf.Sin(2 * radianAngle)) / g));

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float) i / (float)resolution;

            arrArray[i] = CalculateArcPoint(t,distance,radianAngle,g,velocity) + toAdd;
        }

        return arrArray;
    }

    private Vector3 CalculateArcPoint(float t, float maxDistance,float radianAngle,float g,float velocity)
    {
        float x = t * maxDistance;
        float y = x* Mathf.Tan(radianAngle) - ((g*x*x)/(2*velocity*velocity*Mathf.Cos(radianAngle)*Mathf.Cos(radianAngle)));
        return new Vector3(x,y,0);
    }

}
