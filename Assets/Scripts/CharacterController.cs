using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	private Rigidbody m_rb = null;
    public PhysicMaterial m_material;
    public float m_maxSpeed = 10.0f;
    public float m_maxJump = 7.0f;

    public Vector3 m_checkVelocity;

    private float m_flatVelocity;
    private float m_grav;
    private float m_horizontal;
    private bool m_isJumping;
    private bool m_isConnected = false;
    public bool IsConnected
    {
        set { m_isConnected = value; }
        get { return m_isConnected; }
    }

    // Use this for initialization
    void Start () {
		m_rb = GetComponent<Rigidbody> ();
        m_grav = Physics.gravity.magnitude;

	}

    // Update is called once per frame
    void Update () {

		m_horizontal = Input.GetAxis ("Horizontal");
        m_isJumping = Input.GetKeyDown(KeyCode.Space);
	}

    void FixedUpdate()
    {
        CalculateFlatMovement();
        MovePlayer(m_horizontal, m_isJumping);
    }

    void OnCollisionEnter(Collision col)
    {
        Collider coll = col.gameObject.GetComponent<Collider>();
        if (coll)
        {
            m_material = coll.material;
        }
    
    }

    void CalculateFlatMovement()
    {
        float fNorm = m_rb.mass * m_grav;
        float fDynamic = fNorm * m_material.dynamicFriction;
        float Acc = (fDynamic / m_rb.mass);
        float initVelocity = m_maxSpeed - Acc;

        m_flatVelocity = initVelocity;
    }

    public void MovePlayer(float xAxis, bool isJumping)
    {
        if (m_isConnected)
        {
            //early exit 
            return;
        }

        Vector3 moveVelocity = Vector3.zero;
        float xMovement = (xAxis * m_flatVelocity);

        moveVelocity.x = xMovement;
        moveVelocity.y = m_rb.velocity.y;

        if (isJumping && (Mathf.Abs(m_rb.velocity.y) < 0.0001))
        {
            moveVelocity.y = m_maxJump;
        }

        m_rb.velocity = moveVelocity;
        m_checkVelocity = moveVelocity;
    }
}


