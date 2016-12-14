using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampBoost : MonoBehaviour {

    public PhysicMaterial m_material;
    public float m_desiredDistance = 5.0f;

    private float m_landingSpot;
    public float m_distance;
    private float m_grav = Physics.gravity.y;
    public float m_applyForce;
    public bool m_isTouching = false;
    public Rigidbody m_rb;

    void OnCollisionEnter(Collision col)
    {
        m_rb = col.gameObject.GetComponent<Rigidbody>();
        CalculateApplyForce();
        m_rb.velocity = new Vector3(m_applyForce, 0.0f, 0.0f);
        


       // m_isTouching = true;
    }

    void Start () {                        /*0.5 sphere half length*/
        m_landingSpot = transform.position.x + 0.5f + m_desiredDistance;
        m_distance = m_landingSpot - (transform.position.x - 2.5f);

    }

    void Update () {
        //if (m_isTouching)
        //{
        //    m_rb.velocity += new Vector3(m_applyForce, 0.0f, 0.0f);
        //}
    }
    
    void CalculateApplyForce()
    {
        Vector3 rampDirection = transform.rotation.eulerAngles;

        float theta = rampDirection.z * Mathf.Deg2Rad;
        float fGrav = m_rb.mass * m_grav * Mathf.Sin(theta);
        float fNorm = m_rb.mass * m_grav * Mathf.Cos(theta);
        float fDynamic = fNorm * m_material.dynamicFriction;
        float fNet = fGrav + fDynamic;
        float fAcc = fNet / m_rb.mass;
        float initVelocity = Mathf.Sqrt(0 - (2 * fAcc * m_distance));

        m_applyForce = initVelocity;
    }
}
