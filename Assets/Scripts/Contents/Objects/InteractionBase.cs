using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class InteractionBase : MonoBehaviour,IInteractable
{
    public GameObject Player { get; set; }
    public bool CanInteract { get; set; }

    // Start is called before the first frame update
    public virtual void Start()
    {
        Player = GameObject.FindGameObjectWithTag(TAG_PLAYER);
    }

    public virtual void Update()
    {
        if (CanInteract && Input.GetKeyDown(KeyCode.UpArrow))
        {
            Interact();
        }
    }
    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            CanInteract = true;

        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            CanInteract = false;
        }
    }

    public virtual void Interact()
    {
       
    }

  
}
