using System;
using System.Collections.Generic;
using UnityEngine;

namespace Appalachia.Core.Objects.Initialization
{
    [Serializable]
    public class InitializationMetadata
    {
        public InitializationMetadata()
        {
            _nonSerializedTagsHash = new HashSet<string>();
            _tagsHash = new HashSet<string>();
            _nonSerializedTags = new List<string>();
            _tags = new List<string>();
        }

        #region Fields and Autoproperties

        [NonSerialized] private HashSet<string> _nonSerializedTagsHash;

        [NonSerialized] private HashSet<string> _tagsHash;

        [NonSerialized] private List<string> _nonSerializedTags;

        [HideInInspector, SerializeField]
        private List<string> _tags;

        [HideInInspector, SerializeField]
        private string _resetToken;

        #endregion

        public HashSet<string> NonSerializedTagsHash => _nonSerializedTagsHash;

        public HashSet<string> TagsHash => _tagsHash;

        public List<string> NonSerializedTags => _nonSerializedTags;

        public List<string> Tags => _tags;

        public string ResetToken
        {
            get => _resetToken;
            internal set => _resetToken = value;
        }
    }
}
