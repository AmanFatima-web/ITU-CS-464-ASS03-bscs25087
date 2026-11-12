
using UnityEngine;

public class SpeedPickup : MonoBehaviour
{
    [Header("Power Settings")]
    [SerializeField] float speedMultiplier = 3f;
    [SerializeField] float duration = 10f;

    [Header("Appearance")]
    [SerializeField] float size = 1.5f;
    [SerializeField] float rotationSpeed = 100f;

    [Header("Colors")]
    [SerializeField] Color powerColor = new Color(1f, 0.85f, 0f);

    Transform bolt;

    void Start()
    {
        CreateLightning();
    }

    void Update()
    {
        if (bolt != null)
        {
            bolt.Rotate(
                0f,
                0f,
                rotationSpeed * Time.deltaTime
            );
        }
    }

    void CreateLightning()
    {
        GameObject lightning = new GameObject("LightningBolt");

        lightning.transform.SetParent(transform, false);
        lightning.transform.localScale = Vector3.one * size;

        bolt = lightning.transform;

        MeshFilter filter = lightning.AddComponent<MeshFilter>();
        MeshRenderer renderer = lightning.AddComponent<MeshRenderer>();

        filter.mesh = CreateBoltMesh();

        Shader shader = Shader.Find("Universal Render Pipeline/Lit");

        if (shader == null)
            shader = Shader.Find("Standard");

        if (shader == null)
        {
            Debug.LogError("No suitable shader found!");
            return;
        }

        Material mat = new Material(shader);
        mat.color = powerColor;

        if (mat.HasProperty("_EmissionColor"))
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", powerColor * 2.5f);
        }

        renderer.material = mat;
    }

    Mesh CreateBoltMesh()
    {
        Mesh mesh = new Mesh();

        // Classic lightning bolt outline
        Vector2[] points =
        {
            new Vector2(0.18f, 0.55f),
            new Vector2(-0.25f, 0.08f),
            new Vector2(0.04f, 0.08f),
            new Vector2(-0.20f, -0.55f),
            new Vector2(0.28f, -0.03f),
            new Vector2(-0.02f, -0.03f)
        };

        int count = points.Length;
        float depth = 0.12f;

        Vector3[] vertices = new Vector3[count * 2];

        for (int i = 0; i < count; i++)
        {
            vertices[i] = new Vector3(
                points[i].x,
                points[i].y,
                -depth
            );

            vertices[i + count] = new Vector3(
                points[i].x,
                points[i].y,
                depth
            );
        }

        int[] front =
        {
            0, 1, 5,
            1, 2, 5,
            2, 3, 4,
            2, 4, 5
        };

        System.Collections.Generic.List<int> triangles =
            new System.Collections.Generic.List<int>();

        for (int i = 0; i < front.Length; i += 3)
        {
            triangles.Add(front[i]);
            triangles.Add(front[i + 1]);
            triangles.Add(front[i + 2]);

            triangles.Add(front[i + 2] + count);
            triangles.Add(front[i + 1] + count);
            triangles.Add(front[i] + count);
        }

        for (int i = 0; i < count; i++)
        {
            int next = (i + 1) % count;

            triangles.Add(i);
            triangles.Add(i + count);
            triangles.Add(next);

            triangles.Add(next);
            triangles.Add(i + count);
            triangles.Add(next + count);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerController player =
            other.GetComponentInParent<PlayerController>();

        if (player == null)
            return;

        player.BoostSpeed(speedMultiplier, duration);

        if (PowerUpMessage.Instance != null)
        {
            PowerUpMessage.Instance.ShowMessage(
                "SPEED BOOST ACTIVATED!",
                duration
            );
        }

        Debug.Log("SPEED POWER-UP COLLECTED!");

        Destroy(gameObject);
    }
}
