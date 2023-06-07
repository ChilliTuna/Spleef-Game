using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;

    [SerializeField]
    private float spawnHeight;

    [SerializeField]
    private Vector2 vec2Move;

    [SerializeField]
    private Vector3 vec3Move;

    private CharacterController characterController;

    private WorldGenerator worldGenerator;

    private GameObject mainCam;

    [SerializeField]
    private Vector2 camAngle;

    [SerializeField]
    private Vector2 inputVec;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        spawnHeight = transform.position.y;
        worldGenerator = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldGenerator>();
        mainCam = GameObject.FindGameObjectWithTag("MainCamera");
        GetCameraAngle();
    }

    private void Update()
    {
        MoveChar();
    }

    //Changes y input to z movement
    private Vector3 ConstructMoveVec(Vector2 moveInput)
    {
        return new Vector3(vec2Move.x, 0, vec2Move.y);
    }
    private void GetCameraAngle()
    {
        Vector3 realAngle = (worldGenerator.origin - mainCam.transform.position).normalized;
        camAngle = new Vector2(realAngle.x, realAngle.z);
    }

    private void OnMove(InputValue inputVal)
    {
        inputVec = inputVal.Get<Vector2>();
        vec2Move = (camAngle * inputVec.y + new Vector2(-camAngle.x, camAngle.y) * inputVec.x).normalized * moveSpeed;
        
    }

    private void MoveChar()
    {
        vec3Move = ConstructMoveVec(vec2Move) * Time.deltaTime;

        characterController.Move(vec3Move);
    }

}