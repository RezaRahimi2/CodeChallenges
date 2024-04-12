using System.Collections;
using DG.Tweening;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter))]
public class Spiral : MonoBehaviour
{
    [Range(.01f, 1000)] [SerializeField] float radius = 5;
    [Range(.1f, 10f)] [SerializeField] float width = 1.5f;
    [Range(.1f, 10f)] [SerializeField] float height = .5f;
    [Range(.01f, 1000)] [SerializeField] public float length = 9.06f;
    [Range(4, 720)] [SerializeField] int sides = 45;
    [Range(.0f, 100)] [SerializeField] float offset = 2f;
    Mesh mesh;
    [SerializeField]private MeshRenderer meshRenderer;

    public Vector3 m_spiralVelocity; // When the scraping stops, the spiral is accelerated to this velocity

    Vector3[] radiusSurfaceVertices;
    Vector3[] vertices;
    int[] triangles;
    public float torque;

   
    public void Initialize(SurfaceData m_surfaceData)
    {
        // MeshRenderer
        if (gameObject.GetComponent<MeshRenderer>() == null)
        {
            meshRenderer = gameObject.AddComponent(typeof(MeshRenderer)) as MeshRenderer;
        }

        meshRenderer = GetComponent<MeshRenderer>();

        // MeshFilter
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        transform.localScale = new Vector3(10, m_surfaceData.SpiralYScale, 10);
        m_spiralVelocity = m_surfaceData.SpiralVelocity;
        length = .1f;
        Refresh();
        meshRenderer.material.SetColor("_Color", m_surfaceData.Color);
    }

    public void Refresh()
    {
        CreateMesh();
        UpdateMesh();
    }
    
    void CreateMesh()
    {
        radius = 20;
        // Helpers
        float halfWidth = width / 2;
        var sidesIncludingCaps = sides + 2;
        int dist = Mathf.CeilToInt(((int) sidesIncludingCaps) * length);
        Vector3 vertexSurfaceRadius;
        Vector3 vertexInnerTop;
        Vector3 vertexOuterTop;
        Vector3 vertexInnerBottom;
        Vector3 vertexOuterBottom;
        
        transform.position += new Vector3(1, 0, 0) * .025f;
        transform.position += new Vector3(0, 1, 0) * .046f;
        transform.localScale += new Vector3(1, 0, 1) * radius / 20;
        
        // Vertices
        radiusSurfaceVertices = new Vector3[dist];
        vertices = new Vector3[dist * 4];
        float step = offset / sides;
        float y = 0;
        for (var i = 0; i < dist; i++)
        {
            // Initial centered vert
            var angle = i * Mathf.PI * 2 / sides;

            var pos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));
            var upOffset = Vector3.up * y;
            var downOffset = Vector3.up * (y - height);

            // Radius surface helpers
            vertexSurfaceRadius = pos * radius;
            vertexSurfaceRadius -= upOffset;

            // Inner top vertex
            vertexInnerTop = pos / (radius - halfWidth);
            vertexInnerTop -= upOffset;

            // Outer top vertex
            vertexOuterTop = pos / (radius + halfWidth);
            vertexOuterTop -= upOffset;

            // Inner bottom vertex
            vertexInnerBottom = pos / (radius - halfWidth);
            vertexInnerBottom -= downOffset;

            // Outer bottom vertex
            vertexOuterBottom = pos / (radius + halfWidth);
            vertexOuterBottom -= downOffset;

            // Update vertices
            radiusSurfaceVertices[i] = vertexSurfaceRadius;
            vertices[i * 4 + 0] = vertexInnerTop;
            vertices[i * 4 + 1] = vertexOuterTop;
            vertices[i * 4 + 2] = vertexInnerBottom;
            vertices[i * 4 + 3] = vertexOuterBottom;

            y += step;

