using System.Linq;
using UnityEngine;

public static class Extentions
{
    public static Bounds GetBoundsWithChildren(this GameObject gameObject)
    {
        Renderer parentRenderer = gameObject.GetComponent<Renderer>();
 
        Renderer[] childrenRenderers = gameObject.GetComponentsInChildren<Renderer>();
 
        Bounds bounds = parentRenderer != null
            ? parentRenderer.bounds
            : childrenRenderers.FirstOrDefault(x => x.enabled).bounds;
 
        if (childrenRenderers.Length > 0)
        {
            foreach (Renderer renderer in childrenRenderers)
            {
                if (renderer.enabled)
                {
                    bounds.Encapsulate(renderer.bounds);
                }
            }
        }
 
        return bounds;
    }
 
    public static void FocusOn(this Camera camera, GameObject focusedObject, float marginPercentage)
    {
        Bounds bounds = focusedObject.GetBoundsWithChildren();
        float maxExtent = bounds.extents.magnitude;
        float minDistance = (maxExtent * marginPercentage) / Mathf.Sin(Mathf.Deg2Rad * camera.fieldOfView / 2f);
        camera.nearClipPlane = minDistance - maxExtent;
        focusedObject.transform.position = bounds.center;
    }
}