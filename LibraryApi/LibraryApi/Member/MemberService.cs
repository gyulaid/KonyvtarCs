using System.Text.RegularExpressions;
using LibraryApi.Database;

namespace LibraryApi.Member;

public class MemberService
{
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

        throw new KeyNotFoundException("Member not found with id " + id);
    }

    public void CreateMember(Member member)
    {
        if (IsNameValid(member.Name))
        {
            this.libraryContext.Members.Add(member);
            this.libraryContext.SaveChanges();
            this.logger.Log(LogLevel.Information, "A new member was saved");
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
            throw new KeyNotFoundException("Member not found with id: " + id);
        }
    }

    private static bool IsNameValid(string name)
    {
        const string pattern = "^[a-zA-Z]+$";
        return Regex.IsMatch(name, pattern);
    }
}