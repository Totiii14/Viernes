using System.Collections;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float smoothing; 
    [SerializeField] private int maxLookRotation;

    private Vector2 smoothedVelocity;
    private Vector2 currentLookingPos; 
    private float recoilRecoveryDelay = 0.1f; 
    private Vector2 initialLookingPos;

    private Vector3 lastPosition; 
    private bool hasMoved = false; 


    public TutorialPanelController tutorialPanelController; 
    public Vector2 RecoilOffset { get; set; }
    public float RecoilRecoverySpeed { get; set; } 

    public void ApplyRecoilWithRecovery(Vector2 recoil, float recoverySpeed, Vector2 initialPos)
    {
        initialLookingPos = initialPos; 
        RecoilOffset += recoil;
        RecoilRecoverySpeed = recoverySpeed;
        StartCoroutine(RecoverRecoilAfterDelay(recoilRecoveryDelay));
    }
    public Vector2 GetCurrentLookingPos()
    {
        return currentLookingPos;
    }

    private IEnumerator RecoverRecoilAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        while (RecoilOffset != Vector2.zero)
        {
            RecoilOffset = Vector2.MoveTowards(RecoilOffset, Vector2.zero, RecoilRecoverySpeed * Time.deltaTime);
            currentLookingPos = Vector2.Lerp(currentLookingPos, initialLookingPos, RecoilRecoverySpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void Start()
    {
        player = transform.parent.gameObject != null ? transform.parent.gameObject : gameObject;
        lastPosition = transform.position; 
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
    }

    private void Update()
    {
        RotateCamera(); 


        if (transform.position != lastPosition && !hasMoved)
        {
            hasMoved = true;
            tutorialPanelController.ShowTutorial(); 
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            tutorialPanelController.ShowTutorial(); 
        }

        lastPosition = transform.position; 
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x,
            player.transform.position.y + 0.48f,
            player.transform.position.z);
    }

    private void RotateCamera()
    {
        Vector2 inputValues = new Vector2(Input.GetAxisRaw("Mouse X"),
            Input.GetAxisRaw("Mouse Y")); 

        inputValues = Vector2.Scale(inputValues,
            new Vector2(lookSensitivity * smoothing, lookSensitivity * smoothing));

        smoothedVelocity.x = Mathf.Lerp(smoothedVelocity.x, inputValues.x, 1f / smoothing);
        smoothedVelocity.y = Mathf.Lerp(smoothedVelocity.y, inputValues.y, 1f / smoothing);

        currentLookingPos += smoothedVelocity + RecoilOffset; 

        Vector2 recoverySpeed = RecoilOffset * RecoilRecoverySpeed * Time.deltaTime;

        RecoilOffset -= recoverySpeed;

        currentLookingPos.y = Mathf.Clamp(currentLookingPos.y, -maxLookRotation, maxLookRotation);

        transform.localRotation = Quaternion.AngleAxis(-currentLookingPos.y, Vector3.right);
        player.transform.localRotation = Quaternion.AngleAxis(currentLookingPos.x, player.transform.up);
    }
}