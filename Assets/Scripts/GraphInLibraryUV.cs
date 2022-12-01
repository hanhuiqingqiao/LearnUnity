using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GraphInLibraryUV : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab = default;
    [SerializeField, Range(10, 100)]
    int resolution = 10;

    [SerializeField] private FunctionLibraryUV.FunctionName _functionName = default;

    Transform[] points;
    void Awake()
    {
        points = new Transform[resolution * resolution];
        float step = 2f/ resolution;
        Vector3 scale = Vector3.one * step;
        for(int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            Transform point = Instantiate(pointPrefab);
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
        
    }

    void Update()
    {
        FunctionLibraryUV.Function f = FunctionLibraryUV.GetFunction(_functionName);
        float time = Time.time;
        float step = 2f/ resolution;
        float v = 0.5f * step - 1f;
        for(int i = 0, x = 0, z = 0; i < points.Length; i++, x++)
        {
            if (x == resolution)
            {
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            
            points[i].localPosition = f(u, v, time);
        }
    }
}
