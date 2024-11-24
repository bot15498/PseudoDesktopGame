using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Propagandist : AiBehaviorBase
{
    [Tooltip("How long you have to look at this enemy before propaganda starts")]
    public float lookAtTimePropaganda = 2f;
    [Tooltip("length of a propaganda flash")]
    public float propagandaIntervalSec = 3f;
    [Tooltip("How long you have to look at this enemy before death")]
    public float lookAtTimeDeath = 30f;
    public List<Sprite> propagandaImages = new List<Sprite>();
    public Image propagandaImage;

    [SerializeField]
    private bool isVisible = false;
    private bool lastIsVisible = false;
    private float timeSinceStartLook = 0f;
    private float timeSinceEndLook = 0f;
    private int propagandaImageIndex = 0;



    new void Start()
    {

        base.Start();
    }

    new void Update()
    {
        bool currVisibility = IsVisibleByPlayer();
        if (currVisibility && lastIsVisible != currVisibility)
        {
            timeSinceStartLook = 0f;
        }
        else if (!currVisibility && lastIsVisible != currVisibility)
        {
            // Just looked away, so fade out current visibility
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
                // Start fading in the propaganda image in and out. 
                propagandaImage.color = new Color(0.5f, 0.5f, 0.5f, CalculateOpacity(timeSinceStartLook - lookAtTimePropaganda));
            }
            timeSinceStartLook += Time.deltaTime;
        }
        else if (propagandaImage.color.a > 0)
        {
            // Make sure that the propaganda goes away eventually
            propagandaImage.color = new Color(0.5f, 0.5f, 0.5f, Mathf.Lerp(CalculateOpacity(timeSinceStartLook - lookAtTimePropaganda), 0, timeSinceEndLook / propagandaIntervalSec));
            timeSinceEndLook += Time.deltaTime;
            if (propagandaImage.color.a <= 0.1f)
            {
                propagandaImage.color = new Color(0.5f, 0.5f, 0.5f, 0f);
            }
        }
        lastIsVisible = currVisibility;

        base.Update();
    }

    new void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override bool IsObjectInAttackRange()
    {
        return false;
    }

    public override void Attack()
    {
        // do nothing.
    }

    private bool IsVisibleByPlayer()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }

    private int IncrementPropagandaIndex()
    {
        propagandaImageIndex = propagandaImageIndex + 1 % propagandaImages.Count;
        return propagandaImageIndex;
    }

    private float CalculateOpacity(float timestep)
    {
        float toreturn = -Mathf.Cos(timestep / propagandaIntervalSec * Mathf.PI * 2) / 2 + 0.25f;
        if (toreturn <= 0.1f)
        {
            // If we are close enough, swap images
            IncrementPropagandaIndex();
            propagandaImage.sprite = propagandaImages[propagandaImageIndex];
            toreturn = 0;
        }
        return toreturn;
    }
}
