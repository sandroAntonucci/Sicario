using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAim : MonoBehaviour
{

    [SerializeField] private InputActionAsset PlayerControls;

    [SerializeField] private GameObject itemHolder;

    [SerializeField] private GameObject marker;

    private System.Action<InputAction.CallbackContext> aimCallback;

    private InputAction aimAction;

    private GunSway gunSway;

    private Coroutine aimCoroutine;

    public bool isAiming = false;


    private void Awake()
    {
        aimAction = PlayerControls.FindActionMap("Player").FindAction("Aim");

        gunSway = itemHolder.GetComponent<GunSway>();
    }

    private void OnEnable()
    {
        aimAction.Enable();

        aimCallback = ctx => Aim(); // store it ONCE
        aimAction.performed += aimCallback;
    }

    private void OnDisable()
    {
        aimAction.performed -= aimCallback; // remove the exact same instance

        aimAction.Disable();

        if (aimCoroutine != null)
        {
            StopCoroutine(aimCoroutine);
            aimCoroutine = null;
        }
    }

    private void Aim()
    {

        var weapon = PickUpController.weaponEquipped;

        if (!PickUpController.slotFull || weapon == null || weapon.gameObject == null) return;

        var baseWeapon = weapon.gameObject.GetComponent<BaseWeapon>();
        if (baseWeapon == null || baseWeapon.isMelee) return;

        if (aimCoroutine != null) StopCoroutine(aimCoroutine);

        if (isAiming)
        {
            isAiming = false;
            aimCoroutine = StartCoroutine(StopAim());
        }
        else
        {
            isAiming = true;
            aimCoroutine = StartCoroutine(StartAim());
        }

    }

    public IEnumerator StartAim()
    {

        marker.SetActive(false);
        CameraEffects.Instance.ChangeFOVCoroutine = StartCoroutine(CameraEffects.Instance.ChangeFOV(CameraEffects.Instance.defaultFOV * 0.6f, 0.2f));
        gunSway.enabled = false;
        Vector3 targetPosition = PickUpController.weaponEquipped.gameObject.GetComponent<BaseGun>().aimPosition;

        PickUpController.weaponEquipped.gameObject.GetComponent<BaseGun>().recoilStrengthMultiplier /= 2;
        gameObject.GetComponent<PlayerRotate>()._speed /= 2;

        // Instantly set rotation
        itemHolder.transform.localRotation = Quaternion.Euler(0, 90, 0);

        while (Vector3.Distance(itemHolder.transform.localPosition, targetPosition) > 0.01f)
        {
            itemHolder.transform.localPosition = Vector3.Lerp(itemHolder.transform.localPosition, targetPosition, 0.2f);
            yield return null;
        }

        itemHolder.transform.localPosition = targetPosition; // Ensure final position is exactly set

        aimCoroutine = null;
    }

    public IEnumerator StopAim()
    {
        marker.SetActive(true);
        Vector3 targetPosition = new Vector3(0.25f, -0.2f, 0.4f);
        CameraEffects.Instance.ChangeFOVCoroutine = StartCoroutine(CameraEffects.Instance.ChangeFOV(CameraEffects.Instance.defaultFOV, 0.1f));
        gameObject.GetComponent<PlayerRotate>()._speed *= 2;


        PickUpController.weaponEquipped.gameObject.GetComponent<BaseGun>().recoilStrengthMultiplier *= 2;


        while (Vector3.Distance(itemHolder.transform.localPosition, targetPosition) > 0.01f)
        {
            itemHolder.transform.localPosition = Vector3.Lerp(itemHolder.transform.localPosition, targetPosition, 0.1f);
            yield return null;
        }

        isAiming = false;
        itemHolder.transform.localPosition = targetPosition; // Ensure final position is exactly set
        gunSway.enabled = true;

        aimCoroutine = null;
    }
    
    public void ResetAim()
    {

        if (aimCoroutine != null)
        {
            StopCoroutine(aimCoroutine);
            StopCoroutine(CameraEffects.Instance.ChangeFOVCoroutine);
        }

        gameObject.GetComponent<PlayerRotate>()._speed *= 2;
        PickUpController.weaponEquipped.gameObject.GetComponent<BaseGun>().recoilStrengthMultiplier *= 2;

        isAiming = false;
        CameraEffects.Instance.ChangeFOVCoroutine = StartCoroutine(CameraEffects.Instance.ChangeFOV(CameraEffects.Instance.defaultFOV, 0.1f));
        marker.SetActive(true);
        itemHolder.transform.localPosition = new Vector3(0.25f, -0.2f, 0.4f);
        gunSway.enabled = true;
    }


}
