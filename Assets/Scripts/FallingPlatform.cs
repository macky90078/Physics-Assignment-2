using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour {

    private Rigidbody m_rb;
    private Rigidbody m_CollidingRb;

    private float m_timer = 1f;
    private float m_appledForce = 30f;

    private bool m_timerDone = false;
    private bool m_playerCollision = false;

    // Use this for initialization
    void Start () {
        m_rb = transform.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if(m_playerCollision)
        {
            m_timer -= Time.deltaTime;
        }
        if(m_timer < 0)
        {
            m_playerCollision = false;
            m_timerDone = true;
        }
		
	}

    void FixedUpdate()
    {
        if(m_playerCollision)
        {
            m_rb.AddForce(transform.up * m_appledForce);
        } 
        if(!m_playerCollision)
        {
            m_rb.AddForce(transform.up * Physics.gravity.magnitude);
        }
        if(m_timerDone)
        {
            m_rb.AddForce(transform.up * Physics.gravity.y);
        }

    }

    void OnCollisionEnter(Collision col)
    {
        if (col.collider.tag == "Player")
        {
            m_CollidingRb = col.gameObject.GetComponent<Rigidbody>();
            float totalMass = m_rb.mass + m_CollidingRb.mass;
            m_appledForce += totalMass * Physics.gravity.magnitude;
            m_playerCollision = true;
        }
    }
}
