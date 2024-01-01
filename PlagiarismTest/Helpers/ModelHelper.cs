using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlagiarismTest.Helpers
{
    public class ModelHelper
    {
        public static List<User> GenerateUsersData()
        {
            return new()
            {
                new User()
                {
                    Id = new Guid("8ddfa4dd-d8f3-4f3a-bb78-0ded6d5e2305"),
                    Name = "Vlad",
                    Surname = "Zientsov",
                    UniversityYear = 4,
                    GroupName = "IS-01",
                    Email = "vladzentsov@gmail.com",
                    PassHash = "$2a$12$2jqPtyTyTNcFu5AGn.Ka5u/OkTRnnfWStZ2MAA9E1Jp4NG9TTyCtq",
                    Role = Role.User
                },
                new User()
                {
                    Id = new Guid("a393f87e-6dbc-4d2e-b723-e6e30502e319"),
                    Name = "Artem",
                    Surname = "Storoghev",
                    UniversityYear = 3,
                    GroupName = "IS-12",
                    Email = "artemstoroghev@gmail.com",
                    PassHash = "$2a$12$tBDZakYF6O3WFIRJbBediOdgh.1JSyZ4Dez7nhQm6Fao0t.wjeUfC",
                    Role = Role.User
                }
            };
        }

        public static List<Work> GenerateWorksData()
        {
            return new()
            {
                new Work()
                {
                    Id = new Guid("89e202f0-bda2-4713-beee-0c3125266658"),
                    Code = "using Plagiarism_BLL.CoreModels;\r\nusing Plagiarism_BLL.Enums;\r\nusing Plagiarism_BLL.Services.Interfaces;\r\nusing Plagiarism_BLL.UnitOfWork;\r\nusing System;\r\nusing System.Collections;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\n\r\nnamespace Plagiarism_BLL.Services\r\n{\r\n    public class WorkService: IWorkService\r\n    {\r\n        private List<string> ExcludeLines = new List<string>() {\"\",\"{\",\"}\"};\r\n        private List<string> ExcludedStartsWith = new List<string>() { \"using\" };\r\n        private readonly IUnitOfWork _unitOfWork;\r\n        public WorkService(IUnitOfWork unitOfWork)\r\n        {\r\n            _unitOfWork = unitOfWork;\r\n        }\r\n\r\n        public async Task<FullWorkResult> UploadWork(Guid userId, string workName, WorkType workType, string code)\r\n        {\r\n            Work work = new Work()\r\n            {\r\n                Id = Guid.NewGuid(),\r\n                Code = code,\r\n            };\r\n            await _unitOfWork.WorkRepository.CreateAsync(work);\r\n\r\n            WorkInfo workInfo = new WorkInfo()\r\n            {\r\n                WorkName = workName,\r\n                UserId = userId,\r\n                WorkId = work.Id,\r\n                WorkType = workType,\r\n            };\r\n\r\n            await _unitOfWork.WorkInfoRepository.CreateAsync(workInfo);\r\n            await _unitOfWork.SaveAsync();\r\n\r\n            var fullWorkResult = new FullWorkResult()\r\n            {\r\n                Id = work.Id,\r\n                Code = code,\r\n                WorkName = workName,\r\n                WorkType = workType,\r\n            };\r\n\r\n            return fullWorkResult;\r\n        }\r\n        //public async Task GetWorkById(Guid workId)\r\n        //{\r\n        //    return _unitOfWork.\r\n        //}\r\n\r\n        public async Task<CompareWorksResult> CompareWorks(Guid currentWorkId, Guid workToCompareId)\r\n        {\r\n            var currentWork = await _unitOfWork.WorkRepository.GetByIdAsync(currentWorkId);\r\n            var workToCompare = await _unitOfWork.WorkRepository.GetByIdAsync(workToCompareId);\r\n            var currentWorkLines = GetWorkRawLines(currentWork);\r\n            var workToCompareLines = GetWorkRawLines(workToCompare);\r\n\r\n            var identicalLinesList = new List<IdenticalLines>();\r\n\r\n            foreach (var line in currentWorkLines)\r\n            {\r\n                var identicalLinesToCurrentLine = workToCompareLines.Where(l => l == line);\r\n\r\n                List<int> indices = Enumerable.Range(0, workToCompareLines.Count())\r\n                    .Where(i => workToCompareLines[i].Item1 == line.Item1)\r\n                    .ToList();\r\n\r\n                if(indices.Count!=0)\r\n                {\r\n                    List<int> workToCompareLineNumbers = new List<int>();\r\n                    foreach (int index in indices)\r\n                    {\r\n                        workToCompareLineNumbers.Add(workToCompareLines[index].Item2);\r\n                    }\r\n                    var identicalLines = new IdenticalLines()\r\n                    {\r\n                        CurrentWorkLineNumber = line.Item2,\r\n                        WorkToCompareLineNumbers = workToCompareLineNumbers\r\n                    };\r\n                    identicalLinesList.Add(identicalLines);\r\n                }\r\n            }\r\n            double matchPercentage = (double)identicalLinesList.Count() / currentWorkLines.Count();\r\n\r\n            var compareWorksResult = new CompareWorksResult()\r\n            {\r\n                IdenticalLines = identicalLinesList,\r\n                CurrentWork = ParseWork(currentWork),\r\n                WorkToCompare = ParseWork(workToCompare),\r\n                MatchPercentage = matchPercentage\r\n            };\r\n\r\n            return compareWorksResult;\r\n        }\r\n\r\n        private ParsedWork ParseWork(Work work)\r\n        {\r\n            var workLines = GetWorkLines(work);\r\n\r\n            return new ParsedWork()\r\n            {\r\n                Id = work.Id,\r\n                CodeLines = workLines\r\n            };\r\n        }\r\n        private List<(string, int)> GetWorkRawLines(Work work)\r\n        {\r\n            List<string> lines = work.Code.Replace(\" \", \"\").Split(\"\\r\\n\").ToList();\r\n            List<(string,int)> result = new List<(string, int)>();\r\n            int lineIndex = 0;\r\n            foreach (var line in lines)\r\n            {\r\n                if (!ExcludeLines.Contains(line))\r\n                {\r\n                    foreach (string excludedStartsWith in ExcludedStartsWith)\r\n                    {\r\n                        if (!line.StartsWith(excludedStartsWith))\r\n                        {\r\n                            result.Add((line, lineIndex));\r\n                        }\r\n                    }\r\n                }\r\n\r\n                lineIndex++;\r\n            }\r\n\r\n            return result;\r\n        }\r\n\r\n        private string[] GetWorkLines(Work work)\r\n        {\r\n            List<string> lines = work.Code.Split(\"\\r\\n\").ToList();\r\n            return lines.ToArray();\r\n        }\r\n    }\r\n}\r\n"

                },
                new Work()
                {
                    Id = new Guid("8a482b6f-d891-45a7-b628-13483c5fd764"),
                    Code = "using Plagiarism_BLL.CoreModels;\r\nusing Plagiarism_BLL.Enums;\r\nusing Plagiarism_BLL.Services.Interfaces;\r\nusing Plagiarism_BLL.UnitOfWork;\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\n\r\nnamespace Plagiarism_BLL.Services\r\n{\r\n    public class WorkService : IWorkService\r\n    {\r\n        private readonly IUnitOfWork _unitOfWork;\r\n\r\n        private readonly List<string> ExcludedLines = new List<string> { \"\", \"{\", \"}\" };\r\n        private readonly List<string> ExcludedStartsWith = new List<string> { \"using\" };\r\n\r\n        public WorkService(IUnitOfWork unitOfWork)\r\n        {\r\n            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));\r\n        }\r\n\r\n        public async Task<FullWorkResult> UploadWork(Guid userId, string workName, WorkType workType, string code)\r\n        {\r\n            var work = new Work\r\n            {\r\n                Id = Guid.NewGuid(),\r\n                Code = code,\r\n            };\r\n            await _unitOfWork.WorkRepository.CreateAsync(work);\r\n\r\n            var workInfo = new WorkInfo\r\n            {\r\n                WorkName = workName,\r\n                UserId = userId,\r\n                WorkId = work.Id,\r\n                WorkType = workType,\r\n            };\r\n\r\n            await _unitOfWork.WorkInfoRepository.CreateAsync(workInfo);\r\n            await _unitOfWork.SaveAsync();\r\n\r\n            var fullWorkResult = new FullWorkResult\r\n            {\r\n                Id = work.Id,\r\n                Code = code,\r\n                WorkName = workName,\r\n                WorkType = workType,\r\n            };\r\n\r\n            return fullWorkResult;\r\n        }\r\n\r\n        public async Task<CompareWorksResult> CompareWorks(Guid currentWorkId, Guid workToCompareId)\r\n        {\r\n            var currentWork = await _unitOfWork.WorkRepository.GetByIdAsync(currentWorkId);\r\n            var workToCompare = await _unitOfWork.WorkRepository.GetByIdAsync(workToCompareId);\r\n\r\n            var currentWorkLines = GetCleanedLines(currentWork);\r\n            var workToCompareLines = GetCleanedLines(workToCompare);\r\n\r\n            var identicalLinesList = new List<IdenticalLines>();\r\n\r\n            foreach (var line in currentWorkLines)\r\n            {\r\n                var identicalLinesToCurrentLine = workToCompareLines\r\n                    .Where(l => l == line)\r\n                    .ToList();\r\n\r\n                var indices = workToCompareLines\r\n                    .Select((item, index) => (item, index))\r\n                    .Where(pair => pair.item == line)\r\n                    .Select(pair => pair.index)\r\n                    .ToList();\r\n\r\n                if (indices.Count != 0)\r\n                {\r\n                    var workToCompareLineNumbers = indices\r\n                        .Select(index => workToCompareLines[index].Item2)\r\n                        .ToList();\r\n\r\n                    var identicalLines = new IdenticalLines\r\n                    {\r\n                        CurrentWorkLineNumber = line.Item2,\r\n                        WorkToCompareLineNumbers = workToCompareLineNumbers\r\n                    };\r\n                    identicalLinesList.Add(identicalLines);\r\n                }\r\n            }\r\n\r\n            var matchPercentage = (double)identicalLinesList.Count / currentWorkLines.Count;\r\n\r\n            var compareWorksResult = new CompareWorksResult\r\n            {\r\n                IdenticalLines = identicalLinesList,\r\n                CurrentWork = ParseWork(currentWork),\r\n                WorkToCompare = ParseWork(workToCompare),\r\n                MatchPercentage = matchPercentage\r\n            };\r\n\r\n            return compareWorksResult;\r\n        }\r\n\r\n        private ParsedWork ParseWork(Work work)\r\n        {\r\n            var workLines = GetLinesArray(work);\r\n\r\n            return new ParsedWork\r\n            {\r\n                Id = work.Id,\r\n                CodeLines = workLines\r\n            };\r\n        }\r\n\r\n        private List<(string, int)> GetCleanedLines(Work work)\r\n        {\r\n            var lines = work.Code\r\n                .Replace(\" \", \"\")\r\n                .Split(\"\\r\\n\")\r\n                .Where(line => !ExcludedLines.Contains(line) && !ExcludedStartsWith.Any(line.StartsWith))\r\n                .Select((line, index) => (line, index))\r\n                .ToList();\r\n\r\n            return lines;\r\n        }\r\n\r\n        private string[] GetLinesArray(Work work)\r\n        {\r\n            return work.Code.Split(\"\\r\\n\");\r\n        }\r\n    }\r\n}"
                },
                new Work()
                {
                    Id = new Guid("f15d4136-2772-458c-8f7d-763734149718"),
                    Code = "using Plagiarism_BLL.CoreModels;\r\nusing Plagiarism_BLL.Enums;\r\nusing Plagiarism_BLL.Services.Interfaces;\r\nusing Plagiarism_BLL.UnitOfWork;\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Threading.Tasks;\r\n\r\nnamespace Plagiarism_BLL.Services\r\n{\r\n    public class WorkService : IWorkService\r\n    {\r\n        private readonly IUnitOfWork _unitOfWork;\r\n\r\n        private readonly List<string> ExcludedLines = new List<string> { \"\", \"{\", \"}\" };\r\n        private readonly List<string> ExcludedStartsWith = new List<string> { \"using\" };\r\n\r\n        public WorkService(IUnitOfWork unitOfWork)\r\n        {\r\n            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));\r\n        }\r\n\r\n        public async Task<FullWorkResult> UploadWork(Guid userId, string workName, WorkType workType, string code)\r\n        {\r\n            var work = new Work\r\n            {\r\n                Id = Guid.NewGuid(),\r\n                Code = code,\r\n            };\r\n            await _unitOfWork.WorkRepository.CreateAsync(work);\r\n\r\n            var workInfo = new WorkInfo\r\n            {\r\n                WorkName = workName,\r\n                UserId = userId,\r\n                WorkId = work.Id,\r\n                WorkType = workType,\r\n            };\r\n\r\n            await _unitOfWork.WorkInfoRepository.CreateAsync(workInfo);\r\n            await _unitOfWork.SaveAsync();\r\n\r\n            var fullWorkResult = new FullWorkResult\r\n            {\r\n                Id = work.Id,\r\n                Code = code,\r\n                WorkName = workName,\r\n                WorkType = workType,\r\n            };\r\n\r\n            return fullWorkResult;\r\n        }\r\n\r\n        public async Task<CompareWorksResult> CompareWorks(Guid currentWorkId, Guid workToCompareId)\r\n        {\r\n            var currentWork = await _unitOfWork.WorkRepository.GetByIdAsync(currentWorkId);\r\n            var workToCompare = await _unitOfWork.WorkRepository.GetByIdAsync(workToCompareId);\r\n\r\n            var currentWorkLines = GetCleanedLines(currentWork);\r\n            var workToCompareLines = GetCleanedLines(workToCompare);\r\n\r\n            var identicalLinesList = FindIdenticalLines(currentWorkLines, workToCompareLines);\r\n\r\n            var matchPercentage = (double)identicalLinesList.Count / currentWorkLines.Count;\r\n\r\n            var compareWorksResult = new CompareWorksResult\r\n            {\r\n                IdenticalLines = identicalLinesList,\r\n                CurrentWork = ParseWork(currentWork),\r\n                WorkToCompare = ParseWork(workToCompare),\r\n                MatchPercentage = matchPercentage\r\n            };\r\n\r\n            return compareWorksResult;\r\n        }\r\n\r\n        private List<IdenticalLines> FindIdenticalLines(List<(string, int)> currentWorkLines, List<(string, int)> workToCompareLines)\r\n        {\r\n            var identicalLinesList = new List<IdenticalLines>();\r\n\r\n            foreach (var line in currentWorkLines)\r\n            {\r\n                var identicalLinesToCurrentLine = workToCompareLines\r\n                    .Where(l => l.Item1 == line.Item1)\r\n                    .ToList();\r\n\r\n                var indices = workToCompareLines\r\n                    .Select((item, index) => (item, index))\r\n                    .Where(pair => pair.item.Item1 == line.Item1)\r\n                    .Select(pair => pair.index)\r\n                    .ToList();\r\n\r\n                if (indices.Count != 0)\r\n                {\r\n                    var workToCompareLineNumbers = indices\r\n                        .Select(index => workToCompareLines[index].Item2)\r\n                        .ToList();\r\n\r\n                    var identicalLines = new IdenticalLines\r\n                    {\r\n                        CurrentWorkLineNumber = line.Item2,\r\n                        WorkToCompareLineNumbers = workToCompareLineNumbers\r\n                    };\r\n                    identicalLinesList.Add(identicalLines);\r\n                }\r\n            }\r\n\r\n            return identicalLinesList;\r\n        }\r\n    }\r\n}\r\n"
                }
            };
        }

        public static List<WorkInfo> GenetrateWorkInfosWithDetails()
        {
            var users = GenerateUsersData();
            var works = GenerateWorksData();
            return new()
            {
                new WorkInfo()
                {
                    Id = new Guid("998c6463-1928-459a-b0a3-08dc04b4f74a"),
                    UserId = new Guid("8ddfa4dd-d8f3-4f3a-bb78-0ded6d5e2305"),
                    User = users[0],
                    WorkId = new Guid("89e202f0-bda2-4713-beee-0c3125266658"),
                    Work = works[0],
                    WorkName = "Work1",
                    WorkType = WorkType.cs
                },
                new WorkInfo()
                {
                    Id = new Guid("898c6463-1928-459a-b0a3-08dc04b4f74a"),
                    UserId = new Guid("a393f87e-6dbc-4d2e-b723-e6e30502e319"),
                    User = users[1],
                    WorkId = new Guid("8a482b6f-d891-45a7-b628-13483c5fd764"),
                    Work = works[1],
                    WorkName = "Work2",
                    WorkType = WorkType.cs
                },
                new WorkInfo()
                {
                    Id = new Guid("f83c0ac1-1f48-4732-8519-3b51a0db379d"),
                    UserId = new Guid("a393f87e-6dbc-4d2e-b723-e6e30502e319"),
                    User = users[1],
                    WorkId = new Guid("f15d4136-2772-458c-8f7d-763734149718"),
                    Work = works[2],
                    WorkName = "Work3",
                    WorkType = WorkType.cs
                },
            };
        }

        public static List<WorkInfo> GenerateWorkInfosData()
        {
            return new()
            {
                new WorkInfo()
                {
                    Id = new Guid("998c6463-1928-459a-b0a3-08dc04b4f74a"),
                    UserId = new Guid("8ddfa4dd-d8f3-4f3a-bb78-0ded6d5e2305"),
                    WorkId = new Guid("89e202f0-bda2-4713-beee-0c3125266658"),
                    WorkName = "Work1",
                    WorkType = WorkType.cs
                },
                new WorkInfo()
                {
                    Id = new Guid("898c6463-1928-459a-b0a3-08dc04b4f74a"),
                    UserId = new Guid("a393f87e-6dbc-4d2e-b723-e6e30502e319"),
                    WorkId = new Guid("8a482b6f-d891-45a7-b628-13483c5fd764"),
                    WorkName = "Work2",
                    WorkType = WorkType.cs
                },
                new WorkInfo()
                {
                    Id = new Guid("f83c0ac1-1f48-4732-8519-3b51a0db379d"),
                    UserId = new Guid("a393f87e-6dbc-4d2e-b723-e6e30502e319"),
                    WorkId = new Guid("f15d4136-2772-458c-8f7d-763734149718"),
                    WorkName = "Work3",
                    WorkType = WorkType.cs
                },
            };
        }

        public static User GenerateTestUser(Guid userId)
        {
            return new User()
            {
                Id = userId,
                Name = "TestName",
                Surname = "TestSurname",
                Email = "TestEmail",
                GroupName = "TestGroupName",
                Role = Plagiarism_BLL.Enums.Role.User,
                UniversityYear = 3,
                PassHash = "$2a$12$2jqPtyTyTNcFu5AGn.Ka5u/OkTRnnfWStZ2MAA9E1Jp4NG9TTyCtq",
            };
        }
    }
}
