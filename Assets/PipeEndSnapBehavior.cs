using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PipeEnd : MonoBehaviour
{
    public SnapPort currentPort;
    public bool IsSnapped => currentPort != null;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    public float unsnapDistance = 0.2f;

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grabInteractable.selectEntered.AddListener(_ => OnGrabbed());
        // No need to do anything on release here
    }

    void Update()
    {
        if (IsSnapped && Vector3.Distance(transform.position, currentPort.transform.position) > unsnapDistance)
        {
            Unsnap(); // fallback in case something pulls it away
        }
    }

    public void SnapToPort(SnapPort port)
    {
        currentPort = port;

        // Align position, rotation, and scale
        transform.position = port.transform.position;
        transform.rotation = port.transform.rotation;
        transform.localScale = Vector3.one;

        // Freeze in place
        if (rb) rb.isKinematic = true;

        // Lock movement and rotation while snapped
        grabInteractable.trackPosition = false;
        grabInteractable.trackRotation = false;
    }

    public void Unsnap()
    {
        if (!IsSnapped) return;

        currentPort.Unregister();
        currentPort = null;

        // Allow movement again
        if (rb) rb.isKinematic = false;

        grabInteractable.trackPosition = true;
        grabInteractable.trackRotation = true;
    }

    private void OnGrabbed()
    {
        Debug.Log("Pipe End grabbed");

        // Do not unsnap from here — this is now handled by PipeGrabHandler
    }
}