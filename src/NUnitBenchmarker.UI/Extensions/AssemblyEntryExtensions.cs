// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AssemblyEntryExtensions.cs" company="Orcomp development team">
//   Copyright (c) 2008 - 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System;
    using Catel;
    using Models;

    public static class AssemblyEntryExtensions
    {
        #region Methods
        public static void SelectTypes(this ReflectionEntry reflectionEntry, params Type[] types)
        {
            Argument.IsNotNull(() => reflectionEntry);

            foreach (var type in types)
            {
                SelectType(reflectionEntry, type);
            }
        }

        public static void SelectTypes(this ReflectionEntry reflectionEntry, params string[] types)
        {
            Argument.IsNotNull(() => reflectionEntry);

            foreach (var type in types)
            {
                SelectType(reflectionEntry, type);
            }
        }

        public static void SelectType(this ReflectionEntry reflectionEntry, Type type)
        {
            Argument.IsNotNull(() => reflectionEntry);
            Argument.IsNotNull(() => type);

            SelectType(reflectionEntry, type.FullName);
        }

        public static void SelectType(this ReflectionEntry reflectionEntry, string type)
        {
            Argument.IsNotNull(() => reflectionEntry);
            Argument.IsNotNullOrWhitespace(() => type);

            foreach (var child in reflectionEntry.Children)
            {
                var typeEntry = child as TypeEntry;
                if (typeEntry != null)
                {
                    if (string.Equals(typeEntry.TypeFullName, type))
                    {
                        typeEntry.IsChecked = true;
                    }
                }
                else
                {
                    SelectType(child, type);
                }
            }
        }
        #endregion
    }
}