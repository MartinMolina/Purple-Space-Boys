
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : SpaceBoy
{
    Vector2 aimDirection;

    void Update()
    {

        if (active)
        {
            if (movesLeft > 0)
            {
                if (Input.GetKeyDown(KeyCode.W) && !IsObstructed(Vector2.up))
                {
                    Move(Vector2.up);
                }
                else if (Input.GetKeyDown(KeyCode.A) && !IsObstructed(Vector2.left))
                {
                    transform.right = Vector2.left;
                    Move(Vector2.right);
                }
                else if (Input.GetKeyDown(KeyCode.S) && !IsObstructed(Vector2.down))
                {
                    Move(Vector2.down);
                }
                else if (Input.GetKeyDown(KeyCode.D) && !IsObstructed(Vector2.right))
                {
                    transform.right = Vector2.right;
                    Move(Vector2.right);
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    aimDirection = new Vector2(0, Input.GetAxisRaw("Vertical"));
                }
                else
                {
                    aimDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0);
                    transform.right = aimDirection;
                }

                if (weapon != null)
                {
                    Fire(aimDirection);
                }


            }
        }
    }


    bool IsObstructed(Vector2 direction)
    {
        //return Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.1f) + direction / 2, direction, 0.5f, LayerMask.GetMask("Player", "Obstacle", "Wall"));
        return Physics2D.BoxCast(new Vector2(transform.position.x, transform.position.y) + direction, new Vector2(0.25f, 0.25f), 0, Vector2.zero, 0, LayerMask.GetMask("Player", "Obstacle", "Wall"));
    }

}



