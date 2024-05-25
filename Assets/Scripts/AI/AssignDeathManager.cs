using UnityEngine;

public class AssignDeathManager : MonoBehaviour
{
    public DeathManager deathManager;

    void Start()
    {
        // Find all objects with the EnemyAI script
        EnemyAI[] enemyAIs = FindObjectsOfType<EnemyAI>();
        Medkit[] medkits = FindObjectsOfType<Medkit>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // Assign the DeathManager reference to each EnemyAI
        foreach (EnemyAI enemyAI in enemyAIs)
        {
            enemyAI.deathManager = deathManager;
            enemyAI.m_player = player;
        }

        foreach (Medkit medkit in medkits) {
            medkit.deathManager = deathManager;
        }
    }
}

