using UnityEngine;
using System.Collections;
using System;

public class ScriptableObjectDropdownAttribute : PropertyAttribute 
{
	public String scriptableObject { private set; get; }

	public ScriptableObjectDropdownAttribute(String obj)
	{
		this.scriptableObject = obj;
	}
}

