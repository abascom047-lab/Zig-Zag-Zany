using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

namespace Assets.Scripts.Movement {
	public class ForwardMovement2 {
		public float Speed { get; private set; } = 0;

		private bool _forwardPressed = false;
		private bool _backwardPressed = false;

		private const float SPEED_INCREMENT = 0.1f;
		private const float SPEED_DECREMENT = 0.5f;
		private const float HIGH_SPEED = 10f;

		public void Enable(GameInputActions gameInputActions) {
			gameInputActions.Movement.Forward.performed += OnForwardPerformed;
			gameInputActions.Movement.Forward.canceled += OnForwardCanceled;
			gameInputActions.Movement.Backward.performed += OnBackwardPerformed;
			gameInputActions.Movement.Backward.canceled += OnBackwardCanceled;
		}

		public void Disable(GameInputActions gameInputActions) {
			gameInputActions.Movement.Forward.performed -= OnForwardPerformed;
			gameInputActions.Movement.Forward.canceled -= OnForwardCanceled;
			gameInputActions.Movement.Backward.performed -= OnBackwardPerformed;
			gameInputActions.Movement.Backward.canceled -= OnBackwardCanceled;
		}

		public float Update() {
			if (_forwardPressed) {
				Speed = Mathf.Min(Speed + SPEED_INCREMENT * GameTime.Current, HIGH_SPEED);
			} else if (_backwardPressed) {
				Speed = Mathf.Max(Speed - SPEED_INCREMENT * GameTime.Current, -HIGH_SPEED);
			} else {
				Speed = Mathf.MoveTowards(Speed, 0f, SPEED_DECREMENT * GameTime.Current);
			}

			return Speed;
		}

		public Vector3 GetMovement(RailRig rig) {
			return rig.Forward * Speed * GameTime.Current;
		}

		private void OnForwardPerformed(InputAction.CallbackContext context) {
			_forwardPressed = true;
		}

		private void OnForwardCanceled(InputAction.CallbackContext context) {
			_forwardPressed = false;
		}

		private void OnBackwardPerformed(InputAction.CallbackContext context) {
			_backwardPressed = true;
		}

		private void OnBackwardCanceled(InputAction.CallbackContext context) {
			_backwardPressed = false;
		}
	}
}
