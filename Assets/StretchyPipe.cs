using UnityEngine;

[ExecuteAlways]
public class StretchyPipe : MonoBehaviour
{
    [Header("Assign your two endpoint spheres here")]
    public Transform endPointA;
    public Transform endPointB;

    // we'll keep the radius (x/z) at whatever you set in the Inspector
    float radiusX, radiusZ;

    void Awake()
    {
        // cache whatever radius you set on the cylinder
        radiusX = transform.localScale.x;
        radiusZ = transform.localScale.z;
    }

    void LateUpdate()
    {
        if (endPointA == null || endPointB == null)
            return;

        Vector3 posA = endPointA.position;
        Vector3 posB = endPointB.position;
        Vector3 dir  = posB - posA;
        float  len   = dir.magnitude;

        // 1) move to midpoint
        transform.position = (posA + posB) * 0.5f;

        // 2) point the cylinder’s up‐axis along the line between A & B
        if (len > 0.0001f)
            transform.up = dir / len;

        // 3) stretch its Y‐scale so that its world‐height = len
        //    (Unity’s default cylinder is 2 units tall at scale = 1)
        transform.localScale = new Vector3(
            radiusX,
            len * 0.5f,
            radiusZ
        );
    }
}
