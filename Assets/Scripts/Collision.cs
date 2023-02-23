using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    [Header("Layers")]
    public LayerMask groundLayer;

    [Space]

    public bool onGround;
    public bool onWall;
    public bool onRightWall;
    public bool onLeftWall;
    public int wallSide;

    [Space]

    [Header("Collision")]

    public Vector2 collisionSize = new Vector2(0.25f, 0.25f);
    [SerializeField] protected float bottomAngle, rightAngle, leftAngle;
    public Vector2 bottomOffset, rightOffset, leftOffset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        onGround = Physics2D.OverlapBox((Vector2)transform.position + bottomOffset, collisionSize, bottomAngle, groundLayer);
        onWall = Physics2D.OverlapBox((Vector2)transform.position + rightOffset, collisionSize, rightAngle, groundLayer)
            || Physics2D.OverlapBox((Vector2)transform.position + leftOffset, collisionSize, leftAngle, groundLayer);

        onRightWall = Physics2D.OverlapBox((Vector2)transform.position + leftOffset, collisionSize, leftAngle, groundLayer);
        onLeftWall = Physics2D.OverlapBox((Vector2)transform.position + rightOffset, collisionSize, rightAngle, groundLayer);

        wallSide = onRightWall ? -1 : 1;

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };
        Gizmos.DrawWireCube((Vector2)transform.position + bottomOffset, collisionSize);
        Gizmos.DrawWireCube((Vector2)transform.position + rightOffset, new Vector2(collisionSize.y, collisionSize.x));
        Gizmos.DrawWireCube((Vector2)transform.position + leftOffset, new Vector2(collisionSize.y, collisionSize.x));
    }
}
