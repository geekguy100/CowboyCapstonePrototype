using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShooting : MonoBehaviour
{
    private float scaleOfAimingLine = 5f;
    private float defaultHeightOfAimSprite = .1f;
    private SpriteRenderer spr;
    [Tooltip("Where to spawn bullet on player")]
    public Vector2 positionToSpawnBullet;
    [Tooltip("The rotation of the aim should correlate with player rotation")]
    public Quaternion rotationOfAim;
    [Tooltip("adjusts perspective scale or aim rotation")]
    public float aimPerspectiveAngle = 30;
    [Tooltip("bullet prefab object")]
    public GameObject Bullet;
    public float bulletSpawnOffset = 0f;

    public Vector2 aimOffset = new Vector2(0, 0f);
    [Tooltip("amout of time shot is held, will corelate with bloom")]
    public float heldTime = 1.25f;
    [Tooltip("amount of time shot needs to be held to zero in on enemy")]
    public float heldTimeReset = 1.25f;

    [Tooltip("amount of bloom that will sway a bullet after calculations")]
    public float bloomValue;

    [Tooltip("value that determines max and min bloom")]
    public float bloomMod = 8f;

    public GameObject leftAimLine;
    public GameObject rightAimLine;
    private SpriteRenderer lal;
    private SpriteRenderer ral;


    public void Awake()
    {
        lal = leftAimLine.GetComponent<SpriteRenderer>();
        ral = rightAimLine.GetComponent<SpriteRenderer>();

        //get sprite renderer
        spr = gameObject.GetComponent<SpriteRenderer>();

        //get position to spawn bullet
        positionToSpawnBullet = new Vector2(transform.position.x, transform.position.y + bulletSpawnOffset);

        //set the initial distance that the aim line is drawn
        SetDistanceOfAim();
    }

    public void Update()
    {
        //set the rotation of aim line
        SetAimRotation();

        
        SetDistanceOfAim();

        //check if mouse input
        if (Input.GetMouseButton(0))
        {
            if (heldTime > 0)
            {
                heldTime -= Time.deltaTime;
            }
            else
            {
                heldTime = 0;
            }
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            LaunchBullet();
            heldTime = heldTimeReset;
        }

        adjustBloom();
        updateAimLines();
    }

    private void SetAimRotation()
    {
        //obtain the position to point aim at
        Vector2 direction = GetMousePosition() - (Vector2)transform.position;

        //calculate the angle to turn the aim line
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //adjust rotation of angle by perspective
        rotationOfAim = Quaternion.AngleAxis(aimPerspectiveAngle, Vector3.right);

        //set the rotation of the aim
        rotationOfAim *= Quaternion.AngleAxis(angle, Vector3.forward);



        //set the rotation of the aim
        transform.rotation = rotationOfAim;

    }

    // obtains the mouse position
    private Vector2 GetMousePosition()
    {
        //get position of mouse in pixels
        Vector2 position = Input.mousePosition;

        //transfer pixel location to real world location
        position = Camera.main.ScreenToWorldPoint(position);

        //offset shot
        position -= aimOffset;

        //return position to be used
        return position;
    }

    //Will use this later to accomplish joystick aiming
    public void GetJoystickAxis()
    {

    }

    private void LaunchBullet()
    {
        GameObject bullet = Instantiate(Bullet, new Vector2(transform.position.x,transform.position.y + bulletSpawnOffset), getBulletAngle());
        bullet.GetComponent<BulletScript>().origin = gameObject;
    }

    //gets a bullet angle based on bloomvalue
    private Quaternion getBulletAngle()
    {
        Quaternion ang = rotationOfAim;

        //get the adjustment value
       float adjVal = Random.Range(-bloomValue, bloomValue + 1);

        //set the rotation of the shot
        ang *= Quaternion.AngleAxis(adjVal,Vector3.forward);

        return ang;
    }

    //sets the length of the aiming line
    private void SetDistanceOfAim()
    {
        //set the initial scale of the Aiming Line
        spr.size = new Vector2(scaleOfAimingLine, defaultHeightOfAimSprite);
        lal.size = new Vector2(scaleOfAimingLine, defaultHeightOfAimSprite);
        ral.size = new Vector2(scaleOfAimingLine, defaultHeightOfAimSprite);
    }
    

    private void adjustBloom()
    {
        bloomValue = heldTime * bloomMod;

        //adjust aim lines here
    }

    private void updateAimLines()
    {
        Quaternion adj = rotationOfAim;
        adj *= Quaternion.AngleAxis(bloomValue, Vector3.forward);
        leftAimLine.transform.rotation = adj;

        adj = rotationOfAim;
        adj *= Quaternion.AngleAxis(-bloomValue, Vector3.forward);
        rightAimLine.transform.rotation = adj;

        if (bloomValue > 0)
        {
            lal.color = Color.white;
            ral.color = Color.white;
            spr.color = Color.white;
        }
        else
        {
            lal.color = Color.green;
            ral.color = Color.green;
            spr.color = Color.green;
        }
    }
}
