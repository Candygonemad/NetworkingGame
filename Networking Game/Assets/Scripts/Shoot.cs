using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public BulletPatterns currentBulletPattern;
    private float lastShotTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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

                GameObject bul = BulletPool.bulletPoolInstance.GetBullet();
                if(bul != null)
                {
                    bul.transform.position = transform.position;
                    bul.transform.rotation = transform.rotation;
                    bul.SetActive(true);
                    bul.GetComponent<Bullet>().SetMoveDirection(bulDir);
                    bul.GetComponent<Bullet>().damage = currentBulletPattern.bulletPatterns[0].damage;
                }

                angle += angleStep;
            }

            lastShotTime = Time.time;
        }
    }
}
