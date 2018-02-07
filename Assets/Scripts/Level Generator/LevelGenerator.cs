using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	[SerializeField]
	private int levelLength;

	[SerializeField]
	private int startPlatformLength = 5, endPlatformLength = 5;

	[SerializeField]
	private int distanceBetweenPlatforms;

	[SerializeField]
	private Transform platformPrefab, platform_Parent;

	[SerializeField]
	private Transform monster, monster_Parent;

	[SerializeField]
	private Transform healthCollectable, healthCollectable_Parent;

	[SerializeField]
	private float platformPos_MinY = 0f, platformPos_MaxY = 10f;

	[SerializeField]
	private int platformLength_Min = 1, platformLength_Max = 4;

	[SerializeField]
	private float chanceForMonsterExistence = 0.25f, chanceForCollectableExistence = 0.1f;

	[SerializeField]
	private float healthCollectable_MinY = 1f, healthCollectable_MaxY = 3f;

	private float platformLastPosX;

	private enum PlatformType {
		None,
		Flat
	}

	private class PlatformPositionInfo {
		public PlatformType platformType;
		public float positionY;
		public bool hasMonster;
		public bool hasHealthCollectable;

		public PlatformPositionInfo (PlatformType type, float posY, bool has_Monster, bool has_Collectable) {
			platformType = type;
			positionY = posY;
			hasMonster = has_Monster;
			hasHealthCollectable = has_Collectable;
		}

	} // PlatformPositionInfo class

	void Start () {
		GenerateLevel (true);
	}

	void FillOutPositionInfo (PlatformPositionInfo[] platformInfo) {
		int currentPlatformInfoIndex = 0;

		for (int i = 0; i < startPlatformLength; i++) {
			platformInfo [currentPlatformInfoIndex].platformType = PlatformType.Flat;
			platformInfo [currentPlatformInfoIndex].positionY = 0f;
			currentPlatformInfoIndex++;
		}

		while (currentPlatformInfoIndex < levelLength - endPlatformLength) {
			if (platformInfo [currentPlatformInfoIndex - 1].platformType != PlatformType.None) {
				currentPlatformInfoIndex++;
				continue;
			}

			float platformPositionY = Random.Range (platformPos_MinY, platformPos_MaxY);
			int platformLength = Random.Range (platformLength_Min, platformLength_Max);

			for (int i = 0; i < platformLength; i++) {
				bool has_Monster = (Random.Range (0f, 1f) < chanceForMonsterExistence);
				bool has_HealthCollectable = (Random.Range (0f, 1f) < chanceForCollectableExistence);

				platformInfo [currentPlatformInfoIndex].platformType = PlatformType.Flat;
				platformInfo [currentPlatformInfoIndex].positionY = platformPositionY;
				platformInfo [currentPlatformInfoIndex].hasMonster = has_Monster;
				platformInfo [currentPlatformInfoIndex].hasHealthCollectable = has_HealthCollectable;

				currentPlatformInfoIndex++;

				if (currentPlatformInfoIndex > (levelLength - endPlatformLength)) {
					currentPlatformInfoIndex = levelLength - endPlatformLength;
					break;
				}
			}

			for (int i = 0; i < endPlatformLength; i++) {
				platformInfo [currentPlatformInfoIndex].platformType = PlatformType.Flat;
				platformInfo [currentPlatformInfoIndex].positionY = 0f;

				currentPlatformInfoIndex++;
			}

		} // while loop
	}

	void CreatePlatformsFromPositionInfo (PlatformPositionInfo[] platformPositionInfo, bool gameStarted) {
		for (int i = 0; i < platformPositionInfo.Length; i++) {
			PlatformPositionInfo positionInfo = platformPositionInfo [i];

			if (positionInfo.platformType == PlatformType.None) {
				continue;
			}

			Vector3 platformPosition;

			if (gameStarted) {
				platformPosition = new Vector3 (distanceBetweenPlatforms * i, positionInfo.positionY, 0);
			} else {
				platformPosition = new Vector3 (distanceBetweenPlatforms + platformLastPosX, positionInfo.positionY, 0);
			}

			// Save the platform position x for later use
			platformLastPosX = platformPosition.x;

			print (platformLastPosX);

			Transform createBlock = (Transform)Instantiate (platformPrefab, platformPosition, Quaternion.identity);
			createBlock.parent = platform_Parent;

			if (positionInfo.hasMonster) {
				// Code later
			}

			if (positionInfo.hasHealthCollectable) {
				// Code later
			}

		} // for loop
	}

	public void GenerateLevel (bool gameStarted) {
		PlatformPositionInfo[] platformInfo = new PlatformPositionInfo[levelLength];

		for (int i = 0; i < platformInfo.Length; i++) {
			platformInfo [i] = new PlatformPositionInfo (PlatformType.None, -1f, false, false);
		}

		FillOutPositionInfo (platformInfo);
		CreatePlatformsFromPositionInfo (platformInfo, gameStarted);
	}

} // LevelGenerator