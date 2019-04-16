using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // MOVEMENT
    bool escape = false;
    Vector2 mouseLook;
    Vector2 smoothV;

    public float speed = 10.0f;
    public float sensitivity = 5.0f;
    public float smoothing = 2.0f;

    GameManager manager;
    GameObject cameraChild;

    //SHOOT
    public GameObject bulletPrefab;
    Transform bulletSpawn;
    public float bulletSpeed = 60f;
    public float maxBullets = 20f;
    public float totalBullets = 20f;
    float addBulletEverySeconds = 2;

    void Start()
    {
        manager = FindObjectOfType<GameManager>();

        Cursor.lockState = CursorLockMode.Locked;
        foreach (Transform child in transform)
        {
            if (child.tag == "MainCamera")
            {
                cameraChild = child.gameObject;
            }
            else if (child.tag == "BulletSpawn")
            {
                bulletSpawn = child;
            }
        }

        InvokeRepeating("AddBullet", 0, addBulletEverySeconds);

        manager.ImHere();
    }

    void Update()
    {
        if (!manager.isEnded())
            if (!escape)
            {
                //MOVEMENT
                float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
                float straffe = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

                transform.Translate(straffe, 0, translation);

                //ROTATION
                Vector2 md = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
                md = Vector2.Scale(md, new Vector2(sensitivity * smoothing, sensitivity * smoothing));
                smoothV.x = Mathf.Lerp(smoothV.x, md.x, 1f / smoothing);
                smoothV.y = Mathf.Lerp(smoothV.y, md.y, 1f / smoothing);
                mouseLook += smoothV;

                cameraChild.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
                bulletSpawn.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);

                transform.rotation = Quaternion.AngleAxis(mouseLook.x, transform.up);

                // SHOOT
                if (Input.GetMouseButtonDown(0))
                {
                    InvokeRepeating("Fire", .001f, .1f);
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    CancelInvoke("Fire");
                }

                if (Input.GetKeyDown("escape"))
                {
                    Cursor.lockState = CursorLockMode.None;
                    escape = true;
                }
            }
    }

    void Fire()
    {
        if (totalBullets > 0)
        {
            // Create the Bullet from the Bullet Prefab
            var bullet = (GameObject)Instantiate(
                bulletPrefab,
                bulletSpawn.position,
                bulletSpawn.rotation);

            // Add velocity to the bullet
            bullet.GetComponent<Rigidbody>().velocity = bullet.transform.forward * bulletSpeed;

            // Destroy the bullet after 2 seconds
            Destroy(bullet, 2.0f);

            totalBullets--;
        }
    }


    void AddBullet()
    {
        if (totalBullets < maxBullets)
            totalBullets++;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "NPC")
        {
            manager.EndGame();
        }
    }
}
