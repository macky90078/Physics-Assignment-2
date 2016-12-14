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
    private bool m_isJumping = false;
    private bool m_isConnected = false;
    public bool IsConnected
    {
        set { m_isConnected = value; }
        get { return m_isConnected; }
    }

    public bool b_verticalMovement;
    private Launch m_Launch;
    private bool b_inCannon = false;
    private GameObject m_cannon;

    // Use this for initialization
    void Start ()
    {
        m_currentScene = SceneManager.GetActiveScene().buildIndex;
		m_rb = GetComponent<Rigidbody> ();
        m_grav = Physics.gravity.magnitude;
        m_Launch = GetComponent<Launch>();
	}

    // Update is called once per frame
    void Update ()
    {
        if (!b_inCannon) {
            if (!b_verticalMovement) {
                m_horizontal = Input.GetAxis("Horizontal");
            } else if (b_verticalMovement) {
                m_horizontal = Input.GetAxis("Vertical");
            }
        } else if (b_inCannon) {
            m_horizontal = Input.GetAxis("Vertical");
        }

        m_isJumping = Input.GetKeyDown(KeyCode.Space);
        checkForDeathOrReset();
	}

    void FixedUpdate()
    {
        CalculateFlatMovement();
        if (!b_inCannon) {
            MovePlayer(m_horizontal, m_isJumping);
        } else if (b_inCannon) {
            MoveCannon(m_horizontal, m_isJumping);
        }
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
        if (col.collider.tag == "Level1") {
            col.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(LoadScene(col.collider.tag));
        }
        if (col.collider.tag == "Level2") {
            col.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            StartCoroutine(LoadScene(col.collider.tag));
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.GetComponent<Collider>().tag == "Cannon") {
            m_rb.velocity = Vector3.zero;
            m_Launch.enabled = true;
            b_inCannon = true;
            m_cannon = col.transform.parent.gameObject;
            transform.position = m_cannon.transform.position;
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

    private void MoveCannon(float Axis, bool launch) {
        if (launch) {
            m_Launch.LaunchProjectile();
        }

        m_cannon.transform.Rotate(-Axis, 0, 0);
        Vector3 newPos = new Vector3(0, 0, Axis);
        m_Launch.m_referenceTransform.transform.position += newPos;
    }

    private IEnumerator LoadScene(string sceneName) {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(sceneName);
    }
}


