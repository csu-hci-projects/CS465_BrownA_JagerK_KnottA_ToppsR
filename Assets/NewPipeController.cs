using UnityEngine;


public class PipeController : MonoBehaviour
{
    [Header("Drag each endpoint's XRGrabInteractable here")]
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable endPointAInteractable;
    public UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable endPointBInteractable;

    void Awake()
    {
        // When A is grabbed, disable B; when A is released, re-enable B
        endPointAInteractable.selectEntered.AddListener(_ => OnGrab(endPointBInteractable));
        endPointAInteractable.selectExited .AddListener(_ => OnRelease(endPointBInteractable));

        // And vice versa
        endPointBInteractable.selectEntered.AddListener(_ => OnGrab(endPointAInteractable));
        endPointBInteractable.selectExited .AddListener(_ => OnRelease(endPointAInteractable));
    }

    void OnDestroy()
    {
        endPointAInteractable.selectEntered.RemoveAllListeners();
        endPointAInteractable.selectExited .RemoveAllListeners();
        endPointBInteractable.selectEntered.RemoveAllListeners();
        endPointBInteractable.selectExited .RemoveAllListeners();
    }

    void OnGrab(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable toLock)
    {
        // stop the other end from being grabbed/moved
        toLock.enabled = false;
    }

    void OnRelease(UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable toUnlock)
    {
        toUnlock.enabled = true;
    }
}
