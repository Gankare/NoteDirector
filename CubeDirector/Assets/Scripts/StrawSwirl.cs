using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StrawSwirl : MoveStraw
{
    public GameObject Swirl;
    private InputAction suckStart;
    private InputAction suckStop;
    private bool SuckActivated;
    private Vector2 direction;
    private float raycastLenght = 20f;

    void Start()
    {
        var actionMap = actions.FindActionMap("Player");
        suckStart = actionMap.FindAction("StartSuck");
        suckStop = actionMap.FindAction("StopSuck");

        suckStart.performed += StartSucking;
        suckStop.performed += StopSucking;

        suckStart.Enable();
        suckStop.Enable();
        actions.Enable();
    }
    private void OnDisable()
    {
        if (suckStart != null) suckStart.performed -= StartSucking;
        if (suckStop != null) suckStop.performed -= StopSucking;

        if (actions != null)
        {
            actions.Disable();
        }
    }

    public void StartSucking(InputAction.CallbackContext ctx)
    { 
        Swirl.SetActive(true);
        SuckActivated = true;
        Achivements.Instance.SuckMade();
    }
    public void StopSucking(InputAction.CallbackContext ctx)
    {
        Swirl.SetActive(false);
        SuckActivated = false;

    }
    public void ChangeDirection(Vector2 getDirection)
    {
        direction = getDirection;
    }
    void Update()
    {
        if (SuckActivated)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -direction * raycastLenght);
            if (hit.collider.GetComponent<Ball>() != null)
            {
            }
        }
    }
}
