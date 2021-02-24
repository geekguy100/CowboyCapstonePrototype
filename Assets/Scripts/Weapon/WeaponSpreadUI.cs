/*****************************************************************************
// File Name :         WeaponSpreadUI.cs
// Author :            Kyle Grenier
// Creation Date :     02/22/2021
//
// Brief Description : Script to control displaying a WeaponSpreadUI for a weapon user.
*****************************************************************************/
using UnityEngine;

public class WeaponSpreadUI : MonoBehaviour
{
    public GameObject leftAimLine;
    public GameObject rightAimLine;
    public GameObject centerLine;
    private SpriteRenderer lal;
    private SpriteRenderer ral;
    private SpriteRenderer ctrline;

    [Tooltip("Thickness of aim sprite height")]
    public float defaultHeightOfAimSprite = .04f;

    [Tooltip("Length of aim sprite")]
    public float scaleOfAimingLine = 5f;

    [Tooltip("Length of bloom sprites")]
    public float scaleOfBloomLines = 4f;

    private void Awake()
    {
        lal = leftAimLine.GetComponent<SpriteRenderer>();
        ral = rightAimLine.GetComponent<SpriteRenderer>();
        ctrline = centerLine.GetComponent<SpriteRenderer>();
        SetDistanceOfAim();
    }

    /// <summary>
    /// Updates the aim lines to show the weapon spread.
    /// </summary>
    /// <param name="rotationOfAim">The rotation of the bullet to be taken.</param>
    /// <param name="bloomValue">The weapon's bloom value.</param>
    /// <param name="weaponHolder">The Transform using the weapon.</param>
    public void UpdateAimLines(Quaternion rotationOfAim, float bloomValue, Transform weaponHolder, Vector3 targetPosition)
    {
        //obtain the rotation of player aim
        Quaternion adj = rotationOfAim;

        //calculate rotation of left aim line
        adj *= Quaternion.AngleAxis(bloomValue - 90, Vector3.forward);

        //rotate left aim line
        leftAimLine.transform.rotation = adj;

        //reset rotation
        adj = rotationOfAim;

        //calculate rotation of right aim line
        adj *= Quaternion.AngleAxis(-bloomValue - 90, Vector3.forward);

        //rotate right aim line
        rightAimLine.transform.rotation = adj;

        Vector3 rotOfAimEuler = rotationOfAim.eulerAngles;
        rotOfAimEuler.z -= 90;

        centerLine.transform.rotation = Quaternion.Euler(rotOfAimEuler);

        HandleSpriteFlipping(weaponHolder, targetPosition);
        
    }

    /// <summary>
    /// Handles flipping the aim line sprites based on the direction the weapon holder should be looking.
    /// </summary>
    /// <param name="weaponHolder">The Transform using the Weapon.</param>
    /// <param name="targetPosition">The position of the target the weapon holder wants to shoot at.</param>
    private void HandleSpriteFlipping(Transform weaponHolder, Vector3 targetPosition)
    {
        Vector3 dir = (weaponHolder.position - targetPosition);
        if (dir.x > 0 && !ctrline.flipX)
        {
            ctrline.flipX = true;
            lal.flipX = true;
            ral.flipX = true;
        }
        else if (dir.x < 0 && ctrline.flipX)
        {
            ctrline.flipX = false;
            lal.flipX = false;
            ral.flipX = false;
        }
    }

    //sets the length of the aiming line
    private void SetDistanceOfAim()
    {
        //set the initial scale of the Aiming Line
        ctrline.size = new Vector2(scaleOfAimingLine, defaultHeightOfAimSprite);
        lal.size = new Vector2(scaleOfBloomLines, defaultHeightOfAimSprite);
        ral.size = new Vector2(scaleOfBloomLines, defaultHeightOfAimSprite);
    }
}