using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float activeTime = 2f;
    public float cooldownTime = 2f; // Added cooldown time

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 1f;
    public Transform positionToSpawn;

    [Header("Shader Related")]
    public Material mat;

    private bool isTrailActive = false;
    private float nextTrailActivationTime = 0f; // Added next activation time
    private SkinnedMeshRenderer[] skinnedMeshRenderers;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !isTrailActive && Time.time > nextTrailActivationTime)
        {
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
            nextTrailActivationTime = Time.time + cooldownTime; // Set the next activation time
        }
    }

    IEnumerator ActivateTrail(float timeActive)
    {
        while (timeActive > 0)
        {
            timeActive -= meshRefreshRate;

            if (skinnedMeshRenderers == null)
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < skinnedMeshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);

                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                skinnedMeshRenderers[i].BakeMesh(mesh);

                mf.mesh = mesh;
                mr.material = mat;

                Destroy(gObj, meshDestroyDelay);
            }
            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;
    }
}
