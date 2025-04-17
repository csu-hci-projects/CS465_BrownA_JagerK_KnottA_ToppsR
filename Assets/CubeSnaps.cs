using UnityEngine;

public class SnapPort : MonoBehaviour
{
    public PipeEnd snappedEnd;

    private void OnTriggerEnter(Collider other)
    {
        var pipeEnd = other.GetComponent<PipeEnd>();
        if (pipeEnd && snappedEnd == null)
        {
            snappedEnd = pipeEnd;
            pipeEnd.SnapToPort(this);
        }
    }

    public void Unregister()
    {
        snappedEnd = null;
    }
}
