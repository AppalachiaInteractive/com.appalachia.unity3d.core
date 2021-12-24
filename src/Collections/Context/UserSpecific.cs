using System;
using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Collections.Context
{
    [Serializable]
    public abstract class UserSpecific<T, TList> : ContextSpecific<T, TList>
        where TList : AppaList<T>, new()
        where T : ScriptableObject
    {
        #region Fields and Autoproperties

        public Dictionary<string, string> usernameCorrections;

        #endregion

        public override string GetContextKey()
        {
            var username = Environment.UserName.ToLower();

            usernameCorrections ??= new Dictionary<string, string>();

            if (usernameCorrections.ContainsKey("janic"))
            {
                usernameCorrections.Add("janic", "janice");
            }

            if (usernameCorrections.ContainsKey(username))
            {
                username = usernameCorrections[username];
            }

            return username;
        }
    }
}
