using UnityEngine;

public class SnapPort : MonoBehaviour
{
    public PipeEnd snappedEnd;

    private void OnTriggerEnter(Collider other)
    {
        if (snappedEnd != null) return;

        var pipeEnd = other.GetComponent<PipeEnd>();
        if (pipeEnd && !pipeEnd.IsSnapped)
        {
            pipeEnd.SnapToPort(this);
            snappedEnd = pipeEnd;
        }
    }

    public void Unregister()
    {
        snappedEnd = null;
    }
}
