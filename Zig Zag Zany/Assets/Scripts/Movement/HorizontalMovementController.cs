using Assets.Scripts.Utils;
using UnityEngine;

namespace Assets.Scripts.Movement {
	public class HorizontalMovementController : MonoBehaviour {
		public CharacterController CharControl;

		private GameInputActions _gameInputActions;

		void Awake() {
			_gameInputActions = new GameInputActions();
		}

		void OnEnable() {
			_gameInputActions.Enable();
		}

		void OnDisable() {
			_gameInputActions.Disable();
		}

		void Update() {
			float inputAxis = _gameInputActions.Movement.Horizontal.ReadValue<float>();
			if (inputAxis == 0)
				return;

			CharControl.Move(transform.right * inputAxis * GameTime.Current);
		}
	}
}
