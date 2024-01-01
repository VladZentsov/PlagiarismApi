using AutoMapper;
using Plagiarism_BLL.Services.Interfaces;
using Plagiarism_BLL.Services;
using Plagiarism_BLL.UnitOfWork;
using PlagiarismTest.Helpers;
using Plagiarism_BLL.CoreModels;
using System.Reflection;

namespace PlagiarismTest.BLLTests
{
    public class WorkServiceTests
    {
        private IUnitOfWork _unitOfWork;
        private IWorkService _workService;
        private IMapper _mapper;
        [SetUp]
        public void Setup()
        {
            _unitOfWork = UnitOfWorkMock.UnitOfWork;
            _mapper = MapperHelper.CreateMapper();
            _workService = new WorkService(_unitOfWork, _mapper);
        }

        [Test]
        public async Task GetAccountInfo()
        {
            //Arrange
            var user = ModelHelper.GenerateUsersData().FirstOrDefault();
            string code = "using Plagiarism_BLL.CoreModels;\r\nusing Plagiarism_BLL.Enums;\r\nusing Plagiarism_BLL.Services.Interfaces;\r\nusing Plagiarism_BLL.UnitOfWork;\r\nusing System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\n\r\nnamespace Plagiarism_BLL.Services\r\n{\r\n    public class WorkService : IWorkService\r\n    {\r\n        private readonly IUnitOfWork _unitOfWork;\r\n\r\n        private readonly List<string> ExcludedLines = new List<string> { \"\", \"{\", \"}\" };\r\n        private readonly List<string> ExcludedStartsWith = new List<string> { \"using\" };\r\n\r\n        public WorkService(IUnitOfWork unitOfWork)\r\n        {\r\n            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));\r\n        }\r\n\r\n        public async Task<FullWorkResult> UploadWork(Guid userId, string workName, WorkType workType, string code)\r\n        {\r\n            var work = new Work\r\n            {\r\n                Id = Guid.NewGuid(),\r\n                Code = code,\r\n            };\r\n            await _unitOfWork.WorkRepository.CreateAsync(work);";
            string workName = "TestWork";

            var expectedWork = new FullWorkResult()
            {
                Code = code,
                WorkName = workName,
                WorkType = Plagiarism_BLL.Enums.WorkType.cs
            };
            //Act
            var actualWork = await _workService.UploadWork(user.Id, "TestWork", Plagiarism_BLL.Enums.WorkType.cs, code);

            //Assert
            var idProperty = expectedWork.GetType().GetProperty("Id");
            PropertyInfo[] expProps = new PropertyInfo[] { idProperty };
            Comparator.PropertyValuesAreEquals(expectedWork, actualWork, expProps);
        }

        [Test]
        public async Task CompareWorks()
        {
            //Arrange
            var works = ModelHelper.GenerateWorksData();
            double expectedMatchPercentage = 0.5375;
            //Act
            var actualCompareTwoWorksResult = await _workService.CompareWorks(works[0].Id, works[1].Id);
            var actualMatchPercentage = actualCompareTwoWorksResult.MatchPercentage;
            //Assert
            Assert.AreEqual(expectedMatchPercentage, actualMatchPercentage);
        }

        [Test]
        public async Task CompareToAllWorks()
        {
            //Arrange
            var works = ModelHelper.GenerateWorksData();
            double[] expectedMatchPercentage = new double[] { 0.5375, 0.5125 };
            //Act
            var actualCompareTwoWorksResult = await _workService.CompareToAllWorks(works[0].Id);
            var actualMatchPercentage = actualCompareTwoWorksResult.CompareResults.Select(cr=>cr.MatchPercentage);
            //Assert
            for (int i = 0; i < expectedMatchPercentage.Length; i++)
            {
                Assert.AreEqual(expectedMatchPercentage[i], actualMatchPercentage.ElementAt(i));
            }
            
        }

    }
}
