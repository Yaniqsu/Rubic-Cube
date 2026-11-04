using System;
using System.Linq;
using UnityEngine;

namespace YNQ.Utilities
{
	public static class ReflectionUtilities
	{
		public static Type[] GetInheritors (Type baseType)
		{
			return AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(a => a.GetTypes())
				.Where(t => baseType.IsAssignableFrom(t) &&
				t.IsClass &&
				!t.IsAbstract)
				.ToArray();
		}
	} 
}
