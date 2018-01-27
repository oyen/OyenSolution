using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Siritsit.Models;
using System;
using System.Threading.Tasks;

namespace MyBot.Dialogs
{

    [Serializable]
    public class ApplyForm : IDialog<ApplyForm>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var state = new ApplyForm();

            CustomerAccount customerAccount;
            if (context.UserData.TryGetValue<CustomerAccount>("CustomerAccount", out customerAccount))
            {
                state.FirstName = customerAccount.FirstName;
                state.EmailAddress = customerAccount.Email;
            }

            var form = new FormDialog<ApplyForm>(
                state,
                BuildForm,
               FormOptions.PromptInStart);

            context.Call(form, this.AfterBuildForm);
        }

        [Prompt("What is your **{&}**?")]
        public string FirstName { get; set; }

        [Prompt("What is your **{&}**?")]
        public string EmailAddress;

        [Numeric(0, 15)]
        [Prompt("How many adults will attend?")]
        public int AdultOptionCount { get; set; }

        [Numeric(0, 15)]
        [Prompt("How many children will attend?")]
        public int ChildOptionCount { get; set; }

        public static IForm<ApplyForm> BuildForm()
        {
            var form = new FormBuilder<ApplyForm>()
                    .Field(nameof(FirstName),
                    active: (state) =>
                    {
                        return string.IsNullOrEmpty(state.FirstName);
                    })
                    .Field(nameof(EmailAddress),
                    active: (state) =>
                    {
                        return string.IsNullOrEmpty(state.EmailAddress);
                    })
                    .Field(nameof(AdultOptionCount),
                    validate: async (state, value) =>
                    {
                        var result = new ValidateResult { Value = value };
                        result.IsValid = IsInteger(value);
                        return result;
                    })
                    .Field(nameof(ChildOptionCount),
                    validate: async (state, value) =>
                    {
                        var result = new ValidateResult { Value = value };
                        result.IsValid = IsInteger(value);
                        return result;
                    })
                    .Build();

            return (IForm<ApplyForm>)form;
        }

        private async Task AfterBuildForm(IDialogContext context, IAwaitable<ApplyForm> result)
        {
            context.Done(result);
        }

        private static bool IsInteger(object value)
        {
            try
            {
                var s = value.ToString();
                int n;
                if (int.TryParse(s, out n))
                    return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
    }
}