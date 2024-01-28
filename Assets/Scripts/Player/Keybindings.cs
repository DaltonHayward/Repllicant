using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Keybindings", menuName = "Keybindings")]
public class Keybindings : ScriptableObject
{
    public KeyCode up, down, left, right, rotateCameraRight, rotateCameraLeft, interact, pause, inventory, primaryAttack, dodge;

    public KeyCode CheckKey(string key) 
    {
        switch (key)
        {
            case "up":
                return up;

            case "down":
                return down;

            case "left":
                return left;

            case "right":
                return right;

            case "rotateCameraRight":
                return rotateCameraRight;
            
            case "rotateCameraLeft":
                return rotateCameraLeft;

            case "interact":
                return interact;

            case "pause":
                return pause;

            case "inventory":
                return inventory;

            case "primaryAttack":
                return primaryAttack;

            case "dodge":
                return dodge;

            default:
                return KeyCode.None;
        }
    }
}
