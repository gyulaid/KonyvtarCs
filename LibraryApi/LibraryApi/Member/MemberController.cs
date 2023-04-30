using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Member;

[ApiController]
[Route("/members")]
public class MemberController : ControllerBase
{
    private readonly MemberService memberService;

    public MemberController(MemberService memberService)
    {
        this.memberService = memberService;
    }

    [HttpGet]
    public List<Member> GetAllMembers()
    {
        return this.memberService.GetAllMember();
    }

    [HttpGet("{id}")]
    public Member GetMember(int id)
    {
        return this.memberService.GetMemberById(id);
    }

    [HttpPost]
    public Member CreateMember([FromBody] Member member)
    {
        return this.memberService.CreateMember(member);
    }

    [HttpDelete("{id}")]
    public void DeleteMember(int id)
    {
        this.memberService.DeleteMember(id);
    }
}