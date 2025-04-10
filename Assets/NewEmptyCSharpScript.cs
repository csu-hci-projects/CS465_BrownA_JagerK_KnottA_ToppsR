using UnityEngine;

using System.Collections.Generic;

public class PipeGrabHandler : MonoBehaviour
{
    public Transform grabPointA;
    public Transform grabPointB;
    public Transform pipeMesh; 
    public List<Transform> middleSpheres; 

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactableA;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactableB;

    private bool isAGrabbed;
    private bool isBGrabbed;

    void Awake()
    {
        interactableA = grabPointA.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        interactableB = grabPointB.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        interactableA.selectEntered.AddListener(_ => isAGrabbed = true);
        interactableA.selectExited.AddListener(_ => isAGrabbed = false);

        interactableB.selectEntered.AddListener(_ => isBGrabbed = true);
        interactableB.selectExited.AddListener(_ => isBGrabbed = false);
    }

    void Update()
    {
        if (!isAGrabbed && !isBGrabbed)
            return;

        Transform anchor, moving;
        if (isAGrabbed && !isBGrabbed)
        {
            anchor = grabPointB;
            moving = grabPointA;
        }
        else if (isBGrabbed && !isAGrabbed)
        {
            anchor = grabPointA;
            moving = grabPointB;
        }
        else
        {
            return;
        }

        // Position + rotation of pipe
        Vector3 dir = moving.position - anchor.position;
        float dist = dir.magnitude;
        pipeMesh.position = anchor.position + dir * 0.5f;
        pipeMesh.rotation = Quaternion.LookRotation(dir);
        pipeMesh.Rotate(90, 0, 0);

        Vector3 scale = pipeMesh.localScale;
        scale.y = dist * 0.5f;
        pipeMesh.localScale = scale;

        for (int i = 0; i < middleSpheres.Count; i++)
        {
            float t = (i + 1f) / (middleSpheres.Count + 1f); 
            middleSpheres[i].position = Vector3.Lerp(anchor.position, moving.position, t);
        }
    }
}


