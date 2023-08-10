using SharedKernel.Commands;

namespace WebAPI.Commands
{
    public class EditMessageCommand : BaseCommand
    {
        public string Message { get; set; }
    }
}