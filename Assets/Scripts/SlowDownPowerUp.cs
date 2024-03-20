using System.Collections;
using UnityEngine;

public class SlowdownPowerUp : MonoBehaviour
{
    // All this script need to be optimized
    public float duration = 5f;
    public float slowdownFactor = 0.5f;

    private void OnMouseDown()
    {
        FindObjectOfType<DropController>().ApplySlowdownToAllDrops();
        AudioController.Instance.PlaySound("DropSolved");
        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.Translate(Vector2.down *1.5f* Time.deltaTime);

        float powerUpBottom = transform.position.y - (GetComponent<Renderer>().bounds.size.y / 2);

        if (powerUpBottom <= -Screen.height/2 + 5.0f)
            gameObject.SetActive(false);

    }
}
