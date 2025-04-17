using System.Collections.Generic;
using UnityEngine;


public class PipeGrabHandler : MonoBehaviour
{
    [Header("Pipe Elements")]
    public Transform grabPointA;
    public Transform grabPointB;
    public Transform pipeMesh;
    public List<Transform> middleSpheres;

    [Header("Snapping")]
    public Material defaultMaterial;
    public Material halfConnectedMaterial;
    public Material fullyConnectedMaterial;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactableA;
    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable interactableB;

    private bool isAGrabbed;
    private bool isBGrabbed;

    private bool isATemporarilyDetached = false;
    private bool isBTemporarilyDetached = false;

    public bool isASnapped = false;
    public bool isBSnapped = false;

    void Awake()
    {
        interactableA = grabPointA.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        interactableB = grabPointB.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();

        interactableA.selectEntered.AddListener(_ => OnGrabA());
        interactableA.selectExited.AddListener(_ => OnReleaseA());

        interactableB.selectEntered.AddListener(_ => OnGrabB());
        interactableB.selectExited.AddListener(_ => OnReleaseB());
    }

    void Update()
    {
        // If neither point is grabbed or they're both in temp detachment
        if ((!isAGrabbed && !isBGrabbed) || (isATemporarilyDetached || isBTemporarilyDetached))
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
            return; // both are grabbed — optional: handle stretching here
        }

        // Direction and distance
        Vector3 dir = moving.position - anchor.position;
        float dist = dir.magnitude;

        // Update pipe transform
        pipeMesh.position = anchor.position + dir * 0.5f;
        pipeMesh.rotation = Quaternion.LookRotation(dir);
        pipeMesh.Rotate(90, 0, 0);

        Vector3 scale = pipeMesh.localScale;
        scale.y = dist * 0.5f;
        pipeMesh.localScale = scale;

        // Reposition the middle spheres evenly
        for (int i = 0; i < middleSpheres.Count; i++)
        {
            float t = (i + 1f) / (middleSpheres.Count + 1f);
            middleSpheres[i].position = Vector3.Lerp(anchor.position, moving.position, t);
        }
    }

    #region Grab Events

    private void OnGrabA()
    {
        isAGrabbed = true;

        if (isASnapped)
        {
            isATemporarilyDetached = true;
            UnsnapA();
            Invoke(nameof(ClearDetachA), 0.1f);
        }
    }

    private void OnGrabB()
    {
        isBGrabbed = true;

        if (isBSnapped)
        {
            isBTemporarilyDetached = true;
            UnsnapB();
            Invoke(nameof(ClearDetachB), 0.1f);
        }
    }

    private void OnReleaseA()
    {
        isAGrabbed = false;
    }

    private void OnReleaseB()
    {
        isBGrabbed = false;
    }

    private void ClearDetachA() => isATemporarilyDetached = false;
    private void ClearDetachB() => isBTemporarilyDetached = false;

    #endregion

    #region Snap Functions

    public void SnapA(Transform port)
    {
        grabPointA.position = port.position;
        grabPointA.rotation = port.rotation;

        var rb = grabPointA.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        isASnapped = true;
        UpdatePipeColor();
    }

    public void UnsnapA()
    {
        var rb = grabPointA.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = false;

        isASnapped = false;
        UpdatePipeColor();
    }

    public void SnapB(Transform port)
    {
        grabPointB.position = port.position;
        grabPointB.rotation = port.rotation;

        var rb = grabPointB.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = true;

        isBSnapped = true;
        UpdatePipeColor();
    }

    public void UnsnapB()
    {
        var rb = grabPointB.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = false;
        isBSnapped = false;
        UpdatePipeColor();
    }

    private void UpdatePipeColor()
    {
        Material currentMaterial = defaultMaterial;

        if (isASnapped ^ isBSnapped)
        {
            currentMaterial = halfConnectedMaterial;
            Debug.Log("Half-connected material applied");
        }
        else if (isASnapped && isBSnapped)
        {
            currentMaterial = fullyConnectedMaterial;
            Debug.Log("Fully-connected material applied");
        }
        else
        {
            Debug.Log("Default material applied");
        }

        foreach (var sphere in middleSpheres)
        {
            Renderer rend = sphere.GetComponent<Renderer>();
            if (!rend)
                rend = sphere.GetComponentInChildren<Renderer>();

            if (rend)
            {
                Debug.Log($"Setting material on {sphere.name} to {currentMaterial.name}");
                rend.material = currentMaterial;
            }
            else
            {
                Debug.LogWarning($"No Renderer found on {sphere.name}");
            }
        }
    }

    #endregion
}



