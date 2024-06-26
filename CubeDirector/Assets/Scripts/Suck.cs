using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Suck : MoveStraw
{
    private Vector2 direction;

    public void ChangeDirection(Vector2 getDirection)
    {
        direction = getDirection;
    }
}
