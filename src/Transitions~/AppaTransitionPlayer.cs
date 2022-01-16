
using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Transitions
{
    /// <summary>
    ///     This class allows you to reference Transforms whose GameObjects contain transition components.
    ///     If these transition components define TargetAlias names, then this class will also manage them in the inspector.
    /// </summary>
    [System.Serializable]
    public class AppaTransitionPlayer
    {
        #region Static Fields and Autoproperties

        private static Dictionary<string, Alias> tempAliases = new Dictionary<string, Alias>();

        #endregion

        #region Fields and Autoproperties

        // Legacy
        [SerializeField] private float speed = -1.0f;

        [SerializeField] private List<Entry> entries;

        // Legacy
        [SerializeField] private List<Object> targets;

        // Legacy
        [SerializeField] private List<string> aliases;

        // Legacy
        [SerializeField] private List<Transform> roots;

        #endregion

        public bool IsUsed
        {
            get
            {
                if ((entries != null) && (entries.Count > 0))
                {
                    foreach (var entry in entries)
                    {
                        if (entry.Root != null)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        /// <summary>This stores a list of all <b>Transform</b>s containing transitions that will be played, and their settings.</summary>
        public List<Entry> Entries
        {
            get
            {
                if (entries == null)
                {
                    entries = new List<Entry>();
                }

                return entries;
            }
        }

        public float Speed
        {
            set => speed = value;

            get => speed;
        }

        /// <summary>This method will begin all transition entries.</summary>
        public void Begin()
        {
            Validate(false);

            if (entries != null)
            {
                AppaTransition.CurrentAliases.Clear();

                foreach (var entry in entries)
                {
                    foreach (var alias in entry.Aliases)
                    {
                        AppaTransition.AddAlias(alias.Key, alias.Obj);
                    }

                    AppaTransition.BeginAllTransitions(entry.Root, entry.Speed);
                }
            }
        }

        public void Validate(bool validateEntries)
        {
            if ((roots != null) && (roots.Count > 0))
            {
                Entries.Clear();

                foreach (var root in roots)
                {
                    if (root != null)
                    {
                        entries.Add(new Entry { Root = root, Speed = speed > 0.0f ? speed : -1.0f });
                    }
                }

                roots.Clear();
            }

            if (Entries.Count == 0)
            {
                entries.Add(new Entry());
            }

            if ((aliases != null) && (aliases.Count > 0) && (targets != null) && (targets.Count > 0))
            {
                var min = System.Math.Min(aliases.Count, targets.Count);

                for (var i = 0; i < min; i++)
                {
                    foreach (var entry in Entries)
                    {
                        entry.AddAlias(aliases[i], targets[i]);
                    }
                }

                aliases.Clear();
                targets.Clear();
            }

            if (validateEntries)
            {
                foreach (var entry in Entries)
                {
                    if (entry != null)
                    {
                        var pairs = AppaTransition.FindAllAliasTypePairs(entry.Root);

                        // Move entry.Aliases into dictionary
                        foreach (var alias in entry.Aliases)
                        {
                            tempAliases.Add(alias.Key, alias);
                        }

                        entry.Aliases.Clear();

                        // Rebuild entry.Aliases from dictionaries
                        foreach (var pair in pairs)
                        {
                            Alias alias;

                            // Use existing by set type again (it's non-serialized, so it must be set again)
                            if (tempAliases.TryGetValue(pair.Key, out alias))
                            {
                                alias.Type = pair.Value;

                                entry.Aliases.Add(alias);
                            }

                            // Use new
                            else
                            {
                                entry.Aliases.Add(new Alias { Key = pair.Key, Type = pair.Value });
                            }
                        }

                        // Discard remaining
                        tempAliases.Clear();
                    }
                }
            }
        }

        #region Nested type: Alias

        [System.Serializable]
        public class Alias
        {
            #region Fields and Autoproperties

            public Object Obj;
            public string Key;

            [System.NonSerialized] public System.Type Type;

            #endregion
        }

        #endregion

        #region Nested type: Entry

        [System.Serializable]
        public class Entry
        {
            #region Fields and Autoproperties

            [SerializeField] private float speed = -1.0f;
            [SerializeField] private List<Alias> aliases;
            [SerializeField] private Transform root;

            #endregion

            public List<Alias> Aliases
            {
                get
                {
                    if (aliases == null)
                    {
                        aliases = new List<Alias>();
                    }

                    return aliases;
                }
            }

            public float Speed
            {
                set => speed = value;
                get => speed;
            }

            public Transform Root
            {
                set => root = value;
                get => root;
            }

            public void AddAlias(string key, Object obj)
            {
                foreach (var alias in Aliases)
                {
                    if (alias.Key == key)
                    {
                        alias.Key = key;

                        return;
                    }
                }

                aliases.Add(new Alias { Key = key, Obj = obj });
            }
        }

        #endregion
    }
}
