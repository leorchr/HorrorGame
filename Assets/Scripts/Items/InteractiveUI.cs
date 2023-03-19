using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public enum State
{
    CanInteract,
    CanSeePossibleInteraction,
    CantInteract
}

public class InteractiveUI : MonoBehaviour
{
    #region UI Setup
    [Header("===== UI Setup =====")]
    [SerializeField] private float visibleInteractionDist = 5f;
    [SerializeField] private Transform interactionUIPos;

    [HideInInspector] public State currentState;
    private float dist, startScale;
    private GameObject cam;
    private GameObject dot, interaction;

    #endregion

    #region Debug

    [Header("===== Debug =====")]
    [SerializeField] private bool visualDebugging = true;
    [SerializeField] private Color sphereColor = new Color(0.2f, 0.75f, 0.2f, 0.15f);

    #endregion

    private void Start()
    {
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        dot = Instantiate(InteractionHelper.instance.dotPrefab, interactionUIPos.position, Quaternion.identity, transform);
        interaction = Instantiate(InteractionHelper.instance.interactionPrefab, interactionUIPos.position, Quaternion.identity, transform);
        dot.SetActive(false);
        interaction.SetActive(false);
        currentState = State.CantInteract;
        startScale = dot.transform.localScale.x;
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
                float interactionScale = startScale * dist * 0.5f;
                interaction.transform.localScale = new Vector3(interactionScale, interactionScale, interactionScale);
                break;

            case State.CanSeePossibleInteraction:
                dot.SetActive(true);
                interaction.SetActive(false);
                dot.transform.LookAt(cam.transform.position);
                dot.transform.Rotate(new Vector3(90, 0, 0));
                float dotScale = startScale * dist * 0.5f;
                dot.transform.localScale = new Vector3(dotScale, dotScale, dotScale);
                break;

            case State.CantInteract:
                dot.SetActive(false);
                interaction.SetActive(false);
                break;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!visualDebugging) return;
        Handles.color = sphereColor;
        Handles.RadiusHandle(Quaternion.identity, interactionUIPos.position, visibleInteractionDist);
    }
#endif

    private void OnDestroy()
    {
        Destroy(dot);
        Destroy(interaction);
    }

}