using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public float rotationSpeed = 50f;

   void Update()
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
