namespace MyBot.Dialogs
{
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Builder.FormFlow;
    using MyBot.Models;
    using System;
    using System.Threading.Tasks;

    #region enums
    public enum VerticalTypeOptions
    {
        VA = 1,
        VB = 2,
        VC = 3,
    }

    public enum VerticalAOptions
    {
        A1 =1,
        A2,
        A3,
        A4,
        A5
    }

    public enum VerticalBOptions
    {
        B1 = 2,
    }

    public enum VerticalCOptions
    {
        C1 =1,
        C2,
        C3,
        C4,
        C5,
        C6,
    }
    #endregion

    [Serializable]
    public class VerticalChoiceForm : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            var state = new VerticalChoiceForm();

            var form = new FormDialog<VerticalChoiceForm>(
                state,
                BuildForm,
                FormOptions.PromptInStart);

            context.Call(form, this.AfterBuildForm);
        }


        [Prompt("What vertical are you interested in? {||}")]
        public VerticalTypeOptions VerticalType { get; set; }


        [Prompt("What option are you interested in? {||}")]
        public VerticalAOptions? VerticalA { get; set; }


        [Prompt("What option are you interested in? {||}")]
        public VerticalBOptions? VerticalB { get; set; }


        [Prompt("What option are you interested in? {||}")]
        public VerticalCOptions? VerticalC { get; set; }

        public static IForm<VerticalChoiceForm> BuildForm()
        {
            
            return new FormBuilder<VerticalChoiceForm>()
                    .Field(nameof(VerticalType))
                    .Field(nameof(VerticalA),
                        active: (state) =>
                        {
                            return state.VerticalType == VerticalTypeOptions.VA;
                        })
                    .Field(nameof(VerticalB),
                        active: (state) =>
                        {
                            if (state.VerticalType == VerticalTypeOptions.VB)
                            {
                                if (Enum.GetNames(typeof(VerticalBOptions)).Length > 1)
                                {
                                    //if there is more than 1 Vertical B option, then ask the question
                                    return true;
                                }
                                else
                                {
                                    //else set the option by default
                                    state.VerticalB = VerticalBOptions.B1;
                                }
                            }
                            return false;
                        })
                    .Field(nameof(VerticalC),
                        active: (state) =>
                        {
                            return state.VerticalType == VerticalTypeOptions.VC;
                        })
                   
                    .Build();
        }

        private async Task AfterBuildForm(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var o = await result;

                if (o.GetType() == typeof(VerticalChoiceForm))
                {
                    VerticalChoiceForm verticalChoiceForm = await result as VerticalChoiceForm;

                    if (verticalChoiceForm.VerticalA.HasValue)
                        context.UserData.SetValue<int>(ContextKeys.VerticalId, (int)verticalChoiceForm.VerticalA.Value);
                    else if (verticalChoiceForm.VerticalB.HasValue)
                        context.UserData.SetValue<int>(ContextKeys.VerticalId, (int)verticalChoiceForm.VerticalB.Value);
                    else if (verticalChoiceForm.VerticalC.HasValue)
                        context.UserData.SetValue<int>(ContextKeys.VerticalId, (int)verticalChoiceForm.VerticalC.Value);

                    context.Done(verticalChoiceForm);
                }
                else
                {
                    context.Done("ERROR");
                }
            }
            catch (FormCanceledException ex)
            {
                context.Done("QUIT");
            }
            catch (Exception ex)
            {
                await context.PostAsync("I'm sorry, Vertical Choice Form encountered a problem.");
                context.Done("ERROR");
            }

        }


    };
}
