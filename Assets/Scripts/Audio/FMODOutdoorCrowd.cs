using UnityEngine;
using FMODUnity;
using System.Collections.Generic;
/* using Unity.VisualScripting;
//using System.Drawing;
using System;

//using System.Linq;
//using System.Numerics; */

public class FMODOutdoorCrowd : MonoBehaviour
{
    [Header("FMOD Configuration")]
    [Tooltip("FMOD Parameter Name (0 - 50)")]
    public string mixParameterName;
    public StudioEventEmitter eventEmitter;
    public LayerMask environmentLayer;

    [Header("Rays")]
    public GameObject targetObj;
    public FMODSmartReverb scannerSource;
    public int raysCount = 30;
    public float maxScanDistance = 100.0f;
    private Vector3[] rayDirections;
    public float currDistance;
    private List<float> adjustedDist;
    private Vector3[] debugHitPoints;
    private bool[] debugDidHit;

    // Shoot out a handful of rays around camera
    // 
    void Start()
    {
        // Auto-resolve dependency if missing
        if (scannerSource == null)
            scannerSource = FindFirstObjectByType<FMODSmartReverb>();
        // get that collection of ray directions for raycast
        InitializeFibonacciSphere();
    }

    void Update()
    {
        ScanTime();
        eventEmitter.SetParameter(mixParameterName, currDistance);
        adjustedDist.Clear();
    }

     void InitializeFibonacciSphere()
    {
        rayDirections = new Vector3[raysCount];
        debugHitPoints = new Vector3[raysCount];
        debugDidHit = new bool[raysCount];
       
        float goldenRatio = (1 + Mathf.Sqrt(5)) / 2;
        float angleIncrement = Mathf.PI * 2 * goldenRatio;

        for (int i = 0; i < raysCount; i++)
        {
            float t = (float)i / raysCount;

            // Acos ensures equal area distribution (prevents clumping at poles)
            float inclination = Mathf.Acos(1 - 2 * t);
            // Golden Angle ensures spiral distribution
            float azimuth = angleIncrement * i;

            float x = Mathf.Sin(inclination) * Mathf.Cos(azimuth);
            float y = Mathf.Sin(inclination) * Mathf.Sin(azimuth);
            float z = Mathf.Cos(inclination);

            rayDirections[i] = new Vector3(x, y, z);
        }
    }

    void ScanTime()
    {
        float totalDist = 0f;
        adjustedDist = new List<float>();
        // if the ray direction collection is empty, or if the amount of ray directions collected
        // is less than the amount of rays we want, get those ray directions again
        if (rayDirections == null || rayDirections.Length != raysCount) InitializeFibonacciSphere();

        // let's cast those rays
        for (int i = 0; i < raysCount; i++) {
            
            // apply that ray direction to the camera
            Vector3 dir = transform.TransformDirection(rayDirections[i]);

             if (Physics.Raycast(transform.position, dir, out RaycastHit hit, maxScanDistance, environmentLayer))
            {
                debugDidHit[i] = true;
                debugHitPoints[i] = hit.point;
                // check to see if it's a stadium
                // if the hit object's parent's name is stadium
                if (hit.collider.transform.root.name == "Stadium") {
                    // then add the distance to the list of distances
                    Debug.Log("hit stadium");
                    adjustedDist.Add(hit.distance);
                    totalDist += hit.distance;
                    /* debugDidHit[i] = true;
                    debugHitPoints[i] = hit.point; */
                    
                }
            } else  {
                debugHitPoints[i] = transform.position + (dir * maxScanDistance);
                debugDidHit[i] = false;
            }
        }
        // okay yay casting done!
        // get average distance in the list collected
        float avgDist = totalDist / raysCount;
        // assign to distance holder
        currDistance = avgDist;
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying && debugHitPoints != null && debugHitPoints.Length > 0)
        {
            // Runtime Visualization
            for (int i = 0; i < debugHitPoints.Length; i++)
            {
                Gizmos.color = debugDidHit[i] ? new Color(0.5f, 0f, 1f, 0.8f) : new Color(0.5f, 0.5f, 0.5f, 1f);
                Gizmos.DrawLine(transform.position, debugHitPoints[i]);
                if (debugDidHit[i]) Gizmos.DrawSphere(debugHitPoints[i], 0.1f);
            }
        }
        else
        {
            // Editor Preview (Yellow Sphere)
            Gizmos.color = new Color(1f, 0.92f, 0.016f, 0.3f);
            if (rayDirections == null || rayDirections.Length != raysCount) InitializeFibonacciSphere();

            foreach (var localDir in rayDirections)
            {
                Vector3 worldDir = transform.TransformDirection(localDir);
                Gizmos.DrawRay(transform.position, worldDir * maxScanDistance);
            }
            Gizmos.color = new Color(1f, 1f, 1f, 0.1f);
            Gizmos.DrawWireSphere(transform.position, maxScanDistance);
        }
    }




  /*   void Start()
    {
        if (scannerSource == null)
        {
            scannerSource = FindFirstObjectByType<FMODSmartReverb>();
        }
        Debug.Log(scannerSource.distances);
    }

    void Update()
    {
        GetMinimumDistance();
        foreach (var x in adjustedDist)
        {
            Debug.Log(x);
        }
        eventEmitter.SetParameter(mixParameterName, minDistance);
        adjustedDist.Clear();
    }

    void GetMinimumDistance()
    {
        adjustedDist = new List<float>();
        for (int i = 0; i < scannerSource.distances.Length; i++)
        {
            if (scannerSource.distances[i] != 0.0f)
            {
                adjustedDist.Add(scannerSource.distances[i]);
            } 
            i++;
        }
        if (adjustedDist.Any()) minDistance = adjustedDist.Min();
    } */

}
