using System.IO;
using Xunit;
using System.Linq;
using System.Text;
using System.Threading;

namespace DuplicateFinder.Test
{
    public class CollectCandidatesBySize
    {
        [Fact]
        public void collectCandidatesInSizeMode_SHOULD_returnEmpty_WHEN_directoryIsEmpty() {

            var duplicatefinder = getDuplicateFinder();
            var mode = "Empty";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);

            directoryCreation(mode, pathDir, 0);

            var duplicateBySize = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.Size);

            testDirectoryPermanentDeletion(pathDir);

            Assert.Empty(duplicateBySize);

            Thread.Sleep(500);
        }

        [Fact]
        public void collectCandidatesInSizeMode_SHOULD_returnNotEmpty_WHEN_directoryHasDuplicatedItens()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "OneDuplicatedSize";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedItems = 1;

            directoryCreation(mode, pathDir, numberOfDuplicatedItems);

            var duplicateBySize = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.Size);
            var duplicateBySizeList = duplicateBySize.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.NotEmpty(duplicateBySizeList);

            Thread.Sleep(500);
        }

        [Fact]
        public void collectCandidatesInSizeMode_SHOULD_assertOne_WHEN_directoryHasOneDuplicatedBySize()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "OneDuplicatedSize";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedItems = 1;

            directoryCreation(mode, pathDir, numberOfDuplicatedItems);

            var duplicateBySize = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.Size);
            var duplicateBySizeList = duplicateBySize.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.Equal(numberOfDuplicatedItems, duplicateBySizeList.Count);

            Thread.Sleep(500);
        }

        [Fact]
        public void collectCandidatesInSizeMode_SHOULD_assertMoreThanOne_WHEN_directoryHasMoreThanOneDuplicatedBySize()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "MoreThanOneDuplicatedSize";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedItems = 3;

            directoryCreation(mode, pathDir, numberOfDuplicatedItems);

            var duplicateBySize = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.Size);
            var duplicateBySizeList = duplicateBySize.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.Equal(numberOfDuplicatedItems, duplicateBySizeList.Count);

            Thread.Sleep(500);
        }

        #region{CollectCandidatesBySize Setup} 

        private DuplicateFinderWPF.Models.DuplicateFinder getDuplicateFinder() {
            return new DuplicateFinderWPF.Models.DuplicateFinder();
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
                /*for (int dir = 0; dir <= numberOfDuplicatedSizes; dir++)
                {
                    string pathNewDir = Path.Combine(pathDir, " Folder " + dir);
                    Directory.CreateDirectory(pathNewDir);
                    int elements = 2;
                    for (int i = 0; i < elements; i++)
                    {
                        pathFiletxt = Path.Combine(pathNewDir, "test" + i + ".txt");
                        FileStream fs = File.Create(pathFiletxt);
                        fs.Close();
                    }
                }*/
            }
            else if(mode == "MoreThanOneDuplicatedSize")
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

    public class CollectCandidatesBySizeAndName
    {
        [Fact]
        public void collectCandidatesInSizeAndNameMode_SHOULD_returnEmpty_WHEN_directoryIsEmpty()
        {

            var duplicatefinder = getDuplicateFinder();
            var mode = "Empty";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);

            directoryCreation(mode, pathDir, 0);

            var duplicateBySizeAndName = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.SizeAndName);

            testDirectoryPermanentDeletion(pathDir);

            Assert.Empty(duplicateBySizeAndName);

            Thread.Sleep(500);
        }

        [Fact]
        public void collectCandidatesInSizeAndNameMode_SHOULD_returnNotEmpty_WHEN_directoryHasDuplicatedItens()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "OneDuplicatedSizeAndName";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedItems = 1;

            directoryCreation(mode, pathDir, numberOfDuplicatedItems);

            var duplicateBySizeAndName = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.SizeAndName);
            var duplicateBySizeAndNameList = duplicateBySizeAndName.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.NotEmpty(duplicateBySizeAndNameList);

            Thread.Sleep(500);
        }

        [Fact]
        public void collectCandidatesInSizeAndNameMode_SHOULD_assertOne_WHEN_directoryHasOneDuplicatedBySizeAndName()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "OneDuplicatedSizeAndName";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedItems = 1;

            directoryCreation(mode, pathDir, numberOfDuplicatedItems);

            var duplicateBySizeAndName = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.SizeAndName);
            var duplicateBySizeAndNameList = duplicateBySizeAndName.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.Equal(numberOfDuplicatedItems, duplicateBySizeAndNameList.Count);

            Thread.Sleep(500);
        }

        [Fact]
        public void collectCandidatesInSizeAndNameMode_SHOULD_assertMoreThanOne_WHEN_directoryHasMoreThanOneDuplicatedBySizeAndName()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "MoreThanOneDuplicatedSizeAndName";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedItems = 3;

            directoryCreation(mode, pathDir, numberOfDuplicatedItems);

            var duplicateBySizeAndName = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.SizeAndName);
            var duplicateBySizeAndNameList = duplicateBySizeAndName.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.Equal(numberOfDuplicatedItems, duplicateBySizeAndNameList.Count);

            Thread.Sleep(500);
        }

        #region{CollectCandidatesBySizeAndName Setup} 

        private DuplicateFinderWPF.Models.DuplicateFinder getDuplicateFinder()
        {
            return new DuplicateFinderWPF.Models.DuplicateFinder();
        }

        private void directoryCreation(string mode, string pathDir, int numberOfDuplicatedSizes)
        {
            //Test Directory Creation
            Directory.CreateDirectory(pathDir);

            string pathFiletxt;
            if (mode == "Empty") { }
            else if (mode == "OneDuplicatedSizeAndName")
            {
                for (int dir = 0; dir <= numberOfDuplicatedSizes; dir++) 
                {
                    string pathNewDir = Path.Combine(pathDir, " Folder " + dir);
                    Directory.CreateDirectory(pathNewDir);
                    int elements = 1;
                    for (int i = 0; i < elements; i++)
                    {
                        pathFiletxt = Path.Combine(pathNewDir, "test" + i + ".txt");
                        FileStream fs = File.Create(pathFiletxt);
                        fs.Close();
                    } 
                }
            }
            else if (mode == "MoreThanOneDuplicatedSizeAndName")
            {
                for (int dir = 0; dir < numberOfDuplicatedSizes; dir++)
                {
                    string pathNewDir = Path.Combine(pathDir, " Folder " + dir);
                    Directory.CreateDirectory(pathNewDir);
                    for (int j = 0; j < numberOfDuplicatedSizes; j++)
                    {
                            pathFiletxt = Path.Combine(pathNewDir, "test_" + j + ".txt");
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

    public class CheckCanditates
    {
        [Fact]
        public void checkCandidates_SHOULD_returnEmpty_WHEN_directoryIsEmpty()
        {

            var duplicatefinder = getDuplicateFinder();
            var mode = "Empty";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);

            directoryCreation(mode, pathDir, 0);

            var duplicateBySizeAndName = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.SizeAndName);
            var duplicateByMD5 = duplicatefinder.CheckCandidates(duplicateBySizeAndName);

            testDirectoryPermanentDeletion(pathDir);

            Assert.Empty(duplicateByMD5);

            Thread.Sleep(500);
        }

        [Fact]
        public void checkCandidates_SHOULD_returnNotEmpty_WHEN_directoryHasDuplicatedItens()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "OneDuplicatedSizeAndName";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedItems = 1;

            directoryCreation(mode, pathDir, numberOfDuplicatedItems);

            var duplicateBySizeAndName = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.SizeAndName);
            var duplicateByMD5 = duplicatefinder.CheckCandidates(duplicateBySizeAndName);

            testDirectoryPermanentDeletion(pathDir);

            Assert.NotEmpty(duplicateByMD5);

            Thread.Sleep(500);
        }

        [Fact]
        public void checkCandidates_SHOULD_assertOne_WHEN_directoryHasOneDuplicatedByHash()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "OneDuplicatedSizeAndName";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedItems = 1;

            directoryCreation(mode, pathDir, numberOfDuplicatedItems);

            var duplicateBySizeAndName = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.SizeAndName);
            var duplicateByMD5 = duplicatefinder.CheckCandidates(duplicateBySizeAndName);
            var duplicateByMD5List = duplicateByMD5.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.Equal(numberOfDuplicatedItems, duplicateByMD5List.Count);

            Thread.Sleep(500);
        }

        [Fact]
        public void checkCandidates_SHOULD_assertMoreThanOne_WHEN_directoryHasMoreThanOneDuplicatedBySizeAndName()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "MoreThanOneDuplicatedSizeAndName";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedItems = 3;

            directoryCreation(mode, pathDir, numberOfDuplicatedItems);

            var duplicateBySizeAndName = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.SizeAndName);
            var duplicateByMD5 = duplicatefinder.CheckCandidates(duplicateBySizeAndName);
            var duplicateByMD5List = duplicateByMD5.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.Equal(numberOfDuplicatedItems, duplicateByMD5List.Count);

            Thread.Sleep(500);
        }

        #region{CheckCandidates Setup} 

        private DuplicateFinderWPF.Models.DuplicateFinder getDuplicateFinder()
        {
            return new DuplicateFinderWPF.Models.DuplicateFinder();
        }

        private void directoryCreation(string mode, string pathDir, int numberOfDuplicatedSizes)
        {
            //Test Directory Creation
            Directory.CreateDirectory(pathDir);

            string pathFiletxt;
            if (mode == "Empty") { }
            else if (mode == "OneDuplicatedSizeAndName")
            {
                for (int dir = 0; dir <= numberOfDuplicatedSizes; dir++)
                {
                    string pathNewDir = Path.Combine(pathDir, " Folder " + dir);
                    Directory.CreateDirectory(pathNewDir);
                    int elements = 1;
                    for (int i = 0; i < elements; i++)
                    {
                        pathFiletxt = Path.Combine(pathNewDir, "test" + i + ".txt");
                        FileStream fs = File.Create(pathFiletxt);
                        fs.Close();
                    }
                }
            }
            else if (mode == "MoreThanOneDuplicatedSizeAndName")
            {
                for (int dir = 0; dir < numberOfDuplicatedSizes; dir++)
                {
                    string pathNewDir = Path.Combine(pathDir, " Folder " + dir);
                    Directory.CreateDirectory(pathNewDir);
                    for (int j = 0; j < numberOfDuplicatedSizes; j++)
                    {
                        pathFiletxt = Path.Combine(pathNewDir, "test_" + j + ".txt");
                        FileStream fs = File.Create(pathFiletxt);

                        for (int k = 0; k < j; k++)
                        {
                            AddText(fs, "This is some text");
                            AddText(fs, "This is some more text,");
                            AddText(fs, "\r\nand this is on a new line");
                            AddText(fs, "\r\n\r\nThe following is a subset of characters:\r\n");
                        }

                        fs.Close();

                        pathFiletxt = Path.Combine(pathNewDir, "test_" + j + ".doc");
                        FileStream fsw = File.Create(pathFiletxt);

                        for (int k = 0; k < j; k++)
                        {
                            AddText(fsw, "This is some text");
                            AddText(fsw, "This is some more text,");
                            AddText(fsw, "\r\nand this is on a new line");
                            AddText(fsw, "\r\n\r\nThe following is a subset of characters:\r\n");
                        }

                        fsw.Close();
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