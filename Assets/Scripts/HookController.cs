using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private int range;
    [SerializeField] private int pullDistance;
    [SerializeField] private float speed;
    private Vector2 outPos;
    private Vector2 turnPos;
    private bool isReturning;
    private bool flag;

    // Start is called before the first frame update
    void Start()
    {
        outPos = transform.position;
        body.velocity *= speed;
        if (body.velocity.y == 0)
        {
            transform.position += new Vector3(0, 1, 0);
            GetComponent<BoxCollider2D>().offset = new Vector2(0, -1);
        }
    }

    void FixedUpdate()
    {
        if (!isReturning && Vector2.Distance(transform.position, outPos) > range)
        {
            transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
            TurnBack();
        }

    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (flag)
        {
            if (!isReturning)
            {
                transform.position = coll.transform.position;
                TurnBack();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void OnCollisionStay2D(Collision2D coll)
    {
        if (flag)
        {
            if (Vector2.Distance(transform.position, outPos) > 1 && Vector2.Distance(transform.position, turnPos) < pullDistance)
            {
                coll.transform.position = transform.position;
            }
            else
                coll.transform.position = new Vector2(Mathf.Round(coll.transform.position.x), Mathf.Round(coll.transform.position.y));
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        flag = true;
    }

    void TurnBack()
    {
        turnPos = transform.position;
        body.velocity *= -1;
        isReturning = true;
    }


}

