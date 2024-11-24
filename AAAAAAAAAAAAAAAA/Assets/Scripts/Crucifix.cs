using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crucifix : MonoBehaviour
{
    public float crucifixTime = 0f;
    public float maxCrucifixTime = 7f;
    public float timeToCrucificTime = 0.5f;
    public Animator anim;

    [SerializeField]
    private bool isCrossOut = false;
    private EnemyManager enemyManager;
    private bool lastCrossOut = false;


    void Start()
    {
        crucifixTime = maxCrucifixTime;
        enemyManager = FindObjectOfType<EnemyManager>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            anim.SetBool("isKeyPressed", true);
            isCrossOut = true;
        }
        else if (Input.GetKeyUp(KeyCode.F))
        {
            anim.SetBool("isKeyPressed", false);
            enemyManager.RestoreStunStateForAll();
            isCrossOut = false;
        }
        else if(isCrossOut && crucifixTime <= 0)
        {
            anim.SetBool("isKeyPressed", false);
            crucifixTime = 0f;
            enemyManager.RestoreStunStateForAll();
            isCrossOut = false;
        }

        if(isCrossOut && lastCrossOut != isCrossOut)
        {
            enemyManager.ForceStunStateForAll(EnemyAiStunState.RunAway);
        }

        if(isCrossOut)
        {
            // deplt
            crucifixTime -= Time.deltaTime;
        }
        else
        {
            // recharge
            crucifixTime = Mathf.Min(maxCrucifixTime, crucifixTime + Time.deltaTime * timeToCrucificTime);
        }

        lastCrossOut = isCrossOut;
    }
}
