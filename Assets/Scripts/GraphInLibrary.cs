using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GraphInLibrary : MonoBehaviour
{
    [SerializeField]
    Transform pointPrefab = default;
    [SerializeField, Range(10, 100)]
    int resolution = 10;

    [SerializeField] private FunctionLibrary.FunctionName _functionName = default;

    Transform[] points;
    void Awake()
    {
        points = new Transform[resolution];
        float step = 2f/ resolution;
        Vector3 scale = Vector3.one * step;
        Vector3 position = Vector3.zero;
        for(int i = 0; i < points.Length; i++)
        {
            Transform point = Instantiate(pointPrefab);
            position.x = (i + 0.5f) * step - 1f;
            // position.y = position.x * position.x * position.x;
            point.localPosition = position;
            point.localScale = scale;
            point.SetParent(transform, false);
            points[i] = point;
        }
        
    }

    void Update()
    {
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(_functionName);
        float time = Time.time;
        for(int i = 0; i < points.Length; i++)
        {
            Transform point = points[i];
            Vector3 position = point.localPosition;
            position.y = f(position.x, time);
            point.localPosition = position;
        }
    }
}
