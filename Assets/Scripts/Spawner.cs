using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    private Collider spawnArea;

    public GameObject[] fruitPrefabs;
    public GameObject bombPrefab;
    public float minSpawnDelay = 0.25f;
    public float maxSpawnDelay = 1f;

    public float minAngle = -15f;
    public float maxAngle = 15f;

    public float minForce = 18f;
    public float maxForce = 22f;

    public float maxLifeTime = 5f;          // 最长的存在时间

    [Range(0, 1)]
    public float BombChance = 0.05f;
   

    private void Awake()
    {
        spawnArea = GetComponent<Collider>();
    }

    private void OnEnable()
    {
        StartCoroutine(Spawn());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
   
    private IEnumerator Spawn()
    {
        // 对于每一次新的游戏，设置一个缓冲时间
        yield return new WaitForSeconds(2f);

        while (enabled)
        {
            GameObject prefab = fruitPrefabs[UnityEngine.Random.Range(0, fruitPrefabs.Length)];

            if(UnityEngine.Random.Range(0,1f) <= BombChance)
            {
                prefab = bombPrefab;
            }

            // 设置初始位置
            Vector3 position = new Vector3();
            position.x = UnityEngine.Random.Range(spawnArea.bounds.min.x, spawnArea.bounds.max.x);
            position.y = UnityEngine.Random.Range(spawnArea.bounds.min.y, spawnArea.bounds.max.y);
            position.z = UnityEngine.Random.Range(spawnArea.bounds.min.z, spawnArea.bounds.max.z);
            // 设置初始的旋转角度
            Quaternion rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(minAngle, maxAngle));

            GameObject fruit = Instantiate(prefab, position, rotation);
            Destroy(fruit, maxLifeTime);

            float force = UnityEngine.Random.Range(minForce, maxForce);
            fruit.GetComponent<Rigidbody>().AddForce(fruit.transform.up * force, ForceMode.Impulse);
            yield return new WaitForSeconds(UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay));
        }
    }
}
