using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;

namespace MyBot.Dialogs
{

    [Serializable]
    public class MyApplication : IDialog<string>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Call(new ApplyForm(), CompleteApplication);
        }

        private async Task CompleteApplication(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;

            context.Done("SUCCESSFUL");
        }
    }
}