using System.Collections.Generic;
using UnityEngine;

public class HeroResources : MonoBehaviour
{
  public Dictionary<string, Sprite[]> spriteGroup;
  void Awake () {
    spriteGroup = new Dictionary<string, Sprite[]> {
      ["pants"] = Resources.LoadAll<Sprite>("Spritesheets/pants")
    };
  }
}
