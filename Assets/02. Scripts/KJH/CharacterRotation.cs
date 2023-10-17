using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotation : MonoBehaviour
{
    private float startingMousePos;
    private Vector3 startingEulerAngles;

    public float rotationSpeed;
    public Vector3 rotationVector;
    public Vector2 mouseAxis;

    public Vector3 stepAngle;
    private void OnMouseDown()
    {
        print("어라");
        startingMousePos = (Input.mousePosition * mouseAxis).magnitude;
        startingEulerAngles = transform.rotation.eulerAngles;
    }

    private void OnMouseDrag()
    {
        print("어라22");
        float dis = startingMousePos - (Input.mousePosition * mouseAxis).magnitude;

        Vector3 newEulerAngles = startingEulerAngles + rotationVector * rotationSpeed * dis;

        newEulerAngles = new Vector3(Mathf.RoundToInt(newEulerAngles.x / stepAngle.x) * stepAngle.x,
            Mathf.RoundToInt(newEulerAngles.y / stepAngle.y) * stepAngle.y,
            Mathf.RoundToInt(newEulerAngles.z / stepAngle.z) * stepAngle.z);

        transform.rotation = Quaternion.Euler(newEulerAngles);
    }
}
