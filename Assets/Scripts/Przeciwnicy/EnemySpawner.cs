using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int spawnAmount = 5;
    public int waveCount = 3;

    private int currentWave = 0;
    private List<Enemy> activeEnemies = new List<Enemy>();
    private bool isSpawningWave = false;
    private bool winDeclared = false;

    void Start()
    {
        SpawnerManager.Instance.RegisterSpawner(this);
        StartCoroutine(SpawnWave());
    }

    void Update()
    {
        if (activeEnemies.Count > 0)
            activeEnemies.RemoveAll(enemy => enemy == null || !enemy.isAlive);

        if (activeEnemies.Count == 0 && currentWave < waveCount && !isSpawningWave)
        {
            StartCoroutine(SpawnWave());
        }

        if (activeEnemies.Count == 0 && currentWave == waveCount && !winDeclared)
        {
            Debug.Log($"WIN! (Spawner at {transform.position})");
            winDeclared = true;
            SpawnerManager.Instance.ReportWin(this);
        }
    }

    IEnumerator SpawnWave()
    {
        isSpawningWave = true;
        currentWave++;
        Debug.Log($"Spawning Wave {currentWave} at {transform.position}...");

        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject enemyObj = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

            Enemy enemy = enemyObj.GetComponent<Enemy>();
            if (enemy != null)
            {
                activeEnemies.Add(enemy);
            }

            yield return new WaitForSeconds(2f);
        }

        isSpawningWave = false;
    }
}
