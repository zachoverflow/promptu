using System;
using System.Collections.Generic;
using System.Text;
using ZachJohnson.Promptu.UserModel;

namespace ZachJohnson.Promptu.UIModel.Interfaces
{
    internal interface INewUserDialog : IDialog//, IActivatable
    {
        NewUserStep CurrentStep { set; }

        IChooseNewOrExistingProfile ChooseNewOrExistingStep { get; }

        IProfileBasics ProfileBasics { get; }

        IProfileAdvanced ProfileAdvanced { get; }

        IProfileFinish ProfileFinish { get; }

        IExistingProfile ExistingProfile { get; }

        IButton CancelButton { get; }

        IButton NextButton { get; }

        IButton BackButton { get; }

        bool HideNextButton { set; }

        bool HideBackButton { set; }

        void CloseWithOK();
        
        
        //event ParameterlessVoid BackButtonClick;

        //event ParameterlessVoid UseExistingProfileButtonClick;

        //event ParameterlessVoid SelectedProfileChanged;

        //event ParameterlessVoid ExitPromptuButtonClick;

        //string Instructions { set; }

        //string UseExistingProfileText { set; }

        //string CreateNewProfileText { set; }

        //string BackButtonText { set; }

        //string ExitPromptuButtonText { set; }

        //ProfilePlacemark SelectedProfile { get; }

        //bool ShowUseExistingProfileButton { get; set; }

        //bool ShowExitPromptuButton { get; set; }

        //bool ShowInstructions { get; set; }

        //bool ProfileIsSelected { get; }

        //void SetVisibleProfiles(List<ProfilePlacemark> profiles);

        //void ShowDefaultView();

        //void ShowProfileSelectionView();
    }
}
