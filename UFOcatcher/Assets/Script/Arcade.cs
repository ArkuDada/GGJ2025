using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class Arcade : MonoBehaviour
{
	public Transform FeverMeter;
	public List<QuestButtonMesh> mesh;

	public List<Material> iconMats;

	private const float JOYSTICK_MAX_ROT = 15f;
	private const float BUTTON_MAX_DEPRESS = 0.6f;
	// Changes the visual input animation speed as lerp scalar
	private const float JOYSTICK_LERP = 0.2f;
	private const float BUTTON_LERP = 0.2f;
	
	private Vector2 dPadInput = Vector2.zero;
	private bool buttonInput = false;
	private Vector3 buttonStartingLocalPos = Vector3.zero;

	public Transform joystick;
	public Transform button;

	private void Awake()
	{
		foreach (var questButtonMesh in mesh)
		{
			questButtonMesh.BorderMesh.material = new Material(questButtonMesh.BorderMesh.material);
		}

		for (int i = 0; i < 4; i++)
		{
			SetButtonIcon(i, Objects.ObjectType.End);
		}
	}

	private void Start()
	{
		buttonStartingLocalPos = button.transform.localPosition;
	}

	private void Update()
	{
		Quaternion joystickWantAngle_Euler = Quaternion.Euler(new Vector3(-dPadInput.y, 0, dPadInput.x) * JOYSTICK_MAX_ROT);
		Vector3 buttonWantPosition = buttonStartingLocalPos + (buttonInput ? 1 : 0) * BUTTON_MAX_DEPRESS * Vector3.down;
		
		joystick.transform.localRotation = Quaternion.Lerp(joystick.transform.localRotation, joystickWantAngle_Euler, JOYSTICK_LERP);
		button.transform.localPosition = Vector3.Lerp(button.transform.localPosition, buttonWantPosition, BUTTON_LERP);
	}

	public void SetButtonIcon(int index, Objects.ObjectType objectType)
	{
		mesh[index].IconMesh.material = new Material(iconMats[(int)objectType]);
	}


	//fill 0 - 1.f
	public void SetBorderFill(int index, float fill)
	{
		fill = (1.0f - Mathf.Clamp(fill, 0.0f, 1.0f)) * 360.0f;
		mesh[index].BorderMesh.material.SetFloat("_Arc2", fill);
	}

	// For the joystick model to be updated w/ player input
	public void SetVisualJoystickInputs(Vector2 dPadInput) {
		this.dPadInput = dPadInput;
	}

	// For the button model to be updated w/ player input
	public void SetVisualButtonInputs(bool buttonInput)
	{
		this.buttonInput = buttonInput;
	}
}

[Serializable]
public struct QuestButtonMesh
{
	public MeshRenderer IconMesh;
	public MeshRenderer BorderMesh;
}
