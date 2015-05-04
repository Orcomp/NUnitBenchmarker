// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileOpenCommandContainer.cs" company="Wild Gums">
//   Copyright (c) 2008 - 2015 Wild Gums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace NUnitBenchmarker
{
    using System.Threading.Tasks;
    using Catel.MVVM;
    using Orchestra;

    public class ToolsToggleUrlReservationCommandContainer : CommandContainerBase
    {
        public ToolsToggleUrlReservationCommandContainer(ICommandManager commandManager) 
            : base(Commands.Tools.ToggleUrlReservation, commandManager)
        {
        }

        protected override async Task Execute(object parameter)
        {
            await base.Execute(parameter);


        }
    }
}