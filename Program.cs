using System;
using System.IO;


namespace MovingFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\Public\TestFolder\Printed";
            Console.SetWindowSize(50, 10);
            Console.Title = "Print file Transfer";
            MonitorDirectory(path);
            Console.ReadKey();
        }

        private static void MonitorDirectory(string path)
        {
            FileSystemWatcher fileSystemWatcher = new FileSystemWatcher
            {
                Path = path
            };

            fileSystemWatcher.Created += FileSystemWatcher_Created;
            fileSystemWatcher.EnableRaisingEvents = true;
        }

        private static void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            string fileName;

            fileName = Path.GetFileNameWithoutExtension(e.Name);
            Console.WriteLine("File name without extension:('{0}') returns '{1}'",
                e.Name, fileName);

            string woextract = fileName.Substring(0, 6);
            string jobIdextract = fileName.Substring(fileName.Length - 4);

            Console.WriteLine("Work Order number is:" + woextract);
            Console.WriteLine(" Job Id is:" + jobIdextract);
            MovePRTFiles(fileName, jobIdextract);
            MoveWOFiles(jobIdextract);
            MoveJobIDFiles(jobIdextract);
            //Environment.Exit(1000);
        }

        private static void MovePRTFiles(string fileName, string jobIdextract)
        {
            String sourcePath = @"C:\Users\Public\TestFolder\PRT";
            String destPath = @"C:\Users\Public\TestFolder\To Print Queue\";

            //Creates standard path directory structure by combining destPath with WO name
            string woPath = Path.Combine(destPath, fileName);

            //Create folder structure using WO folder name convention
            Directory.CreateDirectory(woPath);
            Directory.CreateDirectory(woPath + "\\Data");
            Directory.CreateDirectory(woPath + "\\Design");
            Directory.CreateDirectory(woPath + "\\Proof");
            Directory.CreateDirectory(woPath + "\\Ready to Print");
            Directory.CreateDirectory(woPath + "\\Reports");
            Directory.CreateDirectory(woPath + "\\Setup");

            //Create string to find all similar file names based on JobId
            String prtFile = "*" + jobIdextract + "*.*";
            string[] prtfiles = Directory.GetFiles(sourcePath, prtFile, SearchOption.AllDirectories);

            //For each file found Copy the file to the final destination then delete original print file
            for (int print = 0; print < prtfiles.Length; print++)
            {
                string file = prtfiles[print];
                string origPrt = new FileInfo(file).Name;

                File.Copy(file, woPath + "\\Ready to Print\\" + origPrt, false);
                Console.WriteLine("Copied file is:" + origPrt + " file size is:" + origPrt.Length);

                //If file exist in PRT folder delete file.
                if (File.Exists(sourcePath + "\\" + origPrt))
                {
                    try
                    {
                        File.Delete(sourcePath + "\\" + origPrt);
                        LogFile(fileName, origPrt);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine(ex.Message);
                        return;
                    }
                }
            }
        }

        private static void OrigFileCopyDel(string sourcePath, string origFile, string file, string destPath)
        {
            File.Copy(file, destPath + "\\" + origFile, false);

            //If file exist in PRT folder delete file.
            if (File.Exists(sourcePath + "\\" + origFile))
            {
                try
                {
                    File.Delete(sourcePath + "\\" + origFile);
                }
                catch (IOException ex)
                {
                    Console.WriteLine(ex.Message);
                    return;
                }
            }
        }

        private static void MoveWOFiles(string jobIdextract)
        {
            String sourcePath = @"C:\Users\Public\TestFolder\Printed";
            String destPath = @"C:\Users\Public\TestFolder\Printed\Production";

            //Create string to find all similar file names based on JobId
            String woclFile = "*" + jobIdextract + "*.pdf";
            string[] wofiles = Directory.GetFiles(sourcePath, woclFile, SearchOption.AllDirectories);

            for (int print = 0; print < wofiles.Length; print++)
            {
                string file = wofiles[print];
                string origFile = new FileInfo(file).Name;

                OrigFileCopyDel(sourcePath, origFile, file, destPath);
            }
        }

        private static void MoveJobIDFiles(string jobIdextract)
        {
            string sourcePath = @"C:\Users\Public\TestFolder";
            String destPath = @"C:\Users\Public\TestFolder\Printed\Production";

            //Create string to find all similar file names based on JobId
            String jobIdFile = jobIdextract + "*.txt";
            string[] jobFiles = Directory.GetFiles(sourcePath, jobIdFile, SearchOption.AllDirectories);

            for (int print = 0; print < jobFiles.Length; print++)
            {
                string file = jobFiles[print];
                string origFile = new FileInfo(file).Name;

                OrigFileCopyDel(sourcePath, origFile, file, destPath);
            }
        }

        private static void LogFile(string fileName, string origPrt)
        {
            using StreamWriter writer = new StreamWriter(@"C:\Users\Public\TestFolder\Log\" + fileName + ".txt", true);
            long fileDate = DateTime.Now.ToFileTime();

            writer.WriteLine("The Work Order:"
                             + origPrt
                             + ", File size is: "
                             + origPrt.Length
                             + ", the date of completion is:"
                             + fileDate);
        }
    }
}
