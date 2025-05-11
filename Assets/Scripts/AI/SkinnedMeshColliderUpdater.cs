using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnedMeshColliderUpdater : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer; // Assign via inspector or find in code
    private MeshCollider meshCollider;
    private Mesh bakedMesh;

    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();

        // Auto-find SkinnedMeshRenderer if not assigned
        if (skinnedMeshRenderer == null)
        {
            skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }

        bakedMesh = new Mesh();
    }

    void LateUpdate()
    {
        if (skinnedMeshRenderer == null || meshCollider == null) return;

        skinnedMeshRenderer.BakeMesh(bakedMesh);

        meshCollider.sharedMesh = null; // Clear first
        meshCollider.sharedMesh = bakedMesh;
    }
}
