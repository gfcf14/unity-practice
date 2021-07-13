using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteMerger : MonoBehaviour {
  [SerializeField] private Sprite[] spritesToMerge = null;
  [SerializeField] private SpriteRenderer finalSpriteRenderer = null;

  private void Start() {
    Merge();
  }

  private void Merge() {
    Resources.UnloadUnusedAssets();

    var newTexture = new Texture2D(256, 256);

    for (int x = 0; x < newTexture.width; x++) {
      for (int y = 0; y < newTexture.height; y++) {
        newTexture.SetPixel(x, y, new Color(1, 1, 1, 0));
      }
    }

    for (int i = 0; i < spritesToMerge.Length; i++) {
      var currentSpriteTexture = spritesToMerge[i].texture;

      for (int x = 0; x < currentSpriteTexture.width; x++) {
        for (int y = 0; y < currentSpriteTexture.height; y++) {
          var color = currentSpriteTexture.GetPixel(x, y).a == 0 ?
            newTexture.GetPixel(x, y) :
            currentSpriteTexture.GetPixel(x, y);

          newTexture.SetPixel(x, y, color);
        }
      }
    }

    newTexture.Apply();
    var finalSprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), new Vector2(0.5f, 0.5f));
    finalSprite.name = "New Sprite";
    finalSpriteRenderer.sprite = finalSprite;
  }
}
