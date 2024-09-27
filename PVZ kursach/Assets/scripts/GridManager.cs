using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 5; // ���������� �����
    public int cols = 9; // ���������� �������
    public float cellSize = 1f; // ������ ������ ������
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
                gridPositions[row, col] = gridOrigin + new Vector3(col * cellSize, row * cellSize, 0);

                // ��� ������� ���������� ����� ����� (���� �����)
                Debug.DrawLine(gridOrigin + new Vector3(col * cellSize, 0, 0), gridOrigin + new Vector3(col * cellSize, rows * cellSize, 0), Color.white, 100f);
                Debug.DrawLine(gridOrigin + new Vector3(0, row * cellSize, 0), gridOrigin + new Vector3(cols * cellSize, row * cellSize, 0), Color.white, 100f);
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
    public Vector2Int GetGridCellFromPosition(Vector3 position)
    {
        int row = Mathf.Clamp(Mathf.FloorToInt(position.y / cellSize), 0, rows - 1);
        int col = Mathf.Clamp(Mathf.FloorToInt(position.x / cellSize), 0, cols - 1);
        return new Vector2Int(row, col);
    }
}
