using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool bulletPoolInstance;

    [SerializeField]
    private GameObject pooledBullet;

    private GameObject[] bullets;
    public int poolLength = 10;

    private void Awake()
    {
        bulletPoolInstance = this;

        bullets = new GameObject[poolLength];
        for (int i = 0; i < poolLength; i++)
        {
            bullets[i] = Instantiate(pooledBullet);
            bullets[i].SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetBullet()
    {
        Debug.Log("Bullets Length: " + bullets.Length);
        if(bullets.Length > 0)
        {
            Debug.Log("Length greater");
            for(int i = 0; i < bullets.Length; i++)
            {
                if (!bullets[i].activeInHierarchy)
                {
                    Debug.Log("Inactive bullet");
                    return bullets[i];
                }
                    
                else
                    Debug.Log("Not active");
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
