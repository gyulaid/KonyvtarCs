using LibraryApi.Database;
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

    [HttpPost]
    public void CreateMember([FromBody] Member member)
    {
        this.memberService.CreateMember(member);
    }
}