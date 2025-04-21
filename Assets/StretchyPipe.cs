using UnityEngine;

[ExecuteAlways]
public class StretchyPipe : MonoBehaviour
{
    [Header("Endpoints")]
    public Transform endPointA;
    public Transform endPointB;

    [Header("Thickness (radius)")]
    public float radiusX = 0.1f;
    public float radiusZ = 0.1f;

    [Header("Pipe Colors (RGBA)")]
    [Tooltip("When neither end is snapped")]
    [ColorUsage(true, true)] public Color colorNone     = new Color(1,1,1, 0.5f);
    [Tooltip("When exactly one end is snapped")]
    [ColorUsage(true, true)] public Color colorOneEnd   = new Color(1,1,0, 0.5f);
    [Tooltip("When both ends are snapped")]
    [ColorUsage(true, true)] public Color colorBothEnds = new Color(0,1,0, 0.5f);

    MeshRenderer meshRenderer;
    Material     instanceMat;
    SnapToCubes  snapA, snapB;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer)
        {
            // 1) Clone the shared material so we don't leak or affect other objects
            instanceMat = new Material(meshRenderer.sharedMaterial);
            meshRenderer.material = instanceMat;

            // 2) Switch it into transparent mode (Standard shader example)
            instanceMat.SetOverrideTag("RenderType", "Transparent");
            instanceMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            instanceMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            instanceMat.SetInt("_ZWrite", 0);
            instanceMat.DisableKeyword("_ALPHATEST_ON");
            instanceMat.EnableKeyword("_ALPHABLEND_ON");
            instanceMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            instanceMat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }

        if (endPointA) snapA = endPointA.GetComponent<SnapToCubes>();
        if (endPointB) snapB = endPointB.GetComponent<SnapToCubes>();
    }

    void LateUpdate()
    {
        if (endPointA == null || endPointB == null) return;

        // --- Stretch & orient the pipe ---
        Vector3 posA = endPointA.position;
        Vector3 posB = endPointB.position;
        Vector3 dir  = posB - posA;
        float  len   = dir.magnitude;

        // 1) Position at the midpoint
        transform.position = (posA + posB) * 0.5f;

        // 2) Rotate so up‑axis points from A to B
        if (len > 0.0001f)
            transform.up = dir / len;

        // 3) Compute local Y‑scale to exactly match world‑space length,
        //    compensating for any parent Y‑scale:
        float parentScaleY = transform.parent
                           ? transform.parent.lossyScale.y
                           : 1f;
        float halfLenLocal = (len * 0.5f) / parentScaleY;

        // 4) Apply corrected scale (radiusX/Z unchanged)
        transform.localScale = new Vector3(
            radiusX,
            halfLenLocal,
            radiusZ
        );

        // --- Color based on snap state ---
        bool aSn = snapA != null && snapA.IsSnapped;
        bool bSn = snapB != null && snapB.IsSnapped;

        Color target = (!aSn && !bSn) ? colorNone
                      : (aSn && bSn) ? colorBothEnds
                                     : colorOneEnd;

        if (instanceMat != null)
            instanceMat.color = target;
    }
}
