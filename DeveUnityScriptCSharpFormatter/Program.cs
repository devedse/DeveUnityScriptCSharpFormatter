using System;
using System.Threading.Tasks;

namespace DeveUnityScriptCSharpFormatter
{
    class Program
    {
        static void Main(string[] args)
        {
            var directories = UnityDirectoryFinder.FindUnityDirectories();
            var scriptTemplateDirectories = UnityDirectoryFinder.GetScriptTemplatesDirectoriesFromUnityDirectories(directories);

            UnityDirectoryFinder.UpdateCSharpFilesInDirectories(scriptTemplateDirectories);

            Task.Delay(2000).Wait();
        }
    }
}