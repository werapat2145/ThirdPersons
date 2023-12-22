using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private float playerSpeed = 6f;
    [SerializeField] private Camera followCamera;
    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 playerVelocity;
    [SerializeField] private float gravityValue = -13f;

    public bool groundedPlayer;
    [SerializeField] private float jumpHeight = 2.5f;
    // Start is called before the first frame update

    public Animator animator;
    public static playercontroller instance;

    private void Awake()
    {
        instance = this; 
    }
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (checkwinner.instance.isWinner)
        {
            case true:
                animator.SetBool("victory", checkwinner.instance.isWinner); 
                break;
            case false:
                Movement();
                break;
        }
       
    }

    void Movement()
    {
        groundedPlayer = characterController.isGrounded;
        if(characterController.isGrounded && playerVelocity.y <- 2f )
        {
            playerVelocity.y = -1f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0)
                                * new Vector3(horizontalInput, 0, verticalInput);
        Vector3 moveDirection = movementInput.normalized;

        characterController.Move(moveDirection * playerSpeed * Time.deltaTime);

        if (moveDirection != Vector3.zero)
        {
            Quaternion desiredRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }

        if(Input.GetButtonDown ("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3f * gravityValue);
            animator.SetTrigger("jumping");
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity* Time.deltaTime);

        animator.SetFloat("speed",Mathf.Abs(moveDirection.x) + Mathf.Abs(moveDirection.z));
        animator.SetBool("ground", characterController.isGrounded);


    }
 
}
