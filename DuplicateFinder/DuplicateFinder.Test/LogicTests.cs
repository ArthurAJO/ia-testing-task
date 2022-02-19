using DuplicateFinder.Logic.Interface;
using Moq;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions.TestingHelpers;
using Xunit;
using System.Linq;
using Mock.Interface;
using System.Text;

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

            var duplicateBySizeAndName = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.Size);

            testDirectoryPermanentDeletion(pathDir);

            Assert.Empty(duplicateBySizeAndName);
        }

        [Fact]
        public void collectCandidatesInSizeMode_SHOULD_returnNotEmpty_WHEN_directoryIsNotEmpty()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "OneDuplicatedSize";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedSizes = 1;

            directoryCreation(mode, pathDir, numberOfDuplicatedSizes);

            var duplicateBySize = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.Size);
            var duplicateBySizeList = duplicateBySize.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.NotEmpty(duplicateBySizeList);

        }

        [Fact]
        public void collectCandidatesInSizeMode_SHOULD_assertOne_WHEN_directoryHasOneDuplicatedSize()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "OneDuplicatedSize";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedSizes = 1;

            directoryCreation(mode, pathDir, numberOfDuplicatedSizes);

            var duplicateBySize = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.Size);
            var duplicateBySizeList = duplicateBySize.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.Equal(numberOfDuplicatedSizes, duplicateBySizeList.Count);

        }

        [Fact]
        public void collectCandidatesInSizeMode_SHOULD_assertMoreThanOne_WHEN_directoryHasMoreThanOneDuplicatedSize()
        {
            var duplicatefinder = getDuplicateFinder();
            var mode = "MoreThanOneDuplicatedSize";
            string dirName = "TestDirectory";
            string pathDir = Path.Combine(Path.GetTempPath(), dirName);
            var numberOfDuplicatedSizes = 3;

            directoryCreation(mode, pathDir, numberOfDuplicatedSizes);

            var duplicateBySize = duplicatefinder.CollectCandidates(
                pathDir, DuplicateFinderWPF.Models.CompareMode.Size);
            var duplicateBySizeList = duplicateBySize.ToList();

            testDirectoryPermanentDeletion(pathDir);

            Assert.Equal(numberOfDuplicatedSizes, duplicateBySizeList.Count);
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

}