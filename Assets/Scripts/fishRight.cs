using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class fishRight : MonoBehaviour
{
    [SerializeField] public GameObject eye;
    [SerializeField] private GameObject outline;
    [SerializeField] private GameObject pointBackBottom;
    [SerializeField] private GameObject pointBackTop;
    [SerializeField] private GameObject pointTailBottom;
    [SerializeField] private GameObject pointTailTop;
    [SerializeField] private GameObject pointBodyBottom;
    [SerializeField] private GameObject pointBodyTop;
    [SerializeField] private GameObject pointFrontBottom;
    [SerializeField] private GameObject pointFrontTop;
    [SerializeField] private EdgeCollider2D triangleCollider;
    private LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = outline.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        updatePoints();
        updateLines();
    }

    // update the position of each point using the helper function UpdatePoint
    void updatePoints()
    {
        float eyeX = eye.transform.position.x;
        float eyeY = eye.transform.position.y;

        // update top of fish point positions
        UpdatePoint(pointFrontTop, new Vector3(eyeX - 1, eyeY + 0.5f, 0));
        UpdatePoint(pointBodyTop, new Vector3(eyeX, eyeY + 1f, 0));
        UpdatePoint(pointTailTop, new Vector3(eyeX + 1, eyeY + 0.5f, 0));
        UpdatePoint(pointBackTop, new Vector3(eyeX + 2, eyeY + 1f, 0));

        // update bottom of fish point positions
        UpdatePoint(pointFrontBottom, new Vector3(eyeX - 1, eyeY - 0.5f, 0));
        UpdatePoint(pointBodyBottom, new Vector3(eyeX, eyeY - 1f, 0));
        UpdatePoint(pointTailBottom, new Vector3(eyeX + 1, eyeY - 0.5f, 0));
        UpdatePoint(pointBackBottom, new Vector3(eyeX + 2, eyeY - 1f, 0));
    }

    // update the lines to ensure they are connected to the points
    void updateLines()
    {
        lineRenderer.SetPosition(0, pointFrontBottom.transform.position);
        lineRenderer.SetPosition(1, pointFrontTop.transform.position);
        lineRenderer.SetPosition(2, pointBodyTop.transform.position);
        lineRenderer.SetPosition(3, pointTailTop.transform.position);
        lineRenderer.SetPosition(4, pointBackTop.transform.position);
        lineRenderer.SetPosition(5, pointBackBottom.transform.position);
        lineRenderer.SetPosition(6, pointTailBottom.transform.position);
        lineRenderer.SetPosition(7, pointBodyBottom.transform.position);
        lineRenderer.SetPosition(8, pointFrontBottom.transform.position);
    }

    // check if a point will collide with the obstacle (triangle)
    bool PointInsideObstacle(Vector2 point)
    {
        // check which colliders the point hits, if one of them is the triangle, return true
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, 0.25f);

        foreach (Collider2D col in colliders)
        {
            if (col.gameObject.tag == "triangle")
            {
                return true;
            }
        }
        return false;
    }

    // update a point given its' ideal position
    void UpdatePoint(GameObject point, Vector3 coordinates)
    {
        Vector3 newPosition = coordinates;

        // if the ideal position is in the ground, raise it so it does not penetrate the ground
        if (newPosition.y < -2.6)
        {
            newPosition = new Vector3(point.transform.position.x, -2.9f, 0);
        }

        // if the ideal position is in the triangle, move it closer to the fish eye until it is not
        while (PointInsideObstacle(newPosition))
        {
            newPosition = Vector3.MoveTowards(newPosition, eye.transform.position, 0.5f);
        }

        point.transform.position = newPosition;

    }
}