/*****************************************************************************
// File Name :         WeaponSpreadUI.cs
// Author :            Kyle Grenier
// Creation Date :     #CREATIONDATE#
//
// Brief Description : ADD BRIEF DESCRIPTION OF THE FILE HERE
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
    public void UpdateAimLines(Quaternion rotationOfAim, float bloomValue)
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