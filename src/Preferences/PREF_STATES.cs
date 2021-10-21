#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Appalachia.CI.Constants;
using Appalachia.Core.Attributes;
using Unity.Mathematics;
using Unity.Profiling;
using UnityEditor;
using UnityEngine;

#endregion

namespace Appalachia.Core.Preferences
{
    public static class PREF_STATES
    {
        private const string _PRF_PFX = nameof(PREF_STATES) + ".";

        public static readonly HashSet<string> _keys = new();
        public static List<string> _groupings = new(128);
        public static readonly HashSet<string> _groupingsParts = new();

        public static readonly PREF_STATE<bool> _bools = new();
        public static readonly PREF_STATE<Bounds> _bounds = new();
        public static readonly PREF_STATE<Color> _colors = new();
        public static readonly PREF_STATE<Gradient> _gradients = new();
        public static readonly PREF_STATE<double> _doubles = new();
        public static readonly PREF_STATE<float> _floats = new();
        public static readonly PREF_STATE<float2> _float2s = new();
        public static readonly PREF_STATE<float3> _float3s = new();
        public static readonly PREF_STATE<float4> _float4s = new();
        public static readonly PREF_STATE<int> _ints = new();
        public static readonly PREF_STATE<quaternion> _quaternions = new();
        public static readonly PREF_STATE<string> _strings = new();
        public static bool _safeToAwaken;

        public static readonly Dictionary<Type, object> _enums = new();

        private static readonly ProfilerMarker _PRF_GetSettingsProviders =
            new(_PRF_PFX + nameof(GetSettingsProviders));

        private static readonly ProfilerMarker _PRF_GetSortedCount =
            new(_PRF_PFX + nameof(GetSortedCount));

        private static readonly ProfilerMarker _PRF_DrawUIGroup =
            new(_PRF_PFX + nameof(DrawUIGroup));

        public static float indentSize = 25f;
        public static float characterSize = 7f;
        private static readonly ProfilerMarker _PRF_DrawUI = new(_PRF_PFX + nameof(DrawUI));

        private static readonly ProfilerMarker _PRF_Awake = new(_PRF_PFX + nameof(Awake));

        private static readonly ProfilerMarker _PRF_Get = new(_PRF_PFX + nameof(Get));

        private static readonly ProfilerMarker _PRF_GetEnumState =
            new(_PRF_PFX + nameof(GetEnumState));

        
        [SettingsProviderGroup]
        public static SettingsProvider[] GetSettingsProviders()
        {
            using (_PRF_GetSettingsProviders.Auto())
            {
                
                Awake();
                
                var providers = new List<SettingsProvider>();
                _groupings = new List<string>();
                
                foreach (var pref in PREF_STATE_BASE.allPreferences)
                {
                    var fullGrouping = $"Preferences/{pref.Grouping}";
                    
                    if (_groupings.Contains(pref.Grouping))
                    {
                        continue;
                    }

                    _groupings.Add(pref.Grouping);

                    var provider = new SettingsProvider(fullGrouping, SettingsScope.User)
                    {
                        guiHandler = searchContext => DrawUI(pref.Grouping), label = ""
                    };
                    
                    providers.Add(provider);
                }

                return providers.ToArray();
            }
        }
        

        private static int GetSortedCount<T>(PREF_STATE<T> prefState)
        {
            using (_PRF_GetSortedCount.Auto())
            {
                if (prefState.SortedValues.Count != prefState.Values.Count)
                {
                    prefState.Sort();
                }

                return prefState.SortedValues.Count;
            }
        }
        
        private static void DrawUI(string currentGrouping)
        {
            using (_PRF_DrawUI.Auto())
            {
                DrawUIGroup(currentGrouping);
            }
        }

        private static void DrawUIGroup(string group)
        {
            using (_PRF_DrawUIGroup.Auto())
            {
                DrawUI(group, _bools);
                DrawUI(group, _bounds);
                DrawUI(group, _colors);
                DrawUI(group, _gradients);
                DrawUI(group, _floats);
                DrawUI(group, _doubles);
                DrawUI(group, _float2s);
                DrawUI(group, _float3s);
                DrawUI(group, _float4s);
                DrawUI(group, _ints);
                DrawUI(group, _quaternions);
                DrawUI(group, _strings);
            }
        }

