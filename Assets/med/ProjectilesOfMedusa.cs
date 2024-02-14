using UnityEngine;

public class ProjectilesOfMedusa : MonoBehaviour
{
    public float damageAmount = 10f;
<<<<<<< HEAD
=======
    private PlayerController _playerController;
>>>>>>> a82d40860757cf7b06239cb4def209837df81af0

    public Mesh mesh;
    private void Start()
    {
        mesh = new Mesh();
        DrawHalfCycle(10, 1, 100, 90, transform.position);
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if it is a resource type of object, like trees
        if (other.GetComponent<Wood>() != null)
        {
            Wood wood = other.GetComponent<Wood>();
            wood.Stoned();
        }
        // Marked as stoned
        else if (other.CompareTag("Player") && Vector3.Angle(other.transform.forward, transform.parent.position) < 90)
        {
            Debug.Log("????");
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
<<<<<<< HEAD
=======

>>>>>>> a82d40860757cf7b06239cb4def209837df81af0
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Wood>() != null)
        {
            Wood wood = other.GetComponent<Wood>();
            //wood.UnStoned();
        }
        else if (other.CompareTag("Player"))
        {
            Debug.Log("Player unstoned");
        }

    }
    void DrawHalfCycle(float radius, float innerRadius, int segments, float angleDegree, Vector3 centerCircle)
    {
        //¶¥µã
        Vector3[] vertices = new Vector3[segments * 2 + 2];
        vertices[0] = centerCircle;
        float angleRad = Mathf.Deg2Rad * angleDegree;
        float angleCur = angleRad;
        float angledelta = angleRad / segments;

        for (int i = 0; i < vertices.Length; i += 2)
        {
            float cosA = Mathf.Cos(angleCur);
            float sinA = Mathf.Sin(angleCur);

            vertices[i] = new Vector3(radius * cosA, innerRadius, radius * sinA);
            angleCur -= angledelta;

        }
        //Èý½ÇÐÎ
        int[] triangles = new int[segments * 6];
        for (int i = 0, vi = 0; i < triangles.Length; i += 6, vi += 2)
        {
            triangles[i] = vi;
            triangles[i + 1] = vi + 3;
            triangles[i + 2] = vi + 1;
            triangles[i + 3] = vi + 2;
            triangles[i + 4] = vi + 3;
            triangles[i + 5] = vi;
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }
}
