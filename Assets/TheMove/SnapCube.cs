// SnapCube.cs
using UnityEngine;
using System.Collections.Generic;

public class SnapCube : MonoBehaviour
{
    public static readonly List<Transform> All = new List<Transform>();

    void OnEnable()  => All.Add(transform);
    void OnDisable() => All.Remove(transform);
}
