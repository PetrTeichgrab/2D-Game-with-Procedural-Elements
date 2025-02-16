using UnityEngine;

public class PlayerSave : MonoBehaviour
{
    public float amplitude = 0.3f;
    public float speed = 1f;     

    private Vector3 startPosition;

    [SerializeField]
    Countdown countdown;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float xOffset = (Mathf.PerlinNoise(Time.time * speed, 0f) - 0.5f) * 2f * amplitude;
        float yOffset = (Mathf.PerlinNoise(0f, Time.time * speed) - 0.5f) * 2f * amplitude;

        transform.position = new Vector3(startPosition.x + xOffset, startPosition.y + yOffset, startPosition.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();

        if (player != null)
        {
            Debug.Log("Player collided with the save item!");

            HandlePlayerCollision(player);
        }
    }

    private void HandlePlayerCollision(Player player)
    {
        player.SetTransparency(1.0f);
        player.DisableGravityMode();  
        player.Respawn();
        gameObject.SetActive(false);
        countdown.StartCountdown = false;
        countdown.HideCountdown();
        Debug.Log("Save item collected, actions applied to the player, and item removed.");
    }
}
