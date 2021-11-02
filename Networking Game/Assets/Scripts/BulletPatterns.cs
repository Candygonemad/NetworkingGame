using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BulletPattern
{
    public float timeBetweenShots;
    public int damage;

    public float startAngle;
    public float endAngle;

    public int bulletsAmount;
}

public class BulletPatterns : MonoBehaviour
{
    public List<BulletPattern> bulletPatterns;

    // Start is called before the first frame update
    void Start()
    {
        bulletPatterns = new List<BulletPattern>();
        BulletPattern pattern1 = new BulletPattern();
        pattern1.timeBetweenShots = .3f;
        pattern1.damage = 1;
        pattern1.startAngle = 0f;
        pattern1.endAngle = 0f;
        pattern1.bulletsAmount = 1;
        bulletPatterns.Add(pattern1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
