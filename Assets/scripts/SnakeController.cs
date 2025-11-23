using UnityEngine;
using System.Collections.Generic;

public class SnakeController : MonoBehaviour
{
    [Header("References")]
    public Transform SnakeHead;
    public List<Transform> BodyParts = new List<Transform>();

    [Header("Settings")]
    public float MoveSpeed = 5f;
    public int Gap = 10;

    [Header("Collision Settings")]
    public float detectionDistance = 0.6f;
    public LayerMask obstacleLayer;

    public bool isMoving = false;
    private List<Vector3> PositionHistory = new List<Vector3>();
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        InitialisePlacement();
    }

    private void Update()
    {
        if (!isMoving && GameManager.Instance.currentHealth > 0)
        {
            CheckForInput();
        }

        if (isMoving)
        {
            MoveSnake();
        }
    }

    private void CheckForInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.transform == SnakeHead)
                {

                    if (IsPathBlocked())
                    {

                        GameManager.Instance.TakeDamage();
                    }
                    else
                    {

                        isMoving = true;
                    }
                }
            }
        }
    }

    bool IsPathBlocked()
    {
        Ray ray = new Ray(SnakeHead.position, transform.forward);
        RaycastHit hit;
        Color debugColor = Color.green;
        bool isBlocked = false;

        if (Physics.Raycast(ray, out hit, detectionDistance, obstacleLayer))
        {
            isBlocked = true;
            debugColor = Color.red;

            //Trigger screen shake
            if (CameraShake.Instance != null)
                CameraShake.Instance.Shake(0.15f);   // duration in seconds

            //Optional vibration
#if UNITY_ANDROID || UNITY_IOS
    Handheld.Vibrate();
#endif
        }

        Debug.DrawRay(SnakeHead.position, transform.forward * detectionDistance, debugColor, 1f);
        return isBlocked;
    }

    void InitialisePlacement()
    {
        PositionHistory.Clear();
        PositionHistory.Add(SnakeHead.position);
        Transform previousPart = SnakeHead;

        foreach (Transform part in BodyParts)
        {
            Vector3 startPos = previousPart.position;
            Vector3 endPos = part.position;
            for (int i = 1; i <= Gap; i++)
            {
                float t = (float)i / Gap;
                PositionHistory.Add(Vector3.Lerp(startPos, endPos, t));
            }
            previousPart = part;
        }
    }

    void MoveSnake()
    {
        transform.position += transform.forward * MoveSpeed * Time.deltaTime;
        PositionHistory.Insert(0, SnakeHead.position);

        int index = 0;
        foreach (Transform body in BodyParts)
        {
            int pointIndex = (index + 1) * Gap;
            if (pointIndex < PositionHistory.Count)
            {
                body.position = PositionHistory[pointIndex];
                int lookIndex = Mathf.Max(0, pointIndex - 1);
                body.LookAt(PositionHistory[lookIndex]);
            }
            index++;
        }

        int maxHistoryNeeded = (BodyParts.Count + 1) * Gap + 5;
        if (PositionHistory.Count > maxHistoryNeeded) PositionHistory.RemoveAt(PositionHistory.Count - 1);
    }


}