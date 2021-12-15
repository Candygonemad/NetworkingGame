using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Shoot : NetworkBehaviour
{
    public BulletPatterns currentBulletPattern;
    private float lastShotTime = 0f;
    public AIDirector director;
    
    // Start is called before the first frame update
    void Start()
    {
        director = GameObject.Find("AI Director").GetComponent<AIDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
            return;
        if(Input.GetKey(KeyCode.Space) && Time.time > lastShotTime + currentBulletPattern.bulletPatterns[0].timeBetweenShots)
        {
            float angleStep = (currentBulletPattern.bulletPatterns[0].endAngle - currentBulletPattern.bulletPatterns[0].startAngle) / currentBulletPattern.bulletPatterns[0].bulletsAmount;
            float angle = currentBulletPattern.bulletPatterns[0].startAngle;

            for(int i = 0; i < currentBulletPattern.bulletPatterns[0].bulletsAmount; i++)
            {
                float bulDirX = transform.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
                float bulDirY = transform.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

                Vector3 bulMoveVector = new Vector3(bulDirX, bulDirY, 0f);
                Vector2 bulDir = (bulMoveVector - transform.position).normalized;

                Debug.Log("Bullet Instance:" + BulletPool.bulletPoolInstance);
                Debug.Log("Bullet: " + BulletPool.bulletPoolInstance.GetBullet());
                GameObject bul = BulletPool.bulletPoolInstance.GetBullet();
                if(bul != null)
                {
                    bul.transform.position = transform.position;
                    bul.transform.rotation = transform.rotation;
                    bul.SetActive(true);
                    bul.GetComponent<Bullet>().SetMoveDirection(bulDir);
                    bul.GetComponent<Bullet>().damage = currentBulletPattern.bulletPatterns[0].damage;

                    if(!IsHost)
                        bul.GetComponent<Bullet>().BulletMoveServerRpc();

                    director.bulletsShot++;
                }

                angle += angleStep;
            }

            lastShotTime = Time.time;
        }

    }

    
}
