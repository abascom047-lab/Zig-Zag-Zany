using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

namespace Assets.Scripts.Movement {
	public class ForwardMovement {
		public Vector3 Center { get; private set; }
		public Vector3 Tangent { get; private set; }
		public Vector3 Right { get; private set; }

		private SplineContainer _spline;
		private float _splineLength = 0;
		private float _splineProgress = 0;
		private float _speed = 0;

		private bool _forwardPressed = false;
		private bool _backwardPressed = false;

		private const float SPEED_INCREMENT = 7.5f;
		private const float SPEED_DECREMENT = 15f;
		private const float HIGH_SPEED = 40f;

		public ForwardMovement(SplineContainer spline) {
			_spline = spline;
			_splineLength = _spline.CalculateLength();
			Center = _spline.EvaluatePosition(_splineProgress);
			Tangent = _spline.EvaluateTangent(_splineProgress);
			Right = Vector3.Cross(Vector3.up, Tangent).normalized;
		}

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

		public void Update() {
			if (_forwardPressed) {
				_speed = Mathf.Min(_speed + SPEED_INCREMENT * GameTime.Current, HIGH_SPEED);
			} else if (_backwardPressed) {
				_speed = Mathf.Max(_speed - SPEED_INCREMENT * GameTime.Current, -HIGH_SPEED);
			} else {
				_speed = Mathf.MoveTowards(_speed, 0f, SPEED_DECREMENT * GameTime.Current);
			}

			if (_speed != 0) {
				float percentage = _speed / _splineLength;
				_splineProgress = Mathf.Clamp(_splineProgress + percentage * GameTime.Current, 0f, 1f);

				Center = _spline.EvaluatePosition(_splineProgress);
				Tangent = _spline.EvaluateTangent(_splineProgress);
				Right = Vector3.Cross(Vector3.up, Tangent).normalized;
			}
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
