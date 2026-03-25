using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=CSeUMTaNFYk
public class PlayerVisibilityMesh : MonoBehaviour
{
    [SerializeField] private float viewRadius = 10f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private int rayCount = 360;
    private Vector3[] previousVertices;

    private Mesh mesh;
    private MeshFilter meshFilter;

    private void Awake()
    {
        mesh = new Mesh { name = "Visibility Mesh" };
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    private void LateUpdate()
    {
        DrawVisibilityMesh();
    }

    private void DrawVisibilityMesh()
    {
        // Get angle of each ray
        float angleStep = 360f / rayCount;
        List<Vector3> vertices = new() { Vector3.zero };

        // Shoot a ray in a circle one by one each frame
        for (int i = 0; i < rayCount; i++)
        {
            // Calculate which direction the ray should shoot
            float angle = i * angleStep;
            Vector2 dir = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            // Get the rays hit endpoint
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, wallLayer);
            Vector2 endPoint;

            if (hit.collider != null)
            {
                endPoint = hit.point - dir * 0.05f;
            }
            else
            {
                endPoint = (Vector2) transform.position + dir * viewRadius;
            }

            vertices.Add(transform.InverseTransformPoint(endPoint));
        }

        // Used for smoothing, keeping the code if we decide we want the shadows to look better later
        //float smoothSpeed = 60f;

        //for (int i = 0; i < vertices.Count; i++)
        //{
        //    if (previousVertices != null && i < previousVertices.Length)
        //    {
        //        vertices[i] = Vector3.Lerp(
        //            previousVertices[i],
        //            vertices[i],
        //            Time.deltaTime * smoothSpeed);
        //    }
        //}

        //previousVertices = vertices.ToArray();

        // Forming a circle FOV using many triangles surrounding the player
        List<int> triangles = new();
        for (int i = 1; i < vertices.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        // Final triangle that should close the circle
        triangles.Add(0);
        triangles.Add(vertices.Count - 1);
        triangles.Add(1);

        mesh.Clear();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    public bool IsPointVisible(Vector2 point)
    {
        if (Vector2.Distance(transform.position, point) > viewRadius) return false;
        Vector2 dir = (point - (Vector2) transform.position).normalized;
        float dist = Vector2.Distance(transform.position, point);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dist, wallLayer);
        return hit.collider == null;
    }

    public float GetViewRadius => viewRadius;
}
