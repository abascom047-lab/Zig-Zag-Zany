using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace Assets.Scripts.Movement {
	public class RailRig {
		public float Progress { get; private set; } = 0;
		public Vector3 Center { get; private set; }
		public Vector3 Forward { get; private set; }
		public Vector3 Right { get; private set; }

		private SplineContainer _spline;

		public RailRig(SplineContainer spline) {
			_spline = spline;
		}

		public void Update(Vector3 position) {
			float3 localPosition = _spline.transform.InverseTransformPoint(position);
			SplineUtility.GetNearestPoint(_spline.Spline, localPosition, out float3 nearestPoint, out float t);

			Progress = t;
			Center = _spline.EvaluatePosition(Progress);
			Forward = _spline.EvaluateTangent(Progress);
			Right = Vector3.Cross(Vector3.up, Forward).normalized;
		}
	}
}
