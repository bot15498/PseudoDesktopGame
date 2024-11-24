using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject thingtotoggle;
    void Start()
    {
        thingtotoggle.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enableIcon()
    {
        thingtotoggle.SetActive(true);
    }

    public void disableIcon()
    {
        thingtotoggle.SetActive(false);
    }

    void OnPointerEnter(PointerEventData eventData)
    {
        thingtotoggle.SetActive(true);

    }

    void OnPointerExit(PointerEventData eventData)
    {
        thingtotoggle.SetActive(false);
    }
}
