using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] GameObject carPrefab;
    [SerializeField] TerrainBlok terrain;
    [SerializeField] float minSpawnDuration = 2;
    [SerializeField] float maxSpawnDuration = 4;

    bool isRight;
    float timer = 3;

    private void Start()
    {
        isRight = Random.value > 0.5f ? true : false;
        timer = Random.Range(minSpawnDuration, maxSpawnDuration ) ;
    }

    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            return;
        }

        timer = Random.Range(minSpawnDuration,maxSpawnDuration);

        var SpawnPos = this.transform.position +
            Vector3.right * (isRight ? -(terrain.Extent + 1) : terrain.Extent + 1);

        var go = Instantiate(
            original: carPrefab,
            position: this.transform.position,
            rotation: Quaternion.Euler(0, isRight ? 90 : -90, 0),
            parent: this.transform);
        var car = go.GetComponent<Car>();
        car.SetUp(terrain.Extent);
    }
}
