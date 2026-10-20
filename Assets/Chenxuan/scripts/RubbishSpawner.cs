using UnityEngine;
using System.Collections.Generic;

public class RubbishSpawner : MonoBehaviour
{
    public GameObject[] rubbishPrefabs;
    public Transform[] spawnPoints;

    public float spawnInterval = 5f;
    public int maxRubbish = 5;

    private float timer;
    private List<GameObject> rubbishList = new List<GameObject>();
    private ProjectResourceController projectController;

    void Start()
    {
        projectController = FindFirstObjectByType<ProjectResourceController>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        rubbishList.RemoveAll(rubbish => rubbish == null);

        if (timer >= spawnInterval)
        {
            timer = 0f;

            if (rubbishList.Count < maxRubbish)
            {
                SpawnRubbish();
            }
        }
    }

    void SpawnRubbish()
    {
        if (rubbishPrefabs.Length == 0 ||
            spawnPoints.Length == 0 ||
            rubbishPrefabs.Length != spawnPoints.Length)
        {
            Debug.LogWarning("Rubbish prefabs and spawn points must match.");
            return;
        }

        List<int> availablePoints = new List<int>();

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i] == null || rubbishPrefabs[i] == null)
                continue;

            bool occupied = false;

            foreach (GameObject item in rubbishList)
            {
                if (item != null &&
                    Vector3.Distance(
                        item.transform.position,
                        spawnPoints[i].position
                    ) < 0.01f)
                {
                    occupied = true;
                    break;
                }
            }

            if (!occupied)
            {
                availablePoints.Add(i);
            }
        }

        if (availablePoints.Count == 0)
        {
            return;
        }

        int randomPoint = availablePoints[
            Random.Range(0, availablePoints.Count)
        ];

        GameObject newRubbish = Instantiate(
            rubbishPrefabs[randomPoint],
            spawnPoints[randomPoint].position,
            Quaternion.identity
        );

        rubbishList.Add(newRubbish);

        if (projectController != null)
        {
            projectController.CurrentProjectConditions.ChangeQuality(-1f);
        }

        Debug.Log("Rubbish count: " + rubbishList.Count);
    }
}
