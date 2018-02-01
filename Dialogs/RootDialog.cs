using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MyBot.Dialogs;
using MyBot.Models;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MyDialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public RootDialog()
        {
        }

        public async Task StartAsync(IDialogContext context)
        {
            //This is a test so I am filling up CustomerAccount data here
            CustomerAccount ca = new CustomerAccount();
            ca.FirstName = "Oyen";
            ca.Email = "oyen@email.com";
            context.UserData.SetValue<CustomerAccount>("CustomerAccount", ca);

            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            //this message is always ignored
            var message = await result;

            await context.PostAsync("Hello there! Let's get started.");

            //Call VerticalChoiceForm that breaks when I update nuget Bot to 13.3.0
            context.Call(new VerticalChoiceForm(), VerticalChoiceFormResumeAfter);
        }

        private async Task VerticalChoiceFormResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;

            //Application Dialog calls ApplyForm where I reported a bug with RESET
            var app = new MyApplication();
            context.Call(app, ApplicationResumeAfter);
        }

        private async Task ApplicationResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            var message = await result;

            //do nothing else in this test
            context.Done(true);
        }
    }




    
}