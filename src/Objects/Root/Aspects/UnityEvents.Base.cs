using System;
using System.Collections.Generic;
using Appalachia.Core.Objects.Dependencies;
using Appalachia.Utility.Async;

// ReSharper disable NotAccessedField.Local
// ReSharper disable StaticMemberInGenericType
#pragma warning disable CS0414

namespace Appalachia.Core.Objects.Root
{
    public partial class AppalachiaBase
    {
        #region Static Fields and Autoproperties

        private static Queue<Func<AppaTask>> _initializationFunctions;

        #endregion

        #region Fields and Autoproperties

        [NonSerialized] private bool _hasBeenInitialized;
        [NonSerialized] private bool _hasBeenEnabled;
        [NonSerialized] private bool _hasBeenDisabled;

        #endregion

        public static Queue<Func<AppaTask>> InitializationFunctions => _initializationFunctions;

        protected virtual bool LogEventFunctions => false;
        public bool HasBeenDisabled => _hasBeenDisabled;
        public bool HasBeenEnabled => _hasBeenEnabled;
        public bool HasBeenInitialized => _hasBeenInitialized;

        /// <summary>
        ///     Runs after the <see cref="Initialize" /> call:
        ///     <list type="bullet">
        ///         <item>When the object is created for the first time (similar to a constructor).</item>
        ///         <item>Every time the object is deserialized.</item>
        ///     </list>
        /// </summary>
        protected virtual async AppaTask WhenEnabled()
        {
            await AppaTask.CompletedTask;
        }

        private async AppaTask HandleInitialization()
        {
            await AppaTask.NextFrame();
            await AppalachiaRepositoryDependencyManager.ValidateDependencies(GetType());

            await ExecuteInitialization();

            _hasBeenInitialized = true;

            await WhenEnabled();

            _hasBeenEnabled = true;
            _hasBeenDisabled = false;
        }
    }
}
#pragma warning restore CS0414
