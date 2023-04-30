using LibraryApi.Database;
using LibraryApi.Exception;

namespace LibraryApi.Lending;

public class LendingService
{
    private const string LENDING_NOT_FOUND = "Lending was not found with id: ";

    private readonly LibraryContext libraryContext;
    private readonly ILogger<LendingService> logger;

    public LendingService(LibraryContext libraryContext, ILogger<LendingService> logger)
    {
        this.libraryContext = libraryContext;
        this.logger = logger;
    }
    
    public List<Lending> GetAllLendings()
    {
        return this.libraryContext.Lendings.ToList();
    }

    public Lending GetLendingById(int id)
    {
        var lending = this.libraryContext.Lendings.Find(id);
        if (lending != null)
        {
            return lending;
        }

        throw new EntityNotFoundException(LENDING_NOT_FOUND + id);
    }

    public Lending CreateLending(Lending lending)
    {
        if (IsReturnDateValid(lending.dateOfLend, lending.dateOfReturn))
        {
            var savedLending = this.libraryContext.Lendings.Add(lending);
            this.libraryContext.SaveChanges();
            this.logger.Log(LogLevel.Information, "A new lending was saved");
            return savedLending.Entity;
        }

        throw new InvalidDataException("Return date can not be sooner than the lend date");
    }

    public Lending ReturnLending(int id, DateTime newDateOfReturn)
    {
        var lending = this.libraryContext.Lendings.Find(id);
        if (lending is null)
        {
            throw new EntityNotFoundException(LENDING_NOT_FOUND + id);
        }

        lending.dateOfReturn = newDateOfReturn;
        this.libraryContext.Lendings.Update(lending);
        this.libraryContext.SaveChanges();
        this.logger.Log(LogLevel.Information, "Book with id: " + lending.inventoryId + "was returned");
        return lending;
    }

    public void DeleteLending(int id)
    {
        var lending = this.libraryContext.Lendings.FindAsync(id);
        if (lending.Result != null)
        {
            this.libraryContext.Lendings.Remove(lending.Result);
            this.libraryContext.SaveChanges();
        }
        else
        {
            throw new EntityNotFoundException(LENDING_NOT_FOUND + id);
        }
    }

    private bool IsReturnDateValid(DateTime lendDate, DateTime? returnDate)
    {
        return returnDate is null || returnDate > lendDate;
    }
}