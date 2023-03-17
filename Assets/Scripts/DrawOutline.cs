using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOutline : MonoBehaviour
{
    // Start is called before the first frame update
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider2D;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider2D = GetComponent<EdgeCollider2D>();
        lineRenderer.positionCount = edgeCollider2D.points.Length;
    }
    void Start()
    {
        Vector2[] points = edgeCollider2D.points;
        for (int i = 0; i < points.Length; i++)
        {
            lineRenderer.SetPosition(i, new Vector3(points[i].x, points[i].y, transform.position.z));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
