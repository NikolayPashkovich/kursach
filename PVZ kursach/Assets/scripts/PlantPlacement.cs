using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlantPlacement : MonoBehaviour
{
    public GridManager gridManager; // Ссылка на менеджер сетки
    public GameObject plantPrefab;  // Префаб растения
    public LineRenderer horizontalLine;  // Линия для горизонтальной полосы
    public LineRenderer verticalLine;    // Линия для вертикальной полосы

    [SerializeField] SpriteRenderer placingPlantImage;
    Vector3 placingPlantShift;
    private SelectButton selectButton = null;
    bool isDelMode = false;
    Plant plantToDel = null;
    [SerializeField] Color delColor;
    [SerializeField] AudioSource audio;
    [SerializeField] AudioClip plantClip;
    void Update()
    {
        if (selectButton != null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector2Int gridCell = gridManager.GetGridCellFromPosition(mousePosition);
            Vector3 cellPosition = gridManager.GetPositionFromGridCell(gridCell);
            if (gridCell != new Vector2Int(-1,-1))
            {
                DrawLines(cellPosition);
                placingPlantImage.transform.position = cellPosition + placingPlantShift;
                if (Input.GetMouseButtonUp(0))
                {
                    PlacePlant(gridCell,cellPosition);
                }
                if (Input.GetMouseButtonUp(1))
                {
                    CancelPlacing();
                }
            }
        }
        if (isDelMode)
        {
            if (Input.GetMouseButtonUp(1))
            {
                CancelDelMode();
                return;
            }
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            Vector2Int gridCell = gridManager.GetGridCellFromPosition(mousePosition);
            Vector3 cellPosition = gridManager.GetPositionFromGridCell(gridCell);
            if (gridCell != new Vector2Int(-1, -1))
            {
                Plant currentPlantToDel = null;
                for (int i = 0; i < GridManager.Instance.plants.Count; i++)
                {
                    if (GridManager.Instance.plants[i].posInGrid == gridCell) { currentPlantToDel = GridManager.Instance.plants[i];break; }
                }
                if (currentPlantToDel == null)
                {
                    if (plantToDel != null)
                    {
                        DeselectPlantToDel();
                    }
                    HideLines();
                    return;
                }
                if (currentPlantToDel != plantToDel)
                {
                    DeselectPlantToDel();
                    plantToDel = currentPlantToDel;
                    currentPlantToDel.spriteRenderer.color = delColor;
                    DrawLines(cellPosition);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    DelPlant();
                }
            }
        }
    }
    void DelPlant()
    {
        audio.clip = plantClip;
        audio.Play();
        Destroy(plantToDel.gameObject);
        CancelDelMode();
    }
    void DeselectPlantToDel()
    {
        if (plantToDel == null) { return; }
        plantToDel.spriteRenderer.color = plantToDel.NormalColor();
        plantToDel = null;
    }
    void CancelPlacing()
    {
        isDelMode = false;
        UIController.DeselectButton();
        selectButton = null;
        placingPlantImage.sprite = null;
        HideLines();
    }
    void HideLines()
    {
        horizontalLine.SetPosition(0, Vector3.zero);
        horizontalLine.SetPosition(1, Vector3.zero);
        verticalLine.SetPosition(0, Vector3.zero);
        verticalLine.SetPosition(1, Vector3.zero);
    }
    public void SelectPlacingPlant(SelectButton selectButton)
    {
        isDelMode = false;
        if (this.selectButton == selectButton)
        {
            CancelPlacing();
            return;
        }
        this.selectButton = selectButton;
        selectButton.Select();
        placingPlantImage.sprite = selectButton.plant.spriteRenderer.sprite;
        placingPlantImage.transform.localScale = selectButton.plant.transform.localScale;
        placingPlantShift = selectButton.plant.GetPosShift();
        placingPlantImage.transform.position = new Vector3(100, 100);
    }
    public void DelButtonClick()
    {
        isDelMode = !isDelMode;
        if (isDelMode) { SelectDelMode(); }
        else { CancelDelMode(); }
    }
    void SelectDelMode()
    {
        CancelPlacing();
        isDelMode = true;
        plantToDel = null;
    }
    void CancelDelMode()
    {
        DeselectPlantToDel();
        HideLines();
        isDelMode = false;
        plantToDel = null; 
    }
        
    void DrawLines(Vector3 cellPosition)
    {
        

        // Рисуем горизонтальную линию
        horizontalLine.SetPosition(0, new Vector3((gridManager.gridOrigin.x-gridManager.cellSize.x/2), cellPosition.y, 0));
        horizontalLine.SetPosition(1, new Vector3((gridManager.gridOrigin.x - gridManager.cellSize.x / 2) + gridManager.cols * gridManager.cellSize.x, cellPosition.y, 0));

        // Рисуем вертикальную линию
        verticalLine.SetPosition(0, new Vector3(cellPosition.x, (gridManager.gridOrigin.y - gridManager.cellSize.y / 2), 0));
        verticalLine.SetPosition(1, new Vector3(cellPosition.x, (gridManager.gridOrigin.y - gridManager.cellSize.y / 2) + gridManager.rows * gridManager.cellSize.y, 0));
    }

    void PlacePlant(Vector2Int gridCell,Vector3 cellPosition)
    {
        
        for (int i = 0; i <GridManager.Instance.plants.Count; i++)
        {
            if (GridManager.Instance.plants[i].posInGrid == gridCell) { return; }
        }
        audio.clip = plantClip;
        audio.Play();
        EconomicController.instance.RemoveSuns(selectButton.plant.GetCost());
        selectButton.ResetTimer();

        GameObject plantObj = Instantiate(selectButton.plant.gameObject, cellPosition, Quaternion.identity);
        Plant plant = plantObj.GetComponent<Plant>();
        plant.posInGrid = gridCell;
        UIController.DeselectButton();
        selectButton = null;
        placingPlantImage.sprite = null;

        horizontalLine.SetPosition(0, Vector3.zero);
        horizontalLine.SetPosition(1, Vector3.zero);
        verticalLine.SetPosition(0, Vector3.zero);
        verticalLine.SetPosition(1, Vector3.zero);
    }
}
