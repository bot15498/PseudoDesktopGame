using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public List<AiBehaviorBase> enemies;

    void Start()
    {
        // whoa
        enemies = FindObjectsOfType<AiBehaviorBase>().ToList();
    }

    void Update()
    {
        
    }

    public void SpawnEnemy()
    {
        // ???/
    }

    public void ForceStunStateForAll(EnemyAiStunState state)
    {
        foreach(var ai in enemies)
        {
            ai.stunState = state;
        }
    }
}
