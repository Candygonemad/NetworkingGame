using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class BulletPool : NetworkBehaviour
{
    public static BulletPool bulletPoolInstance;

    [SerializeField]
    private GameObject pooledBullet;

    private GameObject[] bullets;
    public int poolLength = 10;
    private int currentBullet = 0;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletPoolInstance = this;

        bullets = new GameObject[poolLength];
        for (int i = 0; i < poolLength; i++)
        {
            bullets[i] = Instantiate(pooledBullet);
            if (IsHost || IsServer)
            {
                Debug.Log("Bullet spawn in Host/Server");
                
                bullets[i].GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.ServerClientId);
            }
            else
            {
                Debug.Log("Bullet spawn in Client");
                SpawnBulletServerRpc(i);
            }

            bullets[i].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetBullet()
    {
        if(bullets.Length > 0)
        {
            for(int i = currentBullet; i < bullets.Length; i++)
            {
                if (!bullets[i].activeInHierarchy)
                {
                    currentBullet = i + 1;
                    return bullets[i];
                }
            }
            for(int i = 0; i < currentBullet; i++)
            {
                if (!bullets[i].activeInHierarchy)
                {
                    currentBullet = i + 1;
                    return bullets[i];
                }
            }

            if (currentBullet >= bullets.Length)
                currentBullet = 0;
        }
        else
        {
            StartCoroutine("Reload");
        }

        return null;
    }

    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(3f);
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnBulletServerRpc(int i)
    {
        Debug.Log("Spawning bullets");
        //bullets[i] = Instantiate(pooledBullet);
        bullets[i].GetComponent<NetworkObject>().SpawnWithOwnership(NetworkManager.ServerClientId);
    }
}
