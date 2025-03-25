using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMove : MonoBehaviour
{
    [SerializeField] private List<PointPair> cameraPoints;
    [SerializeField] private float moveDuration = 4f;

    private Dictionary<PointType, Transform> pointMap = new Dictionary<PointType, Transform>();
    
    private PointType currentPointType;

    private bool isMove = false;

    private static List<CameraMove> camerasList = new List<CameraMove>();
    private void Awake()
    {
        foreach (var cameraPoint in cameraPoints)
        {
            this.pointMap.TryAdd(cameraPoint.typePoint, cameraPoint.point);
        }
        
        camerasList.Add(this);

        this.ChangePoint(PointType.Origin);
    }

    private void OnDestroy()
    {
        camerasList.Remove(this);
    }

    public void ChangePoint(PointType pointType)
    {
        if (currentPointType == pointType && this.transform.position == this.pointMap[pointType].position)
            return;

        if (isMove)
            DOTween.Kill(this.transform);

        var newPoint = this.pointMap[pointType];

        this.currentPointType = pointType;

        isMove = true;
        
        this.transform.DOMove(newPoint.position, moveDuration);
        this.transform.DORotate(newPoint.rotation.eulerAngles, moveDuration).OnComplete(() => isMove = false);
    }

    public static CameraMove GetFirst()
    {
        return camerasList[0];
    }
}

public enum PointType
{
    Origin = 0,
    BackPack = 1,
}

[System.Serializable]
public class PointPair
{
    public Transform point;
    public PointType typePoint;
}
