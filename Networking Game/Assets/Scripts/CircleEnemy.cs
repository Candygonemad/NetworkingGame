using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleEnemy : Enemy
{
    [HideInInspector]
    public Vector2 radius;

    [HideInInspector]
    public bool clockwise;

    public float angle;

    public override void Start()
    {
        angle = 2 * Mathf.PI / timeAlive;
    }
    // Update is called once per frame
    protected override void Update()
    {
        int clockwiseInt;

        float x;
        float y;

        if (clockwise)
        {
            clockwiseInt = 1;
            
            x = Mathf.Cos(Mathf.PI + (clockwiseInt * angle));
            y = Mathf.Sin(Mathf.PI + (clockwiseInt * angle));
        }
        else
        {
            clockwiseInt = -1;

            x = Mathf.Cos(clockwiseInt * angle) * radius.x;
            y = Mathf.Sin(clockwiseInt * angle) * radius.y;
        }

        angle += Time.deltaTime;

        direction = new Vector3(x, y, 0);
        transform.position += speed * direction * Time.deltaTime;
        Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
    }
}
