using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiningPlatform : MonoBehaviour {

    public GameObject m_platform;

    public bool m_isPlayerConnected = false;

    public float m_rotationAmount = 40f;

    private Transform m_camera;
    private Rigidbody m_collidingRb;
    private Rigidbody m_platformRb;

    void OnTriggerEnter(Collider col)
    {
        m_collidingRb = col.gameObject.GetComponent<Rigidbody>();

        if(m_collidingRb)
        {
            m_camera = m_collidingRb.transform.GetChild(0);
            m_camera.parent = null;
            Vector3 yOffset = new Vector3(0.0f, 1.0f, 0.0f);
            m_collidingRb.isKinematic = true;
            m_collidingRb.transform.position = m_platform.transform.position + yOffset;
            m_collidingRb.transform.SetParent(m_platform.transform);
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
        GameObject connectedPlayer = m_collidingRb.gameObject;
        Transform child = m_platform.transform.GetChild(1);
        child.parent = null;
        m_camera.transform.SetParent(m_collidingRb.transform);
        m_collidingRb.isKinematic = false;
        Rigidbody onPlat = child.gameObject.GetComponent<Rigidbody>();
        m_collidingRb = onPlat;

        CharacterController controller = connectedPlayer.GetComponent<CharacterController>();
        if (controller)
        {
            controller.IsConnected = false;
            m_isPlayerConnected = false;
        }
    }

	void Start () {
        m_platformRb = m_platform.GetComponent<Rigidbody>();	
	}

    void FixedUpdate()
    {
        float vertical =  m_rotationAmount * Time.fixedDeltaTime;
        m_platformRb.AddTorque(transform.forward * -vertical);
    }
	
	void Update () {

        if (m_isPlayerConnected)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnDisconnect();

            }
        }

    }

}
