﻿#pragma checksum "..\..\..\..\..\Views\Login\SingIn.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8C16FE2615A054BCCA74604C8B3B1EEC9BCD589E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro.IconPacks;
using MahApps.Metro.IconPacks.Converter;
using MvvmCross.Platforms.Wpf.Binding;
using MvvmCross.Platforms.Wpf.Views;
using OV.MVX.ViewModels;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using WPF_OV_OnlineVote.Views.Login;


namespace WPF_OV_OnlineVote.Views.Login {
    
    
    /// <summary>
    /// SingIn
    /// </summary>
    public partial class SingIn : MvvmCross.Platforms.Wpf.Views.MvxWpfView, System.Windows.Markup.IComponentConnector {
        
        
        #line 37 "..\..\..\..\..\Views\Login\SingIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel organizador;
        
        #line default
        #line hidden
        
        
        #line 171 "..\..\..\..\..\Views\Login\SingIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Comunities;
        
        #line default
        #line hidden
        
        
        #line 201 "..\..\..\..\..\Views\Login\SingIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox Provinces;
        
        #line default
        #line hidden
        
        
        #line 300 "..\..\..\..\..\Views\Login\SingIn.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dateOfBirth;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.5.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/WPF_OV_OnlineVote;component/views/login/singin.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\..\Views\Login\SingIn.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "5.0.5.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.organizador = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.Comunities = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 3:
            this.Provinces = ((System.Windows.Controls.ComboBox)(target));
            return;
            case 4:
            this.dateOfBirth = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 5:
            
            #line 357 "..\..\..\..\..\Views\Login\SingIn.xaml"
            ((System.Windows.Controls.PasswordBox)(target)).PasswordChanged += new System.Windows.RoutedEventHandler(this.PasswordBox_PasswordChanged);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 383 "..\..\..\..\..\Views\Login\SingIn.xaml"
            ((System.Windows.Controls.PasswordBox)(target)).PasswordChanged += new System.Windows.RoutedEventHandler(this.PasswordBox_PasswordChanged_1);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