        private static void DrawUI<TR>(string groupFullName, PREF_STATE<TR> prefs)
        {
            using (_PRF_DrawUI.Auto())
            {
                EditorGUI.indentLevel++;

                var sortCount = GetSortedCount(prefs);
                
                for (var index = 0; index < sortCount; index++)
                {
                    var preference = prefs.SortedValues[index];
                    
                    if (preference.Grouping == groupFullName)
                    {
                        preference.Draw();
                    }
                }

                EditorGUI.indentLevel--;
            }
        }

        [MenuItem("Tools/Preferences/Awaken", false, APPA_MENU.TOOLS.GENERAL.PRIORITY)]
        [ExecuteOnEnable]
        public static void Awake()
        {
            using (_PRF_Awake.Auto())
            {
                _safeToAwaken = true;

                _bools.Awake();
                _bounds.Awake();
                _colors.Awake();
                _gradients.Awake();
                _doubles.Awake();
                _floats.Awake();
                _float2s.Awake();
                _float3s.Awake();
                _float4s.Awake();
                _ints.Awake();
                _quaternions.Awake();
                _strings.Awake();

                foreach (var x in _enums)
                {
                    if (x.Value is PREF_STATE_BASE xb)
                    {
                        xb.Awake();
                    }
                }
            }
        }

        public static PREF_STATE<TR> Get<TR>()
        {
            using (_PRF_Get.Auto())
            {
                var typeTR = typeof(TR);

                if (typeTR == typeof(bool))
                {
                    return _bools as PREF_STATE<TR>;
                }

                if (typeTR == typeof(Bounds))
                {
                    return _bounds as PREF_STATE<TR>;
                }

                if (typeTR == typeof(Color))
                {
                    return _colors as PREF_STATE<TR>;
                }

                if (typeTR == typeof(Gradient))
                {
                    return _gradients as PREF_STATE<TR>;
                }

                if (typeTR == typeof(double))
                {
                    return _doubles as PREF_STATE<TR>;
                }

                if (typeTR == typeof(float))
                {
                    return _floats as PREF_STATE<TR>;
                }

                if (typeTR == typeof(float2))
                {
                    return _float2s as PREF_STATE<TR>;
                }

                if (typeTR == typeof(float3))
                {
                    return _float3s as PREF_STATE<TR>;
                }

                if (typeTR == typeof(float4))
                {
                    return _float4s as PREF_STATE<TR>;
                }

                if (typeTR == typeof(int))
                {
                    return _ints as PREF_STATE<TR>;
                }

                if (typeTR == typeof(quaternion))
                {
                    return _quaternions as PREF_STATE<TR>;
                }

                if (typeTR == typeof(string))
                {
                    return _strings as PREF_STATE<TR>;
                }

                if (typeTR.IsEnum || (typeof(TR) == typeof(Enum)))
                {
                    return GetEnumState<TR>();
                }

                throw new NotSupportedException(typeTR.Name);
            }
        }

        public static PREF_STATE<TR> GetEnumState<TR>()
        {
            using (_PRF_GetEnumState.Auto())
            {
                var type = typeof(TR);

                if (!_enums.ContainsKey(type))
                {
                    var p = new PREF_STATE<TR>();

                    _enums.Add(type, p);
                    return p;
                }

                var existing = _enums[type];

                return (PREF_STATE<TR>) existing;
            }
        }

