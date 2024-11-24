using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyManager : MonoBehaviour
{
    public GameObject intro;
    public List<AiBehaviorBase> enemies;
    private bool gonext = false;

    void Start()
    {
        // whoa
        enemies = FindObjectsOfType<AiBehaviorBase>().ToList();
    }

    void Update()
    {
        if(!gonext && enemies.Count == 0)
        {
            StartCoroutine(GoToNextScene());
            gonext = true;
        }
    }

    public void SpawnEnemy()
    {
        // ???/
    }

    public void ForceStunStateForAll(EnemyAiStunState state)
    {
        foreach(var ai in enemies)
        {
            if(((int)ai.stunState) <= ((int)state))
            {
                ai.TempSetStunState(state);
            }
        }
    }

    public void RestoreStunStateForAll()
    {
        foreach (var ai in enemies)
        {
            ai.TempRestoreStunState();
        }
    }

    private IEnumerator GoToNextScene()
    {
        float timer = 0f;
        float maxtime = 2f;
        intro.SetActive(true);
        while (timer < maxtime)
        {

            timer += Time.deltaTime;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        yield return null;
    }
}
