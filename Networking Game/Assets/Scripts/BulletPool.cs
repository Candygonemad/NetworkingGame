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

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Spawn()
    {
        Debug.Log("Got here");
        bulletPoolInstance = this;

        bullets = new GameObject[poolLength];
        for (int i = 0; i < poolLength; i++)
        {
            bullets[i] = Instantiate(pooledBullet);
            bullets[i].GetComponent<NetworkObject>().Spawn();
            bullets[i].SetActive(false);
        }
    }

    public GameObject GetBullet()
    {
        if(bullets.Length > 0)
        {
            for(int i = 0; i < bullets.Length; i++)
            {
                if (!bullets[i].activeInHierarchy)
                {
                    return bullets[i];
                }
            }
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
}
