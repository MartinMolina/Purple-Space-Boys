using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    [SerializeField] private string item;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!collision.GetComponent<Animator>().GetBool(item) && item == "Launcher")
            {
                Destroy(gameObject);
                collision.GetComponent<Animator>().SetBool(item, true);
            }

            if (item == "Health")
            {
                Destroy(gameObject);
                collision.GetComponent<PlayerController>().health += 50;
            }
        }
    }
}
