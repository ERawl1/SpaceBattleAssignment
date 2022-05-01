using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public float camSpeed = 0.05f;
    public float camZoom = 10f;
    public float camRotSpd = 2;
    public float maxHeight = 45f;
    public float minHeight = 5f;

    Vector2 pos1;
    Vector2 pos2;

    void Start()
    {
        
    }


    void Update()
    {
        FastPan();
        

        //Setting up camera movement using Unity's Hor & Ver input settings
        // WASD / Arrow Keys /
        float horSpd = transform.position.y * camSpeed * Input.GetAxis("Horizontal");
        float verSpd = transform.position.y * camSpeed * Input.GetAxis("Vertical");

        //Using Mouse scroll for zooming in and out
        float scrollSpd = Mathf.Log(transform.position.y) * -camZoom * Input.GetAxis("Mouse ScrollWheel");

        //Limiting the camera bounds for scrolling
        if ((transform.position.y >= maxHeight) && (scrollSpd > 0))
        {
            scrollSpd = 0;
        }
        else if ((transform.position.y <= minHeight) && (scrollSpd < 0))
        {
            scrollSpd = 0;
        }

        if ((transform.position.y + scrollSpd) > maxHeight)
        {
            scrollSpd = maxHeight - transform.position.y;
        }
        else if ((transform.position.y + scrollSpd) < minHeight)
        {
            scrollSpd = minHeight - transform.position.y;
        }
    

    //Now for vertical movement on the Y axis
    Vector3 vertMove = new Vector3(0, scrollSpd, 0);
        //Lateral movement
        Vector3 horiMove = horSpd * transform.right;

        //Using vector projection to make sure that camera can move forward properly despite being angled
        Vector3 forwardMove = transform.forward;
        forwardMove.y = 0;
        forwardMove.Normalize();
        forwardMove *= verSpd;

        //Combining everything so far to make move
        Vector3 move = vertMove + horiMove + forwardMove;
        transform.position += move;

        CamRotation();
    }

    void FastPan()
    {
        //If shift is pressed camera speeds are doubled, if not pressed then they are halved
        if (Input.GetKey(KeyCode.LeftShift))
        {
            camSpeed = 0.1f;
            camZoom = 20f;
        }
        else
        {
            camSpeed = 0.05f;
            camZoom = 10f;
        }
    }

    void CamRotation()
    {
        //Middle Mouse Button is pressed
        if (Input.GetMouseButtonDown(2))
        {
            Debug.Log("Pressed middle");
            pos1 = Input.mousePosition;
        }
        //Middle Mouse is held
        if (Input.GetMouseButton(2))
        {
            pos2 = Input.mousePosition;

            float dx = (pos2 - pos1).x * camRotSpd;
            float dy = (pos2 - pos1).y * camRotSpd;

            transform.rotation *= Quaternion.Euler(new Vector3(0, dx, 0));
            transform.GetChild(0).transform.rotation *= Quaternion.Euler(new Vector3(-dy, 0, 0));

            pos1 = pos2;
        }
    }
}
