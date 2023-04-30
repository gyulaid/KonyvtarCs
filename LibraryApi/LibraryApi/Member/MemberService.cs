using System.Text.RegularExpressions;
using LibraryApi.Database;
using LibraryApi.Exception;

namespace LibraryApi.Member;

public class MemberService
{
    private const string MEMBER_NOT_FOUND = "Member was not found with id: ";
    private const string NAME_PATTERN = "^[a-zA-Z]+$";

    private readonly LibraryContext libraryContext;
    private readonly ILogger<MemberService> logger;

    public MemberService(LibraryContext libraryContext, ILogger<MemberService> logger)
    {
        this.libraryContext = libraryContext;
        this.logger = logger;
    }

    public List<Member> GetAllMember()
    {
        return this.libraryContext.Members.ToList();
    }

    public Member GetMemberById(int id)
    {
        var member = this.libraryContext.Members.Find(id);
        if (member != null)
        {
            return member;
        }

        throw new EntityNotFoundException(MEMBER_NOT_FOUND + id);
    }

    public Member CreateMember(Member member)
    {
        if (IsNameValid(member.Name))
        {
            var savedMember = this.libraryContext.Members.Add(member);
            this.libraryContext.SaveChanges();
            this.logger.Log(LogLevel.Information, "A new member was saved");
            return savedMember.Entity;
        }
        else
        {
            throw new ArgumentException("Name must contain only alphabetic characters");
        }
    }

    public void DeleteMember(int id)
    {
        var member = this.libraryContext.Members.FindAsync(id);
        if (member.Result != null)
        {
            this.libraryContext.Members.Remove(member.Result);
            this.libraryContext.SaveChanges();
        }
        else
        {
            throw new KeyNotFoundException(MEMBER_NOT_FOUND + id);
        }
    }

    private static bool IsNameValid(string name)
    {
        return Regex.IsMatch(name, NAME_PATTERN);
    }
}