using SharedKernel.Commands;

namespace WebAPI.Commands
{
    public class DeletePostCommand : BaseCommand
    {
        public string Username { get; set; }
    }
}