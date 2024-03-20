using System.Collections.Generic;
using UnityEngine;

public class DropPool : MonoBehaviour
{
    public static DropPool Instance { get; private set; }

    [SerializeField] private Drop dropPrefab;
    [SerializeField] private int poolSize = 20;
    private Queue<Drop> pool = new Queue<Drop>();

    private float lastSpawnX;
    private const float MinDistanceX = 1.5f;

    private void Awake()
    {
        Instance = this;
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Drop newDrop = Instantiate(dropPrefab);
            newDrop.gameObject.SetActive(false);
            pool.Enqueue(newDrop);
        }
    }

    public Drop GetDrop()
    {
        if (pool.Count > 0)
        {
            Drop drop = pool.Dequeue();
            drop.transform.position = GetRandomPositionAboveScreen();
            drop.gameObject.SetActive(true);
            return drop;
        }
        else
        {
            Drop newDrop = Instantiate(dropPrefab);
            return newDrop;
        }
    }

    public void ReturnDrop(Drop drop)
    {
        drop.gameObject.SetActive(false);
        pool.Enqueue(drop);
    }

    // Get a random position slightly above the screen
    Vector3 GetRandomPositionAboveScreen()
    {
        float screenWidthInWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float randomX;

        do
        {
            randomX = Random.Range(-screenWidthInWorld + 1.0f, screenWidthInWorld - 1.0f);
        }
        // Avoid two consecutive drops in almost the same position
        while (Mathf.Abs(randomX - lastSpawnX) < MinDistanceX);

        lastSpawnX = randomX;

        float spawnY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y + 0.5f;
        return new Vector3(randomX, spawnY, 0);
    }

}
