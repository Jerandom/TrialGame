using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using StarterAssets;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField]
    private float normalSensitivity;
    [SerializeField]
    private float aimSensitivity;
    [SerializeField]
    private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField]
    private GameObject Crosshair;
    [SerializeField]
    GameObject pfArrowObject;
    [SerializeField]
    Transform ArrowPoint;
    Vector3 mouseWorldPosition = Vector3.zero;

    private ThirdPersonController thirdPersonController;
    private StarterAssetsInputs starterAssetsInputs;

    [SerializeField]
    private Transform debugTransform;

    private void Awake()
    {
        starterAssetsInputs = GetComponent<StarterAssetsInputs>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        Crosshair = GameObject.Find("Crosshair");
    }

    void Update()
    {
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2);

        //shoot a ray to the center of the screen to get mouse position
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask))
        {
            //get mouse position from raycast
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
        }

        if (starterAssetsInputs.isAiming && starterAssetsInputs.isEquiped)
        {
            aimVirtualCamera.gameObject.SetActive(true);
            Crosshair.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);

            //store aim direction from the player
            Vector3 worldAimTarget = mouseWorldPosition;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
        }
        else
        {
            aimVirtualCamera.gameObject.SetActive(false);
            Crosshair.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
        }
    }

    public void OnShootArrow()
    {
        Vector3 aimDir = (mouseWorldPosition - ArrowPoint.position).normalized;
        Vector3 rotation = pfArrowObject.transform.eulerAngles.normalized;

        GameObject arrow = Instantiate(pfArrowObject, ArrowPoint.position, Quaternion.LookRotation(aimDir, Vector3.up));
    }
}
