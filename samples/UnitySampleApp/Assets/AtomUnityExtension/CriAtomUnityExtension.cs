using UnityEngine;
using CriWare.Interfaces;

namespace CriWare.Unity
{
	/// <summary>
	/// class containing extension methods for using ADX functions in Unity.
	/// </summary>
	public static class CriAtomUnityExtension
	{
		public static void SetPosition(this IPositionable target, in Vector3 position) =>
			target.SetPosition(new (){ x = position.x, y = position.y, z = position.z });

		public static void SetOrientation(this IPositionable target, in Vector3 forward, in Vector3 up) =>
				target.SetOrientation(new() { x = forward.x, y = forward.y, z = forward.z }, new() { x = up.x, y = up.y, z = up.z });

		public static void SetVelocity(this IPositionable target, in Vector3 velocity) =>
			target.SetVelocity(new() { x = velocity.x, y = velocity.y, z = velocity.z });

		public static void SetOrientation(this IPositionable target, Quaternion rotation) =>
			target.SetOrientation(rotation * Vector3.forward, rotation * Vector3.up);

		public static void SetTransform(this IPositionable target, Transform transform)
		{
			target.SetPosition(transform.position);
			target.SetOrientation(transform.rotation);
		}
	}
}
