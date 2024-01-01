using AutoMapper;
using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.Enums;
using Plagiarism_BLL.Services.Interfaces;
using Plagiarism_BLL.UnitOfWork;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.Services
{
    public class WorkService: IWorkService
    {
        private List<string> ExcludeLines = new List<string>() {"","{","}"};
        private List<string> ExcludedStartsWith = new List<string>() { "using" };
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public WorkService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FullWorkResult> UploadWork(Guid userId, string workName, WorkType workType, string code)
        {
            Work work = new Work()
            {
                Id = Guid.NewGuid(),
                Code = code,
            };
            await _unitOfWork.WorkRepository.CreateAsync(work);

            WorkInfo workInfo = new WorkInfo()
            {
                WorkName = workName,
                UserId = userId,
                WorkId = work.Id,
                WorkType = workType,
            };

            await _unitOfWork.WorkInfoRepository.CreateAsync(workInfo);
            await _unitOfWork.SaveAsync();

            var fullWorkResult = new FullWorkResult()
            {
                Id = work.Id,
                Code = code,
                WorkName = workName,
                WorkType = workType,
            };

            return fullWorkResult;
        }

        public async Task DeleteWork(Guid workId)
        {
            await _unitOfWork.WorkRepository.DeleteAsync(workId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<CompareTwoWorksResult> CompareWorks(Guid currentWorkId, Guid workToCompareId)
        {
            var currentWork = await _unitOfWork.WorkRepository.GetByIdAsync(currentWorkId);
            var workToCompare = await _unitOfWork.WorkRepository.GetByIdAsync(workToCompareId);
            return await CompareTwoWorks(currentWork, workToCompare);
        }
        
        private async Task<CompareTwoWorksResult> CompareTwoWorks(Work currentWork, Work workToCompare)
        {
            var currentWorkLines = GetWorkRawLines(currentWork);
            var workToCompareLines = GetWorkRawLines(workToCompare);

            var identicalLinesList = new List<IdenticalLines>();

            foreach (var line in currentWorkLines)
            {
                var identicalLinesToCurrentLine = workToCompareLines.Where(l => l == line);

                List<int> indices = Enumerable.Range(0, workToCompareLines.Count())
                    .Where(i => workToCompareLines[i].Item1 == line.Item1)
                    .ToList();

                if (indices.Count != 0)
                {
                    List<int> workToCompareLineNumbers = new List<int>();
                    foreach (int index in indices)
                    {
                        workToCompareLineNumbers.Add(workToCompareLines[index].Item2);
                    }
                    var identicalLines = new IdenticalLines()
                    {
                        CurrentWorkLineNumber = line.Item2,
                        WorkToCompareLineNumbers = workToCompareLineNumbers
                    };
                    identicalLinesList.Add(identicalLines);
                }
            }
            double matchPercentage = (double)identicalLinesList.Count() / currentWorkLines.Count();

            var compareWorksResult = new CompareTwoWorksResult()
            {
                IdenticalLines = identicalLinesList,
                CurrentWork = ParseWork(currentWork),
                WorkToCompare = ParseWork(workToCompare),
                MatchPercentage = matchPercentage
            };

            return compareWorksResult;
        }

        public async Task<CompareToAllWorksResult> CompareToAllWorks(Guid currentWorkId)
        {
            CompareToAllWorksResult compareToAllWorksResult = new CompareToAllWorksResult();
            var currentWork = await _unitOfWork.WorkRepository.GetByIdAsync(currentWorkId);
            var allWorksInfos = await _unitOfWork.WorkInfoRepository.GetAllWorkWithDetailsAsync();
            allWorksInfos = allWorksInfos.Where(w=>w.Work.Id != currentWorkId).ToList();
            foreach (var workInfo in allWorksInfos)
            {
                var work = workInfo.Work;
                var compareTwoWorksResult = await CompareTwoWorks(currentWork, work);
                CompareResult compareResult = _mapper.Map<CompareResult>(compareTwoWorksResult);
                compareResult.UserName = workInfo.User.Name;
                compareResult.UserSurname = workInfo.User.Surname;
                compareResult.UserGroupName = workInfo.User.GroupName;
                compareResult.WorkName = workInfo.WorkName;
                compareToAllWorksResult.CompareResults.Add(compareResult);
            }
            compareToAllWorksResult.CurrentWork = ParseWork(currentWork);
            return compareToAllWorksResult;
        }
        private ParsedWork ParseWork(Work work)
        {
            var workLines = GetWorkLines(work);

            return new ParsedWork()
            {
                Id = work.Id,
                CodeLines = workLines
            };
        }
        private List<(string, int)> GetWorkRawLines(Work work)
        {
            List<string> lines = work.Code.Replace(" ", "").Split("\r\n").ToList();
            List<(string,int)> result = new List<(string, int)>();
            int lineIndex = 0;
            foreach (var line in lines)
            {
                if (!ExcludeLines.Contains(line))
                {
                    foreach (string excludedStartsWith in ExcludedStartsWith)
                    {
                        if (!line.StartsWith(excludedStartsWith))
                        {
                            result.Add((line, lineIndex));
                        }
                    }
                }

                lineIndex++;
            }

            return result;
        }
        private string[] GetWorkLines(Work work)
        {
            List<string> lines = work.Code.Split("\r\n").ToList();
            return lines.ToArray();
        }
    }
}
