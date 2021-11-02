using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int numEnemies = 10;
    private float lastTime = 0f;
    public float timeToSpawnNext;
    public GameObject enemy;
    private Camera cam;
    private float height;
    private float width;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        height = 2f * cam.orthographicSize;
        width = height * cam.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Time: " + Time.time + " Last time: " + lastTime + " Time to Next Spawn: " + timeToSpawnNext);
        if(Time.time > lastTime + timeToSpawnNext)
        {
            GameObject singleEnemy;
            singleEnemy = Instantiate(enemy, new Vector3(Random.Range(-.45f, .45f) * width, height, 0), Quaternion.identity);
            singleEnemy.GetComponent<Enemy>().direction = new Vector3(0, -1, 0);
            lastTime = Time.time;
        }
    }
}
