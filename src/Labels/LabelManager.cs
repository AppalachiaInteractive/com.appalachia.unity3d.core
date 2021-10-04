#region

using System;
using System.Collections.Generic;
using System.Linq;
using Appalachia.Core.Math.Stats;
using Appalachia.Core.Math.Stats.Implementations;
using Appalachia.Editing.Assets;
using Appalachia.Editing.Scene;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

#endregion

namespace Appalachia.Core.Labels
{
    public static class LabelManager
    {
        private static string[] _strings;

        private static ValueDropdownList<string> _labelDropdownList;

        private static Dictionary<Type, Dictionary<object, string>> _enumTypeLookup;
        
        public static List<LabelData> labelDatas = new List<LabelData>();
        public static bool Initialized => labelDatas?.Count > 0;

        public static string[] strings
        {
            get
            {
                if (_strings == null)
                {
                    InitializeLabels();
                }

                return _strings;
            }
        }

        public static ValueDropdownList<string> labelDropdownList
        {
            get
            {
                if (_labelDropdownList == null)
                {
                    InitializeLabels();
                }

                return _labelDropdownList;
            }
        }

        public static void InitializeLabels()
        {
            var labels = new Dictionary<string, int>();

            var assetPaths = AssetDatabase.FindAssets("t:Prefab").Select(AssetDatabase.GUIDToAssetPath).ToArray();

            using (var progress = new EditorOnlyProgressBar("Building label list...", assetPaths.Length, true, 200))
            {
                for (var i = 0; i < assetPaths.Length; i++)
                {
                    if (progress.Cancelled)
                    {
                        if (labelDatas == null)
                        {
                            labelDatas = new List<LabelData>();
                        }

                        labelDatas.Clear();
                        return;
                    }

                    ;

                    progress.Increment1AndShowProgressBasic();

                    var assetPath = assetPaths[i];

                    var obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

                    var labelSet = AssetDatabase.GetLabels(obj);

                    for (var j = 0; j < labelSet.Length; j++)
                    {
                        var label = labelSet[j];

                        if (!labels.ContainsKey(label))
                        {
                            labels.Add(label, 1);
                        }
                        else
                        {
                            labels[label] += 1;
                        }
                    }
                }
            }

            if (labelDatas == null)
            {
                labelDatas = new List<LabelData>();
            }

            labelDatas.Clear();

            foreach (var labelSet in labels.OrderByDescending(l => l.Value))
            {
                var newLabelData = new LabelData {label = labelSet.Key, count = labelSet.Value};

                labelDatas.Add(newLabelData);
            }

            _strings = new string[labelDatas.Count];
            _labelDropdownList = new ValueDropdownList<string>();

            for (var i = 0; i < labelDatas.Count; i++)
            {
                var label = labelDatas[i];

                _strings[i] = label.label;
                _labelDropdownList.Add(label.label, label.label);
            }
        }

        public static string GetLabelText(int i)
        {
            return strings[i];
        }

        public static LabelData GetLabelData(int i)
        {
            return labelDatas[i];
        }

        [MenuItem("Tools/Labels/Label Vegetation Prefabs", priority = -1050)]
        public static void MENU_LabelVegetationPrefabs()
        {
            ProcessLabelAssignments(LABELS.vegetations);
        }

        [MenuItem("Tools/Labels/Label Tree Prefabs", priority = -1050)]
        public static void MENU_LabelTreePrefabs()
        {
            ProcessLabelAssignments(LABELS.trees);
        }

        [MenuItem("Tools/Labels/Label Assembly Prefabs", priority = -1050)]
        public static void MENU_LabelAssemblyPrefabs()
        {
            ProcessLabelAssignments(LABELS.assemblies);
        }

        [MenuItem("Tools/Labels/Label Object Prefabs", priority = -1050)]
        public static void MENU_LabelObjectPrefabs()
        {
            ProcessLabelAssignments(LABELS.objects);
        }

