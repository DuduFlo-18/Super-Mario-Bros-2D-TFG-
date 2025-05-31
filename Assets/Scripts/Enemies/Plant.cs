using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

// Este script define el comportamiento de la planta, la planta solo se mueve asi que eso queda reflejado en el Script ShowAndHide.cs 
public class Plant : Enemy
{
    public override void HitFireBall()
    {
        Dead();
        Destroy(transform.parent.gameObject);
    }
    public override void HitStarman()
    {
        Dead();
        Destroy(transform.parent.gameObject);
    }

    public override void HitRollingShell()
    {
        Dead();
        Destroy(transform.parent.gameObject);
    }
}
