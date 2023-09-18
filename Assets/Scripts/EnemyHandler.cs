using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    [SerializeField] GameObject enemyDamageVFX;
    [SerializeField] GameObject enemyExplosionVFX;
    [SerializeField] int pointsAwarded = 10;
    [SerializeField] int enemyHP = 3;

    Rigidbody enemyRigidBody;
    ScoreBoard scoreBoard;
    GameObject parentGameObject;

    void Start()
    {
        scoreBoard = FindObjectOfType<ScoreBoard>();
        parentGameObject = GameObject.FindWithTag("Spawn At Runtime");
        AddRigidBody();
    }

    void AddRigidBody()
    {
        enemyRigidBody = gameObject.AddComponent<Rigidbody>();
        enemyRigidBody.useGravity = false;
        enemyRigidBody.isKinematic = true;
    }

    void OnParticleCollision(GameObject other)
    {
        DamageEnemy();
    }

    void IncreasePlayerScore()
    {
        scoreBoard.IncreaseScore(pointsAwarded);
    }

    void DamageEnemy()
    {
        GameObject vfx = Instantiate(enemyDamageVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;
        
        enemyHP --;
        if (enemyHP <= 0)
        {
            KillEnemy();
        }
    }

    void KillEnemy()
    {
        IncreasePlayerScore();
        GameObject vfx = Instantiate(enemyExplosionVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;
        Destroy(this.gameObject);
    }
}
