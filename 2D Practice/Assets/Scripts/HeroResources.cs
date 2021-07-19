using System.Collections.Generic;
using UnityEngine;

public class HeroResources : MonoBehaviour
{
  public Dictionary<string, Sprite[]> spriteGroup = new Dictionary<string, Sprite[]>();
  void Awake () {
    // spriteGroup = new Dictionary<string, Sprite[]> {
    //   ["pants"] = Resources.LoadAll<Sprite>("Spritesheets/pants"),
    //   ["boots"] = Resources.LoadAll<Sprite>("Spritesheets/boots"),
    //   ["shirt"] = Resources.LoadAll<Sprite>("Spritesheets/shirt"),
    //   ["tunic"] = Resources.LoadAll<Sprite>("Spritesheets/tunic"),
    //   ["belt"] = Resources.LoadAll<Sprite>("Spritesheets/belt")
    // };

    spriteGroup.Add("pants", Resources.LoadAll<Sprite>("Spritesheets/pants"));
    spriteGroup.Add("boots", Resources.LoadAll<Sprite>("Spritesheets/boots"));
    spriteGroup.Add("shirt", Resources.LoadAll<Sprite>("Spritesheets/shirt"));
    spriteGroup.Add("tunic", Resources.LoadAll<Sprite>("Spritesheets/tunic"));
    spriteGroup.Add("belt", Resources.LoadAll<Sprite>("Spritesheets/belt"));

    Debug.Log(spriteGroup.Count);
  }
}
