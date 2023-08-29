using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraManager : MonoBehaviour
{
    private Transform target;
    [SerializeField] private float smoothSpeed = 0.125f; // Speed for camera smoothing
    [SerializeField] private Vector2 minBounds; // Minimum camera bounds (x,y)
    [SerializeField] private Vector2 maxBounds; // Maximum camera bounds (x,y)
    [SerializeField] private float verticalOffset = 2.0f; // Vertical offset for the camera

    public List<SceneBounds> sceneBoundsList;
    [System.Serializable]
    public struct SceneBounds
    {
        public int sceneIndex; // Use this for the scene's build index
        public Vector2 defaultMinBounds;
        public Vector2 defaultMaxBounds;
    }


    private void Awake()
    {
        target = GameObject.FindWithTag("Player")?.transform;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        target = GameObject.FindWithTag("Player")?.transform;

        foreach (var bounds in sceneBoundsList)
        {
            if (scene.buildIndex == bounds.sceneIndex)
            {
                minBounds = bounds.defaultMinBounds;
                maxBounds = bounds.defaultMaxBounds;
                break;
            }
        }
    }

    private void LateUpdate()
    {
        if (!target) return;

        Vector3 desiredPosition = new Vector3(target.position.x, target.position.y + verticalOffset, transform.position.z);
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minBounds.x, maxBounds.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minBounds.y, maxBounds.y);

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

    // This function will draw a rectangle in the Scene view to visualize the bounds
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(minBounds.x, minBounds.y, 0), new Vector3(minBounds.x, maxBounds.y, 0));
        Gizmos.DrawLine(new Vector3(minBounds.x, maxBounds.y, 0), new Vector3(maxBounds.x, maxBounds.y, 0));
        Gizmos.DrawLine(new Vector3(maxBounds.x, maxBounds.y, 0), new Vector3(maxBounds.x, minBounds.y, 0));
        Gizmos.DrawLine(new Vector3(maxBounds.x, minBounds.y, 0), new Vector3(minBounds.x, minBounds.y, 0));
    }

    public void SetMinBound(Vector2 bound)
    {
        minBounds = bound;
    }
}
