using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PipeEnd : MonoBehaviour
{
    public SnapPort currentPort;
    public bool IsSnapped => currentPort != null;

    private XRGrabInteractable grabInteractable;
    private Rigidbody rb;

    public float unsnapDistance = 0.15f; // adjust as needed

    void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        rb = GetComponent<Rigidbody>();

        grabInteractable.selectEntered.AddListener(_ => OnGrabbed());
    }

    void Update()
    {
        if (IsSnapped && currentPort)
        {
            // Force-follow snap target
            transform.position = currentPort.transform.position;
            transform.rotation = currentPort.transform.rotation;

            // Unsnap if moved too far away (failsafe)
            float dist = Vector3.Distance(transform.position, currentPort.transform.position);
            if (dist > unsnapDistance)
            {
                Unsnap();
            }
        }
    }

    public void SnapToPort(SnapPort port)
    {
        if (IsSnapped) return;

        currentPort = port;
        currentPort.snappedEnd = this;

        transform.position = port.transform.position;
        transform.rotation = port.transform.rotation;

        if (rb)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        grabInteractable.trackPosition = false;
        grabInteractable.trackRotation = false;
    }

    public void Unsnap()
    {
        if (!IsSnapped) return;

        currentPort.Unregister();
        currentPort = null;

        if (rb)
        {
            rb.isKinematic = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        grabInteractable.trackPosition = true;
        grabInteractable.trackRotation = true;
    }

    private void OnGrabbed()
    {
        if (IsSnapped)
        {
            Unsnap(); // manual detachment
        }
    }
}