            radius += .008f;
        }

        // Triangles
        var vertexIterationCount = 24;
        var trianglePathVertexCount = dist * vertexIterationCount;
        var triangleCapVertexCount = 12;
        triangles = new int[trianglePathVertexCount + triangleCapVertexCount];
        var vertAnchor = 0;
        var meshAnchor = 0;
        var innerTop = 0;
        var outerTop = 1;
        var innerBottom = 2;
        var outerBottom = 3;
        for (var i = 0; i < dist; i++)
        {
            // Start cap
            if (i == 0)
            {
                triangles[vertAnchor + 0] = innerBottom;
                triangles[vertAnchor + 1] = innerTop;
                triangles[vertAnchor + 2] = outerTop;
                triangles[vertAnchor + 3] = innerBottom;
                triangles[vertAnchor + 4] = outerTop;
                triangles[vertAnchor + 5] = outerBottom;
                vertAnchor += 6;
            }

            // Path
            else if (i < dist - 1)
            {
                // Top quad
                triangles[vertAnchor + 0] = meshAnchor + innerTop;
                triangles[vertAnchor + 1] = meshAnchor + innerTop + 4;
                triangles[vertAnchor + 2] = meshAnchor + outerTop + 4;
                triangles[vertAnchor + 3] = meshAnchor + innerTop;
                triangles[vertAnchor + 4] = meshAnchor + outerTop + 4;
                triangles[vertAnchor + 5] = meshAnchor + outerTop;

                // Right quad
                triangles[vertAnchor + 6] = meshAnchor + outerTop;
                triangles[vertAnchor + 7] = meshAnchor + outerTop + 4;
                triangles[vertAnchor + 8] = meshAnchor + outerBottom + 4;
                triangles[vertAnchor + 9] = meshAnchor + outerTop;
                triangles[vertAnchor + 10] = meshAnchor + outerBottom + 4;
                triangles[vertAnchor + 11] = meshAnchor + outerBottom;

                // Bottom quad
                triangles[vertAnchor + 12] = meshAnchor + outerBottom;
                triangles[vertAnchor + 13] = meshAnchor + outerBottom + 4;
                triangles[vertAnchor + 14] = meshAnchor + innerBottom + 4;
                triangles[vertAnchor + 15] = meshAnchor + outerBottom;
                triangles[vertAnchor + 16] = meshAnchor + innerBottom + 4;
                triangles[vertAnchor + 17] = meshAnchor + innerBottom;

                // Left quad
                triangles[vertAnchor + 18] = meshAnchor + innerBottom;
                triangles[vertAnchor + 19] = meshAnchor + innerBottom + 4;
                triangles[vertAnchor + 20] = meshAnchor + innerTop + 4;
                triangles[vertAnchor + 21] = meshAnchor + innerBottom;
                triangles[vertAnchor + 22] = meshAnchor + innerTop + 4;
                triangles[vertAnchor + 23] = meshAnchor + innerTop;

                // Offset helpers
                vertAnchor += vertexIterationCount;
                meshAnchor += 4;
            }

            // End cap
            else
            {
                vertAnchor += vertexIterationCount;
                triangles[vertAnchor + 0] = meshAnchor + outerTop;
                triangles[vertAnchor + 1] = meshAnchor + innerTop;
                triangles[vertAnchor + 2] = meshAnchor + innerBottom;
                triangles[vertAnchor + 3] = meshAnchor + outerTop;
                triangles[vertAnchor + 4] = meshAnchor + innerBottom;
                triangles[vertAnchor + 5] = meshAnchor + outerBottom;
            }
        }
    }

    void UpdateMesh()
    {
        if (mesh != null)
        {
            mesh.Clear();

            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.RecalculateNormals();
        }
    }


    public void StopScraping(float holdTime)
    {
        // I have used CapsuleCollider since it is a good approximation of the spiral's shape
        CapsuleCollider col = GetComponent<CapsuleCollider>();

        col.enabled = true;

        // Set the velocity of the rigidbody
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.velocity = m_spiralVelocity;
        rb.AddTorque(Vector3.forward * torque * 10 * holdTime);
        DOVirtual.DelayedCall(3, () =>
        {
            rb.velocity = new Vector3(0, 0, 2);
        });
        rb.useGravity = true;
    }

}