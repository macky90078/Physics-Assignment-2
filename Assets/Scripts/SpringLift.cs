using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringLift : MonoBehaviour {

    public float m_compressDisplacement = 3.0f;
    public float m_timer = 13;

    private float m_riseForce;
    private GameObject m_hook;
    private SpringJoint m_springJoint;
    private Rigidbody m_collidingRb;

    private bool m_isPlayerConnected = false;

    void Start()
    {
        m_springJoint = GetComponent<SpringJoint>();
        m_hook = m_springJoint.connectedBody.gameObject;
        m_riseForce = CalculateRiseForce();
    }

    void Update()
    {
        if (m_isPlayerConnected)
        {
            m_timer -= Time.deltaTime;

            if (m_timer > 0)
            {
                m_collidingRb.AddForce(m_riseForce * m_hook.transform.up);
                transform.Translate(Vector3.right * Time.deltaTime);
            } else { m_timer = 0.0f; }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnDisconnect();

            }
        }

    }

    private float CalculateRiseForce()
    {
        return m_springJoint.spring * m_compressDisplacement;
    }

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
}

