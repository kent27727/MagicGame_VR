using System.Collections.Generic;
using UnityEngine;
using PDollarGestureRecognizer;

public class MovementRecognizer : MonoBehaviour
{
    public Transform movementSource;
    public float newPositionThresholdDistance = 0.05f;
    public GameObject debugCubePrefab;
    public bool creationMode = true;
    public string newGestureName;
    public float recognitionThreshold = 0.9f;  
    public List<Gesture> trainingSet = new List<Gesture>();

    private bool isMoving = false;
    private List<Vector3> positionsList = new List<Vector3>();

    void Update()
    {
        if(isMoving)
            UpdateMovement();        
    }

    public void StartMovement()
    {
        isMoving = true;
        positionsList.Clear();

        AddPointFromMovementSource();
    }

    void AddPointFromMovementSource()
    {
        positionsList.Add(movementSource.position);

        //if you want see shapes, you can run it.

        //if (debugCubePrefab)
        //{
        //    GameObject spawnedDebugCube = Instantiate(debugCubePrefab, movementSource.position, Quaternion.identity);
        //    Destroy(spawnedDebugCube, 5);
        //}
    }

    void UpdateMovement()
    {
        Vector3 lastPosition = positionsList[positionsList.Count - 1];

        if (Vector3.Distance(movementSource.position, lastPosition) > newPositionThresholdDistance)
        {
            AddPointFromMovementSource();
        }
    }

    public string EndMovement()
    {
        isMoving = false;
        AddPointFromMovementSource();

        Point[] pointArray = new Point[positionsList.Count];

        for (int i = 0; i < positionsList.Count; i++)
        {
            Vector2 screenPoint = Camera.main.WorldToScreenPoint(positionsList[i]);
            pointArray[i] = new Point(screenPoint.x, screenPoint.y, 0);
        }

        Gesture newGesture = new Gesture(pointArray);

        if(creationMode)
        {
            newGesture.Name = newGestureName;
            trainingSet.Add(newGesture);

            return null;
        }
        else
        {
            Result result = PointCloudRecognizer.Classify(newGesture, trainingSet.ToArray());

            if (result.Score > recognitionThreshold)
                return result.GestureClass;            
            else
                return null;
        }
    }
}
