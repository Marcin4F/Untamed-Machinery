using UnityEngine;
using System.Collections.Generic;

public class SpawnerManager : MonoBehaviour
{
    public static SpawnerManager Instance { get; private set; }

    private List<EnemySpawner> allSpawners = new List<EnemySpawner>();
    private HashSet<EnemySpawner> completedSpawners = new HashSet<EnemySpawner>();



    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterSpawner(EnemySpawner spawner)
    {
        if (!allSpawners.Contains(spawner))
        {
            allSpawners.Add(spawner);
        }
    }

    public void ReportWin(EnemySpawner spawner)
    {
        completedSpawners.Add(spawner);

        if (completedSpawners.Count == allSpawners.Count)
        {
            Debug.Log("ALL WAVES COMPLETE!");
        }
    }
}
