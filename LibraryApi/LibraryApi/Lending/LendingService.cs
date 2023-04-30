using AutoMapper;
using LibraryApi.Database;
using LibraryApi.Exception;
using LibraryApi.Lending.Dto;

namespace LibraryApi.Lending;

public class LendingService
{
    private const string LendingNotFound = "Lending was not found with id: ";
    private const string InvalidReturnDate = "Return date can not be sooner than the lend date";

    private readonly LibraryContext libraryContext;
    private readonly ILogger<LendingService> logger;
    private readonly IMapper mapper;

    public LendingService(LibraryContext libraryContext, ILogger<LendingService> logger, IMapper mapper)
    {
        this.libraryContext = libraryContext;
        this.logger = logger;
        this.mapper = mapper;
    }

    public List<LendingResponseDto> GetAllLendings()
    {
        return mapper.Map<List<LendingResponseDto>>(this.libraryContext.Lendings.ToList());
    }

    public LendingResponseDto GetLendingById(int id)
    {
        var lending = this.libraryContext.Lendings.Find(id);
        if (lending != null)
        {
            return mapper.Map<LendingResponseDto>(lending);
        }

        throw new EntityNotFoundException(LendingNotFound + id);
    }

    public LendingResponseDto CreateLending(CreateLendingDto createDto)
    {
        if (IsReturnDateValid(createDto.DateOfLend, createDto.DateOfReturn))
        {
            var savedLending = this.libraryContext.Lendings.Add(mapper.Map<Lending>(createDto));
            this.libraryContext.SaveChanges();
            this.logger.Log(LogLevel.Information, "A new lending was saved");
            return mapper.Map<LendingResponseDto>(savedLending.Entity);
        }

        throw new ArgumentException(InvalidReturnDate);
    }

    public LendingResponseDto ReturnLending(int id, UpdateLendingDto updateDto)
    {
        var lending = this.libraryContext.Lendings.Find(id);
        if (lending is null)
        {
            throw new EntityNotFoundException(LendingNotFound + id);
        }

        if (!IsReturnDateValid(lending.DateOfLend, updateDto.dateOfReturn))
        {
            throw new ArgumentException(InvalidReturnDate);
        }

        lending.DateOfReturn = updateDto.dateOfReturn;
        this.libraryContext.Lendings.Update(lending);
        this.libraryContext.SaveChanges();
        this.logger.Log(LogLevel.Information, "Book was returned with id: " + lending.BookId);
        return mapper.Map<LendingResponseDto>(lending);
    }

    public void DeleteLending(int id)
    {
        var lending = this.libraryContext.Lendings.FindAsync(id);
        if (lending.Result != null)
        {
            this.libraryContext.Lendings.Remove(lending.Result);
            this.libraryContext.SaveChanges();
            this.logger.Log(LogLevel.Information, "Lending was deleted with id: " + id);
        }
        else
        {
            throw new EntityNotFoundException(LendingNotFound + id);
        }
    }

    private static bool IsReturnDateValid(DateTime lendDate, DateTime? returnDate)
    {
        return returnDate is null || returnDate.Value.CompareTo(lendDate) != -1;
    }
}