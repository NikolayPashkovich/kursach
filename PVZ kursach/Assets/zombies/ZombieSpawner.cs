using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{
    [SerializeField] Zombie[] zombiePrefabs;
    [SerializeField] int[] zombieCost;
    [SerializeField] int[] zombieQuantity;
    [SerializeField] int[] wavesPoints;
    [SerializeField] int waveIndex;
    [SerializeField] float waitTime;
    [SerializeField] float LevelTime;
    [SerializeField] float timer;

    int[] zombieInLinesCount;
    public bool isEndSpavned { get; private set; }


    public bool isPlayerLoose { get; private set; }
    [SerializeField] float minZombieX;

    public void SetData(LevelData levelData)
    {
        zombieQuantity = levelData.zombieQuantity;
        wavesPoints = levelData.wavesPoints;
        LevelTime = levelData.LevelTime;
    }
    private void Start()
    {
        zombieInLinesCount = new int[GridManager.Instance.rows];
    }
    public void GoToGame()
    {
        for (int i = 0; i < GridManager.Instance.zombies.Count; i++)
        {
            Destroy(GridManager.Instance.zombies[i].gameObject);
        }
        isEndSpavned = false;
        isPlayerLoose = false;
    }
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer < waitTime) { return; }
        float vawesTimer = timer - waitTime;
        if (vawesTimer / LevelTime > (float)waveIndex / wavesPoints.Length && waveIndex < wavesPoints.Length)
        {
            //Debug.Log($"vavesTimer = {vawesTimer}\nLevelTime = {LevelTime}\nLevelPurcent = {vawesTimer / LevelTime}\nwaveIndex = {waveIndex}\nwavePoints = {wavesPoints.Length}\nwavePurcent = {waveIndex / wavesPoints.Length}\n" +
            //    $"{vawesTimer / LevelTime} > { waveIndex / wavesPoints.Length}");
            SpawnWave(waveIndex);
            waveIndex+=1;
            if (wavesPoints.Length <= waveIndex)
            {
                isEndSpavned = true;
            }
        }
        if (!isPlayerLoose)
        {
            for (int i = 0; i < GridManager.Instance.zombies.Count; i++)
            {
                if (GridManager.Instance.zombies[i].transform.position.x < minZombieX)
                {
                    isPlayerLoose = true;
                    break;
                }
            }
        }
        EconomicController.instance.UpdateProgressBar(vawesTimer / LevelTime);
    }
    public void SpawnZombiesForStart()
    {
        zombieInLinesCount = new int[GridManager.Instance.rows];
        for (int i = 0; i < zombieQuantity.Length; i++)
        {
            for (int j = 0; j < zombieQuantity[i]; j++)
            {
                SpawnZombie(i);
            }
        }
    }
    void SpawnWave(int index)
    {
        int points = wavesPoints[index];
        while (points > 0)
        {
            int zombieIndex = SelectRandomZombie(points);
            points -= zombieCost[zombieIndex];
            GameObject zombObj = SpawnZombie(zombieIndex);
            zombObj.GetComponent<Zombie>().StartMove();
        }
    }
    GameObject SpawnZombie(int zombieIndex)
    {
        int randomLine = RandomSelector.GetRandomIndexInverseWeigths(zombieInLinesCount, 100);
        zombieInLinesCount[randomLine]++;
        Vector2Int gridCell = new Vector2Int(GridManager.Instance.cols - 1, randomLine);

        Vector3 zombiePos = GridManager.Instance.GetPositionFromGridCell(gridCell)
            + new Vector3(Random.Range(GridManager.Instance.cellSize.x*2, GridManager.Instance.cellSize.x * 4), 0, 0);
        //Vector3 zombiePos = GridManager.Instance.GetCellPosition(Random.Range(0, GridManager.Instance.rows), GridManager.Instance.cols-1) + new Vector3(-GridManager.Instance.cellSize.x, 0,0);
         return Instantiate(zombiePrefabs[zombieIndex].gameObject, zombiePos, Quaternion.Euler(0, 0, 0));
    }
    int SelectRandomZombie(int points)
    {
        List<int> avalibleIndexes = new List<int>();
        for (int i = 0; i < zombieCost.Length; i++)
        {
            if (points >= zombieCost[i] && zombieQuantity.Length > i && zombieQuantity[i] > 0) { avalibleIndexes.Add(i); }
        }
        if (avalibleIndexes.Count == 0) 
        {
            Debug.LogError("Can't select zombie");
            return 0; 
        }
        return avalibleIndexes[Random.Range(0, avalibleIndexes.Count)];
    }
}
