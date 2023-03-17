using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    [Header("Scripts Ref:")]
    public GrapplingRope grappleRope;

    [Header("Layers Settings:")]
    [SerializeField] private bool grappleToAll = false;
    [SerializeField] private int grappableLayerNumber = 9;

    [Header("Main Camera:")]
    public Camera m_camera;

    [Header("Transform Ref:")]
    public Transform gunHolder;
    public Transform gunPivot;
    public Transform firePoint;

    [Header("Physics Ref:")]
    public DistanceJoint2D m_distanceJoint2D;
    public Rigidbody2D m_rigidbody;

    [Header("Rotation:")]
    [SerializeField] private bool rotateOverTime = true;
    [Range(0, 60)][SerializeField] private float rotationSpeed = 4;

    [Header("Distance:")]
    [SerializeField] private bool hasMaxDistance = false;
    [SerializeField] private float maxDistance = 20;

    private enum LaunchType
    {
        Transform_Launch,
        Physics_Launch
    }

    [Header("Launching:")]
    [SerializeField] private bool launchToPoint = true;
    [SerializeField] private LaunchType launchType = LaunchType.Physics_Launch;
    [SerializeField] private float launchSpeed = 1;

    [Header("No Launch To Point")]
    [SerializeField] private bool autoConfigureDistance = false;
    [SerializeField] private float targetDistance = 3;
    [SerializeField] private float targetFrequncy = 1;

    [HideInInspector] public Vector2 grapplePoint;
    [HideInInspector] public Vector2 grappleDistanceVector;

    private float _previousGravity = 1;
    private bool _emptyGrapple = false;
    private AudioSource _audio;

    private void Start()
    {
        grappleRope.enabled = false;
        m_distanceJoint2D.enabled = false;
        _previousGravity = m_rigidbody.gravityScale;
        _audio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            SetGrapplePoint();
            _audio.Play();
        }
        else if (Input.GetKey(KeyCode.Mouse1) && !(_emptyGrapple && grappleRope.waveSize <= 0.5))
        {
            if (grappleRope.enabled)
            {
                RotateGun(grapplePoint, false);
            }
            else
            {
                Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
                RotateGun(mousePos, true);
            }

            m_distanceJoint2D.distance += 2 * Input.mouseScrollDelta.y;

            var pRB = gunHolder.GetComponent<Rigidbody2D>();
            var jointPos = m_distanceJoint2D.connectedAnchor;
            var playerPos = m_distanceJoint2D.attachedRigidbody.position;
            var jointVec = jointPos - playerPos;
            var angle = Vector2.Angle(Vector2.up, jointVec);

            // Debug.Log(angle);
            // angle += 90;
            angle *= Mathf.PI / 180; // Convert to radians
            angle = Mathf.Cos(angle);

            var xcomp = 1000 * pRB.gravityScale * angle * Time.deltaTime;
            if (jointVec.x < 0) {
                xcomp = -xcomp;
            }

            var gravVec = new Vector2(xcomp, 0);
            var pCol = gunHolder.GetComponent<Collision>();
            if (angle <= 0.95 && !pCol.onGround)
                pRB.AddForce(gravVec, ForceMode2D.Force);
            // Debug.Log("Vec: " + jointVec);
            // Debug.Log("Angle: " + angle);
            // Debug.Log("Xcomp: " + xcomp);
            // Debug.Log("---");

            if (launchToPoint && grappleRope.isGrappling)
            {
                if (launchType == LaunchType.Transform_Launch)
                {
                    Vector2 firePointDistance = firePoint.position - gunHolder.localPosition;
                    Vector2 targetPos = grapplePoint - firePointDistance;
                    gunHolder.position = Vector2.Lerp(gunHolder.position, targetPos, Time.deltaTime * launchSpeed);
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) || (_emptyGrapple && grappleRope.waveSize <= 0.5))
        {
            _emptyGrapple = false;
            grappleRope.enabled = false;
            m_distanceJoint2D.enabled = false;
            m_rigidbody.gravityScale = _previousGravity;
        }
        else
        {
            Vector2 mousePos = m_camera.ScreenToWorldPoint(Input.mousePosition);
            RotateGun(mousePos, true);
            m_distanceJoint2D.connectedAnchor = m_rigidbody.transform.position;
        }
    }

    void RotateGun(Vector3 lookPoint, bool allowRotationOverTime)
    {
        Vector3 distanceVector = lookPoint - gunPivot.position;

        float angle = Mathf.Atan2(distanceVector.y, distanceVector.x) * Mathf.Rad2Deg;
        if (rotateOverTime && allowRotationOverTime)
        {
            gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, Quaternion.AngleAxis(angle, Vector3.forward), Time.deltaTime * rotationSpeed);
        }
        else
        {
            gunPivot.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    void SetGrapplePoint()
    {
        Vector2 distanceVector = m_camera.ScreenToWorldPoint(Input.mousePosition) - gunPivot.position;
        if (Physics2D.Raycast(firePoint.position, distanceVector.normalized))
        {
            RaycastHit2D _hit = Physics2D.Raycast(firePoint.position, distanceVector.normalized);
            if (_hit.transform.gameObject.layer == grappableLayerNumber || grappleToAll)
            {
                if (Vector2.Distance(_hit.point, firePoint.position) <= maxDistance || !hasMaxDistance)
                {
                    grapplePoint = _hit.point;
                    grappleDistanceVector = grapplePoint - (Vector2)gunPivot.position;
                    grappleRope.enabled = true;
                }
                else
                {
                    _emptyGrapple = true;
                }
            }
        }
        else
        {
            _emptyGrapple = true;
        }

        if (_emptyGrapple)
        {
            grappleDistanceVector = distanceVector.normalized * maxDistance;
            grapplePoint = (Vector2)gunPivot.position + grappleDistanceVector;
            grappleRope.enabled = true;
        }
    }

    public void Grapple()
    {
        if (!_emptyGrapple)
        {
            m_distanceJoint2D.autoConfigureDistance = false;
            if (!launchToPoint && !autoConfigureDistance)
            {
                m_distanceJoint2D.distance = targetDistance;
                // m_distanceJoint2D.frequency = targetFrequncy;
            }
            if (!launchToPoint)
            {
                if (autoConfigureDistance)
                {
                    m_distanceJoint2D.autoConfigureDistance = true;
                    // m_distanceJoint2D.frequency = 0;
                }

                m_distanceJoint2D.connectedAnchor = grapplePoint;
                m_distanceJoint2D.enabled = true;
            }
            else
            {
                switch (launchType)
                {
                    case LaunchType.Physics_Launch:
                        m_distanceJoint2D.enabled = true;

                        m_distanceJoint2D.connectedAnchor = grapplePoint;

                        Vector2 distanceVector = grapplePoint - (Vector2)gunHolder.position;

                        m_distanceJoint2D.distance = distanceVector.magnitude;
                        // m_distanceJoint2D.distance += 5 * Input.mouseScrollDelta.y;




                        break;
                    case LaunchType.Transform_Launch:
                        _previousGravity = m_rigidbody.gravityScale;
                        m_rigidbody.gravityScale = 0;
                        m_rigidbody.velocity = Vector2.zero;
                        break;
                }
            }
        }

    }

    private void OnDrawGizmosSelected()
    {
        if (firePoint != null && hasMaxDistance)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(firePoint.position, maxDistance);
        }
    }

}
