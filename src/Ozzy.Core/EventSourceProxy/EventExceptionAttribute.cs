﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
#if NUGET
using Microsoft.Diagnostics.Tracing;
#else
using System.Diagnostics.Tracing;
#endif
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if NUGET
namespace EventSourceProxy.NuGet
#else
namespace EventSourceProxy
#endif
{
	/// <summary>
	/// Specifies the EventLevel for an exception generated by a method.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method, AllowMultiple = false)]
	public sealed class EventExceptionAttribute : Attribute
	{
		/// <summary>
		/// Initializes a new instance of the EventExceptionAttribute class.
		/// </summary>
		/// <param name="level">The EventLevel to use for exceptions generated by a method.</param>
		public EventExceptionAttribute(EventLevel level)
		{
			Level = level;
		}

		/// <summary>
		/// Gets the EventLevel to use for exceptions generated by a method.
		/// </summary>
		public EventLevel Level { get; private set; }
	}
}