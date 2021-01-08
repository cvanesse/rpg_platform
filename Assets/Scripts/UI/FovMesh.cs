using UnityEngine;

/// <summary>
/// A field-of-view/range mesh centered on an object. 
/// </summary>
public class FovMesh : MonoBehaviour
{
    /// <summary>
    /// The mesh used by the FovMesh
    /// </summary>
    public Mesh mesh;

    /// <summary>
    /// The MeshFilter used for rendering
    /// </summary>
    private MeshFilter meshFilter;

    /// <summary>
    /// The MeshRenderer
    /// </summary>
    private MeshRenderer meshRenderer;

    /// <summary>
    /// The layerMask for "vision" - should collide with walls and opaque objects, but not entities.
    /// </summary>
    private int layerMask;

    /// <summary>
    /// The angle of vision to use.
    /// </summary>
    private float fovAngle;

    /// <summary>
    /// The center of the FOV.
    /// </summary>
    private float midAngle;

    /// <summary>
    /// The range of the FOV. 
    /// </summary>
    private float fovDistance;

    /// <summary>
    /// The number of rays to trace for rendering the FOV mesh. 
    /// </summary>
    private int rayCount;

    // Gets a unit vector from an angle
    /// <summary>
    /// Gets a unit vector from an angle (in degrees.)
    /// </summary>
    /// <param name="angle">The angle of the unit vector. </param>
    /// <returns>A unit vector pointing along the angle. </returns>
    private Vector2 GetUnitVector(float angle)
    {
        angle = angle * (Mathf.PI / 180f); // Convert to radians
        float x = Mathf.Sin(angle);
        float y = Mathf.Cos(angle);
        return new Vector2(x, y);
    }

    /// <summary>
    /// On awake, the meshes initializes and vision length is set to 0 - and the layerMask of the FovMesh is set appropriately.
    /// </summary>
    private void Awake()
    {
        string[] layers = { "Wall" };
        layerMask = EnvironmentUtilities.GetLayerMaskFromNames(layers);
        
        InitMeshComponents();

        SetVision(0f);
    }

    /// <summary>
    /// Updates the FOV mesh based on the given distance, angle center and range, and ray count. 
    /// </summary>
    /// <param name="fov_distance">The radius of the FOV mesh</param>
    /// <param name="mid_angle">The center angle of the FOV</param>
    /// <param name="fov_angle">The angle range of the FOV</param>
    /// <param name="ray_count">The number of rays to use for rendering the mesh. </param>
    public void SetVision(float fov_distance, float mid_angle = 0f, float fov_angle = 360f, int ray_count = 500)
    {
        fovDistance = fov_distance;
        fovAngle = fov_angle;
        rayCount = ray_count;
        midAngle = mid_angle;
        UpdateFOV();
    }

    /// <summary>
    /// Initializes the meshfilter and meshrenderer for the object.
    /// </summary>
    private void InitMeshComponents()
    {
        mesh = new Mesh();
        mesh.name = "FOV";
        meshFilter = gameObject.AddComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.sortingLayerName = "UI_front";
        meshRenderer.material = Resources.Load("Materials/Alpha_Mask", typeof(Material)) as Material;
    }

    /// <summary>
    /// Performs the raytracing algorithm to update the FovMesh.
    /// </summary>
    private void UpdateFOV()
    {
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

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }
}