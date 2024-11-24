using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialAdvance : MonoBehaviour
{
    // Start is called before the first frame update

    public Sprite[] tutorialsprites;
    public bool dotutorial;
    public Image imagea;
    int currentindex;
    void Start()
    {
        dotutorial = true;
        currentindex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        imagea.sprite = tutorialsprites[currentindex];

        if(dotutorial == true)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                currentindex += 1;
            }
            if(currentindex == 6)
            {
                SceneManager.LoadScene(1);
            }
            
        }
    }
}
