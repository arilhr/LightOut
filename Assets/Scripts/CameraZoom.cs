using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    private Vector2 target;

    private Camera cam;

    private bool zoomActive = false;

    private Vector3 firstPosition;
    private float firstSize;

    public List<Room> rooms = new List<Room>();

    private float doubleClickTime = 0.25f;
    private float lastClickTime;

    private void Start()
    {
        cam = gameObject.GetComponent<Camera>();
        firstPosition = cam.transform.position;
        firstSize = cam.orthographicSize;
        target = firstPosition;
    }

    private void Update()
    {
        if (!zoomActive)
        {
            Zoom();
        }

        //detect double click
        if (Input.GetMouseButtonDown(0))
        {
            float offsetTime = Time.time - lastClickTime;

            if(offsetTime < doubleClickTime)
            { 
                cam.orthographicSize = firstSize;
                cam.transform.position = firstPosition;
                zoomActive = false;
            }

            lastClickTime = Time.time;
        }
    }

    private Room TargetRoom(Vector2 target)
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(target), Vector2.zero);

            if (hit.collider != null)
            {
                Room room = hit.collider.GetComponent<Room>();
                if(room != null)
                {
                    return room;
                }
            }
        }

        return null;
    }

    private Vector2 SetTargetPosition(Vector2 target)
    {
        Vector2 t = Vector2.zero;

        foreach (Room r in rooms)
        {
            if (r == TargetRoom(target))
            {
                t = r.transform.position;
            }
        }
        
        return t;
    }

    private void Zoom()
    {
        target = SetTargetPosition(Input.mousePosition);

        if(target != Vector2.zero)
        {
            if (Input.GetMouseButtonDown(0))
            {
                cam.transform.position = new Vector3(target.x, target.y, cam.transform.position.z);
                cam.orthographicSize = 1.5f;
                zoomActive = true;
            }
        }
    }
}
