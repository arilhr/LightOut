using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Vector2 target;

    private Camera cam;

    public bool zoomActive = false;

    private Vector3 firstPosition; 

    private void Start()
    {
        cam = Camera.main;
        firstPosition = cam.transform.position;
    }

    private void Update()
    {
        GetTarget();

        if (zoomActive)
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 1, 0.1f);
            cam.transform.position = Vector2.Lerp(cam.transform.position, target, 0.1f);
        }
        else
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, 5, 0.05f);
            cam.transform.position = Vector3.Lerp(cam.transform.position, firstPosition, 0.05f);
        }
        
    }

    private void GetTarget()
    {
        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        
        if(hit.collider != null)
        {
            zoomActive = true;
            target = hit.collider.transform.position;
        }
    }

}
