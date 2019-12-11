using UnityEngine;

public class Window
{
    private Segment openingBase;
    private Segmentation segmentation;
    private Plane glass;

    public Window(
        Transform parent,
        Material material,
        WindowParams winParams
    )
    {
        if(winParams.openingStyle == OpeningStyle.ARCH)
            openingBase = new ArchedOpening(winParams.finalSize, material, winParams.winFrameDimensions,winParams.frameColor, winParams); 
        else
            openingBase = new SquareOpening(winParams.finalSize, material, winParams.winFrameDimensions, winParams.frameColor);

        openingBase.obj.transform.parent = parent;
        openingBase.obj.transform.localPosition = winParams.finalPos;
        openingBase.obj.transform.localRotation = winParams.rot;

        segmentation = new Segmentation(openingBase.obj, winParams.finalSize, winParams.segmentDimensions, material, winParams.vertSegPositions, winParams.horSegPositions, winParams.segmentsColor);
        if (winParams.openingStyle == OpeningStyle.ARCH)
            glass = new Plane(material, openingBase.obj.transform, winParams.finalSize, winParams.glassColor, winParams.innerArcF);
        else
            glass = new Plane(material, openingBase.obj.transform, winParams.finalSize, winParams.glassColor);

        glass.obj.transform.localPosition = new Vector3(winParams.finalSize.x, 0, winParams.windowOffset+0.01f);
        glass.obj.transform.localRotation = Quaternion.Euler(new Vector3(0,180,0));


    }

}
