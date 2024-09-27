using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int rows = 5;  // ���������� �����
    public int cols = 9;  // ���������� ��������
    public float cellWidth = 1f;  // ������ �����
    public float cellHeight = 0.75f;  // ������ ����� (������ �� ��������������)
    public Vector3 gridOrigin = Vector3.zero;  // ��������� ������� �����

    public Vector3[,] gridPositions;  // ������ ��� �������� ������� �����

    void Start()
    {
        CreateGrid();  // �������� ����� ��� ������ ����
    }

    // �������� �����
    void CreateGrid()
    {
        gridPositions = new Vector3[rows, cols];

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                // ������� ����� � ������ �� ������, ������ � ���������� ��������� �����
                gridPositions[row, col] = gridOrigin + new Vector3(col * cellWidth, row * cellHeight, 0);
            }
        }
    }

    // ���������� ��������� ������ �� ������� ����
    public Vector3 GetGridCellFromPosition(Vector3 position)
    {
        Vector3 localPosition = position - gridOrigin;  // ������� ��������� ������� ������������ ������ �����

        // ��������� ��������� ������� ������ (�������� ������� �����)
        int col = Mathf.Clamp(Mathf.FloorToInt(localPosition.x / cellWidth), 0, cols - 1);
        int row = Mathf.Clamp(Mathf.FloorToInt(localPosition.y / cellHeight), 0, rows - 1);

        return gridPositions[row, col];  // ���������� ������� ������ ������
    }
}
