// PetaPocoAzure - A Tiny ORMish thing for your POCO's.
// Copyright © 2011-2012 Topten Software.  All Rights Reserved.

// Modified Tom Bull 2013

using System;
using System.Collections.Generic;
using System.Linq;

namespace PetaPocoAzure
{
    /// <summary>
    /// Specifies the primary key column of a poco class, whether the column is auto incrementing
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PrimaryKeyAttribute : Attribute
    {
        public PrimaryKeyAttribute(string primaryKey)
        {
            Value = new List<string>() { primaryKey };
            autoIncrement = true;
        }

        public PrimaryKeyAttribute(string[] primaryKeys)
        {
            Value = primaryKeys.ToList();
            autoIncrement = true;
        }

        public IList<string> Value
        {
            get;
            private set;
        }

        public bool autoIncrement
        {
            get;
            set;
        }
    }

}
