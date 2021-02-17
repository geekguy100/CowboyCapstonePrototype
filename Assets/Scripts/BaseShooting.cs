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

    [Tooltip("current amount of bullets not in the gun")]
    public int extraBulletCount = 36;

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
    public BulletUI bu;
    public BulletCountUI bcu;
    private SpriteRenderer characterSPR;

    [Tooltip("The weapon used to shoot with.")]
    [SerializeField] private Weapon weapon;

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


        characterSPR = gameObject.transform.parent.GetComponentInParent<SpriteRenderer>();
    }

    public void Start()
    {
        bcu.UpdateCount(extraBulletCount);
    }

    public void Update()
    {
        //detect inputs
        DetectInputs();

        //set the rotation of aim line
        SetAimRotation();

        //set the distance of the aim lines //NOT NECESSARY FOR UPDATE
        //SetDistanceOfAim();

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
                //increase recoil time
                recoilTime += recoilAmount;

                //launch bullet
                LaunchBullet();

                //remove bullet
                bulletCount--;


                bu.DecreaseBullets(1);

                //wind timer for pause between shots
                shotPause = maxShotPause;
            }
        }
        else if (shotPause > 0)
        {
            //deduct time between pause
            shotPause -= Time.deltaTime;
        }
        else
        {
            //set pause to zero if below zero
            shotPause = 0;
        }

        //determine if reload needs to occur
        if (bulletCount == 0 && extraBulletCount > 0 || Input.GetKeyDown(KeyCode.R) && bulletCount < maxBulletCount && extraBulletCount > 0 || reloading)
        {
            ReloadWeapon();

            bcu.UpdateCount(extraBulletCount);
        }

        //adjust the bloom ui
        AdjustBloom();

        //update the colors and sizes of the aim lines
        UpdateAimLines();
    }

    public void ReloadWeapon()
    {
        //check if reloading is occuring
        if (reloading == false)
        {
            //set reloading to occuring
            reloading = true;

            //set reloading timer
            reloadTime = reloadTimeMax;
        }
        else
        {
            //deduct time
            reloadTime -= Time.deltaTime;
            //if time is up
            if (reloadTime < 0)
            {
                //reset the amount of bullets in gun
                while (extraBulletCount > 0 && bulletCount < maxBulletCount)
                {
                    bulletCount++;
                    extraBulletCount--;

                    bu.IncreaseBullets(1);
                }


                //set timer to zero
                reloadTime = 0;


                //set reloading to false
                reloading = false;
            }
        }
    }

    //set the rotation of the players aim
    private void SetAimRotation()
    {
        //variable to hold the direction of player aim
        Vector2 direction;

        //determine if keyboard/mouse or gamepad
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

        UpdatePlayerRotation();

    }

    private void UpdatePlayerRotation()
    {
        if (rotationOfAim.w >= .7071068f)
        {
            characterSPR.flipX = true;
        }
        else
        {
            characterSPR.flipX = false;
        }
    }

    // obtains the mouse position
    private Vector2 GetMousePosition()
    {
        //get position of mouse in pixels
        Vector2 position = Input.mousePosition;

        //if no gamepad is being used
        if (!gamePadInput)
        {
            //transfer pixel location to real world location
            position = Camera.main.ScreenToWorldPoint(position);

            //offset shot
            position -= aimOffset;
        }
        else
        {
            //if gamepad is being used set the rotation to nothing
            position = new Vector2();
        }

        //return position to be used
        return position;
    }

    //Will use this later to accomplish joystick aiming
    public Vector2 GetJoystickAxis()
    {
        //adjust the rotationial input
        var angH = Input.GetAxis("RightHorizontal") * 60;
        var angV = Input.GetAxis("RightVertical") * 45;

        //if no input return a more appealing location
        if (angH == 0 && angV == 0)
        {
            return (Vector2)transform.position;
        }

        //return the angle of the joystick input
        return new Vector2(-angV, -angH);
    }

    //fires a bullet
    public void LaunchBullet()
    {
        //create bullet // save object data
        GameObject bullet = Instantiate(Bullet, new Vector2(transform.position.x, transform.position.y + bulletSpawnOffset), GetBulletAngle());
        Physics2D.IgnoreCollision(bullet.GetComponent<Collider2D>(), GetComponentInParent<Collider2D>()); //Igonre collison between our bullet and our body; we don't want to take damage from our own bullet!
        //^^Added by Kyle

        //set the origin in bulletscript
        bullet.GetComponent<BulletScript>().origin = this.gameObject;


    }

    //gets a bullet angle based on bloomvalue
    private Quaternion GetBulletAngle()
    {
        //get the rotation of player angle
        Quaternion ang = rotationOfAim;

        //get the adjustment value
        float adjVal = Random.Range(-bloomValue - 3, bloomValue + 3);

        //set the rotation of the shot
        ang *= Quaternion.AngleAxis(adjVal, Vector3.forward);

        //return the angle
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

    //calculate the bloom
    private void AdjustBloom()
    {
        //calculate the bloom values
        bloomValue = recoilTime * bloomMod;

        //if the value is too large reduce it
        if (recoilTime > maximumBloom)
        {
            recoilTime = maximumBloom;
        }
    }

    //update the aim lines to match the bloom values
    private void UpdateAimLines()
    {
        //obtain the rotation of player aim
        Quaternion adj = rotationOfAim;

        //calculate rotation of left aim line
        adj *= Quaternion.AngleAxis(bloomValue, Vector3.forward);

        //rotate left aim line
        leftAimLine.transform.rotation = adj;

        //reset rotation
        adj = rotationOfAim;

        //calculate rotation of right aim line
        adj *= Quaternion.AngleAxis(-bloomValue, Vector3.forward);

        //rotate right aim line
        rightAimLine.transform.rotation = adj;

        //determie a modifier value to work with bloomvalue
        float modValue = .01f * maximumBloom;

        //calculate a value for the red value
        float tempValR = modValue * bloomValue;

        //calcultes a value for the green value // NOT CURRENTLY USED
        //float tempValG = 1 - modValue * bloomValue;

        //produce a color
        Color tempCol = new Color(tempValR, 1, tempValR, aimLineTransparency);

        //set colors
        lal.color = tempCol;
        ral.color = tempCol;
        spr.color = tempCol;


        //change order of aim line sprites
        /*
        if(transform.rotation.z < 0)
        {
            lal.sortingOrder = 1;
            ral.sortingOrder = 1;
            spr.sortingOrder = 1;
        }
        else
        {
            lal.sortingOrder = -1;
            ral.sortingOrder = -1;
            spr.sortingOrder = -1;
        }
        */
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
