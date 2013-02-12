﻿// PetaPocoAzure - A Tiny ORMish thing for your POCO's.
// Copyright © 2011-2012 Topten Software.  All Rights Reserved.
 
using System;

namespace PetaPocoAzure
{
	/// <summary>
	/// Use the Ignore attribute on POCO class properties that shouldn't be mapped
	/// by PetaPocoAzure.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property)]
	public class IgnoreAttribute : Attribute
	{
	}

}
