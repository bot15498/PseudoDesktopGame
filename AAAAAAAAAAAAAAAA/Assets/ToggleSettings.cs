using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSettings : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject objectToToggle;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleObject()
    {
        objectToToggle.SetActive(!objectToToggle.activeSelf);
    }
}
