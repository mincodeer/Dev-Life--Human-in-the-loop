using UnityEngine;
using System.Collections.Generic;

public class RubbishSpawner : MonoBehaviour
{
    public GameObject rubbish;
    public float spawnInterval = 5f;
    public int maxRubbish = 5;
    public Vector2 minPosition = new Vector2(-3f, -2f);
    public Vector2 maxPosition = new Vector2(5f, 5f);
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
        float x = Random.Range(minPosition.x, maxPosition.x);
        float y = Random.Range(minPosition.y, maxPosition.y);

        Vector3 spawnPosition = new Vector3(x, y, 0f);

        GameObject newRubbish = Instantiate(
            rubbish,
            spawnPosition,
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
