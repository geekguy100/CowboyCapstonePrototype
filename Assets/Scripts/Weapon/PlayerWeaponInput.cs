/*****************************************************************************
// File Name :         PlayerWeaponInput.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : ADD BRIEF DESCRIPTION OF THE FILE HERE
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
            Vector3 dir = (mousePos - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            Quaternion bulletRotation = Quaternion.Euler(0, 0, angle - 90);
            weapon.Shoot(bulletRotation);
            print("PENITS");
        }
    }
}
