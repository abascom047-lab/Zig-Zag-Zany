using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Assets.Scripts.Movement {
	public class MainMovementController : MonoBehaviour {
		public CharacterController CharControl;
		public SplineContainer Spline;

		private RailRig _rig;
		private ForwardMovement2 _forwardMovement;
		private HorizontalMovement2 _horizontalMovement;
		private GameInputActions _gameInputActions;

		void Awake() {
			_rig = new RailRig(Spline);
			_forwardMovement = new ForwardMovement2();
			_horizontalMovement = new HorizontalMovement2();
			_gameInputActions = new GameInputActions();
		}

		void OnEnable() {
			_forwardMovement.Enable(_gameInputActions);
			_gameInputActions.Movement.Enable();
		}

		void OnDisable() {
			_forwardMovement.Disable(_gameInputActions);
			_gameInputActions.Movement.Disable();
		}

		void Update() {
			_rig.Update(CharControl.transform.position);
			_forwardMovement.Update();

			Vector3 forward = _forwardMovement.GetMovement(_rig);
			Vector3 right = _horizontalMovement.GetMovement(_gameInputActions, _rig);

			CharControl.Move(forward + right);
			CharControl.transform.rotation = Quaternion.LookRotation(_rig.Forward);
		}
	}
}
