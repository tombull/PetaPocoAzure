﻿using System;
using System.Reflection;

namespace PetaPocoAzure
{
	/// <summary>
	/// StandardMapper is the default implementation of IMapper used by PetaPocoAzure
	/// </summary>
	public class StandardMapper : IMapper
	{
		/// <summary>
		/// Constructs a TableInfo for a POCO by reading its attribute data
		/// </summary>
		/// <param name="pocoType">The POCO Type</param>
		/// <returns></returns>
		public TableInfo GetTableInfo(Type pocoType)
		{
			return TableInfo.FromPoco(pocoType);
		}

		/// <summary>
		/// Constructs a ColumnInfo for a POCO property by reading its attribute data
		/// </summary>
		/// <param name="pocoProperty"></param>
		/// <returns></returns>
		public ColumnInfo GetColumnInfo(PropertyInfo pocoProperty)
		{
			return ColumnInfo.FromProperty(pocoProperty);
		}

		public Func<object, object> GetFromDbConverter(PropertyInfo TargetProperty, Type SourceType)
		{
			return null;
		}

		public Func<object, object> GetToDbConverter(PropertyInfo SourceProperty)
		{
			return null;
		}
	}
}
