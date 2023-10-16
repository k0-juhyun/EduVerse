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

    [Min(1)]
    public Vector3 stepAngle;
    private void OnMouseDown()
    {
        startingMousePos = (Input.mousePosition * mouseAxis).magnitude;
        startingEulerAngles = transform.rotation.eulerAngles;
    }

    private void OnMouseDrag()
    {
        float dis = startingMousePos - (Input.mousePosition * mouseAxis).magnitude;
        Vector3 newEulerAngles = startingEulerAngles + rotationVector * rotationSpeed * dis;
        transform.rotation = Quaternion.Euler(newEulerAngles);
        newEulerAngles = new Vector3(Mathf.RoundToInt(newEulerAngles.x / stepAngle.x) * stepAngle.x,
            Mathf.RoundToInt(newEulerAngles.y / stepAngle.y) * stepAngle.y,
            Mathf.RoundToInt(newEulerAngles.z / stepAngle.z) * stepAngle.z);
        transform.rotation = Quaternion.Euler(newEulerAngles);
    }
}
