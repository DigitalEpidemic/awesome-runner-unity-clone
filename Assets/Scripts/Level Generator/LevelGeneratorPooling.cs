﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneratorPooling : MonoBehaviour {

	[SerializeField]
	private Transform platform, platform_Parent;

	[SerializeField]
	private Transform monster, monster_Parent;

	[SerializeField]
	private Transform healthCollectable, healthCollectable_Parent;

	[SerializeField]
	private int levelLength = 100;

	[SerializeField]
	private float distanceBetweenPlatforms = 15f;

	[SerializeField]
	private float min_Position_Y = 0f, max_Position_Y = 7f;

	[SerializeField]
	private float chanceForMonsterExistence = 0.25f, chanceForHealthCollectableExistence = 0.1f;

	[SerializeField]
	private float healthCollectable_MinY = 1f, healthCollectable_MaxY = 3f;

	private float platformLastPositionX;
	private Transform[] platform_Array;

	void Start () {
		CreatePlatforms ();
	}

	void CreatePlatforms () {
		platform_Array = new Transform[levelLength];

		// Create new platforms
		for (int i = 0; i < platform_Array.Length; i++) {
			Transform newPlatform = (Transform)Instantiate (platform, Vector3.zero, Quaternion.identity);
			platform_Array [i] = newPlatform;
		}

		// Position new platforms
		for (int i = 0; i < platform_Array.Length; i++) {
			float platformPositionY = Random.Range (min_Position_Y, max_Position_Y);

			Vector3 platformPosition;

			if (i < 2) {
				platformPositionY = 0f;
			}

			platformPosition = new Vector3 (distanceBetweenPlatforms * i, platformPositionY, 0);
			platformLastPositionX = platformPosition.x;
			platform_Array [i].position = platformPosition;
			platform_Array [i].parent = platform_Parent;

			// Spawn monsters and health collectables
			SpawnHealthAndMonsters (platformPosition, i, true);
		}
	}

	public void PoolingPlatforms () {
		for (int i = 0; i < platform_Array.Length; i++) {
			if (!platform_Array [i].gameObject.activeInHierarchy) {
				platform_Array [i].gameObject.SetActive (true);
				float platformPositionY = Random.Range (min_Position_Y, max_Position_Y);
				Vector3 platformPosition = new Vector3 (distanceBetweenPlatforms + platformLastPositionX, platformPositionY, 0);
				platform_Array [i].position = platformPosition;
				platformLastPositionX = platformPosition.x;

				// Spawn health and monsters
				SpawnHealthAndMonsters (platformPosition, i, false);
			}
		}
	}

	void SpawnHealthAndMonsters (Vector3 platformPosition, int i, bool gameStarted) {
		if (i > 2) {
			if (Random.Range (0f, 1f) < chanceForMonsterExistence) {
				if (gameStarted) {
					platformPosition = new Vector3 (distanceBetweenPlatforms * i, platformPosition.y + 0.1f, 0);
				} else {
					platformPosition = new Vector3 (distanceBetweenPlatforms + platformLastPositionX, platformPosition.y + 0.1f, 0);
				}

				Transform createMonster = (Transform)Instantiate (monster, platformPosition, Quaternion.Euler (0, -90, 0));
				createMonster.parent = monster_Parent;
			} // If chance for monster

			if (Random.Range (0f, 1f) < chanceForHealthCollectableExistence) {
				if (gameStarted) {
					platformPosition = new Vector3 (distanceBetweenPlatforms * i, platformPosition.y + Random.Range (healthCollectable_MinY, healthCollectable_MaxY), 0);
				} else {
					platformPosition = new Vector3 (distanceBetweenPlatforms + platformLastPositionX, platformPosition.y + Random.Range (healthCollectable_MinY, healthCollectable_MaxY), 0);
				}

				Transform createHealthCollectable = (Transform)Instantiate (healthCollectable, platformPosition, Quaternion.identity);
				createHealthCollectable.parent = healthCollectable_Parent;

			} // If chance for health collectable
		}
	}

} // LevelGeneratorPooling