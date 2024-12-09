using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using Vml.Core.Dtos;
using Vml.Core.Entities;
using Vml.Core.Enums;
using Vml.Core.Interfaces;
using Vml.Core.Interfaces.V1;
using Vml.Core.Mappings;

namespace Vml.Application.Services.V1
{
    public class DataProcesserService : IDataProcesserService
    {
        private IValidator<DataJobDTO> _validator;
        private readonly IDataProcesserRepository _dataProcesserRepository;
        public DataProcesserService(IDataProcesserRepository dataProcesserRepository, IValidator<DataJobDTO> validator)
        {
            _dataProcesserRepository = dataProcesserRepository;
            _validator = validator;
        }

        public async Task<Result<DataJobDTO>> Create(DataJobDTO dataJob, CancellationToken cancellationToken)
        {
            var result = _validator.Validate(dataJob);
            if (!result.IsValid)
            {
                return Result.Invalid(result.AsErrors());
            }

            var newDataObject = _dataProcesserRepository.Create(new DataJob(dataJob.Name, dataJob.FilePathToProcess));

            if (dataJob.Links.Any())
            {
                foreach (var link in dataJob.Links)
                {
                    newDataObject.AddLink(new Link(link.Rel, link.Href, link.Action, link.Types));
                }
            }

            await _dataProcesserRepository.Commit(cancellationToken);

            return Result.Success(newDataObject.ToDataJobDto());
        }

        public async Task<Result> Delete(Guid dataJobID, CancellationToken cancellationToken)
        {
            var toBeDeleted = await _dataProcesserRepository.ExistsDataJob(dataJobID, cancellationToken);

            if (!toBeDeleted)
            {
                return Result.NotFound($"DataJob with id {dataJobID} Not Found");
            }

            await _dataProcesserRepository.Delete(dataJobID, cancellationToken);
            await _dataProcesserRepository.Commit(cancellationToken);

            return Result.Success();
        }

        public async Task<List<DataJobDTO>> GetAllDataJobs(int pageNumebr, int pageSize, CancellationToken cancellationToken)
        {
            return await _dataProcesserRepository.GetAllDataJobs(pageNumebr, pageSize, cancellationToken);
        }

        public Task<List<string>> GetBackgroundProcessResults(Guid dataJobId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<Result<DataJobStatus>> GetBackgroundProcessStatus(Guid dataJobId, CancellationToken cancellationToken)
        {
            var dataJob = await _dataProcesserRepository.GetDataJob(dataJobId, cancellationToken);

            if (dataJob == null)
            {
                return Result.NotFound($"DataJob with id {dataJobId} Not Found");
            }

            return Result.Success(dataJob.Status);
        }

        public async Task<Result<DataJobDTO>> GetDataJob(Guid id, CancellationToken cancellationToken)
        {
            var dataJob = await _dataProcesserRepository.GetDataJob(id, cancellationToken);

            if (dataJob == null)
            {
                return Result.NotFound($"DataJob with id {id} Not Found");
            }

            return Result.Success(dataJob.ToDataJobDto());
        }

        public async Task<List<DataJobDTO>> GetDataJobsByStatus(int pageNumebr, int pageSize, DataJobStatus status, CancellationToken cancellationToken)
        {
            return await _dataProcesserRepository.GetAllDataJobs(pageNumebr, pageSize, cancellationToken, predicate: job => job.Status == status);
        }

        public async Task<Result<bool>> StartBackgroundProcess(Guid dataJobId, CancellationToken cancellationToken)
        {
            var dataJob = await _dataProcesserRepository.GetDataJob(dataJobId, cancellationToken);

            if (dataJob == null)
            {
                return Result.NotFound($"DataJob with id {dataJobId} Not Found");
            }

            var result = dataJob.SetProcessing();
            if (result.IsSuccess)
            {
                await _dataProcesserRepository.Commit(cancellationToken);
                return Result.Success(true);
            }

            return Result.Forbidden(result.Errors.ToArray());
        }

        public async Task<Result<DataJobDTO>> Update(DataJobDTO dataJob, CancellationToken cancellationToken)
        {
            var result = _validator.Validate(dataJob);
            if (!result.IsValid)
            {
                return Result.Invalid(result.AsErrors());
            }

            var updatedDataJob = await _dataProcesserRepository.GetDataJob(dataJob.Id, cancellationToken);

            if (dataJob == null)
            {
                return Result.NotFound($"DataJob with id {dataJob.Id} Not Found");
            }

            updatedDataJob.UpdateName(dataJob.Name);
            updatedDataJob.UpdateFilePathToProcess(dataJob.FilePathToProcess);

            if (dataJob.Links.Any())
            {
                updatedDataJob.ClearLinks();
                foreach (var link in dataJob.Links)
                {
                    updatedDataJob.AddLink(new Link(link.Rel, link.Href, link.Action, link.Types));
                }
            }


            _dataProcesserRepository.Update(updatedDataJob);
            await _dataProcesserRepository.Commit(cancellationToken);

            return Result.Success(dataJob);
        }
    }
}
