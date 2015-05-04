// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Constants.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System.Windows.Input;
    using InputGesture = Catel.Windows.Input.InputGesture;

    public static class Filters
    {
        public const string AssemblyFiles = "Assembly Files (.dll, .exe)|*.dll;*.exe";
    }

    public static class Commands
    {
        public static class File
        {
            public const string Open = "File.Open";
            public static readonly InputGesture OpenInputGesture = new InputGesture(Key.O, ModifierKeys.Control);

            public const string SaveAllResults = "File.SaveAllResults";
            public static readonly InputGesture SaveAllResultsInputGesture = new InputGesture(Key.S, ModifierKeys.Control);

            public const string Exit = "File.Exit";
            public static readonly InputGesture ExitInputGesture = null;
        }

        public static class Options
        {
            public const string ChangeDefaultAxis = "Options.ChangeDefaultAxis";
            public static readonly InputGesture ChangeDefaultAxisInputGesture = null;
        }

        public static class Tools
        {
            public const string ClearLog = "Tools.ClearLog";
            public static readonly InputGesture ClearLogInputGesture = null;

            public const string ToggleUrlReservation = "Tools.ToggleUrlReservation";
            public static readonly InputGesture ToggleUrlReservationInputGesture = null;
        }
    }
}