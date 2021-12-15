using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemySpawner : NetworkBehaviour
{
    public int numEnemies;
    public float timeToSpawnNext;
    private GameObject enemy;
    private Camera cam;
    private float height;
    private float width;

    private List<GameObject> playersList;
    private GameObject singleEnemy;

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
        
    }

    public void Spawn(int numberOfEnemies, GameObject enemyType, LineDirection lineDirection, float randomSpawnTime)
    {
        timeToSpawnNext = randomSpawnTime;

        StartCoroutine(WaitToSpawnLine(numberOfEnemies, enemyType, lineDirection, randomSpawnTime));
    }

    public void Spawn(int numberOfEnemies, GameObject enemyType, Vector2 radius, bool clockwise, float randomSpawnTime)
    {
        timeToSpawnNext = randomSpawnTime;
        
        StartCoroutine(WaitToSpawnCircle(numberOfEnemies, enemyType, radius,  clockwise, randomSpawnTime));
    }

    public void Spawn(int numberOfEnemies, GameObject enemyType, List<GameObject> players, float randomSpawnTime)
    {
        timeToSpawnNext = randomSpawnTime;

        StartCoroutine(WaitToSpawnFollow(numberOfEnemies, enemyType, players, randomSpawnTime));
    }

    public IEnumerator WaitToSpawnLine(int numberOfEnemies, GameObject enemyType, LineDirection lineDirection, float randomSpawnTime)
    {
        if (!IsOwner)
            yield return new WaitForSeconds(0);

        timeToSpawnNext = randomSpawnTime;
        while (numberOfEnemies > 0)
        {
            //Debug.Log("Waiting to Spawn");
            yield return new WaitForSeconds(timeToSpawnNext);

            enemy = enemyType;
            Debug.Log("Enemy: " + enemy);
            singleEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
            Debug.Log("Single Enemy: " + singleEnemy);
            singleEnemy.GetComponent<Enemy>().direction = new Vector3(0, -1, 0);
            Debug.Log("Line Enemy: " + singleEnemy.GetComponent<LineEnemy>());
            singleEnemy.GetComponent<LineEnemy>().lineDirection = lineDirection;
            if (IsHost || IsServer)
                break;//singleEnemy.GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.ServerClientId);
            else
                SpawnEnemyServerRpc();
            
            

            timeToSpawnNext = Random.Range(1.0f, 3.0f);

            numberOfEnemies--;
        }
    }

    public IEnumerator WaitToSpawnCircle(int numberOfEnemies, GameObject enemyType, Vector2 radius, bool clockwise, float randomSpawnTime)
    {
        if (!IsOwner)
            yield return new WaitForSeconds(0);

        timeToSpawnNext = randomSpawnTime;
        while (numberOfEnemies > 0)
        {
            //Debug.Log("Waiting to Spawn");
            yield return new WaitForSeconds(timeToSpawnNext);

            enemy = enemyType;
            singleEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
            singleEnemy.GetComponent<Enemy>().direction = new Vector3(0, -1, 0);
            singleEnemy.GetComponent<CircleEnemy>().radius = radius;
            singleEnemy.GetComponent<CircleEnemy>().clockwise = clockwise;
            if (IsHost || IsServer)
                break;//singleEnemy.GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.ServerClientId);
            else
                SpawnEnemyServerRpc();
            

            //if(!IsHost)
                

            timeToSpawnNext = Random.Range(1.0f, 3.0f);

            numberOfEnemies--;
        }
    }

    public IEnumerator WaitToSpawnFollow(int numberOfEnemies, GameObject enemyType, List<GameObject> players, float randomSpawnTime)
    {
        if (!IsOwner)
            yield return new WaitForSeconds(0);

        timeToSpawnNext = randomSpawnTime;
        while (numberOfEnemies > 0)
        {
            //Debug.Log("Waiting to Spawn");
            yield return new WaitForSeconds(timeToSpawnNext);

            enemy = enemyType;
            playersList = players;
            

            //if(IsHost || IsServer)
            //{
            //    Debug.Log("Spawned");
            //    singleEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
            //    singleEnemy.GetComponent<NetworkObject>().Spawn();
            //}
            //else
            //{
                //SpawnEnemyServerRpc();
            //}

            GameObject player = playersList[(int)Random.Range(0, playersList.Count)];
            Debug.Log("Player to Follow: " + player);
            singleEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
            singleEnemy.GetComponent<FollowEnemy>().player = player;
            singleEnemy.GetComponent<Enemy>().direction = new Vector3(0, -1, 0);
            if (IsHost || IsServer)
                break;//singleEnemy.GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.ServerClientId);
            else
                SpawnEnemyServerRpc();
            
            //Debug.Log("Follow Enemy: " + singleEnemy.GetComponent<FollowEnemy>());
            


            timeToSpawnNext = Random.Range(1.0f, 3.0f);

            numberOfEnemies--;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnEnemyServerRpc()
    {
        Debug.Log("Spawned");
        //singleEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
        singleEnemy.GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.ServerClientId);
    }
}
