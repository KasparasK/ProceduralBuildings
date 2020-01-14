using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class BaseSizesTests
{
    [Test]
    public void Test_BaseSizeGroundFloor()
    {
        BuildingParams buildingParams = new BuildingParams();
        Vector3 foundationSize = new Vector3(2.5f,0.5f,0.5f);
        BaseParams baseParams = new BaseParams(foundationSize,buildingParams,OpeningStyle.ARCH,OpeningStyle.ARCH);
        Vector3 addTolastSize = baseParams.addToFoundationSize;

        Vector3 expextedMaxSize = foundationSize + addTolastSize;
        Vector3 expextedMinSize = baseParams.minBaseSize;


        Vector3 size = baseParams.GetGroundFloorFinalSize(foundationSize, expextedMinSize, addTolastSize);

        Assert.That(size.x, Is.EqualTo(expextedMaxSize.x).Within(0.01));
        Assert.That(size.z, Is.EqualTo(expextedMaxSize.z).Within(0.01));

        Assert.That(size.y, Is.GreaterThanOrEqualTo(expextedMinSize.y));
        Assert.That(size.y, Is.LessThanOrEqualTo(expextedMaxSize.y));

    }
    [Test]
    public void Test_BaseSizeNoFirewalls()
    {
        BuildingParams buildingParams = new BuildingParams();
        Vector3 lastFloorSize = new Vector3(2.5f, 2f, 2f);
        BaseParams baseParams = new BaseParams(lastFloorSize, buildingParams,1 ,OpeningStyle.ARCH);
        Vector3 addTolastSize = new Vector3(0.1f, 0.1f, 0.1f);

        Vector3 expextedMaxSize = lastFloorSize + addTolastSize;
        Vector3 expextedMinSize = lastFloorSize;


        Vector3 size = baseParams.GetFinalSize(lastFloorSize, addTolastSize);

        Assert.That(size.x, Is.GreaterThanOrEqualTo(expextedMinSize.x));
        Assert.That(size.y, Is.GreaterThanOrEqualTo(expextedMinSize.y));
        Assert.That(size.z, Is.GreaterThanOrEqualTo(expextedMinSize.z));

        Assert.That(size.x, Is.LessThanOrEqualTo(expextedMaxSize.x));
        Assert.That(size.y, Is.LessThanOrEqualTo(expextedMaxSize.y));
        Assert.That(size.z, Is.LessThanOrEqualTo(expextedMaxSize.z));

    }

    [Test]
    public void Test_BaseSizeAllFirewalls()
    {
        BuildingParams buildingParams = new BuildingParams();
        buildingParams.leftFirewall = true;
        buildingParams.rightFirewall = true;
        buildingParams.backFirewall= true;

        Vector3 lastFloorSize = new Vector3(2.5f, 2f, 2f);
        BaseParams baseParams = new BaseParams(lastFloorSize, buildingParams, 1, OpeningStyle.ARCH);
        Vector3 addTolastSize = new Vector3(1, 1, 1);

        Vector3 expextedMaxSize = lastFloorSize + new Vector3(0,addTolastSize.y,addTolastSize.z/2);
        Vector3 expextedMinSize = lastFloorSize;


        Vector3 size = baseParams.GetFinalSize(lastFloorSize, addTolastSize);

        Assert.That(size.x, Is.EqualTo(expextedMaxSize.x).Within(0.01));

        Assert.That(size.y, Is.GreaterThanOrEqualTo(expextedMinSize.y));
        Assert.That(size.y, Is.LessThanOrEqualTo(expextedMaxSize.y));

        Assert.That(size.z, Is.GreaterThanOrEqualTo(expextedMinSize.z));
        Assert.That(size.z, Is.LessThanOrEqualTo(expextedMaxSize.z));

    }

    [Test]
    public void Test_BaseSizeRightFirewall()
    {
        BuildingParams buildingParams = new BuildingParams();
        buildingParams.leftFirewall = false;
        buildingParams.rightFirewall = true;
        buildingParams.backFirewall = false;

        Vector3 lastFloorSize = new Vector3(2.5f, 2f, 2f);
        BaseParams baseParams = new BaseParams(lastFloorSize, buildingParams, 1, OpeningStyle.ARCH);
        Vector3 addTolastSize = new Vector3(1, 1, 1);

        Vector3 expextedMaxSize = lastFloorSize + new Vector3(addTolastSize.x/2, addTolastSize.y, addTolastSize.z);
        Vector3 expextedMinSize = lastFloorSize;


        Vector3 size = baseParams.GetFinalSize(lastFloorSize, addTolastSize);

        Assert.That(size.x, Is.GreaterThanOrEqualTo(expextedMinSize.x));
        Assert.That(size.x, Is.LessThanOrEqualTo(expextedMaxSize.x));

        Assert.That(size.y, Is.GreaterThanOrEqualTo(expextedMinSize.y));
        Assert.That(size.y, Is.LessThanOrEqualTo(expextedMaxSize.y));

        Assert.That(size.z, Is.GreaterThanOrEqualTo(expextedMinSize.z));
        Assert.That(size.z, Is.LessThanOrEqualTo(expextedMaxSize.z));

    }

    [Test]
    public void Test_BaseSizeLeftFirewall()
    {
        BuildingParams buildingParams = new BuildingParams();
        buildingParams.leftFirewall = true;
        buildingParams.rightFirewall = false;
        buildingParams.backFirewall = false;

        Vector3 lastFloorSize = new Vector3(2.5f, 2f, 2f);
        BaseParams baseParams = new BaseParams(lastFloorSize, buildingParams, 1, OpeningStyle.ARCH);
        Vector3 addTolastSize = new Vector3(1, 1, 1);

        Vector3 expextedMaxSize = lastFloorSize + new Vector3(addTolastSize.x / 2, addTolastSize.y, addTolastSize.z);
        Vector3 expextedMinSize = lastFloorSize;


        Vector3 size = baseParams.GetFinalSize(lastFloorSize, addTolastSize);

        Assert.That(size.x, Is.GreaterThanOrEqualTo(expextedMinSize.x));
        Assert.That(size.x, Is.LessThanOrEqualTo(expextedMaxSize.x));

        Assert.That(size.y, Is.GreaterThanOrEqualTo(expextedMinSize.y));
        Assert.That(size.y, Is.LessThanOrEqualTo(expextedMaxSize.y));

        Assert.That(size.z, Is.GreaterThanOrEqualTo(expextedMinSize.z));
        Assert.That(size.z, Is.LessThanOrEqualTo(expextedMaxSize.z));

    }

    [Test]
    public void Test_BaseSizeBackFirewall()
    {
        BuildingParams buildingParams = new BuildingParams();
        buildingParams.leftFirewall = false;
        buildingParams.rightFirewall = false;
        buildingParams.backFirewall = true;

        Vector3 lastFloorSize = new Vector3(2.5f, 2f, 2f);
        BaseParams baseParams = new BaseParams(lastFloorSize, buildingParams, 1, OpeningStyle.ARCH);
        Vector3 addTolastSize = new Vector3(1, 1, 1);

        Vector3 expextedMaxSize = lastFloorSize + new Vector3(addTolastSize.x, addTolastSize.y, addTolastSize.z/2);
        Vector3 expextedMinSize = lastFloorSize;


        Vector3 size = baseParams.GetFinalSize(lastFloorSize, addTolastSize);

        Assert.That(size.x, Is.GreaterThanOrEqualTo(expextedMinSize.x));
        Assert.That(size.x, Is.LessThanOrEqualTo(expextedMaxSize.x));

        Assert.That(size.y, Is.GreaterThanOrEqualTo(expextedMinSize.y));
        Assert.That(size.y, Is.LessThanOrEqualTo(expextedMaxSize.y));

        Assert.That(size.z, Is.GreaterThanOrEqualTo(expextedMinSize.z));
        Assert.That(size.z, Is.LessThanOrEqualTo(expextedMaxSize.z));

    }

    [Test]
    public void Test_BaseSizeLeftAndRightFirewall()
    {
        BuildingParams buildingParams = new BuildingParams();
        buildingParams.leftFirewall = true;
        buildingParams.rightFirewall = true;
        buildingParams.backFirewall = false;

        Vector3 lastFloorSize = new Vector3(2.5f, 2f, 2f);
        BaseParams baseParams = new BaseParams(lastFloorSize, buildingParams, 1, OpeningStyle.ARCH);
        Vector3 addTolastSize = new Vector3(1, 1, 1);

        Vector3 expextedMaxSize = lastFloorSize + new Vector3(0, addTolastSize.y, addTolastSize.z);
        Vector3 expextedMinSize = lastFloorSize;

        Vector3 size = baseParams.GetFinalSize(lastFloorSize, addTolastSize);

        Assert.That(size.x, Is.EqualTo(expextedMaxSize.x).Within(0.01));


        Assert.That(size.y, Is.GreaterThanOrEqualTo(expextedMinSize.y));
        Assert.That(size.y, Is.LessThanOrEqualTo(expextedMaxSize.y));

        Assert.That(size.z, Is.GreaterThanOrEqualTo(expextedMinSize.z));
        Assert.That(size.z, Is.LessThanOrEqualTo(expextedMaxSize.z));

    }
}
