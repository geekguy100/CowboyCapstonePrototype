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
    [SerializeField] private Weapon weapon;

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
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;

            weapon.Shoot(mousePos);
        }
        else if (Input.GetKeyDown(KeyCode.R))
            weapon.Reload();
    }
}
