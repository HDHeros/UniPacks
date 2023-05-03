using System.Text;
using UnityEditor;

namespace HDH.ESG.Editor
{
    internal static class EnumSourceGenerator
    {
        private static readonly StringBuilder s_resultSource = new StringBuilder();
        private static string s_indent;
        private static int s_indentLevel;

        public static void Generate(EnumConst[] constants, string fullEnumName, string folderPath)
        {
            s_resultSource.Clear();
            SplitFullTypeName(fullEnumName, out string enumName, out string @namespace);
            bool hasNamespace = string.IsNullOrEmpty(@namespace) == false;
            AppendHeader(hasNamespace, @namespace, enumName);
            InsertConstants(constants);
            AppendFooter(hasNamespace);
            string path = GetTypePath(folderPath, enumName);
            System.IO.File.WriteAllText(path, s_resultSource.ToString());
            AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
        }

        public static bool IsExist(string fullEnumName, string folderPath)
        {
            SplitFullTypeName(fullEnumName, out string enumName, out _);
            return System.IO.File.Exists(GetTypePath(folderPath, enumName));
        }

        private static string GetTypePath(string folderPath, string enumName) => $"{folderPath}/{enumName}.cs";

        private static void SplitFullTypeName(string fullName, out string typeName, out string @namespace)
        {
            int lastPointIndex = fullName.LastIndexOf('.');
            if (lastPointIndex == -1)
            {
                @namespace = string.Empty;
                typeName = fullName;
                return;
            }

            @namespace =  fullName.Substring(0, lastPointIndex);
            typeName = fullName.Substring(lastPointIndex + 1);
        }
        
        private static void AppendHeader(bool hasNamespace, string @namespace, string enumName)
        {
            if (hasNamespace)
            {
                s_resultSource.Append($"namespace {@namespace}");
                NewLine();
                s_resultSource.Append("{");
                ChangeIndentLevel(s_indentLevel + 1);
                NewLine();
            }

            s_resultSource.Append($"public enum {enumName}");
            NewLine();
            s_resultSource.Append("{");
            ChangeIndentLevel(s_indentLevel + 1);
        }

        private static void InsertConstants(EnumConst[] constants)
        {
            foreach (EnumConst c in constants)
            {
                NewLine();
                s_resultSource.Append(c.Name);
                if (c.SetValueExplicit)
                    s_resultSource.Append($" = {c.Value.ToString()}");
                s_resultSource.Append(',');
            }
        }

        private static void AppendFooter(bool hasNamespace)
        {
            ChangeIndentLevel(s_indentLevel - 1);
            NewLine();
            s_resultSource.Append("}");
            if (!hasNamespace) return;
            ChangeIndentLevel(s_indentLevel - 1);
            NewLine();
            s_resultSource.Append("}");
        }
        
        private static void NewLine()
        {
            s_resultSource.AppendLine();
            s_resultSource.Append(s_indent);
        }

        private static void ChangeIndentLevel(int level)
        {
            s_indentLevel = level;
            var indentBuilder = new StringBuilder();
            for (int i = 0; i < level; i++) 
                indentBuilder.Append("   ");
            s_indent = indentBuilder.ToString();
        }
    }
}