        private static void ProcessLabelAssignments(LabelAssignmentCollection collection)
        {
            var assets = AssetDatabase.FindAssets($"l:{collection.baseTerm} t:Prefab");

            var statsTracker = new StatsTrackerCollection<floatStatsTracker, float>(collection.terms.Length);

            using (new AssetEditingScope())
            {
                for (var i = 0; i < assets.Length; i++)
                {
                    var guid = assets[i];
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);

                    var assetLabels = AssetDatabase.GetLabels(asset);

                    var labelHash = new HashSet<string>();
                    labelHash.AddRange(assetLabels);

                    var renderers = asset.GetComponentsInChildren<MeshRenderer>();

                    if (renderers.Length <= 0)
                    {
                        continue;
                    }

                    var bounds = renderers[0].bounds;
                    var size = bounds.size;
                    
                    var effectiveSize = Vector3.Scale(size, collection.multiplier);
                    
                    var magnitude = effectiveSize.magnitude;

                    var matched = false;
                    var changed = false;

                    for (var termIndex = 0; termIndex < collection.terms.Length; termIndex++)
                    {
                        var term = collection.terms[termIndex];

                        if (!matched && ((magnitude < term.allowedMagnitude) || (termIndex == (collection.terms.Length - 1))))
                        {
                            if (!labelHash.Contains(term.term))
                            {
                                labelHash.Add(term.term);
                                changed = true;
                            }

                            matched = true;

                            statsTracker[termIndex].Track(magnitude);
                        }
                        else if (labelHash.Contains(term.term))
                        {
                            labelHash.Remove(term.term);
                            changed = true;
                        }
                    }

                    if (changed)
                    {
                        AssetDatabase.SetLabels(asset, labelHash.ToArray());
                    }
                }

                for (var termIndex = 0; termIndex < collection.terms.Length; termIndex++)
                {
                    var term = collection.terms[termIndex];

                    var count = statsTracker[termIndex].Count;
                    var min = statsTracker[termIndex].Minimum;
                    var max = statsTracker[termIndex].Maximum;
                    var average = statsTracker[termIndex].Average;

                    //var median = statsTracker[termIndex].Median;

                    //Debug.Log($"[{term}]:  [ {count} ]  ||  Min: {min:F1}  Max: {max:F1}  Mean: {average:F1}  Median: {median:F1}");
                    Debug.Log($"[{term}]:  [ {count} ]  ||  Min: {min:F1}  Max: {max:F1}  Mean: {average:F1}");
                }
            }
        }

        public static void RegisterEnumTypeLabels<T>(T value, string label)
        {
            if (_enumTypeLookup == null)
            {
                _enumTypeLookup = new Dictionary<Type, Dictionary<object, string>>();
            }

            if (!_enumTypeLookup.ContainsKey(typeof(T)))
            {
                _enumTypeLookup.Add(typeof(T), new Dictionary<object, string>());
            }

            if (!_enumTypeLookup[typeof(T)].ContainsKey(value))
            {
                _enumTypeLookup[typeof(T)].Add(value, label);
            }
            else
            {
                _enumTypeLookup[typeof(T)][value] = label;
                
            }
        }

        public static void ApplyLabelsToPrefab<T>(GameObject asset, T value)
        {
            var labelHash = new HashSet<string>();
            var assetLabels = AssetDatabase.GetLabels(asset);

            labelHash.AddRange(assetLabels);

            var renderers = asset.GetComponentsInChildren<MeshRenderer>();

            if (renderers.Length <= 0)
            {
                return;
            }

            var changed = false;

            var lookupType = typeof(T);
            
            if (!_enumTypeLookup.ContainsKey(lookupType))
            {
                return;
            }

            var typeLookup = _enumTypeLookup[lookupType];

            if (!typeLookup.ContainsKey(value))
            {
                return;
            }

            foreach (var possibleLabel in typeLookup)
            {
                if (((T)possibleLabel.Key).Equals(value))
                {
                    if (!labelHash.Contains(possibleLabel.Value))
                    {
                        labelHash.Add(possibleLabel.Value);
                        changed = true;
                    }
                }
                else
                {
                    if (labelHash.Contains(possibleLabel.Value))
                    {
                        labelHash.Remove(possibleLabel.Value);
                        changed = true;
                    }
                }
            }

            if (changed)
            {
                AssetDatabase.SetLabels(asset, labelHash.ToArray());
            }
        }

        
    }
}
