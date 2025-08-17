using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystem2D<T>
{
    int width;
    int height;
    float cellSize;
    Vector3 origin;
    T[,] gridArray;

    CoordinateConverter coordinateConverter;

    public GridSystem2D(int width, int height, float cellSize, Vector3 origin, CoordinateConverter coordinateConverter, bool debug)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.origin = origin;
        this.coordinateConverter = coordinateConverter ?? new VerticalConverter();

        gridArray = new T[width, height];

        if (debug)
        {
            DrawDebugLines();
        }
    }

    Vector3 GetWorldPosition(int x, int y) => coordinateConverter.GridToWorld(x, y, cellSize, origin);

    void DrawDebugLines()
    {
        const float duration = 100f;
        var parent = new GameObject(name: "Debugging");

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // TODO: center text
                CreateWorldText(parent, text: x + "," + y, GetWorldPosition(x, y), dir: coordinateConverter.Forward);
                Debug.DrawLine(start: GetWorldPosition(x, y), end: GetWorldPosition(x, y + 1), Color.white, duration);
                Debug.DrawLine(start: GetWorldPosition(x, y), end: GetWorldPosition(x + 1, y), Color.white, duration);
            }
        }

        Debug.DrawLine(start: GetWorldPosition(x: 0, y: height), end: GetWorldPosition(x: width, y: height), Color.white, duration);
        Debug.DrawLine(start: GetWorldPosition(x: width, y: 0), end: GetWorldPosition(x: width, y: height), Color.white, duration);
    }

    TextMeshPro CreateWorldText(GameObject parent, string text, Vector3 position, Vector3 dir,
        int fontSize = 2, Color color = default, TextAlignmentOptions textAnchor = TextAlignmentOptions.Center, int sortingOrder = 0)
    {
        GameObject gameObject = new GameObject(name: "DebugText_" + text, typeof(TextMeshPro));
        gameObject.transform.SetParent(parent.transform);
        gameObject.transform.position = position;
        gameObject.transform.forward = dir;

        TextMeshPro textMeshPro = gameObject.GetComponent<TextMeshPro>();
        textMeshPro.text = text;
        textMeshPro.fontSize = fontSize;
        textMeshPro.color = color == default ? Color.white : color;
        textMeshPro.alignment = textAnchor;
        textMeshPro.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;

        return textMeshPro;        
    }

    public abstract class CoordinateConverter
    {
        public abstract Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin);

        public abstract Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin);

        public abstract Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin);

        public abstract Vector3 Forward { get; }
    }

    /// <summary>
    /// A coordinate converter for vertical grids, where the grid lies on the X-Y plane.
    /// </summary>
    public class VerticalConverter : CoordinateConverter
    {
        public override Vector3 GridToWorld(int x, int y, float cellSize, Vector3 origin)
        {
            return new Vector3(x, y, 0) * cellSize + origin;
        }

        public override Vector3 GridToWorldCenter(int x, int y, float cellSize, Vector3 origin)
        {
            return new Vector3(x * cellSize + cellSize * 0.5f, y * cellSize + cellSize * 0.5f, 0) + origin;
        }

        public override Vector2Int WorldToGrid(Vector3 worldPosition, float cellSize, Vector3 origin)
        {
            Vector3 gridPosition = (worldPosition - origin) / cellSize;
            var x = Mathf.FloorToInt(gridPosition.x);
            var y = Mathf.FloorToInt(gridPosition.y);
            return new Vector2Int(x, y);
        }

        public override Vector3 Forward => Vector3.forward;
    }
}
