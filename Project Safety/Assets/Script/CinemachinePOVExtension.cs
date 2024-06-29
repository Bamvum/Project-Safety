using UnityEngine;
using Cinemachine;

public class CinemachinePOVExtension : CinemachineExtension
{
    [Header("Script")]
    [SerializeField] PlayerMovement PlayerMovement;
    [SerializeField] DeviceManager deviceManager;

    [Space(10)]
    [SerializeField] float mouseSensitivity= 21.9f;
    [SerializeField] float clampUpperAngle = 40;
    [SerializeField] float clampBottomAngle = 60;
    Vector3 startingRotation;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(vcam.Follow)
        {
            Debug.Log("VCam Follow!!");
            if(stage == CinemachineCore.Stage.Aim)
            {
                if(startingRotation == null) startingRotation = transform.localRotation.eulerAngles;
                startingRotation.x += PlayerMovement.horizontalLookInput * mouseSensitivity * Time.deltaTime;
                startingRotation.y += PlayerMovement.verticalLookInput *  mouseSensitivity * Time.deltaTime;
                startingRotation.y = Mathf.Clamp(startingRotation.y, -clampBottomAngle, clampUpperAngle);
                state.RawOrientation = Quaternion.Euler(-startingRotation.y , startingRotation.x, 0f);
            }
        }
    }
}
