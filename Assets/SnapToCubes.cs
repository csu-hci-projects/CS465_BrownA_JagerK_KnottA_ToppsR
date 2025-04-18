using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable), typeof(Rigidbody))]
public class SnapToCubes : MonoBehaviour
{
    [Tooltip("If empty, finds all GameObjects tagged 'SnapCube'.")]
    public List<Transform> snapTargets = new List<Transform>();

    [Tooltip("How close you must be on release to snap.")]
    public float snapDistance = 0.2f;

    [Tooltip("Once grabbed and pulled beyond this, it unsnaps immediately.")]
    public float unsnapDistance = 0.3f;

    UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable;
    Rigidbody rb;

    bool    isSnapped     = false;
    Transform currentTarget = null;

    void Awake()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        rb               = GetComponent<Rigidbody>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited .AddListener(OnRelease);

        if (snapTargets.Count == 0)
            foreach (var go in GameObject.FindGameObjectsWithTag("SnapCube"))
                snapTargets.Add(go.transform);
    }

    void OnDestroy()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited .RemoveListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        // as soon as you grab the sphere, drop any snap
        if (isSnapped)
            Unsnap();
    }

    void OnRelease(SelectExitEventArgs args)
    {
        // only on *release* do we check if we're close enough to snap
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
        // if you're holding the sphere and pull it away far enough, unsnap immediately
        if (grabInteractable.isSelected && isSnapped)
        {
            float d = Vector3.Distance(transform.position, currentTarget.position);
            if (d >= unsnapDistance)
                Unsnap();
        }
    }

    Transform FindNearest()
    {
        Transform best = null;
        float bestDist = float.MaxValue;
        foreach (var t in snapTargets)
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
        // parent under the cube—but keep the sphere’s world pos/rot/scale
        transform.SetParent(target, /* worldPositionStays: */ true);

        // then zero out local pos/rot so it hugs the cube
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        // freeze physics
        rb.isKinematic     = true;
        rb.linearVelocity        = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        isSnapped     = true;
        currentTarget = target;
    }

    void Unsnap()
    {
        // detach from *any* parent, preserving world transform
        transform.SetParent(null, /* worldPositionStays: */ true);

        // restore physics
        rb.isKinematic = false;

        isSnapped     = false;
        currentTarget = null;
    }
}
