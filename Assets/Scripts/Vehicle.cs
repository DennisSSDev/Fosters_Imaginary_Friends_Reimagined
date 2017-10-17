using UnityEngine;
/// <summary>
/// This is the movement script for the player, which will also allow him to wrap aroudn the screen 
/// +pass along the info from collsion script
/// </summary>
public class Vehicle : MonoBehaviour {
    // Use this for initialization
    public float speed;//variable determining how fast the object will move
    //public Vector3 speed;
    public Vector3 velocity;
    public Vector3 dir;
    private float i = 0f;
    private float j = 0f;
    private float totalRot = 0f;
    public float _tempH;
    public float _tempW;
    public Vector3 store_;
    public Vector3 acceleration;
    public float accelRate;
    public float maxSpeed;
    public Quaternion child_Rotation;
    public float deacceleration;
    public GameObject self;
    private CollisionDetection det;
    /// <summary>
    /// SInce the values of the collision will be important we'll store a reference
    /// </summary>
    private void Start()
    {
        det = GetComponent<CollisionDetection>();
    }
    /// <summary>
    /// Wrap the player on screen
    /// Get the input and move an drotate the player according to vector math
    /// update the alive varibale for the spwn_Manager
    /// </summary>
    private void Update () {
        Wrap();
        if (Input.GetKey(KeyCode.LeftArrow))//rotation conditions
        {
            i += 1f;
            j += 0.1f;
            dir = Quaternion.Euler(0, 0, 5f)*dir;
            totalRot += j;
            store_ = dir;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {           
            i -= 1f;
            j -= 0.1f;
            dir = Quaternion.Euler(0, 0, -5f) * dir;
            totalRot += j;
            store_ = dir;
        }
        if (Input.GetKey(KeyCode.UpArrow))//Acceleration condition
        {
            acceleration = accelRate * dir;
            velocity += acceleration;   
        }
        else//if we release the key for acceleration we deaccelerate
        {
            velocity *= deacceleration;
        }
        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);//to reach a maximum speed I clamp the value
        transform.GetChild(0).transform.position = transform.position + dir*1.5f;
        transform.GetChild(0).transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(transform.GetChild(0).transform.position.y -transform.position.y,
            transform.GetChild(0).transform.position.x - transform.position.x ) * Mathf.Rad2Deg);
        child_Rotation = transform.GetChild(0).transform.rotation;
        transform.position += velocity;
        if (det.output)//pass along the output of the collision
        {
            Spawn_Management var = FindObjectOfType<Spawn_Management>();
            var.alive = false;
            det.output = false;
        }
    }
    /// <summary>
    /// This method will allow the player to stay in bounds of the camera with ease
    /// </summary>
    private void Wrap()
    {
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;
        _tempH = height/2f;
        _tempW = width/2f;
        if (transform.position.x >= width / 2f)
        {
            Vector3 _newPos = new Vector3(-1*transform.position.x+0.2f, transform.position.y, transform.position.z);
            transform.position = _newPos;     
        }
        else if (transform.position.x <= -1 * width / 2f)
        {
            Vector3 _newPos = new Vector3(-1 * transform.position.x - 0.2f, transform.position.y, transform.position.z);
            transform.position = _newPos;
        }
        else if(transform.position.y <= -1 * height / 2f)
        {
            Vector3 _newPos = new Vector3(transform.position.x, -1*transform.position.y-0.2f, transform.position.z);
            transform.position = _newPos;
        }
        else if(transform.position.y >= height / 2f)
        {
            Vector3 _newPos = new Vector3(transform.position.x, -1*transform.position.y + 0.2f, transform.position.z);
            transform.position = _newPos;
        }
    }
}
