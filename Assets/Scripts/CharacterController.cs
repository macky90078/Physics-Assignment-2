using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour {

	private Rigidbody m_rb = null;
    public PhysicMaterial m_material;
    public float m_maxSpeed = 10.0f;
    public float m_maxJump = 7.0f;
    public float desiredRampLaunchDist = 10.0f;

    public Vector3 m_checkVelocity;
    public Vector3 m_rampDirection;

    private int m_currentScene;

    private float m_distance;
    private float m_flatVelocity;
    private float m_grav;
    private float m_horizontal;
    public float m_applyForce;

    private bool m_onRamp = false;
    private bool m_isJumping;
    private bool m_isConnected = false;
    public bool IsConnected
    {
        set { m_isConnected = value; }
        get { return m_isConnected; }
    }

    public bool b_verticalMovement;

    // Use this for initialization
    void Start ()
    {
        m_currentScene = SceneManager.GetActiveScene().buildIndex;
		m_rb = GetComponent<Rigidbody> ();
        m_grav = Physics.gravity.magnitude;
	}

    // Update is called once per frame
    void Update ()
    {

    	if (!b_verticalMovement) {
    		m_horizontal = Input.GetAxis("Horizontal");
    	} else if (b_verticalMovement) {
    		m_horizontal = Input.GetAxis("Vertical");
    	}

        m_isJumping = Input.GetKeyDown(KeyCode.Space);
        checkForDeathOrReset();
	}

    void FixedUpdate()
    {
        CalculateFlatMovement();
        MovePlayer(m_horizontal, m_isJumping);
        if (m_onRamp)
        {
            m_rb.velocity += new Vector3(m_applyForce, 0.0f, 0.0f);
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Collider coll = col.gameObject.GetComponent<Collider>();
        if (coll)
        {
            m_material = coll.material;
        }
        if(col.collider.tag == "EndLevel")
        {
            SceneManager.LoadScene(m_currentScene + 1);
        }
        if (col.collider.tag == "Ramp")
        {
            m_rampDirection = col.transform.rotation.eulerAngles;
            float landingSpot = transform.position.x + desiredRampLaunchDist;
            m_distance = landingSpot - (transform.position.x);
            CalculateRampMovement();
            m_onRamp = true;
        } else { m_onRamp = false; }
       
    }

    void CalculateFlatMovement()
    {
        float fNorm = m_rb.mass * m_grav;
        float fDynamic = fNorm * m_material.dynamicFriction;
        float Acc = (fDynamic / m_rb.mass);
        float initVelocity = m_maxSpeed - Acc;

        m_flatVelocity = initVelocity;
    }

    void CalculateRampMovement()
    {
        int finalVelocity = 0;

        float theta = m_rampDirection.z * Mathf.Deg2Rad;
        float fGrav = m_rb.mass * Physics.gravity.y * Mathf.Sin(theta);
        float fNorm = m_rb.mass * Physics.gravity.y * Mathf.Cos(theta);
        float fDynamic = fNorm * m_material.dynamicFriction;
        float fNet = fGrav + fDynamic;
        float fAcc = fNet / m_rb.mass;
        float initVelocity = Mathf.Sqrt(finalVelocity - (2 * fAcc * m_distance));

        m_applyForce = initVelocity;
    }

    void checkForDeathOrReset()
    {
        if (m_rb.position.y < -4 || Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(m_currentScene);
        }
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

        if (!b_verticalMovement) {
			moveVelocity.x = xMovement;
        } else {
        	moveVelocity.z = xMovement;
        }

        moveVelocity.y = m_rb.velocity.y;

        if (isJumping && (Mathf.Abs(m_rb.velocity.y) < 0.0001))
        {
            moveVelocity.y = m_maxJump;
        }

        m_rb.velocity = moveVelocity;
        m_checkVelocity = moveVelocity;
    }
}


