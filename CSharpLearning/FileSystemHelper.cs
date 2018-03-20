using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace CSharpLearning
{
    /// <summary>
    /// Utility class to help with common file system operations.
    /// </summary>
    class FileSystemHelper
    {
        static void deleteAllFiles(string target_dir)
        {
            foreach (string file in Directory.GetFiles(target_dir))
            {
                File.SetAttributes(file, FileAttributes.Normal);
                File.Delete(file);
            }
        }

        static void deleteAllDirectories(string target_dir)
        {
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }
        }
        /// <summary>
        /// Deletes a directory with files.
        /// </summary>
        /// <param name="target_dir"></param>
        public static void DeleteDirectory(string target_dir)
        {
            if (target_dir == null)
                throw new ArgumentNullException("target_dir");
            try
            {
                deleteAllFiles(target_dir);
                deleteAllDirectories(target_dir);
                Directory.Delete(target_dir, false);
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Creates a directory if it does not already exist.
        /// </summary>
        /// <param name="filePath"></param>
        public static void EnsureDirectory(string filePath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)) && string.IsNullOrWhiteSpace(Path.GetDirectoryName(filePath)) == false)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            }
        }

        /// <summary>
        /// Creates a temporary directory.
        /// </summary>
        /// <returns> Path to the temporary directory.</returns>
        public static string CreateTempDirectory()
        {
            string path = Path.Combine(System.IO.Path.GetTempPath(), Path.GetRandomFileName());
            EnsureDirectory(path);
            return path;
        }

        public static string CreateTempFile()
        {
            return Path.Combine(System.IO.Path.GetTempPath(), Path.GetRandomFileName());
        }

        static string WildcardToRegex(string pattern)
        {
            return "^" + Regex.Escape(pattern).
                Replace("\\*", ".*").
                Replace("\\?", ".") + "$";
        }

        public static IEnumerable<string> ExpandWildcards(string path)
        {
            if (path == null)
                throw new ArgumentNullException("path");
            var splitted = path.Split('\\');
            if (splitted.Last().Contains('*'))
            {
                string dir = String.Join("\\", splitted.Take(splitted.Length - 1));
                var reg = new Regex(WildcardToRegex(splitted.Last()));
                try
                {
                    var files = Directory.GetFiles(dir, "*")
                        .Where(file =>
                        {
                            var name = Path.GetFileName(file);
                            return reg.Match(name).Success;
                        });
                    return files;
                }
                catch (DirectoryNotFoundException)
                {
                    Console.WriteLine(String.Format("Directory {0} not found", dir));
                }
                return new string[0];

            }
            else if (File.GetAttributes(path).HasFlag(FileAttributes.Directory))
            {
                return Directory.GetFiles(path);
            }
            else
            {
                return new string[] { path };
            }
        }

        public static void CheckPathIsRelative(string path)
        {
            if (Path.IsPathRooted(path))
                throw new ArgumentException("Paths must be relative.");
        }

        /// <summary>
        /// Compares two paths to get the relative between base and end. The string has to be a standard file system string like "C:\Program Files\...".
        /// </summary>
        /// <param name="baseDirectory"></param>
        /// <param name="endDirectory"></param>
        /// <returns></returns>
        internal static string GetRelativePath(string baseDirectory, string endDirectory)
        {
            var baseSplit = baseDirectory.Split('\\');
            var endSplit = endDirectory.Split('\\');
            int idx = getSameIndex(baseSplit, endSplit);
            var dots = String.Join("\\", baseSplit.Skip(idx).Select(v => ".."));
            var afterDots = string.Join("\\", endSplit.Skip(idx));

            string temp = System.IO.Path.Combine(dots, afterDots);

            //If the resulting directory is empty, put in a dot.  This allows the hover help to work
            return temp == string.Empty ? "." : temp;
        }

        static int getSameIndex(string[] a, string[] b)
        {
            int end = Math.Min(a.Length, b.Length);
            for (int i = 0; i < end; i++)
            {
                if (a[i] != b[i])
                {
                    return i;
                }
            }
            return end;
        }

        public static string GetAssemblyVersion(string assemblyPath)
        {
            if (assemblyPath == null)
                throw new ArgumentNullException("assemblyPath");
            assemblyPath = Path.GetFullPath(assemblyPath); // this is important to make sure that we take the version number from the file in the current directory, and not the one in TAP_PATH
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(assemblyPath);
            return info.ProductVersion;
        }

        /// <summary> 
        /// Modifies each line in a file by using the modify function.
        /// It does so in-place and out of memory, so large files can be modified.
        /// </summary>
        /// <param name="file"> The file that should be modified.</param>
        /// <param name="modify">The function that can modify each line.</param>
        public static void ModifyLines(string file, Func<string, int, string> modify)
        {
            Queue<string> lineBuffer = new Queue<string>();

            using (var input = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var output = File.Open(file, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            {
                var outputStream = new StreamWriter(output);
                var inputStream = new StreamReader(input);
                int lineCount = 0;

                while (true)
                {
                    string line;
                    if (lineBuffer.Count == 0)
                    {
                        line = inputStream.ReadLine();
                        if (line == null) break;
                    }
                    else
                    {
                        line = lineBuffer.Dequeue();
                    }

                    // if modify returns null, just add it.
                    line = modify(line, lineCount++) ?? line;

                    // make sure that we dont overwrite data that we have not read yet
                    // by reading ahead into a buffer.
                    while (output.Position + line.Length > input.Position)
                    {
                        var addline = inputStream.ReadLine();
                        if (addline == null)
                            break; // reached end of input file.
                        lineBuffer.Enqueue(addline);
                    }

                    outputStream.WriteLine(line);
                }

                outputStream.Flush();

                // File might have become smaller. if so truncate.
                if (output.Length > output.Position)
                    output.SetLength(output.Position);
            }
        }

        public static string EscapeBadPathChars(string path)
        {
            try
            {
                new FileInfo(path);
            }
            catch
            {
                var fname = Path.GetFileName(path);
                var dirname = Path.GetDirectoryName(path);

                var pathchars = Path.GetInvalidPathChars();
                foreach (var chr in pathchars)
                {
                    dirname = dirname.Replace(chr, '_');
                }

                var filechars = Path.GetInvalidFileNameChars();
                foreach (var chr in filechars)
                {
                    fname = fname.Replace(chr, '_');
                }

                path = Path.Combine(dirname, fname);

                new FileInfo(path);
            }
            return path;
        }

        public static string CreateUniqueFileName(string path)
        {
            if (!File.Exists(path)) return path;

            var ext = Path.GetExtension(path);
            var name = Path.GetFileNameWithoutExtension(path);

            int i = 2;

            // Just put some upper bound on this
            while (i < 100000)
            {
                var newPath = name + " (" + i + ")" + ext;

                if (!File.Exists(newPath)) return newPath;
                i++;
            }

            return path;
        }
    }
}
