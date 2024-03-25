using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class SkillsEditorWindow : EditorWindow
{
    private List<Skill> skillsList = new List<Skill>();
    private Skill selectedSkill;
    private bool isNewSkill = false;

    private string nameText = "";
    private string iconText = "";
    private string descriptionText = "";
    private string scopeText = "";
    private string occasionText = "";
    private string userAnimationText = "";
    private string targetAnimationText = "";

    private string searchText = "";

    Vector2 scrollPosition;

    [MenuItem("Game Systems/Skills")]
    public static void ShowWindow()
    {
        // Mostra a janela
        GetWindow<SkillsEditorWindow>("Skills Editor");
    }

    void OnEnable()
    {
        LoadSkills();
    }

    void LoadSkills()
    {
        skillsList.Clear();

        string[] assetPaths = Directory.GetFiles("Assets/Skills", "*.asset");
        foreach (string assetPath in assetPaths)
        {
            Skill skill = AssetDatabase.LoadAssetAtPath<Skill>(assetPath);
            if (skill != null)
            {
                skillsList.Add(skill);
            }
        }
    }

    void OnGUI()
    {
        ShowSearchField();
        EditorGUILayout.BeginHorizontal();

        // Mostra o campo de pesquisa

        EditorGUILayout.BeginVertical(GUILayout.Width(position.width * 0.4f));

        // Botão "New Skill"
        if (GUILayout.Button("New Skill"))
        {
            isNewSkill = true;
            selectedSkill = null;
            ResetSkillFields();
        }

        // Mostra a lista de habilidades
        ShowSkillsList();

        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical();

        // Exibe os detalhes da habilidade selecionada
        ShowSkillDetails();

        // Botão "Save" ou "Salvar alterações"
        if (selectedSkill != null || isNewSkill)
        {
            if (isNewSkill)
            {
                if (GUILayout.Button("Save"))
                {
                    CreateNewSkill();
                }
            }
            else
            {
                if (GUILayout.Button("Salvar alterações"))
                {
                    SaveSkillChanges();
                }
            }
        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.EndHorizontal();
    }

    void ShowSearchField()
    {
        EditorGUILayout.LabelField("Search:");
        EditorGUI.BeginChangeCheck();
        searchText = EditorGUILayout.TextField(searchText);
        if (EditorGUI.EndChangeCheck())
        {
            Repaint(); // Redesenha a janela ao mudar o texto de pesquisa
        }
    }

    void ShowSkillsList()
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        EditorGUILayout.LabelField("Skills");

        for (int i = 0; i < skillsList.Count; i++)
        {
            if (string.IsNullOrEmpty(searchText) || skillsList[i].name.ToLower().Contains(searchText.ToLower()))
            {
                if (GUILayout.Button(skillsList[i].name))
                {
                    isNewSkill = false;
                    selectedSkill = skillsList[i];
                    LoadSelectedSkillDetails();
                }
            }
        }

        EditorGUILayout.EndScrollView();
    }

    void ShowSkillDetails()
    {
        EditorGUILayout.LabelField("Skill Details");

        if (selectedSkill != null || isNewSkill)
        {
            // Campo de texto editável para o nome
            nameText = EditorGUILayout.TextField("Name:", nameText);
            // Campo de texto editável para o ícone
            iconText = EditorGUILayout.TextField("Icon:", iconText);
            // Campo de texto editável para a descrição
            descriptionText = EditorGUILayout.TextField("Description:", descriptionText);
            // Campo de texto editável para o escopo
            scopeText = EditorGUILayout.TextField("Scope:", scopeText);
            // Campo de texto editável para a ocasião
            occasionText = EditorGUILayout.TextField("Occasion:", occasionText);
            // Campo de texto editável para a animação do usuário
            userAnimationText = EditorGUILayout.TextField("User Animation:", userAnimationText);
            // Campo de texto editável para a animação do alvo
            targetAnimationText = EditorGUILayout.TextField("Target Animation:", targetAnimationText);
        }
        else
        {
            EditorGUILayout.LabelField("No Skill Selected");
        }
    }

    void CreateNewSkill()
    {
        if (isNewSkill)
        {
            Skill newSkill = ScriptableObject.CreateInstance<Skill>();
            newSkill.name = nameText;
            // Atribua outras propriedades conforme necessário

            AssetDatabase.CreateAsset(newSkill, "Assets/Skills/" + newSkill.name + ".asset");
            AssetDatabase.SaveAssets();

            skillsList.Add(newSkill);

            isNewSkill = false;
            ResetSkillFields();
        }
    }

    void SaveSkillChanges()
    {
        if (!isNewSkill && selectedSkill != null)
        {
            string oldAssetPath = AssetDatabase.GetAssetPath(selectedSkill);
            string newAssetPath = "Assets/Skills/" + nameText + ".asset";

            AssetDatabase.RenameAsset(oldAssetPath, nameText);
            AssetDatabase.SaveAssets();

            selectedSkill.name = nameText;
            // Atualize outras propriedades conforme necessário

            EditorUtility.SetDirty(selectedSkill);
            AssetDatabase.SaveAssets();

            ResetSkillFields();
        }
    }

    void ResetSkillFields()
    {
        nameText = "";
        iconText = "";
        descriptionText = "";
        scopeText = "";
        occasionText = "";
        userAnimationText = "";
        targetAnimationText = "";
    }

    void LoadSelectedSkillDetails()
    {
        if (selectedSkill != null)
        {
            nameText = selectedSkill.name;
            // Carregue outras propriedades da habilidade selecionada conforme necessário
        }
    }
}
