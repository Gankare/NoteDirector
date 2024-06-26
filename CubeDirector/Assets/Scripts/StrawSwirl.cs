using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StrawSwirl : MoveStraw
{
    public GameObject Swirl;
    private Vector2 direction;
    public void StartSucking()
    { 
        //Add later to when voice is activated
        Achivements.Instance.SuckMade();
    }
    public void ChangeDirection(Vector2 getDirection)
    {
        //Same line in suck.cs, remove one of them ?
        direction = getDirection;
    }
    void Update()
    {
        //Add on trigger enter instead and only enable trigger when voice is activated 
    }
}
