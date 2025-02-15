//using Shortify.Domain.Abstractions;
//using Shortify.Domain.Entities;
//using Shortify.Domain.ValueObjects;

//namespace Shortify.Domain.Services
//{
//    public class UrlShortenerService
//    {
//        private readonly IUrlMappingRepository _repository;
//        private readonly IUrlCodeGenerator _codeGenerator;
//        private readonly IEventBus _eventBus;

//        public UrlShortenerService(
//            IUrlMappingRepository repository,
//            IUrlCodeGenerator codeGenerator,
//            IEventBus eventBus)
//        {
//            _repository = repository;
//            _codeGenerator = codeGenerator;
//            _eventBus = eventBus;
//        }

//        public async Task<Result<UrlMapping>> CreateShortUrlAsync(
//            string originalUrl,
//            Guid ownerId,
//            string customCode = null)
//        {
//            var destinationResult = OriginalUrl.Create(originalUrl);
//            if (destinationResult.IsFailure)
//                return Result.Failure<UrlMapping>(destinationResult.Error);

//            string code = customCode ?? await _codeGenerator.GenerateAsync();
//            var shortCodeResult = ShortUrlCode.Create(code);
//            if (shortCodeResult.IsFailure)
//                return Result.Failure<UrlMapping>(shortCodeResult.Error);

//            // Check for existing code
//            var existing = await _repository.GetByCodeAsync(code);
//            if (existing != null)
//                return Result.Failure<UrlMapping>("Short code already exists");

//            var mappingResult = UrlMapping.Create(
//                shortCodeResult.Value,
//                destinationResult.Value,
//                ownerId);

//            if (mappingResult.IsFailure)
//                return mappingResult;

//            var mapping = mappingResult.Value;
//            await _repository.AddAsync(mapping);

//            foreach (var @event in mapping.DomainEvents)
//            {
//                await _eventBus.PublishAsync(@event);
//            }

//            return Result.Success(mapping);
//        }

//        public async Task<Result<UrlMapping>> RecordClickAsync(string code)
//        {
//            var mapping = await _repository.GetByCodeAsync(code);
//            if (mapping == null)
//                return Result.Failure<UrlMapping>("Short URL not found");

//            if (!mapping.IsActive)
//                return Result.Failure<UrlMapping>("Short URL is not active");

//            if (mapping.IsExpired())
//                return Result.Failure<UrlMapping>("Short URL has expired");

//            mapping.IncrementClicks();
//            await _repository.UpdateAsync(mapping);

//            foreach (var @event in mapping.DomainEvents)
//            {
//                await _eventBus.PublishAsync(@event);
//            }

//            return Result.Success(mapping);
//        }
//    }
//}