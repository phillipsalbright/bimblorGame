using System;
using UnityEngine;

/**
 * Makes the arm move with the mouse on the player prefab.
 */
public class ArmMovementScript : MonoBehaviour
{
    public Vector3 mouse_pos;
    
    /** The arm location (shoulder joint). Used to see if mouse too close to center of player. */
    public Transform target;

    /** Base of the wand (where hand touches it) */
    public Transform target2;

    public Vector3 object_pos;
    
    public float angle;

    public float yRotation;
 
    void FixedUpdate()
    {
        /**
         * only necessary if using Update()
        if (Time.timeScale == 0)
        {
            return
        }
        */
        yRotation = GetComponentInParent<Transform>().rotation.eulerAngles.y;
        mouse_pos = Input.mousePosition;
        mouse_pos.z = 12f; //The distance between the camera and object
        object_pos = GameObject.Find("MainCamera").GetComponent<Camera>().WorldToScreenPoint(target2.position);
        mouse_pos.x -= object_pos.x;
        mouse_pos.y -= object_pos.y;
        Vector3 object_pos2 = GameObject.Find("MainCamera").GetComponent<Camera>().WorldToScreenPoint(target.position);
        Vector3 mouse_pos2 = Input.mousePosition;
        mouse_pos2.x -= object_pos2.x;
        mouse_pos2.y -= object_pos2.y;
        if ((Math.Abs(mouse_pos2.x) + Math.Abs(mouse_pos2.y)) >= 60)
        {
            angle = (Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg) - 90;
            //angle -= 90;
            if (yRotation == 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            } else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, -angle));
            }
        }
    }
}
