using System.IO;
using Xunit;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using DuplicateFinderWPF.Interface;
using System;
using DuplicateFinderWPF.Models;


namespace DuplicateFinder.Test
{
    public class RenameDuplicatedFiles
    {
        
        [Fact]
        public void RenameDuplicatedFiles_SHOULD_returnDuplicateType_WHEN_deliverAReturn()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "OneDuplicatedSize";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedItems = 1;

            directoryCreation(mode, pathDir, numberOfDuplicatedItems);

            var duplicateBySize = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.Size);
            var renameReturn = duplicatefinder.RenameDuplicatedFiles(duplicateBySize);

            testDirectoryPermanentDeletion(pathDir);

            Assert.IsAssignableFrom<IEnumerable<IDuplicate>>(renameReturn);

            Thread.Sleep(500);
        }

        [Fact]
        public void RenameDuplicatedFiles_SHOULD_returnEmpty_WHEN_duplicatedItemsAreEmpty()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "Empty";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedItems = 1;

            directoryCreation(mode, pathDir, numberOfDuplicatedItems);

            var duplicateBySize = duplicatefinder.CollectCandidates(pathDir, CompareMode.Size);

            var count = 0;
            var duplicatedFiles = new Dictionary<string, List<string>>();
            foreach (var file in duplicateBySize)
            {
                foreach (var filePath in file.FilePaths)
                {
                    string countPosition = "" + count;
                    duplicatedFiles.Add(countPosition, new List<string>());
                    duplicatedFiles[countPosition].Add(filePath);
                    count++;
                }
            }

            var renameReturn = duplicatefinder.RenameDuplicatedFiles(duplicateBySize);
            var renameReturnList = renameReturn.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.Empty(renameReturnList);

            Thread.Sleep(500);
        }

        [Fact]
        public void RenameDuplicatedFiles_SHOULD_renameAllTheInputFiles_WHEN_duplicatedItemsAreNotEmpty()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "OneDuplicatedSize";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedItems = 1;

            directoryCreation(mode, pathDir, numberOfDuplicatedItems);

            var duplicateBySize = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.Size);

            var count = 0;
            var duplicatedFiles = new Dictionary<string, List<string>>();
            foreach (var file in duplicateBySize)
            {
                foreach (var filePath in file.FilePaths)
                {
                    string countPosition = "" + count;
                    duplicatedFiles.Add(countPosition, new List<string>());
                    duplicatedFiles[countPosition].Add(filePath);
                    count++;
                }
            }

            var renameReturn = duplicatefinder.RenameDuplicatedFiles(duplicateBySize);
            var renameReturnList = renameReturn.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.Equal(duplicatedFiles.Count, renameReturnList.Count);

            Thread.Sleep(500);
        }

        [Fact]
        public void RenameDuplicatedFiles_SHOULD_renameFiles_WHEN_renamedFileNameAlreadyExist()
        {
            var duplicatefinder = getDuplicateFinder();
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);

            directoryCreationFilesWithNamesAlreadySet(pathDir);

            var duplicateBySize = duplicatefinder.CollectCandidates(pathDir, CompareMode.Size);

            duplicatefinder.RenameDuplicatedFiles(duplicateBySize);

            var resultOne = Path.Combine(pathDir, "test (2).txt");
            var resultTwo = Path.Combine(pathDir, "test (1) (2).txt");
            var resultThree = Path.Combine(pathDir, "test (1) (1) (2).txt");
            var resultFour = Path.Combine(pathDir, "test (1) (1) (1) (1).txt");

            var resultFileNames = Directory.GetFiles(pathDir);

            Assert.Contains(resultOne, resultFileNames);
            Assert.Contains(resultTwo, resultFileNames);
            Assert.Contains(resultThree, resultFileNames);
            Assert.Contains(resultFour, resultFileNames);

            testDirectoryPermanentDeletion(pathDir);

            Thread.Sleep(500);
        }

        [Fact]
        public void RenameDuplicatedFilesWithBlacklistCheck_SHOULD_notRenameFiles_WHEN_pathIsBlacklisted()
        {
            var duplicatefinder = getDuplicateFinder();
            var duplicateBySize = createBlacklistedFiles();

            var renameReturn = duplicatefinder.RenameDuplicatedFilesWithBlacklistCheck(duplicateBySize);
            var renameReturnList = renameReturn.ToList();

            Assert.Empty(renameReturnList);

            Thread.Sleep(500);
        }

        [Fact]
        public void RenameDuplicatedFilesWithBlacklistCheck_SHOULD_renameFiles_WHEN_pathIsNotBlacklisted()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "OneDuplicatedSize";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedItems = 1;

            directoryCreation(mode, pathDir, numberOfDuplicatedItems);

            var duplicateBySize = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.Size);

            
            var renameReturn = duplicatefinder.RenameDuplicatedFilesWithBlacklistCheck(duplicateBySize);
            var renameReturnList = renameReturn.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.NotEmpty(renameReturnList);

            Thread.Sleep(500);
        }

        [Fact]
        public void RenameDuplicatedFilesWithBlacklistCheck_SHOULD_renameAllTheInputFiles_WHEN_deliverAReturn()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "OneDuplicatedSize";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedItems = 1;

            directoryCreation(mode, pathDir, numberOfDuplicatedItems);

            var duplicateBySize = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.Size);

            var count = 0;
            var duplicatedFiles = new Dictionary<string, List<string>>();
            foreach (var file in duplicateBySize)
            {
                foreach (var filePath in file.FilePaths)
                {
                    string countPosition = "" + count;
                    duplicatedFiles.Add(countPosition, new List<string>());
                    duplicatedFiles[countPosition].Add(filePath);
                    count++;
                }
            }

            var renameReturn = duplicatefinder.RenameDuplicatedFilesWithBlacklistCheck(duplicateBySize);
            var renameReturnList = renameReturn.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.Equal(duplicatedFiles.Count, renameReturnList.Count);

            Thread.Sleep(500);
        }

        [Fact]
        public void RenameDuplicatedFilesWithBlacklistCheck_SHOULD_renameFiles_WHEN_renamedFileNameAlreadyExist()
        {
            var duplicatefinder = getDuplicateFinder();
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);

            directoryCreationFilesWithNamesAlreadySet(pathDir);

            var duplicateBySize = duplicatefinder.CollectCandidates(pathDir, CompareMode.Size);

            duplicatefinder.RenameDuplicatedFilesWithBlacklistCheck(duplicateBySize);

            var resultOne = Path.Combine(pathDir, "test (2).txt");
            var resultTwo = Path.Combine(pathDir, "test (1) (2).txt");
            var resultThree = Path.Combine(pathDir, "test (1) (1) (2).txt");
            var resultFour = Path.Combine(pathDir, "test (1) (1) (1) (1).txt");

            var resultFileNames = Directory.GetFiles(pathDir);

            Assert.Contains(resultOne , resultFileNames);
            Assert.Contains(resultTwo , resultFileNames);
            Assert.Contains(resultThree , resultFileNames);
            Assert.Contains(resultFour, resultFileNames);

            testDirectoryPermanentDeletion(pathDir);

            Thread.Sleep(500);
        }

        #region{RenameDuplicatedFiles Setup} 

        private DuplicateFinderWPF.Models.DuplicateFinder getDuplicateFinder()
        {
            return new DuplicateFinderWPF.Models.DuplicateFinder();
        }

        private IEnumerable<IDuplicate> createBlacklistedFiles() 
        {
            var workingDirectory = Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.Parent.FullName;

            string bracklistFilePath = Path.Combine(projectDirectory, "DuplicateFinderWPF/Resources/Files Blacklist.txt");

            string[] blacklistedPaths = File.ReadAllLines(bracklistFilePath);
            var count = 0;

            var blacklistedPathsDictionary = new Dictionary<string, List<string>>();
            foreach (var files in blacklistedPaths)
            {
                string countPosition = "" + count;
                blacklistedPathsDictionary.Add(countPosition, new List<string>());
                blacklistedPathsDictionary[countPosition].Add(files);
                count++;
            }

            //IEnumerable<IDuplicate> blacklistedFiles = new List<IDuplicate>();
            List<IDuplicate> blacklistedFiles = new List<IDuplicate>();
            foreach (var files in blacklistedPathsDictionary)
            {
                blacklistedFiles.Add(new Duplicate(files.Value));
            }
            return blacklistedFiles;
        }
        
        private void directoryCreationFilesWithNamesAlreadySet(string pathDir)
        {
            //Test Directory Creation
            Directory.CreateDirectory(pathDir);

            string pathFiletxt;
            string[] pathFile = { "test.txt", "test (1).txt", "test (1) (1).txt", "test (1) (1) (1).txt" };
            for (int i = 0; i < pathFile.Length; i++)
            {
                pathFiletxt = Path.Combine(pathDir, pathFile[i]);
                FileStream fs = File.Create(pathFiletxt);
                fs.Close();
            }
        }

        private void directoryCreation(string mode, string pathDir, int numberOfDuplicatedSizes)
        {
            //Test Directory Creation
            Directory.CreateDirectory(pathDir);

            string pathFiletxt;
            if (mode == "Empty") { }
            else if (mode == "OneDuplicatedSize")
            {
                int elements = 3;
                for (int i = 0; i < elements; i++)
                {
                    pathFiletxt = Path.Combine(pathDir, "test" + i + ".txt");
                    FileStream fs = File.Create(pathFiletxt);
                    fs.Close();
                }
            }
            else if (mode == "MoreThanOneDuplicatedSize")
            {
                int elements = 2;
                for (int j = 0; j < numberOfDuplicatedSizes; j++)
                {
                    for (int i = 0; i < elements; i++)
                    {
                        pathFiletxt = Path.Combine(pathDir, "test" + i + "_" + j + ".txt");
                        FileStream fs = File.Create(pathFiletxt);

                        for (int k = 0; k < j; k++)
                        {
                            AddText(fs, "This is some text");
                            AddText(fs, "This is some more text,");
                            AddText(fs, "\r\nand this is on a new line");
                            AddText(fs, "\r\n\r\nThe following is a subset of characters:\r\n");
                        }

                        fs.Close();
                    }
                }
            }
        }

        private void testDirectoryPermanentDeletion(string pathDir)
        {
            DirectoryInfo di = new DirectoryInfo(pathDir);
            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (DirectoryInfo dir in di.GetDirectories())
            {
                dir.Delete(true);
            }
            Directory.Delete(pathDir);
        }

        private static void AddText(FileStream fs, string value)
        {
            byte[] info = new UTF8Encoding(true).GetBytes(value);
            fs.Write(info, 0, info.Length);
        }

        #endregion
    }

}