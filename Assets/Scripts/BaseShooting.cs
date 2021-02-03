using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShooting : MonoBehaviour
{
    public float scaleOfAimingLine = 10f;
    public float defaultHeightOfAimSprite = .1f;
    public SpriteRenderer spr;
    public Vector2 positionToSpawnBullet;
    public Quaternion rotationOfAim;
    public GameObject Bullet;


    public void Awake()
    {
        //get sprite renderer
        spr = gameObject.GetComponent<SpriteRenderer>();

        //get position to spawn bullet
        positionToSpawnBullet = new Vector2(transform.position.x, transform.position.y + 1);

        SetDistanceOfAim();
    }

    public void Update()
    {
        SetAimRotation();
        SetDistanceOfAim();
        if (Input.GetMouseButtonDown(0))
        {
            LaunchBullet();
        }
    }

    private void SetAimRotation()
    {
        Vector2 direction = GetMousePosition() - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotationOfAim = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotationOfAim;
    }

    // obtains the mouse position
    private Vector2 GetMousePosition()
    {
        Vector2 position = Input.mousePosition;
        position = Camera.main.ScreenToWorldPoint(position);
        return position;
    }

    //Will use this later to accomplish joystick aiming
    public void GetJoystickAxis()
    {

    }

    private void LaunchBullet()
    {
        Debug.Log("Bang");
        Instantiate(Bullet, positionToSpawnBullet, rotationOfAim);

    }

    //sets the length of the aiming line
    private void SetDistanceOfAim()
    {
        //set the initial scale of the Aiming Line
        spr.size = new Vector2(scaleOfAimingLine, defaultHeightOfAimSprite);
    }





}
