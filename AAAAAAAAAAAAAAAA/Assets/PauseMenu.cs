using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    // Start is called before the first frame update
    bool cursormodeunlocked = false;
    public GameObject pauseScreen;
    public GameObject pausetext;
    public GameObject playtext;
    public GameObject stopText;
    public Image staticimage;
    private bool playerCanRestart = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (playerCanRestart)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                cursormodeunlocked = !cursormodeunlocked;
            }
        }

        if (!playerCanRestart)
        {
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
                stopText.SetActive(false);
            }
            else
            {
                Time.timeScale = 1;
                pausetext.SetActive(false);
                playtext.SetActive(true);
                stopText.SetActive(false);
            }
        }
    }

    public IEnumerator FadeInStatic(float fadeInDuration)
    {
        Time.timeScale = 0.1f;
        Color initialColor = staticimage.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);

        float elapsedTime = 0f;
        while (elapsedTime < fadeInDuration)
        {
            if (elapsedTime > fadeInDuration / 2)
            {
                // Enable the stop game object
                playerCanRestart = true;
                stopText.SetActive(true);
                pausetext.SetActive(false);
                playtext.SetActive(false);
            }
            staticimage.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeInDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
