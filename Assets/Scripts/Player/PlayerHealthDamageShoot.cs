using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthDamageShoot : MonoBehaviour {

	private float distanceBeforeNewPlatforms = 120f;

	private LevelGenerator levelGenerator;

	void Awake () {
		levelGenerator = GameObject.Find (Tags.LEVEL_GENERATOR_GAME_OBJ).GetComponent<LevelGenerator> ();
	}

	void Update () {
		
	}

	void OnTriggerEnter (Collider target) {
		if (target.tag == Tags.MORE_PLATFORMS_TAG) {
			Vector3 temp = target.transform.position;
			temp.x += distanceBeforeNewPlatforms;
			target.transform.position = temp;

			levelGenerator.GenerateLevel (false);
		}
	}

} // PlayerHealthDamageShoot