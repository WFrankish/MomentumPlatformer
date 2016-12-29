using UnityEngine;

public class VectorTools
{
	public static Vector2 ComponentOfInDirectionOf(Vector2 vector, Vector2 direction, float epsilon){
		var output = (Vector2.Dot (vector, direction) / Vector2.Dot (direction, direction)) * direction;
		if (output.magnitude > epsilon) {
			return output;
		} else {
			return new Vector2();
		}
	}

	public static Vector2 Clamp(Vector2 vector){
		return vector.magnitude > 1 ? vector.normalized : vector;
	}
}