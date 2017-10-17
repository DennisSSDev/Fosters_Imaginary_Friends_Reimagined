using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Script : MonoBehaviour {
    // Use this for initialization
    public float speed;//variable determining how fast the tank will move
    //public Vector3 speed;
    public Vector3 velocity;
    public Vector3 dir;
    float i = 0f;
    float j = 0f;
    float totalRot = 0f;
    public float _tempH;
    public float _tempW;
    //for acceleration 
    public Vector3 store_;
    public Vector3 acceleration;
    public float accelRate;
    public float maxSpeed;
    public Quaternion child_Rotation;
    public float deacceleration;
    public KeyCode leftMove;
    public KeyCode rightMove;
    public KeyCode UpMove;

    private PvPCollision det;
    // Update is called once per frame
    private void Start()
    {
        det = GetComponent<PvPCollision>();
    }
    void Update()
    {
        Wrap();

        if (Input.GetKey(leftMove))//rotation conditions
        {
            i += 1f;
            j += 0.1f;
            dir = Quaternion.Euler(0, 0, 5f) * dir;
            totalRot += j;
            store_ = dir;
        }


        else if (Input.GetKey(rightMove))
        {

            i -= 1f;
            j -= 0.1f;
            dir = Quaternion.Euler(0, 0, -5f) * dir;
            totalRot += j;
            store_ = dir;
        }

        if (Input.GetKey(UpMove))//Acceleration condition
        {
            acceleration = accelRate * dir;
            velocity += acceleration;

        }

        else//if we release the key for acceleration we deaccelerate
        {
            velocity *= deacceleration;
        }
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);

        //transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, j);
        //transform.GetChild(0).transform.position = CountArrowLocation(i);
        transform.GetChild(0).transform.position = transform.position + dir * 1.5f;
        //transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(transform.position.y, transform.position.x) * Mathf.Rad2Deg);
        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(transform.GetChild(0).transform.position.y - transform.position.y,
            transform.GetChild(0).transform.position.x - transform.position.x) * Mathf.Rad2Deg);
        child_Rotation = transform.GetChild(0).transform.rotation;
        transform.position += velocity;

        if (det.output)
        {
            Spawn_Management var = FindObjectOfType<Spawn_Management>();
            var.alive = false;
            det.output = false;

        }
    }
    public void Wrap()//method for wrapping the Tank on screen
    {
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        _tempH = height / 2f;
        _tempW = width / 2f;
        if (transform.position.x >= width / 2f)
        {
            Vector3 _newPos = new Vector3(-1 * transform.position.x + 0.2f, transform.position.y, transform.position.z);
            transform.position = _newPos;

        }
        else if (transform.position.x <= -1 * width / 2f)
        {
            Vector3 _newPos = new Vector3(-1 * transform.position.x - 0.2f, transform.position.y, transform.position.z);
            transform.position = _newPos;
        }
        else if (transform.position.y <= -1 * height / 2f)
        {
            Debug.Log("I'm here");
            Vector3 _newPos = new Vector3(transform.position.x, -1 * transform.position.y - 0.2f, transform.position.z);
            transform.position = _newPos;
        }
        else if (transform.position.y >= height / 2f)
        {
            Vector3 _newPos = new Vector3(transform.position.x, -1 * transform.position.y + 0.2f, transform.position.z);
            transform.position = _newPos;
        }
    }
    Vector3 CountArrowLocation(float angle)
    {
        float x = (Mathf.Cos(angle / 12f) * 2f) + transform.position.x;
        float y = (Mathf.Sin(angle / 12f) * 2f) + transform.position.y;
        return new Vector3(x, y, 0f);
    }
    void CountRotationOfArrow(Transform tran)
    {
        float angle = Mathf.Atan2(transform.position.x, transform.position.y) * Mathf.Rad2Deg;
        tran.rotation = Quaternion.Euler(0, 0, angle + 30);
    }
}

