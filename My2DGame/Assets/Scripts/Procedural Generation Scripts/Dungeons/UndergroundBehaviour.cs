using System.Collections;
using UnityEngine;
using Cinemachine;
using System.Linq;
using System.Threading;

public class UndergroundBehaviour : DungeonBehaviour
{
    [SerializeField]
    Item playerSave;

    [SerializeField]
    Player Player;

    [SerializeField]
    DungeonGenerator generator;

    [SerializeField]
    CinemachineVirtualCamera mainVirtualCamera;

    [SerializeField]
    Countdown countdown;

    private bool isPlayerInUnderground;

    Item playerSaveInstance;

    private float cameraCooldown = 7f;
    private float lastCameraMoveTime = -Mathf.Infinity;


    private void Update()
    {
        if (!isPlayerInUnderground)
        {
            InitDungeon();
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            if (Time.time >= lastCameraMoveTime + cameraCooldown)
            {
                lastCameraMoveTime = Time.time;
                StartCoroutine(MoveCameraToItemAndBack(playerSaveInstance));
            }
            else
            {
                Debug.Log($"Camera move is on cooldown. Try again in {Mathf.Ceil(lastCameraMoveTime + cameraCooldown - Time.time)} seconds.");
            }
        }
    }

    public void InitDungeon()
    {
        PlacePlayerAtHighestPosition(Player);
        playerSaveInstance = Instantiate(playerSave, playerSave.transform.position, playerSave.transform.rotation);
        PlaceItemToRandomPosition(playerSaveInstance);
        Player.SetTransparency(0.1f);
        Player.EnableGravityMode();
        isPlayerInUnderground = true;
        StartCoroutine(MoveCameraToItemAndBack(playerSaveInstance));
    }

    private void PlacePlayerAtHighestPosition(Player player)
    {
        Vector2Int highestPosition = new Vector2Int(int.MinValue, int.MinValue);
        foreach (var position in generator.undergroundDungeon.Floor)
        {
            if (position.y > highestPosition.y || (position.y == highestPosition.y && position.x > highestPosition.x))
            {
                highestPosition = position;
            }
        }

        if (player != null)
        {
            player.transform.position = new UnityEngine.Vector3(highestPosition.x, highestPosition.y + 2, player.transform.position.z);
            Debug.Log("Player placed in position: " + new UnityEngine.Vector3(highestPosition.x, highestPosition.y, player.transform.position.z));
        }
        else
        {
            Debug.LogWarning("Player reference is missing.");
        }
    }

    private void PlaceItemToRandomPosition(Item item)
    {
        if (generator.undergroundDungeon.Floor == null || generator.undergroundDungeon.Floor.Count == 0)
        {
            Debug.LogWarning("No available positions in the dungeon floor.");
            return;
        }

        int heightLimit = generator.undergroundDungeon.Height / 2;

        var validPositions = generator.undergroundDungeon.Cave
            .Where(position => position.y < heightLimit && generator.undergroundDungeon.Floor.Contains(new Vector2Int(position.x, position.y - 1)))
            .ToList();

        if (validPositions.Count == 0)
        {
            Debug.LogWarning("No valid positions available under the height limit with floor support.");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, validPositions.Count);
        Vector2Int randomPosition = validPositions[randomIndex];

        if (item != null)
        {
            item.transform.position = new UnityEngine.Vector3(randomPosition.x + 0.5f, randomPosition.y + 0.5f, item.transform.position.z);
            Debug.Log("Item placed at random position: " + new UnityEngine.Vector3(randomPosition.x, randomPosition.y, item.transform.position.z));
        }
        else
        {
            Debug.LogWarning("Item reference is missing.");
        }
    }

    private IEnumerator MoveCameraToItemAndBack(Item item)
    {
        if (mainVirtualCamera == null)
        {
            Debug.LogWarning("Main Cinemachine virtual camera is not assigned.");
            yield break;
        }

        if (item == null)
        {
            Debug.LogWarning("Item instance is null.");
            yield break;
        }

        Transform originalFollowTarget = mainVirtualCamera.Follow;

        GameObject tempFollow = new GameObject("TempFollow");

        tempFollow.transform.position = item.transform.position;

        mainVirtualCamera.Follow = tempFollow.transform;

        yield return SmoothMoveTransform(tempFollow.transform, item.transform.position, 0.5f);

        yield return new WaitForSeconds(2f);

        yield return SmoothMoveTransform(tempFollow.transform, Player.transform.position, 1.5f);

        mainVirtualCamera.Follow = Player.transform;

        Destroy(tempFollow);

        countdown.StartCountdown = true;


        Debug.Log("Camera smoothly moved to item and back to player.");
    }

    private IEnumerator SmoothMoveTransform(Transform movingTransform, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = movingTransform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            movingTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        movingTransform.position = targetPosition;
    }

}
