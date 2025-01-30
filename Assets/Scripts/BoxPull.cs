using UnityEngine;

public class BoxPulling : MonoBehaviour
{
    public float pullDistance = 0.5f; // Max distance to attach for pulling
    public string boxTag = "Box"; // Tag for pullable boxes
    public KeyCode pullKey = KeyCode.Space;

    private Rigidbody2D playerRb;
    private FixedJoint2D joint;
    private Collider2D currentBox;

    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        joint = gameObject.AddComponent<FixedJoint2D>();
        joint.enabled = false; // Initially disabled
    }

    void Update()
    {
        // Check if the player is near a box
        if (Input.GetKeyDown(pullKey))
        {
            Collider2D[] nearbyColliders = Physics2D.OverlapCircleAll(transform.position, pullDistance);

            foreach (Collider2D collider in nearbyColliders)
            {
                if (collider.CompareTag(boxTag))
                {
                    currentBox = collider;
                    AttachToBox();
                    break;
                }
            }
        }

        // Detach when the key is released
        if (Input.GetKeyUp(pullKey) && joint.enabled)
        {
            DetachFromBox();
        }
    }

    void AttachToBox()
    {
        if (currentBox != null)
        {
            Rigidbody2D boxRb = currentBox.GetComponent<Rigidbody2D>();
            if (boxRb != null)
            {
                joint.connectedBody = boxRb;
                joint.enabled = true;
                Debug.Log("Box gwabbed");
            }
        }
    }

    void DetachFromBox()
    {
        joint.enabled = false;
        joint.connectedBody = null;
        currentBox = null;
        Debug.Log("Box wet go");
    }

    void FixedUpdate()
    {
        if (joint.enabled && currentBox != null)
        {
            Vector2 direction = (Vector2)(currentBox.transform.position - transform.position);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, pullDistance);

            // Detach if there's an obstacle between the player and the box
            if (hit.collider != null && hit.collider != currentBox)
            {
                //DetachFromBox();
            }
        }
    }

    void OnDrawGizmos()
    {
        // Visualize the pull distance in the scene view
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, pullDistance);
    }
}