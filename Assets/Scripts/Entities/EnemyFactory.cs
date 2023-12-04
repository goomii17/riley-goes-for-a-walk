using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum EnemyType
// {
//     Hazmat,
//     Swat,
//     BlazeSentinel,
// }

public static class EnemyFactory
{
    public static Entity CreateEnemy(int enemyIndex, GameObject gameObject)
    {
        switch (enemyIndex)
        {
            case 0:
                return new Hazmat(gameObject);
            // case 1:
            //     return new Swat(gameObject);
            // case 2:
            //     return new BlazeSentinel(gameObject);
            default:
                return null;
        }
    }
}
