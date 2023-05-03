namespace HDH.ESG.Editor
{
    internal static class ESGConst
    {
        public static string NotUniqueValueMessage = "Value isn't unique";
        public static string EmptyConstNameMessage = "Name can't be empty.";
        public static string SortByValueButtonText = "SortByValue";
        public static string NotUniqueConstNameMessage = "Name isn't unique.";
        public static string UpdateButtonText = "Update";
        public static string CreateButtonText = "Create";
        public static string ValidationFailedButtonText = " (There are some errors in config. Fix it)";
        public static string NameInNotMatchingNamingRules = "The name is not matching the naming rules.";
        public static string ConfigTypeNameLabelText = "Enum Name";
        public static string InvalidDirectoryMessage = "Directory with this name doesn't exist.";
        public static string SaveFolderLabelText = "Save Folder";
        public static string OpenFolderLabelText = "Open folder";
        public static string PickFolderButtonText = "...";
        public static string AddItemButtonText = "Add";
        public static string SortByNameBtnText = "SortByName";
        
        public const string ConstNamePropPath = "Name";
        public const string ConstSetValueExplicitPropPath = "SetValueExplicit";
        public const string ConstValuePropPath = "Value";
        public const string IsConstNameValidPropPath = "IsNameValid";
        public const string IsConstValueUniquePropPath = "IsValueUnique";
        public const string ConstNameValidationMessagePropPath = "NameValidationMessage";
        
        public const string EConfigFolderPropertyPath = "FolderPath";
        public const string EConfigEnumNamePropPath = "EnumName";
        public const string EConfigConstantsPropertyPath = "Constants";
    }
}