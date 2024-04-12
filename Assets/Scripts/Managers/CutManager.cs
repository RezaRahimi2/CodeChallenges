using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CutManager : MonoBehaviour
{
    [SerializeField] private List<ScraperableWood> m_ScraperableWoods;
    [SerializeField] private ScraperableWood m_lastHittedWood;
    
    public GameObject plane;
    public Transform ObjectContainer;

    // How far away from the slice do we separate resulting objects
    public float separation;

    // Do we draw a plane object associated with the slice
    private Plane slicePlane = new Plane();
    public bool drawPlane;

    private MeshCutter meshCutter;
    private TempMesh biggerMesh, smallerMesh;

    // Use this for initialization
    public void Initialize(float reduceScaleSpeed)
    {
        // Initialize a somewhat big array so that it doesn't resize
        meshCutter = new MeshCutter(256);

        m_ScraperableWoods = FindObjectsOfType<ScraperableWood>().ToList();
        
        m_ScraperableWoods.ForEach(x =>
        {
            x.Initialize(reduceScaleSpeed,this);
        });
    }
    
    public void Cut(ScraperableWood scraperableWood,SurfaceData surfaceData,Transform parentTransform,Vector3 start, Vector3 end, Vector3 depth)
    {
        var planeTangent = (end - start).normalized;

        ObjectContainer = new GameObject().transform;
        ObjectContainer.position = parentTransform.position;
        ObjectContainer.rotation = parentTransform.rotation;
        ObjectContainer.localScale = parentTransform.localScale;
        // if we didn't drag, we set tangent to be on x
        if (planeTangent == Vector3.zero)
            planeTangent = Vector3.right;

        var normalVec = Vector3.Cross(depth, planeTangent);

        if (drawPlane) DrawPlane(start, end, normalVec);

        m_lastHittedWood = scraperableWood;
        SliceObjects(scraperableWood.gameObject, start, normalVec);
        
        GameManager.Instance.HitToScraperableObject(surfaceData);
    }

    void DrawPlane(Vector3 start, Vector3 end, Vector3 normalVec)
    {
        Quaternion rotate = Quaternion.FromToRotation(Vector3.up, normalVec);

        plane.transform.localRotation = rotate;
        plane.transform.position = (end + start) / 2;
        plane.SetActive(true);
    }

   void SliceObjects(GameObject gameObject,Vector3 point, Vector3 normal)
    {
        
        // Put results in positive and negative array so that we separate all meshes if there was a cut made
        List<Transform> positive = new List<Transform>(),
            negative = new List<Transform>();

        bool slicedAny = false;
        
        var transformedNormal = ((Vector3) (gameObject.transform.localToWorldMatrix.transpose * normal)).normalized;

        //Convert plane in object's local frame
        slicePlane.SetNormalAndPosition(
            transformedNormal,
            gameObject.transform.InverseTransformPoint(point));

        slicedAny = SliceObject(ref slicePlane, gameObject, positive, negative) || slicedAny;
        
        // Separate meshes if a slice was made
        if (slicedAny)
            SeparateMeshes(positive, negative, normal);
    }

    bool SliceObject(ref Plane slicePlane, GameObject obj, List<Transform> positiveObjects,
        List<Transform> negativeObjects)
    {
        var mesh = obj.GetComponent<MeshFilter>().mesh;

        if (!meshCutter.SliceMesh(mesh, ref slicePlane))
        {
            // Put object in the respective list
            if (slicePlane.GetDistanceToPoint(meshCutter.GetFirstVertex()) >= 0)
                positiveObjects.Add(obj.transform);
            else
                negativeObjects.Add(obj.transform);

            return false;
        }

        // TODO: Update center of mass

        // Silly condition that labels which mesh is bigger to keep the bigger mesh in the original gameobject
        bool posBigger = meshCutter.PositiveMesh.surfacearea > meshCutter.NegativeMesh.surfacearea;
        //if (posBigger)
        //{
            biggerMesh = meshCutter.PositiveMesh;
            smallerMesh = meshCutter.NegativeMesh;
        //}
        // else
        // {
        //     biggerMesh = meshCutter.NegativeMesh;
        //     smallerMesh = meshCutter.PositiveMesh;
        // }

        // Create new Sliced object with the other mesh
        GameObject newObject = Instantiate(obj, ObjectContainer);
        newObject.transform.SetPositionAndRotation(obj.transform.position, obj.transform.rotation);
        var newObjMesh = newObject.GetComponent<MeshFilter>().mesh;
        //newObject.transform.SetParent(obj.transform.parent);

        // Put the bigger mesh in the original object
        // TODO: Enable collider generation (either the exact mesh or compute smallest enclosing sphere)
        ReplaceMesh(mesh, biggerMesh);
        ReplaceMesh(newObjMesh, smallerMesh);

        obj.GetComponentsInChildren<BoxCollider>().ToList().ForEach(x =>
        {
            Destroy(x);
            
        });
        
        BoxCollider collider = obj.AddComponent<BoxCollider>();
        collider.enabled = false;
        obj.GetComponent<ScraperableWood>().Collider = collider;
        
        BoxCollider box = obj.AddComponent<BoxCollider>();
        box.isTrigger = true;
        
        newObject.GetComponentsInChildren<BoxCollider>().ToList().ForEach(x =>
        {
            Destroy(x);
        });
        
        (posBigger ? positiveObjects : negativeObjects).Add(obj.transform);
        (posBigger ? negativeObjects : positiveObjects).Add(newObject.transform);

        return true;
    }


    /// <summary>
    /// Replace the mesh with tempMesh.
    /// </summary>
    void ReplaceMesh(Mesh mesh, TempMesh tempMesh, MeshCollider collider = null)
    {
        mesh.Clear();
        mesh.SetVertices(tempMesh.vertices);
        mesh.SetTriangles(tempMesh.triangles, 0);
        mesh.SetNormals(tempMesh.normals);
        mesh.SetUVs(0, tempMesh.uvs);

        //mesh.RecalculateNormals();
        mesh.RecalculateTangents();

        if (collider != null && collider.enabled)
        {
            collider.sharedMesh = mesh;
            collider.convex = true;
        }
    }

    void SeparateMeshes(Transform posTransform, Transform negTransform, Vector3 localPlaneNormal)
    {
        // Bring back normal in world space
        Vector3 worldNormal = ((Vector3) (posTransform.worldToLocalMatrix.transpose * localPlaneNormal)).normalized;

        Vector3 separationVec = worldNormal * separation;
        // Transform direction in world coordinates
        posTransform.position += separationVec;
        negTransform.position -= separationVec;
    }

    void SeparateMeshes(List<Transform> positives, List<Transform> negatives, Vector3 worldPlaneNormal)
    {
        int i;
        var separationVector = worldPlaneNormal * separation;

        for (i = 0; i < positives.Count; ++i)
            positives[i].transform.position += separationVector;

        for (i = 0; i < negatives.Count; ++i)
            negatives[i].transform.position -= separationVector;
    }

    public void StopScraping()
    {
        if(m_lastHittedWood != null)
            m_lastHittedWood.Collider.enabled = true;
    }
}