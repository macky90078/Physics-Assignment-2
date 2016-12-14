using UnityEngine;

public class Launch : MonoBehaviour {

    [Tooltip("The time it will take to go ovet the wall. Half the time it takes to go to the target.")]
    public float m_time;

    [Tooltip("Reference Object Transform")]
    public Transform m_referenceTransform;
    public float m_acceleration;

    // Reference to rigidbody.
    private Rigidbody m_rb;
    private Vector3 m_force = Vector3.zero;
    private Vector3 m_desiredDisplacement = Vector3.zero;

    void OnEnable() {
        m_rb = GetComponent<Rigidbody>();
        CalcuateDesiredDisplacement();
    }

    Vector3 GetOffsetPosition(Transform refTransform, bool isBottom) {
        return isBottom ? refTransform.position - (new Vector3(0.0f, refTransform.localScale.y, 0.0f) * 0.5f) : refTransform.position + (new Vector3(0.0f, refTransform.localScale.y, 0.0f) * 0.5f);
    }

    public void LaunchProjectile(bool direction) {
        CalcuateDesiredDisplacement();

        m_force.y = CalculateYImpulse(m_desiredDisplacement.y, m_time);
        if (direction)
        {
            m_force.z = (m_desiredDisplacement.z / m_time) * m_rb.mass;
        } else if (!direction)
        {
            m_force.x = (m_desiredDisplacement.x / m_time) * m_rb.mass;
        }
        
        m_rb.AddForce(m_force / Time.fixedDeltaTime);
        m_force = Vector3.zero;
    }

    void CalcuateDesiredDisplacement() {
        Vector3 ourPos = GetOffsetPosition(this.transform, true);
        Vector3 refPos = GetOffsetPosition(m_referenceTransform, false);
        m_desiredDisplacement = refPos - ourPos;
    }

    float CalculateYImpulse(float displacement, float time) {
        float velocity = (displacement - (0.5f * Physics.gravity.y * (time * time))) / (time);
        //since we are starting at rest, the difference in velocity is the velocity we calculated
        return velocity * m_rb.mass;
    }
}