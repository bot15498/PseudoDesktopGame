using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public int SceneToLoad;
    // Start is called before the first frame update
    void Start()
    {
        // Since this script is on the main menu,
        // use this to load Audio Manager at start of game
        SceneManager.LoadScene("AudioManager", LoadSceneMode.Additive);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void restartScene()
    {
        
        StartCoroutine(Example2());
    }


    public void loadScene()
    {
       
        StartCoroutine(Example());

    }

    IEnumerator Example()
    {

        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneToLoad);

    }

    IEnumerator Example2()
    {

        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
