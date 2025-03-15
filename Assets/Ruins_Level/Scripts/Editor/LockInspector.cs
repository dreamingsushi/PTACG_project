using System.Reflection;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Toggles the Inspector lock state and the Constrain Proportions lock state.
/// </summary>
public static class LockInspector {
    static readonly MethodInfo flipLocked;
    static readonly PropertyInfo constrainProportions;
    const BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Instance;

    static LockInspector() {
        // Cache static MethodInfo and PropertyInfo for performance

        var editorLockTrackerType = typeof(EditorGUIUtility).Assembly.GetType("UnityEditor.EditorGUIUtility+EditorLockTracker");
        flipLocked = editorLockTrackerType.GetMethod("FlipLocked", bindingFlags);

        constrainProportions = typeof(Transform).GetProperty("constrainProportionsScale", bindingFlags);
    }
    
    [MenuItem("Edit/Toggle Inspector Lock %q")]
    public static void Lock() {

        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;

        ActiveEditorTracker.sharedTracker.ForceRebuild();
    }

    [MenuItem("Edit/Toggle Inspector Lock %q", true)]
    public static bool Valid() {
        return ActiveEditorTracker.sharedTracker.activeEditors.Length != 0;
    }
}