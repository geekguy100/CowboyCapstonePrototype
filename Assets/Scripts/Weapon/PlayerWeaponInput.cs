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

        Vector3 mousePos;




        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePos.z = 0;

        spreadUI?.UpdateAimLines(weapon.GetBulletRotation(mousePos), weapon.GetBloomValue(), transform, mousePos);

        if (Input.GetMouseButtonDown(0) || Input.GetAxis("PrimaryAttack") < -.01f)
        {
            weapon.Shoot(mousePos, transform);
        }
        else if (Input.GetKeyDown(KeyCode.R))
            weapon.Reload();
    }


}
