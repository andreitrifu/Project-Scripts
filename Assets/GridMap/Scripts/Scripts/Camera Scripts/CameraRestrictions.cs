using UnityEngine;//

public class CameraRestrictions : MonoBehaviour
{
    public Transform target; 
    public float minX = 2000f; 
    public float maxX = 5000f; 
    public float minZ = 2000f; 
    public float maxZ = 5000f; 

    void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 currentPosition = transform.position;

        float clampedX = Mathf.Clamp(target.position.x, minX, maxX);
        float clampedZ = Mathf.Clamp(target.position.z, minZ, maxZ);

        transform.position = new Vector3(clampedX, currentPosition.y, clampedZ);
    }
}
