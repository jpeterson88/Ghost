using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularPathSpawner : MonoBehaviour
{
    [Tooltip("The prefab to instantiate.")]
    public GameObject objectToSpawn;

    [Tooltip("The radius of the circular path.")]
    public float radius = 5f;

    [Tooltip("The speed of movement along the circular path (in degrees per second).")]
    public float angularSpeed = 90f;

    [Tooltip("The duration (in seconds) for which the objects will move along the path.")]
    public float movementDuration = 3f;

    [Tooltip("The target point where objects will move after completing the circular path.")]
    public Transform endingTarget;

    [Tooltip("The number of objects to spawn.")]
    public int numberOfObjects = 3;

    [Tooltip("The speed at which objects move to the target point.")]
    public float moveToTargetSpeed = 5f;

    private float startTime;
    private bool cancelMovement = false; // Flag to cancel movement
    private List<GameObject> spawnedObjects;

    private void Start()
    {
        spawnedObjects = new List<GameObject>();
        startTime = -1f; // Indicates no objects have been instantiated yet
    }

    /// <summary>
    /// Spawns multiple objects and moves them along the circular path.
    /// </summary>
    public void SpawnAndMoveObjects()
    {
        if (objectToSpawn == null || endingTarget == null)
        {
            Debug.LogWarning("Object to spawn or target point is not assigned.");
            return;
        }

        cancelMovement = false; // Reset the cancellation flag

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Instantiate the object
            GameObject spawnedObject = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
            spawnedObjects.Add(spawnedObject);

            // Start the timer if this is the first object
            if (startTime < 0f)
            {
                startTime = Time.time;
            }

            // Start moving the object along the circular path
            StartCoroutine(MoveAlongCircularPath(spawnedObject.transform, i));
        }
    }

    /// <summary>
    /// Cancels the movement of all objects.
    /// </summary>
    public void CancelMovement()
    {
        cancelMovement = true;
        OnEnd();
    }

    private IEnumerator MoveAlongCircularPath(Transform objTransform, int index)
    {
        float elapsed = 0f;

        while (elapsed < movementDuration)
        {
            if (cancelMovement)
            {
                yield break;
            }

            // Calculate the angle based on elapsed time, angular speed, and object index for spacing
            float angle = ((Time.time - startTime) * angularSpeed + (index * 360f / numberOfObjects)) % 360f;

            // Convert the angle to a position on the circle
            Vector3 offset = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad), 0f) * radius;
            objTransform.position = transform.position + offset;

            elapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(MoveEndToTarget(objTransform));
    }

    private IEnumerator MoveEndToTarget(Transform objTransform)
    {
        while (Vector3.Distance(objTransform.position, endingTarget.position) > 0.01f)
        {
            if (cancelMovement)
            {
                yield break;
            }

            // Move the object towards the target point at the specified speed
            objTransform.position = Vector3.MoveTowards(
                objTransform.position,
                endingTarget.position,
                moveToTargetSpeed * Time.deltaTime
            );

            yield return null;
        }

        // Snap the object to the target position to ensure precision
        objTransform.position = endingTarget.position;

        var fadeAtEnd = objTransform.GetComponent<SpriteFadeToDestroy>();
        if (fadeAtEnd != null)
            fadeAtEnd.Cleanup();
    }

    private void OnEnd()
    {
        foreach (var obj in spawnedObjects)
        {
            if (obj != null)
            {
                var fade = obj.GetComponent<SpriteFadeToDestroy>();
                if (fade != null)
                    fade.Cleanup();
            }
        }
    }
}