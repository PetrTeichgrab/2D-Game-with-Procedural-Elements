using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DungeonBehaviour : MonoBehaviour, IDungeonBehaviour
{
    [SerializeField]
    protected Item portalPrefab;

    public Item portal;

    public Item portal2;
    public bool Completed {  get; set; }

    protected void HandleBossInstance(Character bossInstance, ColorCore colorCore)
    {
        if (bossInstance != null && !bossInstance.isAlive && !Completed)
        {
            StartCoroutine(CallSpawnColorCoreAfterDelay(1.2f, colorCore, bossInstance.transform));
        }
    }

    public void LinkPortalTo(DungeonBehaviour target)
    {
        var portalBehaviour = portal.GetComponent<PortalBehaviour>();
        portalBehaviour.TargetDungeon = target;
        portalBehaviour.TargetPosition = target.GetAvailablePortalTransform();

        if (portal2 != null)
        {
            var portalBehaviour2 = portal2.GetComponent<PortalBehaviour>();
            portalBehaviour2.TargetDungeon = target;
            portalBehaviour2.TargetPosition = target.GetAvailablePortalTransform();
        }
    }

    public Transform GetAvailablePortalTransform()
    {
        if (portal != null) return portal.transform;
        if (portal2 != null) return portal2.transform;
        return this.transform;
    }



    private int CalculateEnemyCount(int floorCount, int baseCount, float scalingFactor, int minCount, int maxCount)
    {
        int enemyCount = Mathf.RoundToInt(baseCount + floorCount * scalingFactor);
        int count = Mathf.Clamp(enemyCount, minCount, maxCount);
        return count;
    }

    public void GenerateEnemies(Dungeon dungeon, List<(Character prefab, Action<Character> positionSetter)> enemyConfigs, int baseCount = 5, float scalingFactor = 0.015f, int minCount = 5, int maxCount = 50)
    {
        foreach (var room in dungeon.RoomList)
        {
            int enemyCount = CalculateEnemyCount(room.FloorList.Count, baseCount, scalingFactor, minCount, maxCount);

            if (enemyConfigs.Count == 0)
            {
                Debug.LogWarning("Nebyl nalezen ��dn� typ nep��tel k vygenerov�n�!");
                continue;
            }

            int enemiesPerType = Mathf.Max(1, enemyCount / enemyConfigs.Count);
            int remainingEnemies = enemyCount;

            foreach (var (prefab, positionSetter) in enemyConfigs)
            {
                int countForThisType = Mathf.Min(enemiesPerType, remainingEnemies);
                remainingEnemies -= countForThisType;

                for (int i = 0; i < countForThisType; i++)
                {
                    var enemy = Instantiate(prefab);
                    positionSetter(enemy);
                }
            }
        }

        Debug.Log("Nep��tel� byli vygenerov�ni podle velikosti m�stnost�.");
    }


    protected bool IsPlayerInsideDungeon(Player player, Dungeon dungeon)
    {
        Vector3Int playerPosition = Vector3Int.FloorToInt(player.transform.position);

        return playerPosition.x >= dungeon.DungeonBounds.xMin &&
               playerPosition.x < dungeon.DungeonBounds.xMax &&
               playerPosition.y >= dungeon.DungeonBounds.yMin &&
               playerPosition.y < dungeon.DungeonBounds.yMax;
    }

    protected IEnumerator CallSpawnColorCoreAfterDelay<T>(float delay, T colorCorePrefab, Transform spawnTransform) where T : ColorCore
    {
        yield return new WaitForSeconds(delay);
        SpawnColorCore(colorCorePrefab, spawnTransform);
    }
    private T SpawnColorCore<T>(T colorCorePrefab, Transform spawnTransform) where T : ColorCore
    {
        return Instantiate(colorCorePrefab, spawnTransform.position, spawnTransform.rotation);
    }

    protected void GenerateObjects<T>(List<(T prefab, Action<T> positionSetter, int count)> configs) where T : UnityEngine.Object
    {
        foreach (var config in configs)
        {
            for (int i = 0; i < config.count; i++)
            {
                if (config.prefab != null)
                {
                    T obj = Instantiate(config.prefab);
                    config.positionSetter(obj);
                }
            }
        }
    }

    protected void GenerateDungeonObjects<TItem, TEnemy>(List<(TItem prefab, Action<TItem> positionSetter, int count)> itemConfigs, List<(TEnemy prefab, Action<TEnemy> positionSetter, int count)> enemyConfigs
)        where TItem : UnityEngine.Object where TEnemy : UnityEngine.Object
    {
        GenerateObjects(itemConfigs);
        GenerateObjects(enemyConfigs);
    }


    public virtual void CreateAndSetPositions()
    {
    }

    public virtual void Create()
    {
    }
}
