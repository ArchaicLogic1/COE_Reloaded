using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cape : MonoBehaviour
{
    [SerializeField]  int length;
    [SerializeField] LineRenderer myLineRenderer;
    [SerializeField] Vector3[] segmentPoses;
    [SerializeField] Vector3[] segmentVelocity;
    [SerializeField] Transform targetDir;
    [SerializeField] float targetDist;
    [SerializeField] float smoothSpeed, trailSpeed;

    [SerializeField] float billowSpeed;
    [SerializeField] float billowMagnitude;
    [SerializeField] Transform billowDir;
    


    // Start is called before the first frame update
    void Start()
    {
        myLineRenderer.positionCount = length;
        segmentPoses = new Vector3[length];
        segmentVelocity = new Vector3[length];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        billowDir.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * billowSpeed) * billowMagnitude);
        segmentPoses[0] = targetDir.position;
        for (int i = 1; i < segmentPoses.Length; i++)
        {
            segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i],segmentPoses[i-1] + targetDir.right* targetDist,ref segmentVelocity[i], smoothSpeed+ i/trailSpeed);

        }
        myLineRenderer.SetPositions(segmentPoses);
    }
}
