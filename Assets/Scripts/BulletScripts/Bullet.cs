/*****************************************************************************
// File Name :         Bullet.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : ADD BRIEF DESCRIPTION OF THE FILE HERE
*****************************************************************************/
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [Tooltip("The travel speed of the bullet.")]
    [SerializeField] protected float speed = 5f;

    // The damage the bullet inflicts.
    protected int characterDamage = 1;

    // The amount of damage this bullet does to cover.
    protected int coverDamage = 1;

    // If this bullet belongs to an enemy, it will not damage other enemies.
    protected bool isEnemyBullet = false;

    public void Init(int characterDamage, int coverDamage, bool isEnemyBullet)
    {
        this.characterDamage = characterDamage;
        this.coverDamage = coverDamage;
        this.isEnemyBullet = isEnemyBullet;
    }
}
