using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostJump : MonoBehaviour {

    public float m_displacement = 10.0f;

    private float m_initialVelocity;
    private float m_grav;
    private bool m_isActive = true;

    public Rigidbody m_rb;

	// Use this for initialization
	void Start () {
        m_grav = Physics.gravity.y;
	}
	
	// Update is called once per frame
	void Update () {

		
	}

    void OnCollisionEnter(Collision col)
    {
        if (m_isActive)
        {
            m_rb = col.gameObject.GetComponent<Rigidbody>();
            GetComponent<Renderer>().material.color = Color.red;
            CalculateImpulse();
            m_rb.velocity = new Vector3(0.0f, m_initialVelocity, 0.0f);
            m_isActive = false;
        }
    }

    void CalculateImpulse()
    {
        int finalVelocity = 0;
        float initVelocity = Mathf.Sqrt(finalVelocity - (2 * m_grav * m_displacement));
        m_initialVelocity = initVelocity;
    }
}
