using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

	private Rigidbody m_rb = null;
    public Vector3 m_maxSpeed = new Vector3(1.0f, 5.0f, 0.0f);

    private bool m_isConnected = false;
    public bool IsConnected
    {
        set { m_isConnected = value; }
        get { return m_isConnected; }
    }

    // Use this for initialization
    void Start () {
		m_rb = GetComponent<Rigidbody> ();


	}

	public void MovePlayer (float xAxis, bool isJumping)
	{
        if (m_isConnected)
        {
            //early exit 
            return;
        }

        Vector3 moveVelocity = Vector3.zero;
        Vector3 xMovement = (xAxis * m_maxSpeed.x * transform.right);
        moveVelocity = xMovement;

        moveVelocity.y = m_rb.velocity.y;
        if (isJumping && (Mathf.Abs(m_rb.velocity.y) < 0.0001f))
        {
            moveVelocity.y = m_maxSpeed.y;
        }
        m_rb.velocity = moveVelocity;
    }

    // Update is called once per frame
    void Update () {

		float horizontal = Input.GetAxis ("Horizontal");
        bool isJumping = Input.GetKeyDown(KeyCode.Space);
		MovePlayer (horizontal, isJumping);
	}
}


