using HDH.Audio.Confgis;
using UnityEditor;

namespace HDH.Audio.Editor
{
    [CustomEditor(typeof(AudioConfig))]
    public class AudioConfigEditor : UnityEditor.Editor
    {
        private SerializedProperty _audioClips;
        private SerializedProperty _volumePriority;
        private SerializedProperty _output;
        private SerializedProperty _limitPlaybacksCount;
        private SerializedProperty _maxParallelPlaybacksCount;
        private SerializedProperty _mute;
        private SerializedProperty _bypassEffects;
        private SerializedProperty _bypassListenerEffects;
        private SerializedProperty _bypassReverbZones;
        private SerializedProperty _loop;
        private SerializedProperty _priority;
        private SerializedProperty _volume;
        private SerializedProperty _pitchMin;
        private SerializedProperty _pitchMax;
        private SerializedProperty _stereoPan;
        private SerializedProperty _spatialBlend;
        private SerializedProperty _reverbZoneMix;
        private SerializedProperty _dopplerLevel;
        private SerializedProperty _spread;
        private SerializedProperty _rolloffMode;
        private SerializedProperty _minDistance;
        private SerializedProperty _maxDistance;
        
        protected virtual void OnEnable()
        {
            _audioClips = serializedObject.FindProperty("_audioClips");
            _volumePriority = serializedObject.FindProperty("_volumePriority");
            _output = serializedObject.FindProperty("_output");
            _limitPlaybacksCount = serializedObject.FindProperty("_limitPlaybacksCount");
            _maxParallelPlaybacksCount = serializedObject.FindProperty("_maxParallelPlaybacksCount");
            _mute = serializedObject.FindProperty("_mute");
            _bypassEffects = serializedObject.FindProperty("_bypassEffects");
            _bypassListenerEffects = serializedObject.FindProperty("_bypassListenerEffects");
            _bypassReverbZones = serializedObject.FindProperty("_bypassReverbZones");
            _loop = serializedObject.FindProperty("_loop");
            _priority = serializedObject.FindProperty("_priority");
            _volume = serializedObject.FindProperty("_volume");
            _pitchMin = serializedObject.FindProperty("_pitchMin");
            _pitchMax = serializedObject.FindProperty("_pitchMax");
            _stereoPan = serializedObject.FindProperty("_stereoPan");
            _spatialBlend = serializedObject.FindProperty("_spatialBlend");
            _reverbZoneMix = serializedObject.FindProperty("_reverbZoneMix");
            _dopplerLevel = serializedObject.FindProperty("_dopplerLevel");
            _spread = serializedObject.FindProperty("_spread");
            _rolloffMode = serializedObject.FindProperty("_rolloffMode");
            _minDistance = serializedObject.FindProperty("_minDistance");
            _maxDistance = serializedObject.FindProperty("_maxDistance");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(_audioClips);
            EditorGUILayout.PropertyField(_volumePriority);
            EditorGUILayout.PropertyField(_output);
            EditorGUILayout.PropertyField(_limitPlaybacksCount);
            if (_limitPlaybacksCount.boolValue)
                EditorGUILayout.PropertyField(_maxParallelPlaybacksCount);
            EditorGUILayout.PropertyField(_mute);
            EditorGUILayout.PropertyField(_bypassEffects);
            EditorGUILayout.PropertyField(_bypassListenerEffects);
            EditorGUILayout.PropertyField(_bypassReverbZones);
            EditorGUILayout.PropertyField(_loop);
            EditorGUILayout.PropertyField(_priority);
            EditorGUILayout.PropertyField(_volume);
            EditorGUILayout.PropertyField(_pitchMin);
            EditorGUILayout.PropertyField(_pitchMax);
            EditorGUILayout.PropertyField(_stereoPan);
            EditorGUILayout.PropertyField(_spatialBlend);
            EditorGUILayout.PropertyField(_reverbZoneMix);
            EditorGUILayout.PropertyField(_dopplerLevel);
            EditorGUILayout.PropertyField(_spread);
            EditorGUILayout.PropertyField(_rolloffMode);
            EditorGUILayout.PropertyField(_minDistance);
            EditorGUILayout.PropertyField(_maxDistance);
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}