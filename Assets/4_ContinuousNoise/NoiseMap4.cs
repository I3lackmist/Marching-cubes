using UnityEngine;

public class NoiseMap4 : MonoBehaviour {

	[SerializeField]
	public Texture2D texture;

	[SerializeField]
	public Vector2Int chunkIndex;
	public Sprite sprite;

	public void SetOwnTexture(int width, int height) {
		texture = new Texture2D(width, height);
	}

	public void SetOwnSprite() {
		sprite = Sprite.Create(
			texture,
			new Rect(0.0f, 0.0f, texture.width, texture.height),
			new Vector2(0.5f, 0.5f),
			64f
		);

		gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
	}
}
