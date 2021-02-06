using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShooting : MonoBehaviour
{
    public float scaleOfAimingLine = 5f;
    public float defaultHeightOfAimSprite = .1f;
    public SpriteRenderer spr;
    public Vector2 positionToSpawnBullet;
    public Quaternion rotationOfAim;
    public float aimPerspectiveAngle = 30;
    public GameObject Bullet;
    public float bulletSpawnOffset = 0f;
    public Vector2 aimOffset = new Vector2(0, 0f);

    public float testValue;


    public void Awake()
    {
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
        if (Input.GetMouseButtonDown(0))
        {
            LaunchBullet();
        }
    }

    private void SetAimRotation()
    {
        //obtain the position to point aim at
        Vector2 direction = GetMousePosition() - (Vector2)transform.position;

        //calculate the angle to turn the aim line
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

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
        GameObject bullet = Instantiate(Bullet, new Vector2(transform.position.x,transform.position.y + bulletSpawnOffset), rotationOfAim);
        bullet.GetComponent<BulletScript>().origin = gameObject;
    }

    //sets the length of the aiming line
    private void SetDistanceOfAim()
    {
        //set the initial scale of the Aiming Line
        spr.size = new Vector2(scaleOfAimingLine, defaultHeightOfAimSprite);
    }
}
