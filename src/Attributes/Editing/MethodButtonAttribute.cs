using UnityEngine;

namespace Appalachia.Core.Attributes.Editing
{
    /// <summary>
    ///     Usage:
    ///     <para>   #if UNITY_EDITOR</para>
    ///     <para>   [MethodButton("MethodName1", "MethodName2", "MethodNameN")]  // Must match a the method name!</para>
    ///     <para>   [SerializeField] private bool showButtons; // this bool is mandatory!</para>
    ///     <para>   #endif</para>
    /// </summary>
    public class MethodButtonAttribute : PropertyAttribute
    {
        /// <summary>
        ///     Default constructor.
        /// </summary>
        public MethodButtonAttribute(params string[] args)
        {
            MethodNames = args;
        }

        #region Fields and Autoproperties

        /// <summary>
        ///     Array of different method names for which a button will be displayed.
        ///     <para>"MethodOne", MethodTwo" ... "MethodN"</para>
        /// </summary>
        public string[] MethodNames { get; }

        #endregion
    }
}
