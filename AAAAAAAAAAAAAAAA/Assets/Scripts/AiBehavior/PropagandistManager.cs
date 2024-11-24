using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PropagandistManager : MonoBehaviour
{
    [Tooltip("How long you have to look at this enemy before propaganda starts")]
    public float lookAtTimePropaganda = 0f;
    [Tooltip("length of a propaganda flash")]
    public float propagandaIntervalSec = 3f;
    public float fadeOutTime = 1.5f;
    [Tooltip("How long you have to look at this enemy before death")]
    public float lookAtTimeDeath = 20f;
    public float propagandaSwapInterval = 5f;
    public float maxPropagandaOpacity = 0.35f;
    public List<Sprite> propagandaImages = new List<Sprite>();
    public Image propagandaImage;
    public bool isVisible = false;
    public List<Propagandist> propagang = new List<Propagandist>();

    [SerializeField]
    private bool lastIsVisible = false;
    [SerializeField]
    private float timeSinceStartLook = 0f;
    [SerializeField]
    private float timeSinceEndLook = 0f;
    [SerializeField]
    private float timeSinceSwap = 0f;
    [SerializeField]
    private int propagandaImageIndex = 0;
    [SerializeField]
    private float currFadeInOpacity = 0f;
    private GameObject player;
    

    void Start()
    {
        propagandaImage.sprite = propagandaImages[0];
        player = GameObject.FindGameObjectsWithTag("Player").FirstOrDefault();
    }

    void Update()
    {
        bool currVisibility = false;
        foreach(var propa in propagang)
        {
            currVisibility |= propa.IsVisibleByPlayer();
        }
        if(player.GetComponent<PlayerHealth>().health <= 0)
        {
            currVisibility = true;
        }

        if (currVisibility && lastIsVisible != currVisibility)
        {
            timeSinceStartLook = 0f;
        }
        else if (!currVisibility && lastIsVisible != currVisibility)
        {
            // Just looked away, so fade out current visibility
            timeSinceEndLook = 0f;
        }

        if (currVisibility)
        {
            if (timeSinceStartLook >= lookAtTimeDeath)
            {
                // Time to die
                PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
                playerHealth.TakeDamage(9999);
            }
            else if (timeSinceStartLook >= lookAtTimePropaganda)
            {
                float step = (timeSinceStartLook - lookAtTimePropaganda) / (lookAtTimeDeath - lookAtTimePropaganda);
                currFadeInOpacity = Mathf.Lerp(0, maxPropagandaOpacity, step);
                propagandaImage.color = new Color(0.5f, 0.5f, 0.5f, currFadeInOpacity);
            }

            // Handle swap time
            if (timeSinceSwap >= propagandaSwapInterval)
            {
                IncrementPropagandaIndex();
                propagandaImage.sprite = propagandaImages[propagandaImageIndex];
                timeSinceSwap = 0;
            }

            //timeSinceSwap += Time.deltaTime;
            timeSinceStartLook += Time.deltaTime;
        }
        else if (propagandaImage.color.a > 0)
        {
            propagandaImage.color = new Color(0.5f, 0.5f, 0.5f, Mathf.Lerp(currFadeInOpacity, 0, timeSinceEndLook / fadeOutTime));
            if (timeSinceEndLook < fadeOutTime)
            {
                timeSinceEndLook += Time.deltaTime;
            }
        }

        lastIsVisible = currVisibility;
    }

    private int IncrementPropagandaIndex()
    {
        propagandaImageIndex = (propagandaImageIndex + 1) % propagandaImages.Count;
        return propagandaImageIndex;
    }
}
