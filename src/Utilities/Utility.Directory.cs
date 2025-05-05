using System.IO;


namespace LogicGamer.Core.Utilities
{
    public static partial class Utility
    {
        public class Directory
        {
            /// <summary>
            /// 确保目录存在，并清空其内容（包括所有文件和子目录）
            /// </summary>
            /// <param name="directoryPath">要检查和清理的目录路径（绝对路径）</param>
            public static void EnsureAndClearDirectory(string directoryPath)
            {
                if (!System.IO.Directory.Exists(directoryPath))
                {
                    System.IO.Directory.CreateDirectory(directoryPath);
                }

                // 删除所有文件
                foreach (var file in System.IO.Directory.GetFiles(directoryPath))
                {
                    System.IO.File.Delete(file);
                }

                // 删除所有子目录
                foreach (var dir in System.IO.Directory.GetDirectories(directoryPath))
                {
                    System.IO.Directory.Delete(dir, true); // true 表示递归删除
                }
            }
            
            /// <summary>
            /// 将一个文件从源路径复制到目标目录，并使用指定文件名保存。
            /// </summary>
            /// <param name="sourceFilePath">源文件的完整路径（绝对路径）</param>
            /// <param name="targetFileName">目标文件名（含扩展名）</param>
            public static void CopyFileTo(string sourceFilePath,string targetFileName)
            {
                if (!File.Exists(sourceFilePath))
                {
                    throw new FileNotFoundException($"源文件不存在: {sourceFilePath}");
                }
                
                // 获取目标文件的目录路径
                string targetDir = Path.GetDirectoryName(targetFileName);

                // 如果目标目录不存在，则递归创建
                if (!string.IsNullOrEmpty(targetDir)&&!System.IO.Directory.Exists(targetDir))
                {
                    System.IO.Directory.CreateDirectory(targetDir);
                }
                File.Copy(sourceFilePath, targetFileName, true);
            }
        }
    }

}