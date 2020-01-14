using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class RoofPositionTests 
{
    [Test]
    public void Test_CalculateRoofPosPositive()
    {
        RoofParams roofParams = new RoofParams(Vector3.zero, Vector3.zero);

        Vector3 lastBaseSize = new Vector3(2, 2, 2);
        Vector3 finalRoofSize = new Vector3(2.5f,3,2.5f);

        Vector3 expectedPos = new Vector3(0,2,-0.25f);

        Vector3 pos = roofParams.GetFinalPosition(lastBaseSize, finalRoofSize);

        Assert.That(pos,Is.EqualTo(expectedPos));
    }
    [Test]
    public void Test_CalculateRoofPosNegative()
    {
        RoofParams roofParams = new RoofParams(Vector3.zero, Vector3.zero);

        Vector3 lastBaseSize = new Vector3(2, 2, 2);
        Vector3 finalRoofSize = new Vector3(1.5f, 1, 1.5f);

        Vector3 expectedPos = new Vector3(0, 2, 0.25f);

        Vector3 pos = roofParams.GetFinalPosition(lastBaseSize, finalRoofSize);

        Assert.That(pos, Is.EqualTo(expectedPos));
    }
}
