using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//Editor del componete Save
#region save
[CustomEditor(typeof(Save))]
public class SaveEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.DrawDefaultInspector();

        //Añade datos extras en el inspector
        #region inspector
        EditorGUILayout.Space(10);

        Save save = (Save)target;

        serializedObject.Update();

        UIExtras.Separador();

        if (GUILayout.Button("Eliminar data"))
        {
            save.Eliminar();
        }


        EditorGUILayout.Space(10);
        save.mensajes = EditorGUILayout.Toggle("Mostrar mensajes: ", save.mensajes);
        EditorGUILayout.Space(10);

        serializedObject.ApplyModifiedProperties();
        #endregion
    }
}
#endregion

//Editor del componete Sonidos
#region sonidos
[CustomEditor(typeof(Sonidos))]
public class SonidosEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GUILayout.Label("Sonidos: Administra facilmente tus sonidos", EditorStyles.boldLabel);
        EditorGUILayout.Space(10);

        base.DrawDefaultInspector();
    }
}
#endregion

public static class UIExtras
{
    public static void Separador(int altura = 1)
    {
        Rect rect = EditorGUILayout.GetControlRect(false, altura);
        rect.height = altura;
        EditorGUI.DrawRect(rect, new Color(0.5f, 0.5f, 0.5f, 1));
    }
}