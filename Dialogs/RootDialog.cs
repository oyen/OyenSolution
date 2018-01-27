using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using MyBot.Dialogs;
using Siritsit.Models;
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

            //a bunch of questions are usually asked here to determine which LUIS Dialog I will call because I have several
            //right now, let's just go straight to the Application dialog

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