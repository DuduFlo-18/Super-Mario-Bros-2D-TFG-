using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Plant : Enemy
{
    public override void HitFireBall()
    {
        Destroy(transform.parent.gameObject);
    }
    public override void HitStarman()
    {
        Destroy(transform.parent.gameObject);
    }

    public override void HitRollingShell()
    {
        Destroy(transform.parent.gameObject);
    }
}
