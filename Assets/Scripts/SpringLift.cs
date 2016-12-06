using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringLift : MonoBehaviour {

    private GameObject m_hook;
    private SpringJoint m_springJoint;
    private Vector3 m_savePos;
    private Rigidbody m_collidingRb;

    public bool m_isPlayerConnected = false;

    void OnTriggerEnter(Collider col)
    {
        m_collidingRb = col.gameObject.GetComponent<Rigidbody>();

        if (m_collidingRb)
        {

            Vector3 yOffset = new Vector3(0.0f, 0.6f, 0.0f);
            m_hook.transform.position = col.gameObject.transform.position + yOffset;
            m_hook.transform.SetParent(col.gameObject.transform);
            m_springJoint.connectedBody = m_collidingRb;
            CharacterController controller = col.gameObject.GetComponent<CharacterController>();
            if (controller)
            {
                controller.IsConnected = true;
                m_isPlayerConnected = true;
            }
        }
    }

    void OnDisconnect()
    {
        GameObject connectedPlayer = m_springJoint.connectedBody.gameObject;
        Transform child = connectedPlayer.transform.GetChild(1);
        child.parent = null;
        Rigidbody ogHook = child.gameObject.GetComponent<Rigidbody>();
        m_springJoint.connectedBody = ogHook;

        CharacterController controller = connectedPlayer.GetComponent<CharacterController>();
        if (controller)
        {
            controller.IsConnected = false;
            m_isPlayerConnected = false;
        }
    }

    // Use this for initialization
    void Start()
    {
        m_springJoint = GetComponent<SpringJoint>();
        m_hook = m_springJoint.connectedBody.gameObject;
        m_savePos = m_hook.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isPlayerConnected)
        {
            m_collidingRb.AddForce(30 * m_hook.transform.up);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnDisconnect();
                m_hook.transform.position = m_savePos;

            }
        }

    }
}

