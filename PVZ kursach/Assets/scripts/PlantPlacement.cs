using UnityEngine;

public class PlantPlacement : MonoBehaviour
{
    public GridManager gridManager; // ������ �� �������� �����
    public GameObject plantPrefab;  // ������ ��������
    public LineRenderer horizontalLine;  // ����� ��� �������������� ������
    public LineRenderer verticalLine;    // ����� ��� ������������ ������

    private bool isPlacing = false;  // ����, ����������� �� ����� �������

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // ������� ����� ������ ����
        {
            isPlacing = true; // ���������� ����� �������
        }

        if (Input.GetMouseButtonUp(0)) // ���������� ����� ������ ����
        {
            PlacePlant(); // ��������� ��������
            isPlacing = false; // ������������ ����� �������
        }

        if (isPlacing) // ���� ����� ������� �������, ������ �����
        {
            DrawLines();
        }
    }

    void DrawLines()
    {
        // �������� ���������� ���� � ������� �����������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // ������� ��������� ������ �� ������� ����
        Vector2Int gridCell = gridManager.GetGridCellFromPosition(mousePosition);
        Vector3 cellPosition = gridManager.GetCellPosition(gridCell.x, gridCell.y);

        // ������ �������������� �����
        horizontalLine.SetPosition(0, new Vector3(0, cellPosition.y, 0));
        horizontalLine.SetPosition(1, new Vector3(gridManager.cols * gridManager.cellSize, cellPosition.y, 0));

        // ������ ������������ �����
        verticalLine.SetPosition(0, new Vector3(cellPosition.x, 0, 0));
        verticalLine.SetPosition(1, new Vector3(cellPosition.x, gridManager.rows * gridManager.cellSize, 0));
    }

    void PlacePlant()
    {
        // �������� ���������� ���� � ������� �����������
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;

        // ������� ��������� ������
        Vector2Int gridCell = gridManager.GetGridCellFromPosition(mousePosition);
        Vector3 cellPosition = gridManager.GetCellPosition(gridCell.x, gridCell.y);

        // ��������� �������� � ���� ������
        Instantiate(plantPrefab, cellPosition, Quaternion.identity);
    }
}
