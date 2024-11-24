using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BellWhip : MonoBehaviour
{
    public Transform origin; // Custom origin point (assign in the Inspector)
    public float rayLength = 10f; // Maximum length of the ray
    public float drawSpeed = 5f; // Speed at which the line is drawn
    public float retractSpeed = 5f; // Speed at which the line is retracted

    private Vector3 endPoint; // Calculated endpoint of the ray
    private Vector3 currentEndPoint; // Current endpoint of the line during drawing
    private bool isDrawing = false; // Whether the line is being drawn
    private bool isRetracting = false; // Whether the line is being retracted
    bool canwhip;
    public Animator anim;

    private LineRenderer lineRenderer; // LineRenderer to draw the line

    public PullIn currentpullin;
    public AudioClip grappleclip;


    void Start()
    {
        // Initialize the LineRenderer
        lineRenderer= GetComponent<LineRenderer>();
        canwhip = true;
    }

    void Update()
    {
        // Start drawing when right mouse button is clicked
        if (Input.GetKeyDown(KeyCode.Mouse1) && origin != null && canwhip == true)
        {
            StartDrawing();
            AudioSource.PlayClipAtPoint(grappleclip, transform.position, 0.4f);
            anim.Play("Whip_active", -1, 0f);
            canwhip = false;
        }

        // Handle the drawing process
        if (isDrawing)
        {
            DrawLine();
        }

        // Handle the retraction process
        if (isRetracting)
        {
            RetractLine();
        }
    }

    // Initializes the drawing process
    void StartDrawing()
    {
        // Cast a ray to determine the endpoint
        Ray ray = new Ray(origin.position, origin.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayLength))
        {
            // Set the endpoint to the hit point
            if (hit.collider.gameObject.GetComponent<PullIn>() != false)
            {
                currentpullin = hit.collider.gameObject.GetComponent<PullIn>();
            }
            endPoint = hit.point;
            Debug.Log($"Ray hit: {hit.collider.name}");
        }
        else
        {
            // Set the endpoint to the maximum ray length
            endPoint = origin.position + origin.forward * rayLength;
        }

        // Initialize line drawing
        currentEndPoint = origin.position;
        lineRenderer.SetPosition(0, origin.position);
        lineRenderer.SetPosition(1, origin.position);
        isDrawing = true;
        isRetracting = false;
    }

    // Handles the line drawing process
    void DrawLine()
    {
        // Update the origin point to follow the transform
        Vector3 startPoint = origin.position;

        // Incrementally draw the line towards the endpoint
        Vector3 direction = (endPoint - startPoint).normalized;
        currentEndPoint += direction * drawSpeed * Time.deltaTime;

        // Update the LineRenderer
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, currentEndPoint);

        // Stop drawing when the line reaches the endpoint
        if (Vector3.Distance(currentEndPoint, startPoint) >= Vector3.Distance(endPoint, startPoint))
        {
            currentEndPoint = endPoint;
            isDrawing = false; // Stop further updates
            OnLineComplete(); // Trigger custom logic when drawing is complete
        }
    }

    // Handles the line retraction process
    void RetractLine()
    {
        // Update the origin point to follow the transform
        Vector3 startPoint = origin.position;

        // Incrementally retract the line back to the start point
        Vector3 direction = (startPoint - currentEndPoint).normalized;
        currentEndPoint += direction * retractSpeed * Time.deltaTime;

        // Update the LineRenderer
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, currentEndPoint);

        // Stop retracting when the line reaches the start point
        if (Vector3.Distance(currentEndPoint, startPoint) <= 0.1f)
        {
            currentEndPoint = startPoint;
            lineRenderer.positionCount = 0;
            lineRenderer.positionCount = 2;
            canwhip = true;
            isRetracting = false; // Stop further updates

            Debug.Log("Line fully retracted!");
        }
    }

    // Custom function triggered when the line finishes drawing
    void OnLineComplete()
    {
        Debug.Log("Line drawing complete!");
        // Add your custom logic here
        if (currentpullin != null)
        {
            currentpullin.LaunchTowardsPlayer();
        }
        // Start retracting the line
        isRetracting = true;
        currentpullin = null;
    }
}
