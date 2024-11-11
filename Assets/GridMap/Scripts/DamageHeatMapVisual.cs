using UnityEngine;

public class DamageHeatMapVisual : MonoBehaviour
{
    private Grid damageGrid;
    private Mesh mesh;
    private bool updateMesh;
    public float gridTransparency = 0.5f;
    private Color damageColor;

    private void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        damageColor = new Color(1f, 0f, 0f, gridTransparency); // Red color for damage cells
    }

    public void SetDamageGrid(Grid damageGrid)
    {
        this.damageGrid = damageGrid;
        UpdateDamageHeatMapVisual();

        damageGrid.OnGridValueChanged += DamageGrid_OnGridValueChanged;
    }

    private void DamageGrid_OnGridValueChanged(object sender, Grid.OnGridValueChangedEventArgs e)
    {
        updateMesh = true;
    }

    private void LateUpdate()
    {
        if (updateMesh)
        {
            updateMesh = false;
            UpdateDamageHeatMapVisual();
        }
    }

    private void UpdateDamageHeatMapVisual()
    {
        int totalCells = damageGrid.GetWidth() * damageGrid.GetHeight();
        HeatMapMeshData meshData = new HeatMapMeshData(totalCells);

        Vector3 quadSize = new Vector3(1, 1) * damageGrid.GetCellSize();

        for (int x = 0; x < damageGrid.GetWidth(); x++)
        {
            for (int y = 0; y < damageGrid.GetHeight(); y++)
            {
                int index = x * damageGrid.GetHeight() + y;

                int damageValue = damageGrid.GetValue(x, y);
                float damageValueNormalized = (float)damageValue / Grid.HEAT_MAP_MAX_VALUE;
                Vector2 damageValueUV = new Vector2(damageValueNormalized, 0f);

                // Create a single CellMeshData struct for each cell
                meshData.cells[index] = new CellMeshData
                {
                    pos = damageGrid.GetWorldPosition(x, y) + quadSize * .5f,
                    quadSize = quadSize,
                    uv00 = damageValueUV,
                    uv11 = damageValueUV,
                    color = damageColor
                };
            }
        }

        meshData.ApplyToMesh(mesh);
    }

    // Struct to store mesh data for each cell
    public struct CellMeshData
    {
        public Vector3 pos;
        public Vector3 quadSize;
        public Vector2 uv00;
        public Vector2 uv11;
        public Color color;
    }

    public struct HeatMapMeshData
    {
        public Vector3[] vertices;
        public Vector2[] uv;
        public int[] triangles;
        public CellMeshData[] cells;

        public HeatMapMeshData(int totalCells)
        {
            vertices = new Vector3[totalCells * 4];
            uv = new Vector2[totalCells * 4];
            triangles = new int[totalCells * 6];
            cells = new CellMeshData[totalCells];
        }

        public void ApplyToMesh(Mesh mesh)
        {
            AssignVerticesAndUVs();
            AssignTriangles();
            UpdateMesh(mesh);
        }

        private void AssignVerticesAndUVs()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                int vIndex = i * 4;

                vertices[vIndex] = cells[i].pos + new Vector3(-cells[i].quadSize.x, 0, cells[i].quadSize.y) * 0.5f;
                vertices[vIndex + 1] = cells[i].pos + new Vector3(-cells[i].quadSize.x, 0, -cells[i].quadSize.y) * 0.5f;
                vertices[vIndex + 2] = cells[i].pos + new Vector3(cells[i].quadSize.x, 0, -cells[i].quadSize.y) * 0.5f;
                vertices[vIndex + 3] = cells[i].pos + new Vector3(cells[i].quadSize.x, 0, cells[i].quadSize.y) * 0.5f;

                uv[vIndex] = new Vector2(cells[i].uv00.x, cells[i].uv11.y);
                uv[vIndex + 1] = new Vector2(cells[i].uv00.x, cells[i].uv00.y);
                uv[vIndex + 2] = new Vector2(cells[i].uv11.x, cells[i].uv00.y);
                uv[vIndex + 3] = new Vector2(cells[i].uv11.x, cells[i].uv11.y);
            }
        }

        private void AssignTriangles()
        {
            for (int i = 0; i < cells.Length; i++)
            {
                int vIndex = i * 4;
                int tIndex = i * 6;

                triangles[tIndex] = vIndex;
                triangles[tIndex + 1] = vIndex + 3;
                triangles[tIndex + 2] = vIndex + 1;
                triangles[tIndex + 3] = vIndex + 1;
                triangles[tIndex + 4] = vIndex + 3;
                triangles[tIndex + 5] = vIndex + 2;
            }
        }

        private void UpdateMesh(Mesh mesh)
        {
            mesh.vertices = vertices;
            mesh.uv = uv;
            mesh.triangles = triangles;
        }
    }
}
