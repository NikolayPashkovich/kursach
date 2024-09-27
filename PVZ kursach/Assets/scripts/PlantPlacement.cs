using UnityEngine;

public class PlantPlacement : MonoBehaviour
{
    public GridManager gridManager;  // ������ �� ������ �����
    public GameObject plantPrefab;  // ������ ��������
    public LineRenderer horizontalLine;  // ����� ��� �����������
    public LineRenderer verticalLine;  // ����� ��� ���������
    public Material transparentLineMaterial;  // �������������� �������� ��� �����

    private bool isPlacing = false;  // ����, ������� �� ����� �������

    void Update()
    {
        // ���� ������� ����� �������, ����������� ����������� ����
        if (isPlacing)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 cellPosition = gridManager.GetGridCellFromPosition(mousePosition);

            // ��������� ��������� ����� �� ������
            DrawLines(cellPosition);

            // ���� �������� ������ ���� - ��������� ������� ��������
            if (Input.GetMouseButtonUp(0))
            {
                PlacePlant(cellPosition);
            }
        }
    }

    // ��������� ������ ������� ��������
    public void ActivatePlantPlacement()
    {
        isPlacing = true;
        horizontalLine.enabled = true;
        verticalLine.enabled = true;

        // ��������� �������������� �������� � ������
        horizontalLine.material = transparentLineMaterial;
        verticalLine.material = transparentLineMaterial;
    }

    // ������� �������� � ��������� ������
    void PlacePlant(Vector3 cellPosition)
    {
        // ������� �������� � ��������� �������
        Instantiate(plantPrefab, cellPosition, Quaternion.identity);

        // ��������� ����� ����� �������
        horizontalLine.enabled = false;
        verticalLine.enabled = false;

        // ��������� ����� �������
        isPlacing = false;
    }

    // ��������� �����, ������� ������������ � ��������� ������
    void DrawLines(Vector3 cellPosition)
    {
        // ��������� ��������� ����� ��� ��������� �����
        Vector3 origin = gridManager.gridOrigin;

        // �������������� �����
        horizontalLine.SetPosition(0, new Vector3(origin.x, cellPosition.y, 0));
        horizontalLine.SetPosition(1, new Vector3(origin.x + gridManager.cols * gridManager.cellWidth, cellPosition.y, 0));

        // ������������ �����
        verticalLine.SetPosition(0, new Vector3(cellPosition.x, origin.y, 0));
        verticalLine.SetPosition(1, new Vector3(cellPosition.x, origin.y + gridManager.rows * gridManager.cellHeight, 0));
    }
}
