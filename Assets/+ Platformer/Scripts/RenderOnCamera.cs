using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderOnCamera : MonoBehaviour
{
    [SerializeField] GameObject renderers;
    Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        //SetRenderers(false);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if(collision.gameObject.CompareTag("Camera"))
    //    {
    //        SetRenderers(true);
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if(collision.gameObject.CompareTag("Camera"))
    //    {
    //        SetRenderers(true);
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Camera"))
    //    {
    //        SetRenderers(false);
    //    }
    //}

    public void SetRenderers(bool setActive)
    {
        if (setActive)
            Debug.Log($"Activiating {setActive}", gameObject);
        if (renderer != null && renderer.enabled == !setActive)
            renderer.enabled = setActive;
        if (renderers != null && renderers.activeSelf == !setActive)
            renderers.SetActive(setActive);
    }

}
