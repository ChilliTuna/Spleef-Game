using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float force = 100f;

    public float midAirForce = 10f;

    [SerializeField]
    private float maxSpeed = 10f;

    [SerializeField]
    private Vector3 spawnPosition;

    [SerializeField]
    private float despawnHeight = -5f;

    [SerializeField]
    private Vector2 vec2Move;

    [SerializeField]
    private Vector3 vec3Move;

    [SerializeField]
    private Vector3 vel;

    [SerializeField]
    private bool inAir = false;

    [SerializeField]
    private PhysicMaterial standardMaterial;

    [SerializeField]
    private PhysicMaterial inAirMaterial;

    private CharacterController characterController;

    private WorldGenerator worldGenerator;

    private GameObject mainCam;

    private float camAngle;

    private Vector2 inputVec;

    private Rigidbody rigidBody;

    private PlayerGroundChecker groundChecker;

    private ForceMode forceMode = ForceMode.Acceleration;

    private CapsuleCollider capsuleCollider;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        spawnPosition = transform.position;
        worldGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldGenerator>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        rigidBody = GetComponent<Rigidbody>();
        groundChecker = transform.Find("GroundChecker").gameObject.GetComponent<PlayerGroundChecker>();
        GetCameraAngle();
    }

    private void Update()
    {
        if (groundChecker.currentCollisions > 0)
        {
            OnGround();
        }
        else
        {
            InAir();
        }
        vel = rigidBody.velocity;
        MoveChar();
    }

    private void OnGround()
    {
        if (inAir == true)
        {
            inAir = false;
            rigidBody.maxLinearVelocity = maxSpeed;
            forceMode = ForceMode.Acceleration;
            rigidBody.drag = 10;
            capsuleCollider.material = standardMaterial;
        }
    }

    private void InAir()
    {
        if (inAir == false)
        {
            inAir = true;
            rigidBody.maxLinearVelocity = 100;
            forceMode = ForceMode.Force;
            rigidBody.drag = 0;
            capsuleCollider.material = inAirMaterial;
        }
        if (transform.position.y <= despawnHeight)
        {
            Respawn();
        }
    }

    private void GetCameraAngle()
    {
        Vector3 realAngle = (worldGenerator.origin - mainCam.transform.position).normalized;
        camAngle = Vector2.SignedAngle(new Vector2(realAngle.x, realAngle.z), new Vector2(0, 1));
    }

    private void OnMove(InputValue inputVal)
    {
        inputVec = inputVal.Get<Vector2>();
        vec2Move = Quaternion.AngleAxis(camAngle, -Vector3.forward) * inputVec.normalized * force;
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

    private void Respawn()
    {
        transform.position = spawnPosition;
        rigidBody.velocity = Vector3.zero;
    }
}