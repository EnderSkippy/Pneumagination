using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

[CustomEditor(typeof(DEAD_Animatronic))]
public class DEAD_Window_Animatronic : Editor
{
    DEAD_Animatronic dead_Animatronic;

    void OnEnable()
    {
        dead_Animatronic = target as DEAD_Animatronic;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawDefaultInspector();
        serializedObject.ApplyModifiedProperties();

        // Check if the variable has changed
        if (GUI.changed)
        {
            UpdateAnimatorController();
        }
    }

    void UpdateAnimatorController()
    {
        //Check for empty Animator Controller
        Animator animator = dead_Animatronic.GetComponent<Animator>();
        if (animator.runtimeAnimatorController == null)
        {
            Debug.LogError("'" + dead_Animatronic.name + "' needs an Animator Controller in the Animator script.");
            return;
        }

        //Setup Animator Controller
        string assetPath = AssetDatabase.GetAssetPath(animator.runtimeAnimatorController);
        AnimatorController controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(assetPath);

        //Add Layers
        DEAD_Actuator[] actuators = dead_Animatronic.GetActuatorInfoCopy();
        controller.RemoveLayer(0);
        for (int i = 0; i < actuators.Length; i++)
        {
            AnimatorControllerLayer newLayer = new AnimatorControllerLayer();
            newLayer.name = controller.MakeUniqueLayerName(actuators[i].actuationName + " DTU(" + actuators[i].dtuIndex.ToString() + ") ID(" + i + ")");
            newLayer.stateMachine = new AnimatorStateMachine();
            newLayer.stateMachine.name = newLayer.name;
            newLayer.stateMachine.hideFlags = HideFlags.HideInHierarchy;
            newLayer.stateMachine.AddState(new AnimatorState()
            {
                name = actuators[i].actuationName,
                speed = 0,
                timeParameterActive = true,
                timeParameter = actuators[i].dtuIndex.ToString(),
                motion = actuators[i].animation,
            }, Vector3.zero);
            if (AssetDatabase.GetAssetPath(controller) != "")
            {
                AssetDatabase.AddObjectToAsset(newLayer.stateMachine, AssetDatabase.GetAssetPath(controller));
            }
            newLayer.blendingMode = AnimatorLayerBlendingMode.Additive;
            newLayer.defaultWeight = 1f;
            controller.AddLayer(newLayer);
        }

        //Add Parameters
        int[] dtuIndexes = dead_Animatronic.GetDTUIndexes();
        dtuIndexes = dtuIndexes.Distinct().ToArray();
        for (int i = 0; i < dtuIndexes.Length; i++)
        {
            controller.AddParameter(dtuIndexes[i].ToString(),AnimatorControllerParameterType.Float);
        }

        animator.runtimeAnimatorController = controller;
    }
}
