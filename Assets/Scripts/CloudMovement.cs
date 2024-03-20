using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    // All this script need to be optimized
    public float speed = 2.0f;
    public float direction = 1.0f;

    private float limitX = 10.0f;

    void Update()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);

        if (Mathf.Abs(transform.position.x) > limitX)
        {
            transform.position = new Vector2(-transform.position.x, transform.position.y);
        }
    }
}
