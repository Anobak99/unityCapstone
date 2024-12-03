using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapRope : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;
    public bool ropeHolding = false;
    private DistanceJoint2D ropeJoint;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        ropeJoint = gameObject.AddComponent<DistanceJoint2D>();
        ropeJoint.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (ropeHolding)
            {
                DetachFromRope();
            }
            else
            {
                TryAttachToRope();
            }
        }
    }

    void TryAttachToRope()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5f);
        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerRigidbody =collider.GetComponent<Rigidbody2D>();
                playerRigidbody.gravityScale = 0;

                ropeJoint.connectedBody = playerRigidbody;
                ropeJoint.autoConfigureDistance = false;
                ropeJoint.distance = 0.5f;
                ropeJoint.enabled = true;
                ropeHolding = true;
                break;
            }
        }
    }

    void DetachFromRope()
    {
        ropeJoint.connectedBody = null;
        ropeJoint.enabled = false;
        ropeHolding = false;
    }
}
