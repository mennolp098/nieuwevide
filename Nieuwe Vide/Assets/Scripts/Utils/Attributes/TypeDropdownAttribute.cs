using UnityEngine;
using System;

public class TypeDropdownAttribute : PropertyAttribute
{
	public Type BaseType { private set; get; }

	public TypeDropdownAttribute(Type baseType)
	{
		BaseType = baseType;
	}
}