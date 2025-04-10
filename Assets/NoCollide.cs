using UnityEngine;


public class DisableCollisionWhileGrabbed : MonoBehaviour
{
    private Collider col;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    void Awake()
    {
        col = GetComponent<Collider>();
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        grab.selectEntered.AddListener(_ => col.enabled = false);
        grab.selectExited.AddListener(_ => col.enabled = true);
    }
}

