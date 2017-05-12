using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DeveUnityScriptCSharpFormatter
{
    public static class UnityDirectoryFinder
    {
        public static IEnumerable<string> FindUnityDirectories()
        {
            string[] potentialUnityLocations = new string[] {
                "Program Files",
                "Program Files (x86)"
            };

            var drives = DriveInfo.GetDrives();

            var foundUnityDirs = new List<string>();

            foreach (var drive in drives)
            {
                foreach (var potentialLocation in potentialUnityLocations)
                {
                    try
                    {
                        var combinedPath = Path.Combine(drive.RootDirectory.FullName, potentialLocation);
                        var allDirs = Directory.GetDirectories(combinedPath);
                        foreach (var dir in allDirs)
                        {
                            var dirName = Path.GetFileName(dir);
                            if (dirName.ToLowerInvariant().Contains("unity"))
                            {
                                foundUnityDirs.Add(dir);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            return foundUnityDirs;
        }

        public static IEnumerable<string> GetScriptTemplatesDirectoriesFromUnityDirectories(IEnumerable<string> unitydirs)
        {
            foreach (var unityDir in unitydirs)
            {
                var combinedPath = Path.Combine(unityDir, @"Editor\Data\Resources\ScriptTemplates");
                if (Directory.Exists(combinedPath))
                {
                    yield return combinedPath;
                }
            }
        }

        public static void UpdateCSharpFilesInDirectories(IEnumerable<string> scriptTemplateDirs)
        {
            foreach (var dir in scriptTemplateDirs)
            {
                var files = Directory.GetFiles(dir);
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file);
                    if (fileName.ToLowerInvariant().Contains("c#"))
                    {
                        UpdateCSharpFile(file);
                    }
                }
            }
        }

        private static string[] _files;
        private static string[] Files
        {
            get
            {
                if (_files == null)
                {
                    var assembly = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                    var filesFolder = Path.Combine(assembly, "Files");
                    _files = Directory.GetFiles(filesFolder);
                }
                return _files;
            }
        }

        public static void UpdateCSharpFile(string file)
        {
            var fileName = Path.GetFileName(file);

            var foundFile = Files.FirstOrDefault(t => string.Equals(fileName, Path.GetFileName(t), StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(foundFile))
            {
                File.Copy(foundFile, file, true);
                Console.WriteLine("Copied over: " + file);
            }
        }
    }
}
