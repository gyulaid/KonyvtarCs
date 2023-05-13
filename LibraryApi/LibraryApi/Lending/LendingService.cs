using AutoMapper;
using LibraryApi.Contracts.Lending;
using LibraryApi.Database;
using LibraryApi.Exception;
using Microsoft.EntityFrameworkCore;

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
        return this.mapper.Map<List<LendingResponseDto>>(this.libraryContext.Lendings
            .Include(x => x.Book)
            .Include(y => y.Member)
            .ToList());
    }

    public LendingResponseDto GetLendingById(int id)
    {
        var lending = this.libraryContext.Lendings
            .Include(x => x.Book)
            .FirstOrDefault(lending => lending.Id == id);
        if (lending != null)
        {
            return this.mapper.Map<LendingResponseDto>(lending);
        }

        throw new EntityNotFoundException(LendingNotFound + id);
    }

    public List<LendingResponseDto> GetLendingsByMemberId(int? memberId)
    { 
        var lendingList = this.libraryContext.Lendings
            .Include(props => props.Member)
            .Include(props => props.Book)
            .Where(lendings => lendings.Member.Id == memberId)
            .ToList();

        return this.mapper.Map<List<LendingResponseDto>>(lendingList);
    }

    public List<LendingResponseDto> GetActiveLendingsByMemberId(int? memberId)
    {
        var lendingList = this.libraryContext.Lendings
            .Include(props => props.Member)
            .Include(props => props.Book)
            .Where(lendings => lendings.Member.Id == memberId && lendings.DateOfReturn == null)
            .ToList();

        return this.mapper.Map<List<LendingResponseDto>>(lendingList);
    }

    public LendingResponseDto CreateLending(CreateLendingDto createDto)
    {
        if (!IsDateValid(createDto.DateOfLend, createDto.DeadlineOfReturn))
        {
            throw new InvalidDateException(InvalidReturnDate);
        }

        if (!this.IsBookAvailable(createDto.BookId))
        {
            throw new NotAvailableException("The wanted book is not available");
        }

        var lending = this.mapper.Map<Lending>(createDto);
        this.mapForeignIdsToEntities(lending, createDto);
        var savedLending = this.libraryContext.Lendings.Add(lending);
        this.updateBookAvailability(createDto.BookId, false);
        this.libraryContext.SaveChanges();
        this.logger.Log(LogLevel.Information, "A new lending was saved");

        return this.mapper.Map<LendingResponseDto>(savedLending.Entity);
    }

    private void mapForeignIdsToEntities(Lending lending, CreateLendingDto createDto)
    {
        lending.Book = this.libraryContext.Books.Find(createDto.BookId);
        lending.Member = this.libraryContext.Members.Find(createDto.MemberId);
    }

    public LendingResponseDto ReturnLending(int id, UpdateLendingDto updateDto)
    {
        var lending = this.libraryContext.Lendings
            .Include(x => x.Book)
            .FirstOrDefault(lending => lending.Id == id);

        if (lending is null)
        {
            throw new EntityNotFoundException(LendingNotFound + id);
        }

        if (!IsDateValid(lending.DateOfLend, updateDto.dateOfReturn))
        {
            throw new InvalidDateException(InvalidReturnDate);
        }

        lending.DateOfReturn = updateDto.dateOfReturn;
        this.libraryContext.Lendings.Update(lending);
        this.updateBookAvailability(lending.Book.Id, true);
        this.libraryContext.SaveChanges();
        this.logger.Log(LogLevel.Information, "Book was returned with id: " + lending.Book.Id);
        return this.mapper.Map<LendingResponseDto>(lending);
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

    private static bool IsDateValid(DateTime earlyDate, DateTime? lateDate)
    {
        return lateDate != null && lateDate.Value.CompareTo(earlyDate) != -1;
    }

    private bool IsBookAvailable(int BookId)
    {
        var book = this.libraryContext.Books.FirstOrDefault(b => b.IsAvailable && b.Id == BookId);
        return book is { IsAvailable: true };
    }

    private void updateBookAvailability(int id, bool Availability)
    {
        var book = this.libraryContext.Books.Find(id);
        if (book == null)
        {
            return;
        }

        book.IsAvailable = Availability;
        this.libraryContext.Books.Update(book);
    }
}