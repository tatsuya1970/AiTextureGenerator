using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TexureAiGenerate))]
public class texureAiGenerateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TexureAiGenerate texureAiGenerate = (TexureAiGenerate)target;

        // インターフェースの描画
        DrawDefaultInspector(); // 既存のインスペクターのフィールドを描画

        // OKボタン
        if (GUILayout.Button("Generate"))
        {
            texureAiGenerate.onClick(); // HogeのOnClickメソッドを呼び出す
        }
    }
}

