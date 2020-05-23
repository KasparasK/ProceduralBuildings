using UnityEngine;
using NUnit.Framework;
public class RoofSizeTests 
{
 
    [Test]
    public void Test_CalculateRoofSizePositive()
    {
        RoofParams roofParams = new RoofParams(Vector3.zero, Vector3.zero);

        Vector3 atticSize = new Vector3(2, 2, 2);
        float roofThiccness = 0.1f;
        float zToAdd = 0.3f;

        Vector3 expectedSize = new Vector3(roofThiccness, 4, 2+zToAdd);

        Vector3 size = roofParams.GetFinalSize(atticSize, zToAdd, roofThiccness);

        Assert.That(size, Is.EqualTo(expectedSize));
    }
}
