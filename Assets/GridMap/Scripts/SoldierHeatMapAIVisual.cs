using UnityEngine;

public class SoldierHeatMapAIVisual : MonoBehaviour
{
    private Grid soldierGridAI;
    private Mesh mesh;
    private bool updateMesh;
    public float gridTransparency = 0.5f;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void SetSoldierGridAI(Grid soldierGridAI)
    {
        this.soldierGridAI = soldierGridAI;
        UpdateSoldierHeatMapAIVisual();

        soldierGridAI.OnGridValueChanged += SoldierGridAI_OnGridValueChanged;
    }

    private void SoldierGridAI_OnGridValueChanged(object sender, Grid.OnGridValueChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateSoldierHeatMapAIVisual();
        }
    }

    private void UpdateSoldierHeatMapAIVisual()
    {
        MeshUtils.CreateEmptyMeshArrays(soldierGridAI.GetWidth() * soldierGridAI.GetHeight(), out Vector3[] vertices, out Vector2[] uv, out int[] triangles);

        for (int x = 0; x < soldierGridAI.GetWidth(); x++)
        {
            for (int y = 0; y < soldierGridAI.GetHeight(); y++)
            {
                int index = x * soldierGridAI.GetHeight() + y;
                Vector3 quadSize = new Vector3(1, 1) * soldierGridAI.GetCellSize();

                int soldierValue = soldierGridAI.GetValue(x, y);
                float soldierValueNormalized = (float)soldierValue / Grid.HEAT_MAP_MAX_VALUE;

                Color color = new Color(1f, 0f, 0f, gridTransparency); // Red color for soldiers

                Vector2 soldierValueUV = new Vector2(soldierValueNormalized, 0f);

                UpdateHeatMapVisual(vertices, uv, triangles, index, soldierGridAI.GetWorldPosition(x, y) + quadSize * .5f, quadSize, soldierValueUV, soldierValueUV, color);
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    private void UpdateHeatMapVisual(Vector3[] vertices, Vector2[] uv, int[] triangles, int index, Vector3 pos, Vector3 quadSize, Vector2 uv00, Vector2 uv11, Color color)
    {
        int vIndex = index * 4;
        int vIndex0 = vIndex;
        int vIndex1 = vIndex + 1;
        int vIndex2 = vIndex + 2;
        int vIndex3 = vIndex + 3;

        vertices[vIndex0] = pos + new Vector3(-quadSize.x, 0, quadSize.y) * 0.5f;
        vertices[vIndex1] = pos + new Vector3(-quadSize.x, 0, -quadSize.y) * 0.5f;
        vertices[vIndex2] = pos + new Vector3(quadSize.x, 0, -quadSize.y) * 0.5f;
        vertices[vIndex3] = pos + new Vector3(quadSize.x, 0, quadSize.y) * 0.5f;

        // Relocate UVs
        uv[vIndex0] = new Vector2(uv00.x, uv11.y);
        uv[vIndex1] = new Vector2(uv00.x, uv00.y);
        uv[vIndex2] = new Vector2(uv11.x, uv00.y);
        uv[vIndex3] = new Vector2(uv11.x, uv11.y);

        // Create triangles
        int tIndex = index * 6;

        triangles[tIndex + 0] = vIndex0;
        triangles[tIndex + 1] = vIndex3;
        triangles[tIndex + 2] = vIndex1;

        triangles[tIndex + 3] = vIndex1;
        triangles[tIndex + 4] = vIndex3;
        triangles[tIndex + 5] = vIndex2;
    }
}