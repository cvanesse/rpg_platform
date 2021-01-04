using UnityEngine;

// Utility functions for checking the environment
public static class EnvironmentUtilities
{
    public static int GetLayerMaskFromNames(string[] layer_names)
    {
        int mask = 0;
        foreach (string name in layer_names)
        {
            mask += 1 << LayerMask.NameToLayer(name);
        }
        return mask;
    }

    // Casts a ray from the origin along a direction and returns the maximum distance
    // Before a collision along that ray.
    public static float CollisionDistAlongRay(
        Vector2 origin, Vector2 direction,
        float max_distance = Mathf.Infinity, int layer_mask = ~0)
    {
        // Checks all layers by default.
        RaycastHit2D hit = Physics2D.Raycast(origin, direction, max_distance, layer_mask);

        if (hit.collider == null)
        {
            return max_distance;
        }

        return hit.distance;
    }
}