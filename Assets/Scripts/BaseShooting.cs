using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShooting : MonoBehaviour
{
    [Tooltip("is gamepad being used")]
    public bool gamePadInput = false;

    [Tooltip("max time between stops")]
    public float maxShotPause = .33f;

    [Tooltip("time until next stop")]
    public float shotPause = 0f;

    [Tooltip("Max amount of bullets")]
    public int maxBulletCount = 6;

    [Tooltip("Current amount of bullets")]
    public int bulletCount = 6;

    [Tooltip("returns true when reloading")]
    public bool reloading = false;

    [Tooltip("amount of time it takes to reload")]
    public float reloadTimeMax = 2.5f;

    [Tooltip("current amount of time it takes to reload")]
    public float reloadTime = 0f;

    [Tooltip("Thickness of aim sprite height")]
    public float defaultHeightOfAimSprite = .04f;

    [Tooltip("Length of aim sprite")]
    public float scaleOfAimingLine = 5f;

    [Tooltip("Length of bloom sprites")]
    public float scaleOfBloomLines = 4f;

    [Tooltip("The transparency of the aim lines")]
    public float aimLineTransparency = .6f;

    [Tooltip("Where to spawn bullet on player")]
    public Vector2 positionToSpawnBullet;

    [Tooltip("The rotation of the aim should correlate with player rotation")]
    public Quaternion rotationOfAim;

    [Tooltip("adjusts perspective scale or aim rotation")]
    public float aimPerspectiveAngle = 30;

    [Tooltip("bullet prefab object")]
    public GameObject Bullet;

    [Tooltip("The offset of bullet spawn from weapon gameobject")]
    public float bulletSpawnOffset = 0f;

    [Tooltip("Offset the aim lines from the cursor")]
    public Vector2 aimOffset = new Vector2(0, 0f);

    [Tooltip("amout of time shot is held, will corelate with bloom")]
    public float recoilTime = 0f;

    [Tooltip("amount of time shot needs to be held to zero in on enemy")]
    public float recoilAmount = 1f;

    [Tooltip("amount of bloom that will sway a bullet after calculations")]
    public float bloomValue;

    [Tooltip("value that determines max and min bloom")]
    public float bloomMod = 8f;

    [Tooltip("The maxmimum amount of bloom")]
    public float maximumBloom = 3f;

    public GameObject leftAimLine;
    public GameObject rightAimLine;
    private SpriteRenderer lal;
    private SpriteRenderer ral;
    private SpriteRenderer spr;

    public void Awake()
    {
        // obtain sprite renderers from the bloom lines
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
        //detect inputs
        DetectInputs();



        //set the rotation of aim line
        SetAimRotation();

        //set the distance of the aim lines
        SetDistanceOfAim();

        //if recoil is occuring
        if (recoilTime > 0)
        {
            //decrease recoil
            recoilTime -= Time.deltaTime;
        }
        else
        {
            // keep recoil at 0
            recoilTime = 0;
        }

        //limits speed shots can be fired
        if (shotPause == 0)
        {
            //check if mouse input
            if (Input.GetMouseButtonDown(0) && bulletCount > 0 || Input.GetAxis("PrimaryAttack") < -.01f && bulletCount > 0)
            {
                recoilTime += recoilAmount;
                LaunchBullet();
                bulletCount--;
                shotPause = maxShotPause;

            }

        }
        else if (shotPause > 0)
        {
            shotPause -= Time.deltaTime;
        }
        else
        {
            shotPause = 0;
        }


        if (bulletCount == 0 || Input.GetKeyDown(KeyCode.R) && bulletCount < maxBulletCount || reloading)
        {
            if (reloading == false)
            {
                reloading = true;
                reloadTime = reloadTimeMax;
            }
            else
            {
                reloadTime -= Time.deltaTime;
                if (reloadTime < 0)
                {
                    reloadTime = 0;
                    bulletCount = maxBulletCount;
                    reloading = false;
                }
            }
        }

        AdjustBloom();
        UpdateAimLines();
    }

    private void SetAimRotation()
    {
        Vector2 direction;

        if (gamePadInput == true)
        {
            //obtain the position to point aim at
            direction = GetJoystickAxis() - (Vector2)transform.position;
        }
        else
        {
            //obtain the position to point aim at
            direction = GetMousePosition() - (Vector2)transform.position;
        }

        //calculate the angle to turn the aim line
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        //adjust rotation of angle by perspective
        //rotationOfAim = Quaternion.AngleAxis(aimPerspectiveAngle, Vector3.right);

        //set the rotation of the aim
        rotationOfAim = Quaternion.AngleAxis(angle, Vector3.forward);

        //set the rotation of the aim
        transform.rotation = rotationOfAim;

    }

    // obtains the mouse position
    private Vector2 GetMousePosition()
    {
        //get position of mouse in pixels
        Vector2 position = Input.mousePosition;

        if (!gamePadInput)
        {
            //transfer pixel location to real world location
            position = Camera.main.ScreenToWorldPoint(position);

            //offset shot
            position -= aimOffset;
        }
        else
        {
            position = new Vector2();
        }


        //return position to be used
        return position;
    }

    //Will use this later to accomplish joystick aiming
    public Vector2 GetJoystickAxis()
    {
        var angH = Input.GetAxis("RightHorizontal") * 60;
        var angV = Input.GetAxis("RightVertical") * 45;

        //if no input return a more appealing location
        if(angH == 0 && angV == 0)
        {
            return (Vector2)transform.position;
        }

        return new Vector2(-angV, -angH);
    }

    private void LaunchBullet()
    {
        GameObject bullet = Instantiate(Bullet, new Vector2(transform.position.x, transform.position.y + bulletSpawnOffset), GetBulletAngle());
        bullet.GetComponent<BulletScript>().origin = gameObject;
    }

    //gets a bullet angle based on bloomvalue
    private Quaternion GetBulletAngle()
    {
        Quaternion ang = rotationOfAim;

        //get the adjustment value
        float adjVal = Random.Range(-bloomValue - 3, bloomValue + 3);

        //set the rotation of the shot
        ang *= Quaternion.AngleAxis(adjVal, Vector3.forward);

        return ang;
    }

    //sets the length of the aiming line
    private void SetDistanceOfAim()
    {
        //set the initial scale of the Aiming Line
        spr.size = new Vector2(scaleOfAimingLine, defaultHeightOfAimSprite);
        lal.size = new Vector2(scaleOfBloomLines, defaultHeightOfAimSprite);
        ral.size = new Vector2(scaleOfBloomLines, defaultHeightOfAimSprite);
    }


    private void AdjustBloom()
    {

        bloomValue = recoilTime * bloomMod;

        if (recoilTime > maximumBloom)
        {
            recoilTime = maximumBloom;
        }


    }

    private void UpdateAimLines()
    {
        Quaternion adj = rotationOfAim;
        adj *= Quaternion.AngleAxis(bloomValue, Vector3.forward);
        leftAimLine.transform.rotation = adj;

        adj = rotationOfAim;
        adj *= Quaternion.AngleAxis(-bloomValue, Vector3.forward);
        rightAimLine.transform.rotation = adj;

        float modValue = .01f * maximumBloom;

        float tempValR = modValue * bloomValue;
        float tempValG = 1 - modValue * bloomValue;
        Color tempCol = new Color(tempValR, 1, tempValR, aimLineTransparency);


        if (bloomValue > 0)
        {
            lal.color = tempCol;
            ral.color = tempCol;
            spr.color = tempCol;
        }
        else
        {
            lal.color = tempCol;
            ral.color = tempCol;
            spr.color = tempCol;
        }
    }

    private void DetectInputs()
    {
        if (GetJoystickAxis() != (Vector2)transform.position)
        {
            gamePadInput = true;
        }
        
        if(Input.GetMouseButtonDown(0))
        {

            gamePadInput = false;
        }
    }
}
