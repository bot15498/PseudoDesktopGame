using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    bool cursormodeunlocked;
    public GameObject pauseScreen;
    public GameObject pausetext;
    public GameObject playtext;
    void Start()
    {
        cursormodeunlocked = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            cursormodeunlocked = !cursormodeunlocked;

        }

        if (cursormodeunlocked == true)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        pauseScreen.SetActive(cursormodeunlocked);

        if (cursormodeunlocked == true)
        {
            Time.timeScale = 0;
            pausetext.SetActive(true);
            playtext.SetActive(false);
        }
        else
        {
            Time.timeScale = 1;
            pausetext.SetActive(false);
            playtext.SetActive(true);
        }


    }
}
