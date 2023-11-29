using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SkyboxAiGenerate))]
public class skyboxAiGenerateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        SkyboxAiGenerate skyboxAiGenerate = (SkyboxAiGenerate)target;

        // インターフェースの描画
        DrawDefaultInspector(); // 既存のインスペクターのフィールドを描画

        // OKボタン
        if (GUILayout.Button("Generate"))
        {
            skyboxAiGenerate.onClick(); // onClickメソッドを呼び出す
        }
    }
}

