using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
public class BasePosTests 
{
    [Test]
    public void Test_BasePosGroundFloor()
    {
        BuildingParams buildingParams = new BuildingParams();
        Vector3 lastFloorSize = new Vector3(2.5f, 2f, 2f);
        Vector3 addTolastSize = new Vector3(1f, 1f, 1f);

        BaseParams baseParams = new BaseParams(lastFloorSize, buildingParams, 1, OpeningStyle.ARCH);
        baseParams.finalSize = lastFloorSize + addTolastSize;

        Vector3 expextedPos = new Vector3(-0.5f, 2, -0.5f);

        Vector3 pos = baseParams.GetGroundFloorFinalPosition(lastFloorSize);

        Assert.That(pos.x, Is.EqualTo(expextedPos.x).Within(0.001));
    }

    [Test]
    public void Test_BasePosNoFirewall()
    {
        BuildingParams buildingParams = new BuildingParams();
        Vector3 lastFloorSize = new Vector3(2.5f, 2f, 2f);
        Vector3 addTolastSize = new Vector3(1f, 1f, 1f);

        BaseParams baseParams = new BaseParams(lastFloorSize, buildingParams, 1, OpeningStyle.ARCH);
        baseParams.finalSize = lastFloorSize + addTolastSize;


        Vector3 expextedPos = new Vector3(-0.5f,2,-0.5f);


        Vector3 pos = baseParams.GetFinalPosition(lastFloorSize);

        Assert.That(pos.x, Is.EqualTo(expextedPos.x).Within(0.001));

    }

    [Test]
    public void Test_BasePosAllFirewall()
    {
        BuildingParams buildingParams = new BuildingParams();
        buildingParams.leftFirewall = true;
        buildingParams.rightFirewall = true;
        buildingParams.backFirewall = true;

        Vector3 lastFloorSize = new Vector3(2.5f, 2f, 2f);
        Vector3 addTolastSize = new Vector3(1f, 1f, 1f);

        BaseParams baseParams = new BaseParams(lastFloorSize, buildingParams, 1, OpeningStyle.ARCH);
        baseParams.finalSize = lastFloorSize + addTolastSize;

        Vector3 expextedPos = new Vector3(-1, 2, -1);

        Vector3 pos = baseParams.GetFinalPosition(lastFloorSize);

        Assert.That(pos.x, Is.EqualTo(expextedPos.x).Within(0.001));

    }

    [Test]
    public void Test_BasePosLeftFirewall()
    {
        BuildingParams buildingParams = new BuildingParams();
        buildingParams.leftFirewall = true;
        buildingParams.rightFirewall = false;
        buildingParams.backFirewall = false;

        Vector3 lastFloorSize = new Vector3(2.5f, 2f, 2f);
        Vector3 addTolastSize = new Vector3(1f, 1f, 1f);

        BaseParams baseParams = new BaseParams(lastFloorSize, buildingParams, 1, OpeningStyle.ARCH);
        baseParams.finalSize = lastFloorSize + addTolastSize;

        Vector3 expextedPos = new Vector3(0, 2, -0.5f);

        Vector3 pos = baseParams.GetFinalPosition(lastFloorSize);

        Assert.That(pos.x, Is.EqualTo(expextedPos.x).Within(0.001));

    }

    [Test]
    public void Test_BasePosRightFirewall()
    {
        BuildingParams buildingParams = new BuildingParams();
        buildingParams.leftFirewall = false;
        buildingParams.rightFirewall = true;
        buildingParams.backFirewall = false;

        Vector3 lastFloorSize = new Vector3(2.5f, 2f, 2f);
        Vector3 addTolastSize = new Vector3(1f, 1f, 1f);

        BaseParams baseParams = new BaseParams(lastFloorSize, buildingParams, 1, OpeningStyle.ARCH);
        baseParams.finalSize = lastFloorSize + addTolastSize;

        Vector3 expextedPos = new Vector3(-1, 2, -0.5f);

        Vector3 pos = baseParams.GetFinalPosition(lastFloorSize);

        Assert.That(pos.x, Is.EqualTo(expextedPos.x).Within(0.001));

    }

    [Test]
    public void Test_BasePosBackFirewall()
    {
        BuildingParams buildingParams = new BuildingParams();
        buildingParams.leftFirewall = false;
        buildingParams.rightFirewall = false;
        buildingParams.backFirewall = true;

        Vector3 lastFloorSize = new Vector3(2.5f, 2f, 2f);
        Vector3 addTolastSize = new Vector3(1f, 1f, 1f);

        BaseParams baseParams = new BaseParams(lastFloorSize, buildingParams, 1, OpeningStyle.ARCH);
        baseParams.finalSize = lastFloorSize + addTolastSize;

        Vector3 expextedPos = new Vector3(-0.5f, 2, -1);

        Vector3 pos = baseParams.GetFinalPosition(lastFloorSize);

        Assert.That(pos.x, Is.EqualTo(expextedPos.x).Within(0.001));

    }
}
