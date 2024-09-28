using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 5; // ���������� �����
    public int cols = 9; // ���������� �������
    public Vector2 cellSize = new Vector2( 1,1); // ������ ������ ������
    public Vector3 gridOrigin = Vector3.zero; // �������, � ������� ���������� �����


    private Vector3[,] gridPositions; // ������ ��� �������� ������� �����

    void Start()
    {
        CreateGrid(); // ������� ����� ��� ������ ����
    }

    void CreateGrid()
    {
        gridPositions = new Vector3[rows, cols]; // �������������� ������

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // ��������� ������� ������ ������ � ������ ��������� ������� gridOrigin
                gridPositions[row, col] = gridOrigin + new Vector3(col * cellSize.x, row * cellSize.y, 0);

                // ��� ������� ���������� ����� ����� (���� �����)
                Debug.DrawLine(gridOrigin + new Vector3(col * cellSize.x, 0, 0), gridOrigin + new Vector3(col * cellSize.x, rows * cellSize.x, 0), Color.white, 100f);
                Debug.DrawLine(gridOrigin + new Vector3(0, row * cellSize.y, 0), gridOrigin + new Vector3(cols * cellSize.y, row * cellSize.y, 0), Color.white, 100f);
            }
        }
    }


    // ����� ��� ��������� ������� ������
    public Vector3 GetCellPosition(int row, int col)
    {
        if (row >= 0 && row < rows && col >= 0 && col < cols)
        {
            return gridPositions[row, col];
        }
        return Vector3.zero;
    }

    // ����� ��� ���������� ��������� ������ �� ������� ����
    public Vector2Int GetGridCellFromPosition(Vector2 position)
    {

        position = position - (Vector2)gridOrigin + (cellSize/2);
        if (position.x < 0 || position.y < 0 || position.x > cols * cellSize.x || position.y > rows*cellSize.y) { return new Vector2Int(-1, -1); }
        int row = Mathf.Clamp(Mathf.FloorToInt(position.y / cellSize.y), 0, rows );
        int col = Mathf.Clamp(Mathf.FloorToInt(position.x / cellSize.x), 0, cols );
        return new Vector2Int(row, col);
    }
}
