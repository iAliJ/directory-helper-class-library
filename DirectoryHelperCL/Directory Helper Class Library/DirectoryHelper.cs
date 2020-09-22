using System;
using System.IO;

namespace DirectoryHelperCL
{
    public class DirectoryHelper
    {
        /// <summary>
        /// Renames a directory folder, return error if directory not found.
        /// </summary>
        /// <param name="source">Path of the directory to be renamed.</param>
        /// <param name="newName">New name of the source directory.</param>
        public static void DirectoryRename(string source, string newName)
        {
            DirectoryInfo dir = new DirectoryInfo(source);
            if (!dir.Exists)
                throw new Exception($"Directory {source} does not exist");
            string mainDir = source.Substring(0, source.LastIndexOf('\\'));
            string newDir = Path.Combine(mainDir, newName);
            Directory.Move(source, newDir);
        }

        /// <summary>
        /// Copy a directory to a destination location.
        /// </summary>
        /// <param name="sourceDir">Source directory path</param>
        /// <param name="destDir">Path of destination directory</param>
        /// <param name="copySubDirs">Option to copy sub directories</param>
        public static void DirectoryCopy(string sourceDir, string destDir, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDir);
            destDir += sourceDir.Substring(sourceDir.LastIndexOf('\\'));
            // If the destination directory exist, throw an exception.
            // dest folder should be dest path + source folder name
            if (Directory.Exists(destDir))
            {
                throw new Exception($"Folder {destDir} already exist");
            }

            // If source file does not exist
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDir);
            }

            // Get all sub directories in source directory
            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = Path.Combine(destDir, file.Name);
                file.CopyTo(tempPath, false);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = Path.Combine(destDir, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}
