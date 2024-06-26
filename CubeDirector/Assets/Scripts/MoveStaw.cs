using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveStraw : MonoBehaviour
{
    private Vector3[] regions;
    private int currentRegionIndex = -1; // No region selected initially
    private Vector3 targetPosition;
    public InputActionAsset actions;
    private StrawSwirl swirlScript;
    private InputAction aButtonAction;
    private InputAction dButtonAction;
    private InputAction sButtonAction;
    private InputAction wButtonAction;

    private float cd = 0.25f;
    private float timeBetweenDirections = 1f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        swirlScript = GetComponent<StrawSwirl>();
        // Check if actions are assigned
        if (actions == null)
        {
            Debug.LogError("InputActionAsset is not assigned.");
            return;
        }

        // Retrieve the action map (assuming it's named "Player")
        var actionMap = actions.FindActionMap("Player");
        if (actionMap == null)
        {
            Debug.LogError("Action map 'Player' not found.");
            return;
        }

        // Initialize actions
        aButtonAction = actionMap.FindAction("A");
        dButtonAction = actionMap.FindAction("D");
        sButtonAction = actionMap.FindAction("S");
        wButtonAction = actionMap.FindAction("W");

        // Check if actions are found
        if (aButtonAction == null || dButtonAction == null || sButtonAction == null || wButtonAction == null)
        {
            Debug.LogError("One or more actions not found in the InputActionAsset.");
            return;
        }

        // Add event listeners
        aButtonAction.performed += ChangeStrawToLeft;
        dButtonAction.performed += ChangeStrawToRight;
        sButtonAction.performed += ChangeStrawToBottom;
        wButtonAction.performed += ChangeStrawToTop;

        // Enable actions
        aButtonAction.Enable();
        dButtonAction.Enable();
        sButtonAction.Enable();
        wButtonAction.Enable();
        actions.Enable();

        // Define regions
        regions = new Vector3[4];
        regions[0] = new Vector3(0, 0.5f, 0); // Left edge midpoint
        regions[1] = new Vector3(1, 0.5f, 0); // Right edge midpoint
        regions[2] = new Vector3(0.5f, 0, 0); // Bottom edge midpoint
        regions[3] = new Vector3(0.5f, 1, 0); // Top edge midpoint
    }

    private void OnDisable()
    {
        if (aButtonAction != null) aButtonAction.performed -= ChangeStrawToLeft;
        if (dButtonAction != null) dButtonAction.performed -= ChangeStrawToRight;
        if (sButtonAction != null) sButtonAction.performed -= ChangeStrawToBottom;
        if (wButtonAction != null) wButtonAction.performed -= ChangeStrawToTop;

        if (actions != null)
        {
            actions.Disable();
        }
    }

    void Update()
    {
        timeBetweenDirections += Time.deltaTime;
        if (currentRegionIndex != -1)
        {
            FollowMouseWithinRegion();
        }
    }

    public void ChangeStrawToLeft(InputAction.CallbackContext ctx)
    {
        if (timeBetweenDirections > cd && currentRegionIndex != 0)
        {
            swirlScript.StopSucking(ctx);
            timeBetweenDirections = 0;
            swirlScript.ChangeDirection(Vector2.left);
            SetRegion(0);
        }
    }

    public void ChangeStrawToRight(InputAction.CallbackContext ctx)
    {
        if (timeBetweenDirections > cd && currentRegionIndex != 1) 
        {
            swirlScript.StopSucking(ctx);
            timeBetweenDirections = 0;
            swirlScript.ChangeDirection(Vector2.right);
            SetRegion(1);
        }
    }

    public void ChangeStrawToBottom(InputAction.CallbackContext ctx)
    {
        if (timeBetweenDirections > cd && currentRegionIndex != 2)
        {
            swirlScript.StopSucking(ctx);
            timeBetweenDirections = 0;
            swirlScript.ChangeDirection(Vector2.down);
            SetRegion(2);
        }
    }

    public void ChangeStrawToTop(InputAction.CallbackContext ctx)
    {
        if (timeBetweenDirections > cd && currentRegionIndex != 3)
        {
            swirlScript.StopSucking(ctx);
            timeBetweenDirections = 0;
            swirlScript.ChangeDirection(Vector2.up);
            SetRegion(3);
        }
    }

    void SetRegion(int regionIndex)
    {
        currentRegionIndex = regionIndex;
        Vector3 viewportPosition = regions[regionIndex];
        targetPosition = Camera.main.ViewportToWorldPoint(new Vector3(viewportPosition.x, viewportPosition.y, Camera.main.nearClipPlane + 10));
        transform.position = targetPosition;
    }

    void FollowMouseWithinRegion()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 viewportPosition = Camera.main.ScreenToViewportPoint(mousePosition);

        // Adjust the viewportPosition to clamp it along the edge
        switch (currentRegionIndex)
        {
            case 0: // Left edge
                viewportPosition.x = 0; // Fixed x position for left edge
                viewportPosition.y = Mathf.Clamp(viewportPosition.y, 0, 1);
                transform.eulerAngles = new Vector3(0,0,-90);
                break;
            case 1: // Right edge
                viewportPosition.x = 1; // Fixed x position for right edge
                viewportPosition.y = Mathf.Clamp(viewportPosition.y, 0, 1);
                transform.eulerAngles = new Vector3(0, 0, 90);
                break;
            case 2: // Bottom edge
                viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0, 1);
                viewportPosition.y = 0; // Fixed y position for bottom edge
                transform.eulerAngles = new Vector3(0, 0, 0);
                break;
            case 3: // Top edge
                viewportPosition.x = Mathf.Clamp(viewportPosition.x, 0, 1);
                viewportPosition.y = 1; // Fixed y position for top edge
                transform.eulerAngles = new Vector3(0, 0, -180);
                break;
        }

        Vector3 worldPosition = Camera.main.ViewportToWorldPoint(new Vector3(viewportPosition.x, viewportPosition.y, Camera.main.nearClipPlane + 10));
        transform.position = worldPosition;
    }
}
