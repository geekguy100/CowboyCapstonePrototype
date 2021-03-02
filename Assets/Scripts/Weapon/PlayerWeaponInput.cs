/*****************************************************************************
// File Name :         PlayerWeaponInput.cs
// Author :            Kyle Grenier
// Creation Date :     02/20/2021
//
// Brief Description : Script to handle player input to shoot weapons.
*****************************************************************************/
using UnityEngine;

public class PlayerWeaponInput : MonoBehaviour
{
    [Tooltip("The Weapon to fire.")]
    [SerializeField] private Weapon weapon;

    [Tooltip("The WeaponSpreadUI to display the weapon's spread.")]
    [SerializeField] private WeaponSpreadUI spreadUI;

    [Tooltip("Is the player using a controller")]
    [SerializeField] private bool gamePadInput = false;

    private void Start()
    {
        if (weapon == null)
        {
            Debug.LogWarning("No weapon could be found on the player.");
            Destroy(this);
            return;
        }
    }

    private void Update()
    {
        //detect inputs
        DetectInputs();

        Vector3 mousePos;

        if (gamePadInput)
        {
            mousePos = (Vector3)GetJoystickAxis();
        }
        else
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        mousePos.z = 0;

        spreadUI?.UpdateAimLines(weapon.GetBulletRotation(mousePos), weapon.GetBloomValue(), transform, mousePos);

        if (Input.GetMouseButtonDown(0)|| Input.GetAxis("PrimaryAttack") < -.01f)
        {
            weapon.Shoot(mousePos, transform);
        }
        else if (Input.GetKeyDown(KeyCode.R))
            weapon.Reload();
    }

    public Vector2 GetJoystickAxis()
    {
        //adjust the rotationial input
        var angH = Input.GetAxis("RightHorizontal") * 60;
        var angV = Input.GetAxis("RightVertical") * 45;

        /*
        //if no input return a more appealing location
        if (angH == 0 && angV == 0)
        {
            return (Vector2)transform.position;
        }*/



        //return the angle of the joystick input
        return new Vector2(-angV, -angH) - (Vector2)transform.position;
    }

    //detect what inputs the player is using
    private void DetectInputs()
    {
        //determine if joystick in use
        if (GetJoystickAxis() != (Vector2)transform.position)
        {
            //set input to gamepad
            gamePadInput = true;
        }

        //determine if mouse click
        if (Input.GetMouseButtonDown(0))
        {
            //set input to keyboard and mouse
            gamePadInput = false;
        }
    }
}
