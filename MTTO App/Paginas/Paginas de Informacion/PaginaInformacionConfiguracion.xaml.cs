﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Rg.Plugins.Popup.Services;

namespace MTTO_App
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PaginaInformacionConfiguracion : Rg.Plugins.Popup.Pages.PopupPage
    {
        public PaginaInformacionConfiguracion(string source)
        {
            InitializeComponent();

            BindingContext = new MTTO_App.ViewModel.PaginaInformacionViewModel(source);
        }

        //===================================================================================================
        //===================================================================================================

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Console.WriteLine("\n=================================================");
            Console.WriteLine("=================================================");
            Console.WriteLine("SE APERTURO LA PAGINA DE INFORMACION DE CONFIGURACION");
            Console.WriteLine("=================================================");
            Console.WriteLine("=================================================\n");
        }

        //===================================================================================================
        //===================================================================================================

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            Console.WriteLine("\n=================================================");
            Console.WriteLine("=================================================");
            Console.WriteLine("SE CLAUSURO LA PAGINA DE INFORMACION DE CONFIGURACION");
            Console.WriteLine("=================================================");
            Console.WriteLine("=================================================\n");
        }

        //===================================================================================================
        //===================================================================================================

        //===========================================================================================================================================
        //===========================================================================================================================================

        // ### Methods for supporting animations in your popup page ###

        // Invoked before an animation appearing
        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();
        }

        // Invoked after an animation appearing
        protected override void OnAppearingAnimationEnd()
        {
            base.OnAppearingAnimationEnd();
        }

        // Invoked before an animation disappearing
        protected override void OnDisappearingAnimationBegin()
        {
            base.OnDisappearingAnimationBegin();
        }

        // Invoked after an animation disappearing
        protected override void OnDisappearingAnimationEnd()
        {
            base.OnDisappearingAnimationEnd();
        }

        protected override Task OnAppearingAnimationBeginAsync()
        {
            return base.OnAppearingAnimationBeginAsync();
        }

        protected override Task OnAppearingAnimationEndAsync()
        {
            return base.OnAppearingAnimationEndAsync();
        }

        protected override Task OnDisappearingAnimationBeginAsync()
        {
            return base.OnDisappearingAnimationBeginAsync();
        }

        protected override Task OnDisappearingAnimationEndAsync()
        {
            return base.OnDisappearingAnimationEndAsync();
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }

        //===========================================================================================================================================
        //===========================================================================================================================================

        //CLAURUSA DE LA PAGINA MEDIANTE BOTON

        [Obsolete]
        async void OnClose(object sender, EventArgs e)
        {
            await PopupNavigation.PopAllAsync();
        }
    }
}