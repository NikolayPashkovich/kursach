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

    int[] zombieInLInesQuntity;
    private void Start()
    {
        zombieInLInesQuntity = new int[GridManager.Instance.rows];
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
        }
    }
    void SpawnWave(int index)
    {
        int points = wavesPoints[index];
        while (points > 0)
        {
            int zombieIndex = SelectRandomZombie(points);
            points -= zombieCost[zombieIndex];
            Vector2Int gridCell = new Vector2Int(GridManager.Instance.cols - 1, RandomSelector.GetRandomIndexInverseWeigths(zombieInLInesQuntity));
            Vector3 zombiePos = GridManager.Instance.GetPositionFromGridCell(gridCell) 
                + new Vector3(Random.Range(GridManager.Instance.cellSize.x, GridManager.Instance.cellSize.x*2), 0, 0);
            //Vector3 zombiePos = GridManager.Instance.GetCellPosition(Random.Range(0, GridManager.Instance.rows), GridManager.Instance.cols-1) + new Vector3(-GridManager.Instance.cellSize.x, 0,0);
            GameObject zombObj = Instantiate(zombiePrefabs[zombieIndex].gameObject, zombiePos, Quaternion.Euler(0, 0, 0));
            zombObj.GetComponent<Zombie>().StartMove();
        }
    }
    int SelectRandomZombie(int points)
    {
        List<int> avalibleIndexes = new List<int>();
        for (int i = 0; i < zombieCost.Length; i++)
        {
            if (points >= zombieCost[i]) { avalibleIndexes.Add(i); }
        }
        if (avalibleIndexes.Count == 0) 
        {
            Debug.LogError("Can't select zombie");
            return 0; 
        }
        return avalibleIndexes[Random.Range(0, avalibleIndexes.Count)];
    }
}
