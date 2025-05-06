using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable), typeof(Rigidbody))]
public class SnapToCubes : MonoBehaviour
{
    [Tooltip("How close you must be on release to snap.")]
    public float snapDistance = 0.2f;

    [Tooltip("Once grabbed and pulled beyond this, it unsnaps immediately.")]
    public float unsnapDistance = 0.3f;

    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    Rigidbody rb;

    bool     isSnapped     = false;
    Transform currentTarget = null;

    // Expose a read‑only property so other scripts (e.g. StretchyPipe) can query snap state
    public bool IsSnapped => isSnapped;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb               = GetComponent<Rigidbody>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited .AddListener(OnRelease);
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited .RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (isSnapped)
            Unsnap();
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (!isSnapped)
        {
            var nearest = FindNearest();
            if (nearest != null &&
                Vector3.Distance(transform.position, nearest.position) <= snapDistance)
            {
                Snap(nearest);
            }
        }
    }

    void Update()
    {
        if (grabInteractable.isSelected && isSnapped)
        {
            float d = Vector3.Distance(transform.position, currentTarget.position);
            if (d >= unsnapDistance)
                Unsnap();
        }
    }

    Transform FindNearest()
    {
        Transform best     = null;
        float     bestDist = float.MaxValue;

        foreach (var t in SnapCube.All)
        {
            float d = Vector3.Distance(transform.position, t.position);
            if (d < bestDist)
            {
                bestDist = d;
                best     = t;
            }
        }

        return best;
    }

    void Snap(Transform target)
    {
        // 1) Parent under target, preserving world transform
        transform.SetParent(target, true);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // 2) Clear any existing motion *before* switching to kinematic
        rb.linearVelocity        = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 3) Then make the body kinematic so physics no longer moves it
        rb.isKinematic     = true;

        isSnapped     = true;
        currentTarget = target;
    }

    void Unsnap()
    {
        transform.SetParent(null, true);
        rb.isKinematic = false;

        isSnapped     = false;
        currentTarget = null;
    }
}