        public static bool Draw(this PREF_BASE preference)
        {
            EditorGUI.BeginChangeCheck();

            var indent = EditorGUI.indentLevel * indentSize;

            if (preference.NiceLabel == null)
            {
                preference.NiceLabel = ObjectNames.NicifyVariableName(preference.Label);
            }

            var labelWidth = indent + (preference.NiceLabel.Length * characterSize);
            var currentLabelWidth = EditorGUIUtility.labelWidth;

            EditorGUIUtility.labelWidth = currentLabelWidth;

            var realType = preference.GetType();
            var typeTR = realType.GetGenericArguments().FirstOrDefault();

            if (typeTR == typeof(bool))
            {
                return DoDraw((PREF<bool>) preference, _bools);
            }

            if (typeTR == typeof(Bounds))
            {
                return DoDraw((PREF<Bounds>) preference, _bounds);
            }

            if (typeTR == typeof(Color))
            {
                return DoDraw((PREF<Color>) preference, _colors);
            }

            if (typeTR == typeof(Gradient))
            {
                return DoDraw((PREF<Gradient>) preference, _gradients);
            }

            if (typeTR == typeof(double))
            {
                return DoDraw((PREF<double>) preference, _doubles);
            }

            if (typeTR == typeof(float))
            {
                return DoDraw((PREF<float>) preference, _floats);
            }

            if (typeTR == typeof(float2))
            {
                return DoDraw((PREF<float2>) preference, _float2s);
            }

            if (typeTR == typeof(float3))
            {
                return DoDraw((PREF<float3>) preference, _float3s);
            }

            if (typeTR == typeof(float4))
            {
                return DoDraw((PREF<float4>) preference, _float4s);
            }

            if (typeTR == typeof(int))
            {
                return DoDraw((PREF<int>) preference, _ints);
            }

            if (typeTR == typeof(quaternion))
            {
                return DoDraw((PREF<quaternion>) preference, _quaternions);
            }

            if (typeTR == typeof(string))
            {
                return DoDraw((PREF<string>) preference, _strings);
            }

            if (typeTR.IsEnum || (typeTR == typeof(Enum)))
            {
                if (_GetEnumStateLookup == null)
                {
                    _GetEnumStateLookup = new Dictionary<Type, MethodInfo>();
                }

                if (_DoDrawLookup == null)
                {
                    _DoDrawLookup = new Dictionary<Type, MethodInfo>();
                }

                MethodInfo enumStateMethod;

                if (_GetEnumStateLookup.ContainsKey(typeTR))
                {
                    enumStateMethod = _GetEnumStateLookup[typeTR];
                }
                else
                {
                    var unrealizedMethod = typeof(PREF_STATES).GetMethod(nameof(GetEnumState));
                    enumStateMethod = unrealizedMethod.MakeGenericMethod(typeTR);

                    _GetEnumStateLookup.Add(typeTR, enumStateMethod);
                }

                var prefs = enumStateMethod.Invoke(null, null);

                MethodInfo drawMethod;

                if (_DoDrawLookup.ContainsKey(typeTR))
                {
                    drawMethod = _DoDrawLookup[typeTR];
                }
                else
                {
                    var unrealizedMethod = typeof(PREF_STATES).GetMethod(
                        nameof(DoDraw),
                        BindingFlags.Static | BindingFlags.NonPublic
                    );
                    
                    drawMethod = unrealizedMethod.MakeGenericMethod(typeTR);

                    _DoDrawLookup.Add(typeTR, drawMethod);
                }

                return (bool)drawMethod.Invoke(null, new[] {preference, prefs});
            }

            throw new NotSupportedException(typeTR.Name);
        }

        private static Dictionary<Type, MethodInfo> _GetEnumStateLookup;
        private static Dictionary<Type, MethodInfo> _DoDrawLookup;

        public static bool Draw<TP>(this PREF<TP> preference)
        {
            var prefs = Get<TP>();

            return DoDraw(preference, prefs);
        }

        private static bool DoDraw<TP>(PREF<TP> preference, PREF_STATE<TP> prefs)
        {
            EditorGUI.BeginChangeCheck();
            
            if (preference.NiceLabel == null)
            {
                preference.NiceLabel = ObjectNames.NicifyVariableName(preference.Label);
            }
            
            var indent = EditorGUI.indentLevel * indentSize;
            
            var currentLabelWidth = EditorGUIUtility.labelWidth;
            var labelWidth = indent + (preference.NiceLabel.Length * characterSize);
            
            EditorGUIUtility.labelWidth = labelWidth;
            preference.Value = prefs.API.Draw(
                preference.NiceLabel,
                preference.Value,
                preference.Low,
                preference.High
            );

            var changed = false;
            
            if (EditorGUI.EndChangeCheck())
            {
                changed = true;
                
                prefs.API.Save(
                    preference.Key,
                    preference.Value,
                    preference.Low,
                    preference.High
                );
            }

            EditorGUIUtility.labelWidth = currentLabelWidth;

            return changed;
        }

/*
_bools
_bounds
_colors
_gradients
_doubles
_floats
_float2s
_float3s
_float4s
_ints
_quaternions
_strings

bool
Bounds
Color
Gradient
double
float
float2
float3
float4
int
quaternion
string
*/
    }
}
