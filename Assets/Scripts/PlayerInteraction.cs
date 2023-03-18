using UnityEngine;
using UnityEngine.InputSystem;

public enum InteractionType
{
    None,
    Pickup,
    Inspect,
    OpenDoor
}

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance;
    private Inventory _inventory;
    private InteractionType _possibleInteraction = InteractionType.None;
    private Pickable _possiblePickable;
    private Interactive _possibleInteractive;

    private void Start()
    {
        if (Instance) Destroy(this);
        else Instance = this;
        Invoke("SetInventory", 0.1f);
    }

    private void SetInventory()
    {
        _inventory = Inventory.Instance;
    }

    public void SetInteraction(InteractionType interaction)
    {
        _possibleInteraction = interaction;
        //montrer ici ou créer un script InteractionHelper qu'il est possible d'intéragir dans le jeu
        //highlight les contours de l'objet, afficher un icone de touche, etc...
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (_possibleInteraction != InteractionType.None && ctx.started)
        {
            if (_possibleInteraction == InteractionType.Pickup && _possiblePickable)
            {
                Pickup();
            }
            else if (_possibleInteraction != InteractionType.Pickup)
            {
                Interact();
            }
        }
    }

    private void Pickup()
    {
        _inventory.AddToInventory(_possiblePickable.item);
        _possiblePickable.gameObject.SetActive(false);
        SetInteraction(InteractionType.None);
    }

    private void Interact()
    {
        _possibleInteractive.OnInteraction();
        if (_possibleInteractive && _possibleInteractive.onlyOnce)
        {
            DisableInteractive();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Pickable"))
        {
            _possiblePickable = other.GetComponent<Pickable>();
            SetInteraction(InteractionType.Pickup);
            other.GetComponent<InteractiveUI>().currentState = State.CanInteract;
        }
        else if (other.transform.CompareTag("Interactive"))
        {
            Interactive interactive = other.GetComponent<Interactive>();
            if(interactive == null) return;
            _possibleInteractive = interactive;
            SetInteraction(_possibleInteractive.interactionType);
            other.GetComponent<InteractiveUI>().currentState = State.CanInteract;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Pickable") || other.transform.CompareTag("Interactive"))
        {
            StopInteractive();
            other.GetComponent<InteractiveUI>().currentState = State.CanSeePossibleInteraction;
        }
    }

    public void StopInteractive()
    {
        SetInteraction(InteractionType.None);
        _possibleInteractive = null;
    }
    
    private void DisableInteractive()
    {
        _possibleInteractive.GetComponent<SphereCollider>().enabled = false;
        Destroy(_possibleInteractive.gameObject.GetComponent<InteractiveUI>());
        Destroy(_possibleInteractive);
        SetInteraction(InteractionType.None);
    }
}
