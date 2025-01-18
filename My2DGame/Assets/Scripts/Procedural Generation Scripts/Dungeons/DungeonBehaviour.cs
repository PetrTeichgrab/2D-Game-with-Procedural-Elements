using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class DungeonBehaviour : MonoBehaviour, IDungeonBehaviour
{
    public bool Completed {  get; set; }

    protected void HandleBossInstance(Character bossInstance, ColorCore colorCore)
    {
        if (bossInstance != null && !bossInstance.isAlive && !Completed)
        {
            StartCoroutine(CallSpawnColorCoreAfterDelay(1.2f, colorCore, bossInstance.transform));
        }
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
