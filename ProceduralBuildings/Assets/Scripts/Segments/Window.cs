using UnityEngine;

public class Window
{
    private Segment openingBase;
    private Segmentation segmentation;
    private Plane glass;
    private WindowParams winParams;
    public Window(
        Transform parent,
        Material material,
        WindowParams _winParams
    )
    {
        winParams = _winParams;

        CreateWindow(parent, material);
    }

    void CreateWindow(Transform parent, Material material)
    {

        if (winParams.openingStyle == OpeningStyle.ARCH)
            openingBase = new ArchedOpening(winParams.finalSize, material, winParams.archedOpeningParams);
        else
            openingBase = new SquareOpening(winParams.finalSize, material, winParams.squareOpeningParams);

        openingBase.obj.transform.parent = parent;
        openingBase.obj.transform.localPosition = winParams.finalPos;
        openingBase.obj.transform.localRotation = winParams.finalRot;

        segmentation = new Segmentation(openingBase.obj.transform, winParams.finalSize, material, winParams.segmentationParams);

        glass = new Plane(material, openingBase.obj.transform, winParams.finalSize, winParams.glassParams);
        glass.obj.transform.localPosition = winParams.glassParams.finalPos;
        glass.obj.transform.localRotation = winParams.glassParams.finalRot;

    }

}
