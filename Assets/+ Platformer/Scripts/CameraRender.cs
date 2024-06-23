using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRender : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SetRenderers(collision, true);
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    SetRenderers(collision, true);
    //}

    private void OnTriggerExit2D(Collider2D collision)
    {
        SetRenderers(collision, false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        SetRenderers(collision, true);
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    SetRenderers(collision, true);
    //}

    private void OnCollisionExit2D(Collision2D collision)
    {
        SetRenderers(collision, false);
    }

    void SetRenderers(Collider2D collider, bool setActive)
    {
        var renderOnCamera = collider.gameObject.GetComponent<RenderOnCamera>();
        if (renderOnCamera != null)
        {
            renderOnCamera.SetRenderers(setActive);
        }
    }

    void SetRenderers(Collision2D collider, bool setActive)
    {
        var renderOnCamera = collider.gameObject.GetComponent<RenderOnCamera>();
        if (renderOnCamera != null)
        {
            renderOnCamera.SetRenderers(setActive);
        }
    }

}
