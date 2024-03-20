using System.Collections;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    // All this script need to be optimized
    public GameObject slowdownPowerUpPrefab;
    public float spawnInterval = 20f;

    private void Start()
    {
        StartCoroutine(SpawnPowerUpRoutine());
    }

    IEnumerator SpawnPowerUpRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (GameController.Instance.canSpawnPowerUps)
            {
                SpawnPowerUp();
            }
        }
    }

    void SpawnPowerUp()
    {
        Instantiate(slowdownPowerUpPrefab, GetRandomPositionAboveScreen(), Quaternion.identity);
    }

    Vector3 GetRandomPositionAboveScreen()
    {
        float screenWidthInWorld = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0)).x;
        float randomX = Random.Range(-screenWidthInWorld + 1.0f, screenWidthInWorld - 1.0f);

        float spawnY = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 0)).y + 0.5f;
        return new Vector3(randomX, spawnY, 0);
    }
}
