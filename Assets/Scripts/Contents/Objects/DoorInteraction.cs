using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : MonoBehaviour,IInteractable
{
    public GameObject Player { get; set; }
    public bool CanInteract { get; set; }

    public void Interact()
    {
        throw new System.NotImplementedException();
    }
}
