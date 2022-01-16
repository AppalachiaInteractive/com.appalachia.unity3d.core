using UnityEngine;

namespace Appalachia.Core.Transitions
{
    public abstract class AppaMethodWithStateAndTarget : AppaMethodWithState
    {
        #region Fields and Autoproperties

        [UnityEngine.Serialization.FormerlySerializedAs("Data.TargetAlias")]
        public string Alias;

        #endregion

        public abstract System.Type GetTargetType();

        /// <summary>This allows you to get the current <b>Target</b> value, or an aliased override.</summary>
        public T GetAliasedTarget<T>(T current)
            where T : Object
        {
            if (string.IsNullOrEmpty(Alias) == false)
            {
                var target = default(Object);

                if (AppaTransition.CurrentAliases.TryGetValue(Alias, out target))
                {
                    if (target is T)
                    {
                        return (T)target;
                    }

                    if (target is GameObject)
                    {
                        var gameObject = (GameObject)target;

                        return gameObject.GetComponent(typeof(T)) as T;
                    }
                }
            }

            return current;
        }
    }
}
