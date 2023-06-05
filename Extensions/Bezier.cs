using UnityEngine;

namespace Z
{
	public static class Bezier
	{
		// via StackOverflow

		public static Vector3 Calculate(
			float f,
			Vector3 startPoint,
			Vector3 startTangent,
			Vector3 endTangent,
			Vector3 endPoint)
		{
			float u = 1 - f;
			float tt = f * f;
			float uu = u * u;
			float uuu = uu * u;
			float ttt = tt * f;
			Vector3 p = uuu * startPoint;
			p += 3 * uu * f * startTangent;
			p += 3 * u * tt * endTangent;
			p += ttt * endPoint;
			return p;
		}

		public static Vector3 Calculate(float f, Ray startRay, Ray endRay)
		{
			return Calculate(
				f,
				startRay.origin,
				startRay.origin + startRay.direction,
				endRay.origin + endRay.direction,
				endRay.origin);
		}
	}
}
