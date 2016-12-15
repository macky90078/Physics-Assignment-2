using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

    public Transform[] m_horizontalPoints;
    public Transform[] m_verticalPoints;
    public float m_platformSpeed;

    private Queue<Transform> m_pointsQueue;

    private bool b_onPlatform = false;

    void Start() {
        m_pointsQueue = new Queue<Transform>();
        QueueWaypoint(m_horizontalPoints);
    }

    void Update() {
        float speed = m_platformSpeed * Time.deltaTime;
        MovetoPoint(speed);
        UpdateWaypoints();
    }

    void OnCollisionEnter(Collision other) {
        if (other.collider.tag == "Player") {
            other.transform.parent = transform;
            QueueWaypoint(m_verticalPoints);
        }
    }

    void OnCollisionExit(Collision other) {
        if (other.collider.tag == "Player") {
            other.transform.parent = null;
            QueueWaypoint(m_horizontalPoints);
        }
    }

    void QueueWaypoint(Transform[] wayPoints) {
        foreach (Transform point in wayPoints) {
            m_pointsQueue.Enqueue(point);
        }
    }

    void MovetoPoint(float speed) {
        transform.position = Vector3.MoveTowards(transform.position, m_pointsQueue.Peek().localPosition, speed);
    }

    void UpdateWaypoints() {
        if (transform.position == m_pointsQueue.Peek().position) {
            if (m_pointsQueue.Count < m_horizontalPoints.Length || m_pointsQueue.Count < m_verticalPoints.Length) {
                m_pointsQueue.Enqueue(m_pointsQueue.Peek());
            }
            m_pointsQueue.Dequeue();
        }
    }
}
