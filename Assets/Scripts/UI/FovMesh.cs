using UnityEngine;
using System.Linq;
using System;

// A field-of-view mesh centered at an object.
public class FovMesh : MonoBehaviour
{
    private Mesh mesh;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private PolygonCollider2D meshCollider;

    private int layerMask;
    private float fovAngle;
    private float midAngle;
    private float fovDistance;
    private int rayCount;

    private bool mouseIsInside; // This mouse detection functionality should be moved to a different object which inherits from this.

    // Gets a unit vector from an angle
    private Vector3 GetUnitVector(float angle)
    {
        angle = angle * (Mathf.PI / 180f); // Convert to radians
        float x = Mathf.Sin(angle);
        float y = Mathf.Cos(angle);
        return new Vector3(x, y);
    }

    private void Awake()
    {
        string[] layers = { "Wall" };
        layerMask = EnvironmentUtilities.GetLayerMaskFromNames(layers);
        mouseIsInside = false;

        InitMeshComponents();

        SetVision(0f);
    }

    public void SetVision(float fov_distance, float mid_angle = 0f, float fov_angle = 360f, int ray_count = 100)
    {
        fovDistance = fov_distance;
        fovAngle = fov_angle;
        rayCount = ray_count;
        midAngle = mid_angle;
        UpdateFOV();
    }

    // Initializes a meshfilter an meshrenderer on the object
    private void InitMeshComponents()
    {
        mesh = new Mesh();
        mesh.name = "FOV";
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;
        meshCollider = gameObject.AddComponent<PolygonCollider2D>();
        meshCollider.isTrigger = true;

        var rb2D = gameObject.AddComponent<Rigidbody2D>();
        rb2D.bodyType = RigidbodyType2D.Static;

        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "UI_front";
        meshRenderer.material = Resources.Load("Materials/Alpha_Mask", typeof(Material)) as Material;
    }

    private void UpdateFOV()
    {
        // Field of view if 360 degrees to start.
        //Vector3 origin = gameObject.transform.position;
        Vector3 origin = Vector3.zero;
        float angle = midAngle - fovAngle / 2;
        float angle_increase = fovAngle / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;
        int vid = 1;
        int tid = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            // Check for obstructions along the ray.
            Vector3 direction = GetUnitVector(angle);
            float dist = EnvironmentUtilities.CollisionDistAlongRay(
                origin + transform.position, direction, fovDistance, layer_mask: layerMask);

            // Set the vertices of the FovMesh.
            Vector3 vertex = origin + direction * dist;
            vertices[vid] = vertex;

            if (i > 0)
            {
                triangles[tid + 0] = 0; // Start every triangle on the origin;
                triangles[tid + 1] = vid - 1;
                triangles[tid + 2] = vid;

                tid += 3;
            }

            vid += 1;
            angle -= angle_increase;
        }

        meshCollider.points = Array.ConvertAll(vertices, item => (Vector2)item);

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public bool GetMouseInside()
    {
        return mouseIsInside;
    }

    public void OnMouseEnter()
    {
        mouseIsInside = true;
    }

    public void OnMouseExit()
    {
        mouseIsInside = false;
    }

}