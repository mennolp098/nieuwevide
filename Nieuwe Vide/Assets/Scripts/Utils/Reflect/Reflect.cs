using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

/// <summary>
/// A class containing commonly used reflection operations.
/// </summary>
public static class Reflect
{

	/// <summary>
	/// Get all types from a particular type. The results contains all types inheriting from the given type.
	/// </summary>
	/// <typeparam name="T">The base type.</typeparam>
	/// /// <param name="checkInterfaces">Whether or not to include interfaces in the search.</param>
	/// <returns>An IEnumerable containing all types.</returns>
	public static IEnumerable<Type> AllTypesFrom<T>(bool checkInterfaces = false)
	{
		return AllTypesFrom(typeof(T), checkInterfaces);
	}

	/// <summary>
	/// Get all types from a particular type. The results contains all types inheriting from the given type.
	/// </summary>
	/// <param name="type">The base type.</param>
	/// <param name="checkInterfaces">Whether or not to include interfaces in the search.</param>
	/// <returns>An IEnumerable containing all types.</returns>
	public static IEnumerable<Type> AllTypesFrom(Type type, bool checkInterfaces = false)
	{
		Assembly projAssembly = Assembly.GetAssembly(typeof(Reflect));

		return projAssembly.GetTypes().Where(t =>
			{
				// Filter out interfaces, abstract classes and the base-type.
				if(t.IsInterface || t.IsAbstract || t == type)
					return false;

				if(checkInterfaces)
				{
					foreach(Type interfaceType in t.GetInterfaces())
					{
						if(type.IsAssignableFrom(interfaceType))
						{
							return true;
						}
					}
				}

				return type.IsAssignableFrom(t);
			});
	}

	/// <summary>
	/// Get all types from a particular type as a string. The results contains all types inheriting from the given type.
	/// </summary>
	/// <typeparam name="T">The base type.</typeparam>
	/// <param name="checkInterfaces">Whether or not to include interfaces in the search.</param>
	/// <returns>An IEnumerable containing all types.</returns>
	public static IEnumerable<string> AllTypeStringsFrom<T>(bool checkInterfaces = false)
	{
		return AllTypeStringsFrom(typeof(T), checkInterfaces);
	}

	/// <summary>
	/// Get all types from a particular type as a string. The results contains all types inheriting from the given type.
	/// </summary>
	/// <param name="type">The base type.</param>
	/// <param name="checkInterfaces">Whether or not to include interfaces in the search.</param>
	/// <returns>An IEnumerable containing all types.</returns>
	public static IEnumerable<string> AllTypeStringsFrom(Type type, bool checkInterfaces = false)
	{
		IEnumerable<Type> types = AllTypesFrom(type);
		List<string> result = new List<string>();

		foreach(Type t in types)
		{
			result.Add(t.FullName);
		}

		return result;
	}
}