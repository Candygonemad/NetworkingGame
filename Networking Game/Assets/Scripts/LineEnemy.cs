using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineEnemy : Enemy
{
    [HideInInspector]
    public LineDirection lineDirection;

    // Start is called before the first frame update
    public override void Start()
    {
        if (lineDirection == LineDirection.Down)
            direction = new Vector3(0, -1, 0);
        else if (lineDirection == LineDirection.Left)
            direction = new Vector3(-1, 0, 0);
        else if (lineDirection == LineDirection.Right)
            direction = new Vector3(1, 0, 0);
    }
}
