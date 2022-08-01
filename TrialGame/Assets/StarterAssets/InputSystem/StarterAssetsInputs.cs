using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool isEquiped;
		public bool isAiming;
		public bool isShooting;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}
		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnCursorUnlock(InputValue value)
		{
            if (cursorLocked)
            {
				Cursor.lockState = CursorLockMode.None;
				LookInput(new Vector2(0, 0));
				cursorLocked = false;
				cursorInputForLook = false;
			}
			else if (!cursorLocked)
            {
				Cursor.lockState = CursorLockMode.Locked;
				cursorLocked = true;
				cursorInputForLook = true;
			}
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnEquip(InputValue value)
		{
			isEquiped = !isEquiped;
		}

		public void OnAim(InputValue value)
		{
            if (isEquiped)
            {
				AimInput(value.isPressed);
			}
		}

		public void OnShoot(InputValue value)
		{
            if (isAiming)
            {
				ShootingInput(value.isPressed);
			}
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		public void AimInput(bool newAimingState)
		{
			isAiming = newAimingState;
		}

		public void ShootingInput(bool newShootingState)
		{
			isShooting = newShootingState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		public void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}