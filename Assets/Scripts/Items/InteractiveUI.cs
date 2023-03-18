using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public enum State
{
    CanInteract,
    CanSeePossibleInteraction,
    CantInteract
}

public class InteractiveUI : MonoBehaviour
{
    [HideInInspector] public State currentState;

    private float dist, dotStartScale, interactionStartScale;
    [SerializeField] private float visibleInteractionDist = 5f;
    [SerializeField] private Transform interactionUIPos;

    private GameObject cam;
    private GameObject dot, interaction;

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        dot = Instantiate(InteractionHelper.instance.dotPrefab, interactionUIPos.position, Quaternion.identity, transform);
        interaction = Instantiate(InteractionHelper.instance.interactionPrefab, interactionUIPos.position, Quaternion.identity, transform);
        dot.SetActive(false);
        interaction.SetActive(false);
        currentState = State.CantInteract;
        dotStartScale = dot.transform.localScale.x;
        interactionStartScale = interaction.transform.localScale.x;
    }

    private void Update()
    {
        dist = Vector3.Distance(cam.transform.position, interactionUIPos.position);
        if (currentState != State.CanInteract && dist <= visibleInteractionDist)
        {
            currentState = State.CanSeePossibleInteraction;
        }
        else if (dist > visibleInteractionDist)
        {
            currentState = State.CantInteract;
        }
        

        switch (currentState) {
            case State.CanInteract:
                dot.SetActive(false);
                interaction.SetActive(true);
                interaction.transform.LookAt(cam.transform.position);
                interaction.transform.Rotate(new Vector3(90, 0, 0));
                float interactionScale = interactionStartScale * dist * 0.5f;
                interaction.transform.localScale = new Vector3(interactionScale, interactionScale, interactionScale);
                break;

            case State.CanSeePossibleInteraction:
                dot.SetActive(true);
                interaction.SetActive(false);
                dot.transform.LookAt(cam.transform.position);
                dot.transform.Rotate(new Vector3(90, 0, 0));
                float dotScale = dotStartScale * dist * 0.5f;
                dot.transform.localScale = new Vector3(dotScale, dotScale, dotScale);
                break;

            case State.CantInteract:
                dot.SetActive(false);
                interaction.SetActive(false);
                break;
        }
    }

    private void OnDestroy()
    {
        Destroy(dot);
        Destroy(interaction);
    }

}