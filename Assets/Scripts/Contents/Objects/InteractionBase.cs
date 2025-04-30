using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class InteractionBase : MonoBehaviour, IInteractable
{
    [HideInInspector]
    public InteractionType _interactionType;
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
            UIManager.Instance.InteractionStartUI.SetUI(_interactionType, this.gameObject);

            UIManager.Instance.InteractionStartUI.FadeIn();
        }
    }

    public virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == Player)
        {
            CanInteract = false;
            UIManager.Instance.InteractionStartUI.FadeOut();
        }
    }

    public virtual void Interact()
    {

    }


}
