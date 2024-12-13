using System.IO;
using UnityEngine;

namespace Storex {
    public static class StorexVault 
    {
        const string DefaultFileName = "GameData"; 
        const string DirectoryName = "SaveData"; 
        const string FileType = ".json";

        static string FileName = "GameData";

        public static void Save<T>(T data, string fileName = null) {
            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);

            var name = fileName ?? FileName;
            var filePath = GetFullPath(name);
            var fullyJson = JsonUtility.ToJson(data, true);

#if !UNITY_EDITOR
            fullyJson = StorexEncrypter.Encrypt(fullyJson);
#endif
            
            FileName = DefaultFileName;
            File.WriteAllText(filePath, fullyJson);
        }
        
        public static T Load<T>(string fileName = null) {
            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);

            var name = fileName ?? FileName;
            var filePath = GetFullPath(name);
            if (!File.Exists(filePath))
                return default;
            
            var fullyJson = File.ReadAllText(filePath);
            
#if !UNITY_EDITOR
            fullyJson = StorexEncrypter.Decrypt(fullyJson);
#endif

            FileName = DefaultFileName;
            return JsonUtility.FromJson<T>(fullyJson);
        }
        
        
        public static void ChangeFileName(string fileName) => FileName = fileName;
        public static bool SaveExist(string fileName) => File.Exists(GetFullPath(fileName));
        static bool DirectoryExists() =>  Directory.Exists(DirectoryName);
        static string GetFullPath(string fileName) => DirectoryName + "/" + fileName + FileType; 
        
        // TODO:: Create custom formattor allow you make everything in ove file with different types
        // TODO:: Template 
        #region Custom Formatting
        /*
         
        [Serializable]
        public struct DataHolder<T> {
            public T Value;
        }
        
        
        // TODO:: Support Save/Load by keys 
        public static void Save<T>(string key, T data, OperationType operationType = OperationType.Append)
        {
            if (!Directory.Exists(DirectoryName))
                Directory.CreateDirectory(DirectoryName);

            var filePath = GetFullPath(FileName);
            var lines = File.ReadAllLines(filePath).ToList();
            if (!File.Exists(filePath) || lines.Count <= 0) 
                lines.AddRange(new []{"{","}"});

            var existKey = GetKeyLine(key, lines);
            if (existKey.Item2 != -1) {
                // Modify 
                if (operationType == OperationType.Replaced) {
                    lines[existKey.Item2] = CreateNewValue(existKey.Item1, data);
                }
                else {
                    
                }
            }
            else {
                var wrapper = new DataHolder<T>{
                    Value = data
                };
                
                var value = JsonUtility.ToJson(wrapper).Trim('}', '{');
                var replaceName = ChangeKeyName(value, key);
                var formatLine = FormatLine(replaceName);
                
                lines[^1] = formatLine;   
                lines.Add("}");
            }

            var file = lines.CreateString();
            File.WriteAllText(filePath, file);
        }

        public static T Load<T>(string key) {
            var fullPath = GetFullPath(FileName);

            if (!File.Exists(fullPath)) {
                Debug.LogWarning($"File not found at path: {fullPath}");
                return default;
            }
            
            var keyLine = GetKeyLine(key).Item1;
            if (string.IsNullOrEmpty(keyLine)) {
                Debug.LogWarning($"Key '{key}' not found in JSON file.");
                return default;
            }
            
            var jsonLine = ConvertLineToJson(keyLine);
            jsonLine = jsonLine.Replace(" ", "");
            jsonLine = ChangeName(jsonLine, key, "Value");
            
            return JsonUtility.FromJson<DataHolder<T>>(jsonLine).Value;
        }
        
        static (string, int) GetKeyLine(string key, IEnumerable<string> lines = null) {
            lines ??= File.ReadAllLines(GetFullPath(FileName));
            
            var i = 0;
            foreach (var line in lines) {
                if(line.Contains($"\"{key}\"")) 
                    return (line, i);

                i++;
            }

            return (string.Empty, -1);
        }
        
        static string FormatLine(string data) =>  "    " + data + ",";
        static string FormatToJson<T>(string key, T newValue) => $"{key}: {newValue}";
        static string ChangeKeyName(string jsonValue, string key) => ChangeName(jsonValue, "Value", key);
        static string ChangeName(string text, string from, string to) => text.Replace($"\"{from}\"", $"\"{to}\"");
        static string CreateNewValue<T>(string line, T newValue) => FormatToJson(GetKeyFromLine(line), newValue) + ",";
        static string ConvertLineToJson(string line) => "{" + line.Remove(line.Length - 1) + "}";
        static string GetKeyFromLine(string line) => line.Split(':')[0];
        
        */
        
        #endregion
    }
    
    public enum OperationType { 
        Replaced, 
        Append 
    }
}