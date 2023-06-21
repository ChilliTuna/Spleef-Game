using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;

    [SerializeField]
    private float maxSpeed = 5f;

    [SerializeField]
    private float spawnHeight;

    [SerializeField]
    private Vector2 vec2Move;

    [SerializeField]
    private Vector3 vec3Move;

    private CharacterController characterController;

    private WorldGenerator worldGenerator;

    private GameObject mainCam;

    private float camAngle;

    private Vector2 inputVec;

    private Rigidbody rigidBody;

    private PlayerGroundChecker groundChecker;

    public Vector3 vel;

    private ForceMode forceMode = ForceMode.Acceleration;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        spawnHeight = transform.position.y;
        worldGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldGenerator>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        rigidBody = GetComponent<Rigidbody>();
        groundChecker = transform.Find("GroundChecker").gameObject.GetComponent<PlayerGroundChecker>();
        GetCameraAngle();
    }

    private void Update()
    {
        if(groundChecker.currentCollisions > 0)
        {
            rigidBody.maxLinearVelocity = maxSpeed;
            forceMode = ForceMode.Acceleration;
            rigidBody.drag = 10;
        }
        else
        {
            rigidBody.maxLinearVelocity = 1000;
            forceMode = ForceMode.Force;
            rigidBody.drag = 0;
        }
        vel = rigidBody.velocity;
        MoveChar();
    }

    private void GetCameraAngle()
    {
        Vector3 realAngle = (worldGenerator.origin - mainCam.transform.position).normalized;
        camAngle = Vector2.SignedAngle(new Vector2(realAngle.x, realAngle.z), new Vector2(0, 1));
    }

    private void OnMove(InputValue inputVal)
    {
        inputVec = inputVal.Get<Vector2>();
        vec2Move = Quaternion.AngleAxis(camAngle, -Vector3.forward) * inputVec.normalized * moveSpeed;
    }

    //Changes y input to z movement
    private Vector3 ConstructMoveVec(Vector2 moveInput)
    {
        return new Vector3(moveInput.x, 0, moveInput.y);
    }

    private void MoveChar()
    {
        vec3Move = ConstructMoveVec(vec2Move);

        rigidBody.AddForce(vec3Move, forceMode);
        //characterController.Move(vec3Move);
    }
}